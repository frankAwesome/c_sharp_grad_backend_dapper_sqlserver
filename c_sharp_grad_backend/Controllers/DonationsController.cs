using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using c_sharp_grad_backend.Data;
using c_sharp_grad_backend.Dtos;
using c_sharp_grad_backend.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace c_sharp_grad_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonationsController : ControllerBase
    {
        IConfiguration configuration;
        public DonationsController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var values = await connection.QueryAsync<Donations>("SELECT * FROM TableDonations");
                return Ok(values);
            }        
        }

        [Authorize]
        [HttpPost("adddonation")]
        public async Task<IActionResult> AddDonation(Donations donation)
        {
            var cell = "";
            var firstname = "";

            using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var query = "INSERT INTO [dbo].[TableDonations]([Username], [Amount], [OrganizationId], [TimeStamp], [Description], [ExtraOne], [ExtraTwo], [ExtraThree]) VALUES(@Username, @Amount, @OrganizationId, @TimeStamp, @Description, @ExtraOne, @ExtraTwo, @ExtraThree)";
                await connection.ExecuteAsync(query, donation);
            }

            using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var values = await connection.QueryFirstAsync<UserProfile>(String.Format("SELECT * FROM TableUserProfiles WHERE Username like '{0}'", donation.Username));
                cell = values.Cell;
                
                firstname = values.Firstname;
            }

             
            decimal amount = donation.Amount;

            var message = String.Format("Dear {0}, thank you for donating R{1:0.00} to the COVID-19 cause.", firstname , amount);

            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("To", cell[0] == '+' ? cell : "+27" + cell.Remove(0,1) ));
            nvc.Add(new KeyValuePair<string, string>("From", "+17748477045"));
            nvc.Add(new KeyValuePair<string, string>("Body", message));

            var httpClient = new HttpClient();
            var encoding = new ASCIIEncoding();
            var authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(encoding.GetBytes(string.Format("{0}:{1}", "ACfa70b55ad0f6e2fd05d0d41f5f6c8931", "2e1201887185d89077b3ab4b6659b7cb"))));
            httpClient.DefaultRequestHeaders.Authorization = authHeader;

            var req = new HttpRequestMessage(HttpMethod.Post, "https://api.twilio.com/2010-04-01/Accounts/ACfa70b55ad0f6e2fd05d0d41f5f6c8931/Messages.json") { Content = new FormUrlEncodedContent(nvc) };
            var response = await httpClient.SendAsync(req);


            return StatusCode(201);
        }

        [Authorize]
        [HttpPost("userdonations")]
        public async Task<IActionResult> UserDonations(UserForDonationDto userForDonationDto)
        {
            using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var values = await connection.QueryAsync<Donations>(String.Format("SELECT * FROM TableDonations WHERE Username like '{0}'", userForDonationDto.Username));
                return Ok(values);
            }
        }
    }
}