using System;
using System.Collections.Generic;
using System.Linq;
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

            using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var query = "INSERT INTO [dbo].[TableDonations]([Username], [Amount], [OrganizationId], [TimeStamp], [Description], [ExtraOne], [ExtraTwo], [ExtraThree]) VALUES(@Username, @Amount, @OrganizationId, @TimeStamp, @Description, @ExtraOne, @ExtraTwo, @ExtraThree)";
                await connection.ExecuteAsync(query, donation);
            }

            using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var values = await connection.QueryFirstAsync<UserProfile>(String.Format("SELECT * FROM TableUserProfiles WHERE Username like '{0}'", donation.Username));
                cell = values.Cell;
            }

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