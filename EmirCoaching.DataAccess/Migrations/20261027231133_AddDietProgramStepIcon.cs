using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmirCoaching.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDietProgramStepIcon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Icon",
                table: "DietProgramSteps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DietProgramSteps",
                keyColumn: "Id",
                keyValue: 1,
                column: "Icon",
                value: 1);

            migrationBuilder.UpdateData(
                table: "DietProgramSteps",
                keyColumn: "Id",
                keyValue: 2,
                column: "Icon",
                value: 2);

            migrationBuilder.UpdateData(
                table: "DietProgramSteps",
                keyColumn: "Id",
                keyValue: 3,
                column: "Icon",
                value: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "DietProgramSteps");
        }
    }
}
