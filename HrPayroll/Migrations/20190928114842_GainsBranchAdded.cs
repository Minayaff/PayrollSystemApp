using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class GainsBranchAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Gains",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Gains_BranchId",
                table: "Gains",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gains_Branches_BranchId",
                table: "Gains",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gains_Branches_BranchId",
                table: "Gains");

            migrationBuilder.DropIndex(
                name: "IX_Gains_BranchId",
                table: "Gains");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Gains");
        }
    }
}
