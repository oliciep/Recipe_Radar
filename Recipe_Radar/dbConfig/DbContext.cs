using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Radar.dbConfig
{
    public class YourDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=C:\\Users\\olive\\source\\repos\\Recipe_Radar\\Recipe_Radar\\dbConfig\\Recipes.db");
            }
        }

        public void AddUser(User newUser)
        {
            Users.Add(newUser);
            SaveChanges();
        }

        public List<User> ListUsers()
        {
            return Users.ToList();
        }
    }

    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}