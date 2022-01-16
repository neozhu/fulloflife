using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Razor.Infrastructure.Persistence.Migrations
{
    public partial class fix_product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SmalllImages",
                table: "Products",
                newName: "SmallImages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SmallImages",
                table: "Products",
                newName: "SmalllImages");
        }
    }
}
