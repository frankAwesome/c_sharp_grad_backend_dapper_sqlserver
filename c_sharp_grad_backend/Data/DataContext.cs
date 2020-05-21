using c_sharp_grad_backend.Controllers;
using c_sharp_grad_backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace c_sharp_grad_backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Donations> TableDonations { get; set; }

        public DbSet<User> TableUsers { get; set; }


        public DbSet<UserProfile> TableUserProfiles { get; set; }


    }
}
