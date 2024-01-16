﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Recipe_Radar.dbConfig;

#nullable disable

namespace Recipe_Radar.Migrations
{
    [DbContext(typeof(YourDbContext))]
    partial class YourDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("Recipe_Radar.dbConfig.Ingredient", b =>
                {
                    b.Property<int>("IngredientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Amount")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("RecipeID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("IngredientId");

                    b.HasIndex("RecipeID");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("Recipe_Radar.dbConfig.Recipe", b =>
                {
                    b.Property<int>("RecipeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Instructions")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ReadyTime")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Servings")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("RecipeID");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("Recipe_Radar.dbConfig.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Recipe_Radar.dbConfig.UserRecipe", b =>
                {
                    b.Property<int>("UserRecipeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("RecipeID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserRecipeID");

                    b.HasIndex("RecipeID");

                    b.HasIndex("UserID");

                    b.ToTable("UserRecipes");
                });

            modelBuilder.Entity("Recipe_Radar.dbConfig.Ingredient", b =>
                {
                    b.HasOne("Recipe_Radar.dbConfig.Recipe", "Recipe")
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Recipe_Radar.dbConfig.UserRecipe", b =>
                {
                    b.HasOne("Recipe_Radar.dbConfig.Recipe", "Recipe")
                        .WithMany("UserRecipes")
                        .HasForeignKey("RecipeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Recipe_Radar.dbConfig.User", "User")
                        .WithMany("UserRecipes")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Recipe_Radar.dbConfig.Recipe", b =>
                {
                    b.Navigation("Ingredients");

                    b.Navigation("UserRecipes");
                });

            modelBuilder.Entity("Recipe_Radar.dbConfig.User", b =>
                {
                    b.Navigation("UserRecipes");
                });
#pragma warning restore 612, 618
        }
    }
}
