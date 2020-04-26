using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.Data.Migrations
{
    public partial class Edit_Book_Add_FileSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "Books",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Books");
        }
    }
}
