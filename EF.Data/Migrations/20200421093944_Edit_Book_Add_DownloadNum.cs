using Microsoft.EntityFrameworkCore.Migrations;

namespace EF.Data.Migrations
{
    public partial class Edit_Book_Add_DownloadNum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DownloadNum",
                table: "Books",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadNum",
                table: "Books");
        }
    }
}
