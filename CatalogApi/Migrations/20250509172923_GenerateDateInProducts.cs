using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogApi.Migrations
{
    /// <inheritdoc />
    public partial class GenerateDateInProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql
            (
                @"INSERT INTO Products(Name, Description, Price, ImageUrl, Stock, RegistrationDate, CategoryId) 
                VALUES(""Beer"", "" "", 10.22, ""Beer.jpg"", 5.45, now(), 1)"
            );
            migrationBuilder.Sql
            (
                @"INSERT INTO Products(Name, Description, Price, ImageUrl, Stock, RegistrationDate, CategoryId) 
                VALUES(""Cake"", "" "", 27.98, ""Cake.jpg"", 60.00, now(), 2)"
            );
            migrationBuilder.Sql
            (
                @"INSERT INTO Products(Name, Description, Price, ImageUrl, Stock, RegistrationDate, CategoryId) 
                VALUES(""Chicken"", "" "", 15.68, ""Chicken.jpg"", 50.78, now(), 3)"
            );        
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Categories WHERE Name = 'Beer'");
            migrationBuilder.Sql("DELETE FROM Categories WHERE Name = 'Cake'");
            migrationBuilder.Sql("DELETE FROM Categories WHERE Name = 'Chicken'");
        }
    }
}
