using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmirCoaching.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MergeCoachTitleAndSeedFaqs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Koç bölümü artık kullanıcının kendi unvanını kullanır; boşsa varsayılan verilir
            migrationBuilder.Sql("UPDATE AspNetUsers SET Title = N'Baş Antrenör' WHERE IsCoach = 1 AND (Title IS NULL OR Title = N'');");

            migrationBuilder.DropColumn(
                name: "CoachTitle",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "Faqs",
                columns: new[] { "Id", "Answer", "CreatedAt", "DeletedAt", "Group", "IsDeleted", "Question", "SortOrder", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Evet, programın temel taşı kan tahlilinizdir. Tahlil sonuçlarınız detaylıca analiz edilerek hormon, vitamin ve mineral seviyelerinize tam uyumlu kişiselleştirilmiş bir beslenme ve takviye protokolü hazırlanır.", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, false, "Kan tahlili yaptırmak zorunlu mu, süreç nasıl işliyor?", 1, null },
                    { 2, "Kesinlikle. Evde sahip olduğunuz ekipmanlara (dumbbell, direnç bandı vb.) veya tamamen kendi vücut ağırlığınıza özel antrenman programı hazırlanır. Doğru mekanik ve tükeniş prensibiyle evde de maksimum gelişim sağlanır.", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, false, "Spor salonuna gitmiyorum, evde antrenman paketi yeterli verimi sağlar mı?", 2, null },
                    { 3, "Asla. Bütçenize, günlük yaşam akışınıza, sevdiğiniz/sevemediğiniz gıdalara ve tahlil sonuçlarınıza özel kalori ve makro hesaplaması yapılarak tamamen sürdürülebilir bir liste oluşturulur.", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, false, "Hazırlanan beslenme programı matbu (standart) bir liste mi olacak?", 3, null },
                    { 4, "Haftalık düzenli form kontrolleri (tartı ve fotoğraf analizi) ile gelişiminiz takip edilir. Ayrıca WhatsApp üzerinden bizzat birebir iletişim kurarak sorularınızı anında iletebilirsiniz.", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, false, "Süreç boyunca iletişim ve takip nasıl yapılıyor?", 4, null },
                    { 5, "Gelişiminize ve haftalık form kontrollerinize göre ihtiyaç duyulan anlarda programınız anında revize edilir. Vücudunuzun plato evresine girmesine izin verilmeden sürekli gelişim hedeflenir.", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, false, "Antrenman veya beslenme programım ne sıklıkla güncelleniyor?", 5, null },
                    { 6, "6 ve 12 Aylık paket alan danışanlarıma özel olarak süreç boyunca her Cumartesi 20 dakikalık birebir Zoom görüşmesi yapılır. Bu görüşmelerde haftalık gelişim analizi, antrenman/beslenme revizyonları ve zihinsel motivasyon takibi yüz yüze gerçekleştirilir.", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, false, "6 ve 12 Aylık paketlerdeki VIP Zoom görüşmesi nedir?", 6, null },
                    { 7, "Ödeme tamamlandıktan sonra tarafınıza detaylı danışan bilgi formu ve tahlil rehberi iletilir. Form ve tahliller tamamlanıp bana ulaştıktan sonraki 48 saat içinde programınız hazırlanır ve süreciniz başlar.", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, false, "Paketi satın aldıktan sonra süreç ne zaman başlar?", 7, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Faqs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Faqs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Faqs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Faqs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Faqs",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Faqs",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Faqs",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.AddColumn<string>(
                name: "CoachTitle",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
