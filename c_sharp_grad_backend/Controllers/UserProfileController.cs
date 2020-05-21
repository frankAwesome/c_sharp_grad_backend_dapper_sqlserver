using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using c_sharp_grad_backend.Data;
using c_sharp_grad_backend.Dtos;
using c_sharp_grad_backend.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace c_sharp_grad_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly DataContext _context;
        IConfiguration configuration;

        public UserProfileController(DataContext context, IConfiguration _configuration)
        {
            _context = context;
            configuration = _configuration;
        }


        [HttpPost]
        public async Task<IActionResult> Get(UserForProfileDto userForProfileDto)
        {
            using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var values = await connection.QueryFirstAsync<UserProfile>(String.Format("SELECT * FROM TableUserProfiles WHERE Username like '{0}'", userForProfileDto.Username));
                return Ok(values);
            }
        }


        [HttpPost("addprofile")]
        public async Task<IActionResult> AddProfile(UserProfile userProfile)
        {
            using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var query = "INSERT INTO [dbo].[TableUserProfiles]([AvatarOne], [AvatarTwo], [AvatarThree], [Username], [Firstname], [Lastname], [Email], [AddressOne], [AddressTwo], [Country], [Zip], [PaymentType], [NameOnCard], [CardNumber], [ExpirationOnCard], [CVV], [ExtraPropOne], [ExtraPropTwo], [ExtraPropThree], [ExtraPropFour]) VALUES(@AvatarOne, @AvatarTwo, @AvatarThree, @Username, @Firstname, @Lastname, @Email, @AddressOne, @AddressTwo, @Country, @Zip, @PaymentType, @NameOnCard, @CardNumber, @ExpirationOnCard, @CVV, @ExtraPropOne, @ExtraPropTwo, @ExtraPropThree, @ExtraPropFour)";
                await connection.ExecuteAsync(query, userProfile);
            }

            return StatusCode(201);
        }


        [HttpPut("editprofile")]
        public async Task<IActionResult> EditProfile(UserProfile userProfile)
        {
            //var result = await _context.TableUserProfiles.SingleOrDefaultAsync(b => b.Username == userProfile.Username);
            //if (result != null)
            //{
            //    result.Firstname = userProfile.Firstname;
            //    result.Lastname = userProfile.Lastname;
            //    result.AvatarOne = userProfile.AvatarOne;
            //    result.AvatarTwo = userProfile.AvatarTwo;
            //    result.AvatarThree = userProfile.AvatarThree;
            //    result.Email = userProfile.Email;
            //    result.AddressOne = userProfile.AddressOne;
            //    result.AddressTwo = userProfile.AddressTwo;
            //    result.Country = userProfile.Country;
            //    result.Zip = userProfile.Zip;
            //    result.NameOnCard = userProfile.NameOnCard;
            //    result.PaymentType = userProfile.PaymentType;
            //    result.CardNumber = userProfile.CardNumber;
            //    result.CVV = userProfile.CVV;
            //    result.ExpirationOnCard = userProfile.ExpirationOnCard;
            //    result.ExtraPropOne = userProfile.ExtraPropOne;
            //    result.ExtraPropTwo = userProfile.ExtraPropTwo;
            //    result.ExtraPropThree = userProfile.ExtraPropThree;
            //    result.ExtraPropFour = userProfile.ExtraPropFour;

            //    _context.SaveChanges();
            //}

            return StatusCode(200);
        }

    }
}