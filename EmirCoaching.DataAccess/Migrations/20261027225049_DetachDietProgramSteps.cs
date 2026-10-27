using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmirCoaching.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DetachDietProgramSteps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DietProgramSteps_DietPrograms_DietProgramId",
                table: "DietProgramSteps");

            migrationBuilder.DropIndex(
                name: "IX_DietProgramSteps_DietProgramId",
                table: "DietProgramSteps");

            migrationBuilder.DropColumn(
                name: "DietProgramId",
                table: "DietProgramSteps");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DietProgramId",
                table: "DietProgramSteps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DietProgramSteps",
                keyColumn: "Id",
                keyValue: 1,
                column: "DietProgramId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "DietProgramSteps",
                keyColumn: "Id",
                keyValue: 2,
                column: "DietProgramId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "DietProgramSteps",
                keyColumn: "Id",
                keyValue: 3,
                column: "DietProgramId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_DietProgramSteps_DietProgramId",
                table: "DietProgramSteps",
                column: "DietProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_DietProgramSteps_DietPrograms_DietProgramId",
                table: "DietProgramSteps",
                column: "DietProgramId",
                principalTable: "DietPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
