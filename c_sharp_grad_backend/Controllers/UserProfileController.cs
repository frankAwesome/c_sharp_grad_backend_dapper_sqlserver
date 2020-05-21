﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using c_sharp_grad_backend.Data;
using c_sharp_grad_backend.Dtos;
using c_sharp_grad_backend.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
            using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var query = "UPDATE TableUserProfiles SET AvatarOne=@AvatarOne, AvatarTwo=@AvatarTwo, AvatarThree=@AvatarThree, Firstname=@Firstname, Lastname=@Lastname, Email=@Email, AddressOne=@AddressOne, AddressTwo=@AddressTwo, Country=@Country, Zip=@Zip, PaymentType=@PaymentType, NameOnCard=@NameOnCard, CardNumber=@CardNumber, ExpirationOnCard=@ExpirationOnCard, CVV=@CVV, ExtraPropOne=@ExtraPropOne, ExtraPropTwo=@ExtraPropTwo, ExtraPropThree=@ExtraPropThree, ExtraPropFour=@ExtraPropFour WHERE Username like @Username";
                await connection.ExecuteAsync(query, userProfile);
            }

            return StatusCode(200);
        }

    }
}