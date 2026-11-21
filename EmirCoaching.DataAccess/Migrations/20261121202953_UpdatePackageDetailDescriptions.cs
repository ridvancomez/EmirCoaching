using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmirCoaching.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePackageDetailDescriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 1,
                column: "ShortDescription",
                value: "Hedeflerinize ulaşmanız için özel olarak tasarlanmış, bilimsel temellere dayanan 3 aylık kapsamlı gelişim planı.");

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 2,
                column: "ShortDescription",
                value: "Hedeflerinize ulaşmanız için özel olarak tasarlanmış, bilimsel temellere dayanan 6 aylık kapsamlı gelişim planı.");

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 3,
                column: "ShortDescription",
                value: "Hedeflerinize ulaşmanız için özel olarak tasarlanmış, bilimsel temellere dayanan 12 aylık kapsamlı gelişim planı.");

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 4,
                column: "ShortDescription",
                value: "Hedeflerinize ulaşmanız için özel olarak tasarlanmış, bilimsel temellere dayanan 3 aylık kapsamlı gelişim planı.");

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 5,
                column: "ShortDescription",
                value: "Hedeflerinize ulaşmanız için özel olarak tasarlanmış, bilimsel temellere dayanan 6 aylık kapsamlı gelişim planı.");

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 6,
                column: "ShortDescription",
                value: "Hedeflerinize ulaşmanız için özel olarak tasarlanmış, bilimsel temellere dayanan 12 aylık kapsamlı gelişim planı.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 1,
                column: "ShortDescription",
                value: "3 aylık program: antrenman, beslenme, takip ve koçluk modülleri.");

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 2,
                column: "ShortDescription",
                value: "6 aylık program: antrenman, beslenme, takip ve koçluk modülleri.");

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 3,
                column: "ShortDescription",
                value: "12 aylık program: antrenman, beslenme, takip ve koçluk modülleri.");

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 4,
                column: "ShortDescription",
                value: "Evde antrenmanla 3 aylık program: antrenman, beslenme, takip ve koçluk modülleri.");

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 5,
                column: "ShortDescription",
                value: "Evde antrenmanla 6 aylık program: antrenman, beslenme, takip ve koçluk modülleri.");

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 6,
                column: "ShortDescription",
                value: "Evde antrenmanla 12 aylık program: antrenman, beslenme, takip ve koçluk modülleri.");
        }
    }
}
