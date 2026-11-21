using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmirCoaching.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ReviseDietFormAndPackageHighlights : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Eski başvurular pakete bağlı değil, zorunlu PackageId'ye uyamaz; yalnızca test verisi vardı (kullanıcı onayı)
            migrationBuilder.Sql("DELETE FROM ConsentRecords WHERE DietFormSubmissionId IS NOT NULL;");
            migrationBuilder.Sql("DELETE FROM DietFormSubmissions;");

            migrationBuilder.DropColumn(
                name: "ActivityLevel",
                table: "DietFormSubmissions");

            migrationBuilder.DropColumn(
                name: "Allergies",
                table: "DietFormSubmissions");

            migrationBuilder.DropColumn(
                name: "AvoidedFoods",
                table: "DietFormSubmissions");

            migrationBuilder.DropColumn(
                name: "SleepHours",
                table: "DietFormSubmissions");

            migrationBuilder.DropColumn(
                name: "WaterLiters",
                table: "DietFormSubmissions");

            // EF bunları yeniden adlandırma sanıyordu (Supplements → ClientFormUrl, MealsPerDay → PackageId); ilgisiz kolonlar
            migrationBuilder.DropColumn(
                name: "Supplements",
                table: "DietFormSubmissions");

            migrationBuilder.DropColumn(
                name: "MealsPerDay",
                table: "DietFormSubmissions");

            migrationBuilder.AddColumn<int>(
                name: "PackageId",
                table: "DietFormSubmissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ClientFormUrl",
                table: "DietFormSubmissions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconKey",
                table: "PackageHighlights",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubText",
                table: "PackageHighlights",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "antrenman", null, "Kişiye Özel Antrenman" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "beslenme", null, "Kalori & Beslenme Planı" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "tahlil", null, "Kan Tahlili Analizi & Revize" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "takvim", null, "Haftalık Form Kontrolleri" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "whatsapp", null, "WhatsApp Birebir İletişim" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "IconKey", "SubText" },
                values: new object[] { "zoom", "Her Cumartesi · 20 dk birebir" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "antrenman", null, "Kişiye Özel Antrenman" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "beslenme", null, "Kalori & Beslenme Planı" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "tahlil", null, "Kan Tahlili Analizi & Revize" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "takvim", null, "Haftalık Form Kontrolleri" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "whatsapp", null, "WhatsApp Birebir İletişim" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "IconKey", "SubText" },
                values: new object[] { "zoom", "Her Cumartesi · 20 dk birebir" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "antrenman", null, "Kişiye Özel Antrenman" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "beslenme", null, "Kalori & Beslenme Planı" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "tahlil", null, "Kan Tahlili Analizi & Revize" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "takvim", null, "Haftalık Form Kontrolleri" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "whatsapp", null, "WhatsApp Birebir İletişim" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "IconKey", "SubText" },
                values: new object[] { "zoom", "Her Cumartesi · 20 dk birebir" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "antrenman", null, "Kişiye Özel Antrenman" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "beslenme", null, "Kalori & Beslenme Planı" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "tahlil", null, "Kan Tahlili Analizi & Revize" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "takvim", null, "Haftalık Form Kontrolleri" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "whatsapp", null, "WhatsApp Birebir İletişim" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "IconKey", "SubText" },
                values: new object[] { "zoom", "Her Cumartesi · 20 dk birebir" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "antrenman", null, "Kişiye Özel Antrenman" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "beslenme", null, "Kalori & Beslenme Planı" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "tahlil", null, "Kan Tahlili Analizi & Revize" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "takvim", null, "Haftalık Form Kontrolleri" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "whatsapp", null, "WhatsApp Birebir İletişim" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "IconKey", "SubText" },
                values: new object[] { "zoom", "Her Cumartesi · 20 dk birebir" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "antrenman", null, "Kişiye Özel Antrenman" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "beslenme", null, "Kalori & Beslenme Planı" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "tahlil", null, "Kan Tahlili Analizi & Revize" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "takvim", null, "Haftalık Form Kontrolleri" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "IconKey", "SubText", "Text" },
                values: new object[] { "whatsapp", null, "WhatsApp Birebir İletişim" });

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "IconKey", "SubText" },
                values: new object[] { "zoom", "Her Cumartesi · 20 dk birebir" });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 8999m);

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 15999m);

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 3,
                column: "Price",
                value: 27999m);

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 4,
                column: "Price",
                value: 5999m);

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 5,
                column: "Price",
                value: 10999m);

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 6,
                column: "Price",
                value: 18999m);

            migrationBuilder.CreateIndex(
                name: "IX_DietFormSubmissions_PackageId",
                table: "DietFormSubmissions",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_DietFormSubmissions_Packages_PackageId",
                table: "DietFormSubmissions",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DietFormSubmissions_Packages_PackageId",
                table: "DietFormSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_DietFormSubmissions_PackageId",
                table: "DietFormSubmissions");

            migrationBuilder.DropColumn(
                name: "IconKey",
                table: "PackageHighlights");

            migrationBuilder.DropColumn(
                name: "SubText",
                table: "PackageHighlights");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "DietFormSubmissions");

            migrationBuilder.DropColumn(
                name: "ClientFormUrl",
                table: "DietFormSubmissions");

            migrationBuilder.AddColumn<int>(
                name: "MealsPerDay",
                table: "DietFormSubmissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Supplements",
                table: "DietFormSubmissions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActivityLevel",
                table: "DietFormSubmissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Allergies",
                table: "DietFormSubmissions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvoidedFoods",
                table: "DietFormSubmissions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SleepHours",
                table: "DietFormSubmissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WaterLiters",
                table: "DietFormSubmissions",
                type: "decimal(4,1)",
                precision: 4,
                scale: 1,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 1,
                column: "Text",
                value: "Kişiye özel antrenman planlaması");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 2,
                column: "Text",
                value: "Kalorisi hesaplanmış beslenme planı");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 3,
                column: "Text",
                value: "Kan tahlili analizi & revize");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 4,
                column: "Text",
                value: "Haftalık durum değerlendirmesi");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 5,
                column: "Text",
                value: "Birebir WhatsApp iletişimi");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 7,
                column: "Text",
                value: "Kişiye özel antrenman planlaması");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 8,
                column: "Text",
                value: "Kalorisi hesaplanmış beslenme planı");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 9,
                column: "Text",
                value: "Kan tahlili analizi & revize");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 10,
                column: "Text",
                value: "Haftalık durum değerlendirmesi");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 11,
                column: "Text",
                value: "Birebir WhatsApp iletişimi");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 13,
                column: "Text",
                value: "Kişiye özel antrenman planlaması");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 14,
                column: "Text",
                value: "Kalorisi hesaplanmış beslenme planı");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 15,
                column: "Text",
                value: "Kan tahlili analizi & revize");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 16,
                column: "Text",
                value: "Haftalık durum değerlendirmesi");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 17,
                column: "Text",
                value: "Birebir WhatsApp iletişimi");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 19,
                column: "Text",
                value: "Kişiye özel antrenman planlaması");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 20,
                column: "Text",
                value: "Kalorisi hesaplanmış beslenme planı");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 21,
                column: "Text",
                value: "Kan tahlili analizi & revize");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 22,
                column: "Text",
                value: "Haftalık durum değerlendirmesi");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 23,
                column: "Text",
                value: "Birebir WhatsApp iletişimi");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 25,
                column: "Text",
                value: "Kişiye özel antrenman planlaması");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 26,
                column: "Text",
                value: "Kalorisi hesaplanmış beslenme planı");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 27,
                column: "Text",
                value: "Kan tahlili analizi & revize");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 28,
                column: "Text",
                value: "Haftalık durum değerlendirmesi");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 29,
                column: "Text",
                value: "Birebir WhatsApp iletişimi");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 31,
                column: "Text",
                value: "Kişiye özel antrenman planlaması");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 32,
                column: "Text",
                value: "Kalorisi hesaplanmış beslenme planı");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 33,
                column: "Text",
                value: "Kan tahlili analizi & revize");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 34,
                column: "Text",
                value: "Haftalık durum değerlendirmesi");

            migrationBuilder.UpdateData(
                table: "PackageHighlights",
                keyColumn: "Id",
                keyValue: 35,
                column: "Text",
                value: "Birebir WhatsApp iletişimi");

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 3450m);

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 8250m);

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 3,
                column: "Price",
                value: 14900m);

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 4,
                column: "Price",
                value: 3450m);

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 5,
                column: "Price",
                value: 8250m);

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 6,
                column: "Price",
                value: 14900m);
        }
    }
}
