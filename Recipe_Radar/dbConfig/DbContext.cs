using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Recipe_Radar.apiObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Radar.dbConfig
{
    public class YourDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<UserRecipe> UserRecipes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=C:\\Users\\olive\\source\\repos\\Recipe_Radar\\Recipe_Radar\\dbConfig\\Recipes.db");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .HasKey(r => r.RecipeID);

            modelBuilder.Entity<Ingredient>()
                .HasKey(e => e.IngredientId);

            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Ingredients)
                .WithOne(i => i.Recipe)
                .HasForeignKey(i => i.RecipeID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRecipe>()
                .HasKey(ur => ur.UserRecipeID);

            modelBuilder.Entity<UserRecipe>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRecipes)
                .HasForeignKey(ur => ur.UserID);

            modelBuilder.Entity<UserRecipe>()
                .HasOne(ur => ur.Recipe)
                .WithMany(r => r.UserRecipes)
                .HasForeignKey(ur => ur.RecipeID);
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

        public List<Recipe> ListRecipes()
        {
            return Recipes.ToList();
        }
    }

    public class User
    {
        public User()
        {
            UserRecipes = new List<UserRecipe>();
        }
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<UserRecipe> UserRecipes { get; set; }
    }

    public class Ingredient
    {
        [Key]
        public int IngredientId { get; set; }
        public int RecipeID { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }
        public Recipe Recipe { get; set; }
    }

    public class Recipe
    {
        public int RecipeID { get; set; }
        public string Title { get; set; }
        public int ReadyTime { get; set; }
        public int Servings { get; set; }
        public string Image { get; set; }
        public string Instructions { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<UserRecipe> UserRecipes { get; set; }
    }

    public class UserRecipe
    {
        public int UserRecipeID { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; }
    }
}