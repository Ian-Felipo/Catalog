using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogApi.Migrations
{
    /// <inheritdoc />
    public partial class GenerateDateInCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Categories(Name, ImageUrl) Values('Drinks', 'Drinks.jpg')");
            migrationBuilder.Sql("INSERT INTO Categories(Name, ImageUrl) Values('Desserts', 'Desserts.jpg')");
            migrationBuilder.Sql("INSERT INTO Categories(Name, ImageUrl) Values('Meats', 'Meats.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Categories WHERE Name = 'Drinks'");
            migrationBuilder.Sql("DELETE FROM Categories WHERE Name = 'Desserts'");
            migrationBuilder.Sql("DELETE FROM Categories WHERE Name = 'Meats'");
        }
    }
}
