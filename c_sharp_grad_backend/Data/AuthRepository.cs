using c_sharp_grad_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace c_sharp_grad_backend.Data
{
    public class AuthRepository : IAuthRepository
    {
        IConfiguration configuration;

        public AuthRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public async Task<User> Login(string username, string password)
        {
            User user = null;

            using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    user = await connection.QueryFirstAsync<User>(String.Format("SELECT * FROM TableUsers WHERE Username like '{0}'", username));
                }
                catch{}
                


                if (user == null)
                {
                    return null;
                }

                if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                {
                    return null;
                }

                return user;
            }      
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }

                }
            }

            return true;
        }


        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;

            user.PasswordSalt = passwordSalt;

            using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                var query = "INSERT INTO [dbo].[TableUsers]([Username], [ProfileImage], [PasswordHash], [PasswordSalt]) VALUES(@Username, @ProfileImage, @PasswordHash, @PasswordSalt)";
                await connection.ExecuteAsync(query, user);
            }

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            User user = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    user = await connection.QueryFirstAsync<User>(String.Format("SELECT * FROM TableUsers WHERE Username like '{0}'", username));
                }        
            }
            catch { }

            if (user != null)
                return true;
            else
                return false;
        }
    }
}
