using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
//using WebApi;
using System.Diagnostics.Metrics;

namespace remailDotNetAPI.Services
{
    public class DBService : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source = Users.db;");


        public DbSet<Models.User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.User>().HasData(

            new Models.User() { Id = 1, UserName = "testuser1", OriginalUserName = "testuser1", Password = "testpass1", CreatedAt = DateTime.Now, UserRole = Services.UserRoles.BasicUser },

            new Models.User() { Id = 2, UserName = "testuser2", OriginalUserName = "testuser2", Password = "testpass2", CreatedAt = DateTime.Now, UserRole = Services.UserRoles.BasicUser }

            );
        }
    }
}