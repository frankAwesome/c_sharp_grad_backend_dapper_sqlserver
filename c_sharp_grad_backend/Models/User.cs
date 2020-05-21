using System;

namespace c_sharp_grad_backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] ProfileImage { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }


    }
}