using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmirCoaching.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class HomeHeroLineBreak : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PageContents",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ButtonUrl", "Title" },
                values: new object[] { "/paketler", "Kendini, bahanelerinden\ndaha GÜÇLÜ hale getir" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PageContents",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ButtonUrl", "Title" },
                values: new object[] { "/paketler.html", "Kendini, bahanelerinden daha GÜÇLÜ hale getir" });
        }
    }
}
