using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Razor.Infrastructure.Persistence.Migrations
{
    public partial class shop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sequence",
                table: "Products",
                newName: "Sort");

            migrationBuilder.RenameColumn(
                name: "Pictures",
                table: "Products",
                newName: "SmalllImages");

            migrationBuilder.RenameColumn(
                name: "IsPublish",
                table: "Products",
                newName: "IsSingle");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Products",
                newName: "Remark");

            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnable",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Labels",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

             

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    MinCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryDistance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropColumn(
                name: "Images",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsEnable",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Labels",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Sort",
                table: "Products",
                newName: "Sequence");

            migrationBuilder.RenameColumn(
                name: "SmalllImages",
                table: "Products",
                newName: "Pictures");

            migrationBuilder.RenameColumn(
                name: "Remark",
                table: "Products",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "IsSingle",
                table: "Products",
                newName: "IsPublish");

             
        }
    }
}
