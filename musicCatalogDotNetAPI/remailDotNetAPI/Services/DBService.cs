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

namespace musicCatalogDotNetAPI.Services
{
    public class DBService : DbContext
    {

        /*private readonly ConnectionStrings _configuration;

        public DBService(ConnectionStrings configuration)
        {
            _configuration = configuration;
        }*/

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source = Users.db;");


        public DbSet<Models.User> User { get; set; }
        public DbSet<Models.Song> Song { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.User>().HasData(

            new Models.User { UserId = 1, UserName = "testuser1", OriginalUserName = "testuser1", Password = "testpass1", CreatedAt = DateTime.Now, UserRole = Services.UserRoles.BasicUser },

            new Models.User { UserId = 2, UserName = "testuser2", OriginalUserName = "testuser2", Password = "testpass2", CreatedAt = DateTime.Now, UserRole = Services.UserRoles.BasicUser }

            //new Models.User(1, "testuser1", "testpass1", Services.UserRoles.BasicUser), //{ UserId = 1, UserName = "testuser1", OriginalUserName = "testuser1", Password = "testpass1", CreatedAt = DateTime.Now, UserRole = Services.UserRoles.BasicUser },

            //new Models.User(1, "testuser2", "testpass2", Services.UserRoles.BasicUser)

            );
        }
    }
}

public class ConnectionStrings
{
    public string UsersContext { get; set; }

}