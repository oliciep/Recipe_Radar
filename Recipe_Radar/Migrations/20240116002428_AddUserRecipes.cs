using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recipe_Radar.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRecipes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRecipe_Recipes_RecipeID",
                table: "UserRecipe");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRecipe_Users_UserID",
                table: "UserRecipe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRecipe",
                table: "UserRecipe");

            migrationBuilder.RenameTable(
                name: "UserRecipe",
                newName: "UserRecipes");

            migrationBuilder.RenameIndex(
                name: "IX_UserRecipe_UserID",
                table: "UserRecipes",
                newName: "IX_UserRecipes_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_UserRecipe_RecipeID",
                table: "UserRecipes",
                newName: "IX_UserRecipes_RecipeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRecipes",
                table: "UserRecipes",
                column: "UserRecipeID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRecipes_Recipes_RecipeID",
                table: "UserRecipes",
                column: "RecipeID",
                principalTable: "Recipes",
                principalColumn: "RecipeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRecipes_Users_UserID",
                table: "UserRecipes",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRecipes_Recipes_RecipeID",
                table: "UserRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRecipes_Users_UserID",
                table: "UserRecipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRecipes",
                table: "UserRecipes");

            migrationBuilder.RenameTable(
                name: "UserRecipes",
                newName: "UserRecipe");

            migrationBuilder.RenameIndex(
                name: "IX_UserRecipes_UserID",
                table: "UserRecipe",
                newName: "IX_UserRecipe_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_UserRecipes_RecipeID",
                table: "UserRecipe",
                newName: "IX_UserRecipe_RecipeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRecipe",
                table: "UserRecipe",
                column: "UserRecipeID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRecipe_Recipes_RecipeID",
                table: "UserRecipe",
                column: "RecipeID",
                principalTable: "Recipes",
                principalColumn: "RecipeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRecipe_Users_UserID",
                table: "UserRecipe",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
