using Microsoft.EntityFrameworkCore.Migrations;

namespace MishMash.Data.Migrations
{
    public partial class AddIsFollowInUserChannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFollow",
                table: "UserChannels",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFollow",
                table: "UserChannels");
        }
    }
}
