using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmirCoaching.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyDietProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationWeeks",
                table: "DietPrograms");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "DietPrograms");

            migrationBuilder.UpdateData(
                table: "DietProgramSteps",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Paketini aldıktan sonra koçun WhatsApp'tan danışan bilgi formunu gönderir; hedeflerini ve beslenme alışkanlıklarını orada paylaşırsın.");

            migrationBuilder.UpdateData(
                table: "DietProgramSteps",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Formun ve tahlillerin ulaşınca uzmanımız planını 48 saat içinde hazırlar.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationWeeks",
                table: "DietPrograms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "DietPrograms",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "DietProgramSteps",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Hedeflerini ve beslenme alışkanlıklarını içeren detaylı formu doldur.");

            migrationBuilder.UpdateData(
                table: "DietProgramSteps",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Uzman koçumuz verilerini analiz ederek sana özel planını 24 saat içinde hazırlar.");

            migrationBuilder.UpdateData(
                table: "DietPrograms",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DurationWeeks", "Price" },
                values: new object[] { 4, 1950m });
        }
    }
}
