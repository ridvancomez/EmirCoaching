using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmirCoaching.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseSteps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    TimeLabel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IconKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseSteps", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PurchaseSteps",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Description", "IconKey", "IsDeleted", "SortOrder", "TimeLabel", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ödemeden sonra koçun WhatsApp'tan danışan bilgi formunu ve tahlil rehberini gönderir. Formu doldurur, tahlillerini iletirsin.", "form", false, 1, "Gün 1", "Form + Kan Tahlili", null },
                    { 2, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tahlil sonuçların ve hedefine göre kişiye özel antrenman ve beslenme programın hazırlanır.", "saat", false, 2, "En geç 48 saat", "48 Saatte Program", null },
                    { 3, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "Haftalık form kontrolleri (tartı + fotoğraf) ve WhatsApp üzerinden birebir iletişim.", "takvim", false, 3, "Her hafta", "Haftalık Takip", null },
                    { 4, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "Gelişimine göre program anında güncellenir; plato evresine girmene izin verilmez.", "revize", false, 4, "İhtiyaç oldukça", "Revize", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseSteps");
        }
    }
}
