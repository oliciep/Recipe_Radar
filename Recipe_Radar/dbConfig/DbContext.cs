using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Recipe_Radar.apiObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
// File to store and initialise all SQLite tables, as well as declaring any propagations
/// </summary>
namespace Recipe_Radar.dbConfig
{
    /// <summary>
    // Main class definition, main methods and table declarations
    /// </summary>
    public class YourDbContext : DbContext
    {
        // Declaring the Users table, for storing User information
        public DbSet<User> Users { get; set; }
        // Declaring the Recipes table, for storing Recipe information
        public DbSet<Recipe> Recipes { get; set; }
        // Declaring the Ingredients table, for storing lists of ingredients that the Recipes table uses
        public DbSet<Ingredient> Ingredients { get; set; }
        // Declaring the UserRecipes table, for storing links between Users and their Recipes
        public DbSet<UserRecipe> UserRecipes { get; set; }

        /// <summary>
        // On configuration of the SQL database, this method finds the database
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=C:\\Users\\olive\\source\\repos\\Recipe_Radar\\Recipe_Radar\\dbConfig\\Recipes.db");
            }
        }

        /// <summary>
        // Main definitions for properties and links of tables
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Declaring the primary key of the Recipes table (RecipeID)
            modelBuilder.Entity<Recipe>()
                .HasKey(r => r.RecipeID);

            // Declaring the primary key of the Ingredients table (IngredientID)
            modelBuilder.Entity<Ingredient>()
                .HasKey(e => e.IngredientId);

            // Declaring the properties of the Recipes Table
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Ingredients)
                .WithOne(i => i.Recipe)
                // Declares foreign key of Recipes table (Ingredients' RecipeID), allowing to link to Ingredients table
                .HasForeignKey(i => i.RecipeID)
                .IsRequired()
                // Creates link that will delete corresponding ingredients records when Recipes record is deleted
                .OnDelete(DeleteBehavior.Cascade);

            // Declaring the primary key of the UserRecipes table (UserRecipeID)
            modelBuilder.Entity<UserRecipe>()
                .HasKey(ur => ur.UserRecipeID);

            // Declaring the properties of the UserRecipes Table
            modelBuilder.Entity<UserRecipe>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRecipes)
                // Declares foreign key of table (Users' UserID), allowing to link to Users
                .HasForeignKey(ur => ur.UserID);

            modelBuilder.Entity<UserRecipe>()
                .HasOne(ur => ur.Recipe)
                .WithMany(r => r.UserRecipes)
                // Declares foreign key of table (Recipes' RecipeID), allowing to link to Recipes
                .HasForeignKey(ur => ur.RecipeID);
        }

        /// <summary>
        // Method to remove recipe from database
        /// </summary>
        public void RemoveRecipe(int userId, int recipeId)
        {
            using (var context = new YourDbContext())
            {
                // Find the UserRecipe entry
                var userRecipe = context.UserRecipes.FirstOrDefault(ur => ur.UserID == userId && ur.RecipeID == recipeId);

                if (userRecipe != null)
                {
                    // Remove UserRecipe entry
                    context.UserRecipes.Remove(userRecipe);
                    context.SaveChanges();

                    // Check if the recipe is not associated with any other users in UserRecipe
                    var isRecipeUsedByOtherUsers = context.UserRecipes.Any(ur => ur.RecipeID == recipeId);

                    if (!isRecipeUsedByOtherUsers)
                    {
                        // Find the recipe
                        var recipe = context.Recipes.FirstOrDefault(r => r.RecipeID == recipeId);

                        if (recipe != null)
                        {
                            // Check if the recipe has associated ingredients
                            if (recipe.Ingredients != null)
                            {
                                // Remove associated ingredients
                                context.Ingredients.RemoveRange(recipe.Ingredients);
                            }

                            // Remove the recipe
                            context.Recipes.Remove(recipe);
                            context.SaveChanges();
                        }
                    }
                }
            }
        }


        /// <summary>
        // Method to add user to database via Users table
        /// </summary>
        public void AddUser(User newUser)
        {
            Users.Add(newUser);
            SaveChanges();
        }

        /// <summary>
        // Method to list all of users information in Users table
        /// </summary>
        public List<User> ListUsers()
        {
            return Users.ToList();
        }

        /// <summary>
        // Method to list all of recipes information in Recipes table
        /// </summary>
        public List<Recipe> ListRecipes()
        {
            return Recipes.ToList();
        }
    }

    /// <summary>
    // Class definition for Users table
    /// </summary>
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

    /// <summary>
    // Class definition for Ingredients table
    /// </summary>
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

    /// <summary>
    // Class definition for Recipes table
    /// </summary>
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

    /// <summary>
    // Class definition for UserRecipes table
    /// </summary>
    public class UserRecipe
    {
        public int UserRecipeID { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; }
    }
}