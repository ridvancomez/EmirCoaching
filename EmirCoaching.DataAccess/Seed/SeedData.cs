using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.DataAccess.Seed;

public static class SeedData
{
    public const int AdminRoleId = 1;
    public const string AdminRoleName = "Admin";

    // HasData değerleri sabit olmalı; DateTime.UtcNow her migration'da fark üretir.
    private static readonly DateTime SeedDate = new(2026, 10, 23, 0, 0, 0, DateTimeKind.Utc);

    private const string ZoomItemText = "Yüz Yüze Zoom Görüşmesi";
    private const string ZoomLockedLabel = "6 ve 12 Aylık Pakette";

    // Front-end paket-detay.html modül kartları (toplam 15 kalem; Zoom satırı pakete göre değişir)
    private static readonly (string Title, string IconKey, string[] Items)[] Modules =
    [
        ("Antrenman", "antrenman",
            ["Kişiye Özel Antrenman Planlaması", "Haftalık Program Güncellemesi", "Hareket Tekniği Videoları", "Sakatlık Uyarlaması"]),
        ("Beslenme", "beslenme",
            ["Kalorisi Hesaplanmış Beslenme Planı", "Öğün Şablonları", "Dışarıda Yeme Rehberi", "Supplement Planlaması"]),
        ("Takip & Ölçüm", "takip-olcum",
            ["Haftalık Durum Değerlendirmesi", "Vücut Kompozisyonu Takibi", "Aylık Detaylı Rapor"]),
        ("Koçluk", "kocluk",
            ["Birebir WhatsApp İletişimi", "Anlık Program Revizeleri", "Kan Tahlili Analizi & Revize", ZoomItemText])
    ];

    // Front-end fiyat kartı (.fiyat-karti__list)
    private static readonly string[] Highlights =
        ["Kişiye Özel Planlama", "Haftalık Düzenli Takip", "7/24 WhatsApp Desteği", "Detaylı Gelişim Raporları"];

    public static void Seed(ModelBuilder builder)
    {
        SeedPackages(builder);
        SeedBlogCategories(builder);
        SeedLegalDocuments(builder);
        SeedSiteSetting(builder);
        SeedRoles(builder);
    }

    private static void SeedPackages(ModelBuilder builder)
    {
        var basePackages = new[]
        {
            new { Name = "Başlangıç & Deneme", Slug = "baslangic-deneme", Months = 3, Price = 3450m,
                  Subtitle = "3 Aylık Başlangıç Programı" },
            new { Name = "Dönüşüm Protokolü", Slug = "donusum-protokolu", Months = 6, Price = 8250m,
                  Subtitle = "6 Aylık Dönüşüm Programı" },
            new { Name = "VIP Yaşam Tarzı", Slug = "vip-yasam-tarzi", Months = 12, Price = 14900m,
                  Subtitle = "12 Aylık Yaşam Tarzı Programı" }
        };

        var packages = new List<Package>();
        var modules = new List<PackageModule>();
        var items = new List<PackageModuleItem>();
        var highlights = new List<PackageHighlight>();
        int packageId = 0, moduleId = 0, itemId = 0, highlightId = 0;

        foreach (var location in new[] { PackageLocation.Salonda, PackageLocation.Evde })
        {
            for (var i = 0; i < basePackages.Length; i++)
            {
                var p = basePackages[i];
                var isEvde = location == PackageLocation.Evde;
                var place = isEvde ? "evde antrenmanla " : "";

                packages.Add(new Package
                {
                    Id = ++packageId,
                    Name = p.Name,
                    Slug = isEvde ? $"{p.Slug}-evde" : p.Slug,
                    Subtitle = p.Subtitle,
                    DurationMonths = p.Months,
                    Price = p.Price,
                    Location = location,
                    ShortDescription = $"{(isEvde ? "Evde antrenmanla " : "")}{p.Months} aylık program: " +
                                       "antrenman, beslenme, takip ve koçluk modülleri.",
                    SortOrder = i + 1,
                    IsActive = true,
                    MetaTitle = $"{p.Name}{(isEvde ? " (Evde)" : "")} Paket Detayı | Emir Coaching",
                    MetaDescription = $"Emir Coaching {p.Name}{(isEvde ? " (Evde)" : "")}: {place}{p.Months} aylık program. " +
                                      "Antrenman, beslenme, takip ve koçluk modülleriyle toplam 15 kalem içerik.",
                    CreatedAt = SeedDate
                });

                for (var m = 0; m < Modules.Length; m++)
                {
                    var module = Modules[m];
                    modules.Add(new PackageModule
                    {
                        Id = ++moduleId,
                        PackageId = packageId,
                        Title = module.Title,
                        IconKey = module.IconKey,
                        SortOrder = m + 1,
                        CreatedAt = SeedDate
                    });

                    for (var t = 0; t < module.Items.Length; t++)
                    {
                        var text = module.Items[t];
                        var isZoom = text == ZoomItemText;
                        var isLocked = isZoom && p.Months < 6;

                        items.Add(new PackageModuleItem
                        {
                            Id = ++itemId,
                            ModuleId = moduleId,
                            Text = text,
                            State = !isZoom ? ModuleItemState.Standart
                                : isLocked ? ModuleItemState.Kilitli
                                : ModuleItemState.Ozel,
                            LockedLabel = isLocked ? ZoomLockedLabel : null,
                            SortOrder = t + 1,
                            CreatedAt = SeedDate
                        });
                    }
                }

                for (var h = 0; h < Highlights.Length; h++)
                {
                    highlights.Add(new PackageHighlight
                    {
                        Id = ++highlightId,
                        PackageId = packageId,
                        Text = Highlights[h],
                        SortOrder = h + 1,
                        CreatedAt = SeedDate
                    });
                }
            }
        }

        builder.Entity<Package>().HasData(packages);
        builder.Entity<PackageModule>().HasData(modules);
        builder.Entity<PackageModuleItem>().HasData(items);
        builder.Entity<PackageHighlight>().HasData(highlights);
    }

    // Front-end blog.html data-category değerleri
    private static void SeedBlogCategories(ModelBuilder builder)
    {
        builder.Entity<BlogCategory>().HasData(
            new BlogCategory { Id = 1, Name = "Beslenme", Slug = "beslenme", CreatedAt = SeedDate },
            new BlogCategory { Id = 2, Name = "Hipertrofi", Slug = "hipertrofi", CreatedAt = SeedDate },
            new BlogCategory { Id = 3, Name = "Kuvvet", Slug = "kuvvet", CreatedAt = SeedDate },
            new BlogCategory { Id = 4, Name = "Mentalite", Slug = "mentalite", CreatedAt = SeedDate },
            new BlogCategory { Id = 5, Name = "Toparlanma", Slug = "toparlanma", CreatedAt = SeedDate });
    }

    private static void SeedLegalDocuments(ModelBuilder builder)
    {
        LegalDocument Pdf(int id, LegalDocumentType type, string title, string file) => new()
        {
            Id = id, Type = type, Title = title, Version = 1, FilePath = $"/yasal-metinler/{file}",
            PublishedAt = SeedDate, IsCurrent = true, CreatedAt = SeedDate
        };

        builder.Entity<LegalDocument>().HasData(
            Pdf(1, LegalDocumentType.Kvkk, "KVKK Aydınlatma Metni", "kvkk-aydinlatma-metni.pdf"),
            Pdf(2, LegalDocumentType.AcikRiza, "Açık Rıza Metni", "acik-riza-metni.pdf"),
            Pdf(3, LegalDocumentType.Cerez, "Çerez Politikası", "cerez-politikasi.pdf"),
            Pdf(4, LegalDocumentType.IptalIade, "İptal ve İade Koşulları", "iptal-ve-iade-kosullari.pdf"),
            new LegalDocument
            {
                Id = 5, Type = LegalDocumentType.Gizlilik, Title = "Gizlilik Politikası", Version = 1,
                ContentHtml = GizlilikPolitikasiContent.Html,
                PublishedAt = SeedDate, IsCurrent = true, CreatedAt = SeedDate
            });
    }

    // Değerler front-end'den alındı; e-posta ve adres geçicidir, panelden güncellenecek.
    private static void SeedSiteSetting(ModelBuilder builder)
    {
        builder.Entity<SiteSetting>().HasData(new SiteSetting
        {
            Id = 1,
            WhatsApp = "905453712515",
            Phone = "+90 545 371 25 15",
            Email = "info@emircoaching.com",
            Address = "Levent Mah. Büyükdere Cad. No:199, Şişli / İstanbul",
            Instagram = "https://instagram.com/",
            WorkingHours = "Pazartesi - Cumartesi: 08:00 - 22:00 | Pazar: 10:00 - 20:00",
            CreatedAt = SeedDate
        });
    }

    private static void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole<int>>().HasData(new IdentityRole<int>
        {
            Id = AdminRoleId,
            Name = AdminRoleName,
            NormalizedName = AdminRoleName.ToUpperInvariant(),
            ConcurrencyStamp = "5c1f6b0e-2f4e-4c55-9a51-3b7c7d0b6a11"
        });
    }

    // Admin kullanıcısı HasData ile değil, çalışma anında yapılandırmadan oluşturulur:
    // parola hash'i kaynak koda ve migration'a girmesin diye.
    // Gerekli ayarlar (user-secrets): AdminUser:Email, AdminUser:Password, AdminUser:FullName
    public static async Task SeedAdminUserAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(typeof(SeedData));
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        var email = configuration["AdminUser:Email"];
        var password = configuration["AdminUser:Password"];
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning("AdminUser:Email / AdminUser:Password tanımlı değil; admin kullanıcısı oluşturulmadı.");
            return;
        }

        if (await userManager.FindByEmailAsync(email) is not null)
            return;

        var admin = new AppUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            FullName = configuration["AdminUser:FullName"] ?? "Admin"
        };

        var result = await userManager.CreateAsync(admin, password);
        if (!result.Succeeded)
        {
            logger.LogError("Admin kullanıcısı oluşturulamadı: {Errors}",
                string.Join(", ", result.Errors.Select(e => e.Description)));
            return;
        }

        await userManager.AddToRoleAsync(admin, AdminRoleName);
        logger.LogInformation("Admin kullanıcısı oluşturuldu: {Email}", email);
    }
}
