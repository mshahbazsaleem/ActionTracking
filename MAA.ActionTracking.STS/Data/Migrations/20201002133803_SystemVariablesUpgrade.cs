using Microsoft.EntityFrameworkCore.Migrations;

namespace MAA.ActionTracking.STS.Data.Migrations
{
    public partial class SystemVariablesUpgrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "SystemVariables",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "SystemVariables");
        }
    }
}
