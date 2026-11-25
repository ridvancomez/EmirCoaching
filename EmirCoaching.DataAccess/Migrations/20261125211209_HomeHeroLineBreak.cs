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
            // Yalnızca seed değerini hâlâ taşıyan alanlar güncellenir; panelde değiştirilmiş başlık/adres korunur
            migrationBuilder.Sql(
                "UPDATE PageContents SET Title = N'Kendini, bahanelerinden' + CHAR(10) + N'daha GÜÇLÜ hale getir' " +
                "WHERE Id = 1 AND Title = N'Kendini, bahanelerinden daha GÜÇLÜ hale getir';");
            migrationBuilder.Sql(
                "UPDATE PageContents SET ButtonUrl = N'/paketler' WHERE Id = 1 AND ButtonUrl = N'/paketler.html';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE PageContents SET Title = N'Kendini, bahanelerinden daha GÜÇLÜ hale getir' " +
                "WHERE Id = 1 AND Title = N'Kendini, bahanelerinden' + CHAR(10) + N'daha GÜÇLÜ hale getir';");
            migrationBuilder.Sql(
                "UPDATE PageContents SET ButtonUrl = N'/paketler.html' WHERE Id = 1 AND ButtonUrl = N'/paketler';");
        }
    }
}
