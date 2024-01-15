using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Recipe_Radar.apiObjects;
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
        public DbSet<RecipeInformation> Recipes { get; set; }
        public DbSet<ExtendedIngredient> Ingredients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=C:\\Users\\olive\\source\\repos\\Recipe_Radar\\Recipe_Radar\\dbConfig\\Recipes.db");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeInformation>()
                .HasMany(r => r.ExtendedIngredients)
                .WithOne()
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
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

    public class ExtendedIngredient
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }
    }
    public class Recipe
    {
        public int RecipeID { get; set; }
        public string RecipeTitle { get; set; }
        public int ReadyTime { get; set; }
        public int Servings { get; set; }
        public string Image { get; set; }
        public string Instructions { get; set; }
        public List<ExtendedIngredient> ExtendedIngredients { get; set; }
    }
}