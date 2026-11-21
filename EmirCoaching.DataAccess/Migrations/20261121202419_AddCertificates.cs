using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmirCoaching.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCertificates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Issuer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IssuerFullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false),
                    LogoPath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Certificates",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Detail", "IsActive", "IsDeleted", "Issuer", "IssuerFullName", "LogoPath", "SortOrder", "Title", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, "TVGFBF", "Türkiye Vücut Geliştirme, Fitness ve Bilek Güreşi Federasyonu", "/images/sertifika-tvgfbf.png", 1, "2. Kademe Temel Antrenör Belgesi", null, 2021 },
                    { 2, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "EREPS Seviye 4", true, false, "FMI", "Fitness Movement Institute", "/images/sertifika-fmi.png", 2, "Nefes, Nörolojik Düzenleme ve Postür Eğitimi", null, 2022 },
                    { 3, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, "FMI", "Fitness Movement Institute", "/images/sertifika-fmi.png", 3, "Personal Trainer EQF Level 4", null, 2023 },
                    { 4, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, "Biopeak", "Biopeak", "/images/sertifika-biopeak.png", 4, "Shoulder Performance Specialist", null, 2024 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certificates");
        }
    }
}
