using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class posit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Salaries_PositionId",
                table: "Salaries");

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_PositionId",
                table: "Salaries",
                column: "PositionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Salaries_PositionId",
                table: "Salaries");

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_PositionId",
                table: "Salaries",
                column: "PositionId",
                unique: true);
        }
    }
}
