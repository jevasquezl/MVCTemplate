using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Compañia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoreSaleId = table.Column<int>(type: "int", nullable: false),
                    CreatedId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpdatedId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_AspNetUsers_CreatedId",
                        column: x => x.CreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Company_AspNetUsers_UpdatedId",
                        column: x => x.UpdatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Company_Store_StoreSaleId",
                        column: x => x.StoreSaleId,
                        principalTable: "Store",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_CreatedId",
                table: "Company",
                column: "CreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_StoreSaleId",
                table: "Company",
                column: "StoreSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_UpdatedId",
                table: "Company",
                column: "UpdatedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
