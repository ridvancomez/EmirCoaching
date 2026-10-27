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
    private const string ShopierPlaceholder = "https://www.shopier.com/"; // gerçek ürün linkleri panelden girilecek

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

    // Front-end paketler.html kart listesi (.paketler-card__features); Zoom satırı 3 aylıkta kilitli
    private static readonly string[] Highlights =
    [
        "Kişiye özel antrenman planlaması", "Kalorisi hesaplanmış beslenme planı", "Kan tahlili analizi & revize",
        "Haftalık durum değerlendirmesi", "Birebir WhatsApp iletişimi", ZoomItemText
    ];

    public static void Seed(ModelBuilder builder)
    {
        SeedPackages(builder);
        SeedBlogCategories(builder);
        SeedTransformationTypes(builder);
        SeedLegalDocuments(builder);
        SeedSiteSetting(builder);
        SeedSocialLinks(builder);
        SeedPageContents(builder);
        SeedDietProgram(builder);
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
                    PurchaseUrl = ShopierPlaceholder,
                    IsFeatured = p.Months == 6, // "En Çok Tercih Edilen": her kategoride Dönüşüm Protokolü
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

                        items.Add(new PackageModuleItem
                        {
                            Id = ++itemId,
                            ModuleId = moduleId,
                            Text = text,
                            IsLocked = IsZoomLocked(text, p.Months),
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
                        IsLocked = IsZoomLocked(Highlights[h], p.Months),
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

    // Zoom görüşmesi yalnızca 6 ve 12 aylık paketlerde var
    private static bool IsZoomLocked(string text, int months) => text == ZoomItemText && months < 6;

    // Front-end degisimler.html kart etiketleri
    private static void SeedTransformationTypes(ModelBuilder builder)
    {
        builder.Entity<TransformationType>().HasData(
            new TransformationType { Id = 1, Name = "Yağ Kaybı", Slug = "yag-kaybi", SortOrder = 1, CreatedAt = SeedDate },
            new TransformationType { Id = 2, Name = "Kas Kazanımı", Slug = "kas-kazanimi", SortOrder = 2, CreatedAt = SeedDate },
            new TransformationType
            {
                Id = 3, Name = "Kondisyon & Yağ Kaybı", Slug = "kondisyon-yag-kaybi", SortOrder = 3, CreatedAt = SeedDate
            });
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
        const string initialNote = "İlk yayın.";

        LegalDocument Pdf(int id, LegalDocumentType type, string title, string file) => new()
        {
            Id = id, Type = type, Title = title, Version = 1, Note = initialNote, FilePath = $"/yasal-metinler/{file}",
            PublishedAt = SeedDate, IsCurrent = true, CreatedAt = SeedDate
        };

        var documents = new[]
        {
            Pdf(1, LegalDocumentType.Kvkk, "KVKK Aydınlatma Metni", "kvkk-aydinlatma-metni.pdf"),
            Pdf(2, LegalDocumentType.AcikRiza, "Açık Rıza Metni", "acik-riza-metni.pdf"),
            Pdf(3, LegalDocumentType.Cerez, "Çerez Politikası", "cerez-politikasi.pdf"),
            Pdf(4, LegalDocumentType.IptalIade, "İptal ve İade Koşulları", "iptal-ve-iade-kosullari.pdf"),
            new LegalDocument
            {
                Id = 5, Type = LegalDocumentType.Gizlilik, Title = "Gizlilik Politikası", Version = 1, Note = initialNote,
                ContentHtml = GizlilikPolitikasiContent.Html,
                PublishedAt = SeedDate, IsCurrent = true, CreatedAt = SeedDate
            },
            Pdf(6, LegalDocumentType.TicariIleti, "Ticari Elektronik İleti Onay Metni",
                "ticari-elektronik-ileti-onay-metni.pdf")
        };

        builder.Entity<LegalDocument>().HasData(documents);

        // Servisteki kuralın seed karşılığı: her sürüm revizyon geçmişine de yazılır
        builder.Entity<LegalDocumentRevision>().HasData(documents.Select(d => new LegalDocumentRevision
        {
            Id = d.Id, LegalDocumentId = d.Id, Type = d.Type, Version = d.Version, Note = d.Note,
            RevisedAt = d.PublishedAt, CreatedAt = SeedDate
        }));
    }

    // Değerler front-end'den alındı; e-posta ve adres geçicidir, panelden güncellenecek.
    private static void SeedSiteSetting(ModelBuilder builder)
    {
        builder.Entity<SiteSetting>().HasData(new SiteSetting
        {
            Id = 1,
            Phone = "+90 545 371 25 15",
            Email = "info@emircoaching.com",
            LegalEmail = "legal@emircoaching.com",
            Address = "Levent Mah. Büyükdere Cad. No:199, Şişli / İstanbul",
            WorkingDays1 = "Pazartesi - Cumartesi",
            WorkingHours1 = "08:00 - 22:00",
            WorkingDays2 = "Pazar",
            WorkingHours2 = "10:00 - 20:00",
            CreatedAt = SeedDate
        });
    }

    // Front-end iletisim.html sosyal kartları; Instagram/YouTube/Facebook adresleri geçicidir
    private static void SeedSocialLinks(ModelBuilder builder)
    {
        SocialLink Link(int id, SocialPlatform platform, string url, string description) => new()
        {
            Id = id, Platform = platform, Url = url, Description = description, SortOrder = id, IsActive = true,
            CreatedAt = SeedDate
        };

        builder.Entity<SocialLink>().HasData(
            Link(1, SocialPlatform.WhatsApp, "https://wa.me/905453712515",
                "Hızlı destek ve program danışmanlığı için anında ulaş."),
            Link(2, SocialPlatform.Instagram, "https://instagram.com/", "Günlük motivasyon ve öğrenci değişimleri."),
            Link(3, SocialPlatform.YouTube, "https://youtube.com/", "Antrenman teknikleri ve beslenme tüyoları."),
            Link(4, SocialPlatform.Facebook, "https://facebook.com/", "Topluluğumuza katıl ve güncel duyuruları takip et."));
    }

    // Sayfa hero/breadcrumb içerikleri front-end'den alındı; lorem ipsum açıklamalar panelden değiştirilecek.
    private static void SeedPageContents(ModelBuilder builder)
    {
        const string lorem = "Ya da ona benzer bir şey. Bedenin yoldaşları rahattır, ancak seçkin değildirler. " +
                             "Ve nibh, o alacalı olan, onun giriş bölümü, hayattır; böylece bir şey olabilir.";
        const string kanit = "Herkesin başlangıç noktası farklıdır. Önemli olan, hedefe giden yolda gösterilen " +
                             "kararlılık ve doğru stratejidir. İşte kanıtı.";
        const string heroBg = "/images/paketlerslider.png";

        var id = 0;
        PageContent Page(string key, string title, string? highlight, string? description,
            string? background, string? buttonUrl = null) => new()
        {
            Id = ++id, PageKey = key, Title = title, HighlightText = highlight, Description = description,
            BackgroundImagePath = background, ButtonUrl = buttonUrl, CreatedAt = SeedDate
        };

        builder.Entity<PageContent>().HasData(
            Page("home", "Kendini, bahanelerinden daha GÜÇLÜ hale getir", "GÜÇLÜ", lorem, "/images/slider.png",
                "/paketler.html"),
            Page("paketler", "SANA UYGUN PAKETİ SEÇ, DÖNÜŞÜMÜ BAŞLAT", "DÖNÜŞÜMÜ", lorem, heroBg),
            Page("paket-detay", "Detaylı İnceleme", null,
                "Hedeflerinize ulaşmanız için özel olarak tasarlanmış, bilimsel temellere dayanan kapsamlı gelişim planı.",
                heroBg),
            Page("blog", "Blog", null, null, heroBg),
            Page("degisimler", "Rakamlar Değil, Gerçek İnsanlar", "Gerçek İnsanlar", kanit, heroBg),
            Page("hakkimda", "BİR SALONDAN, BİNLERCE DÖNÜŞÜME", "BİNLERCE",
                "Gerçek değişim geçici motivasyonlarla değil, sürdürülebilir sistemlerle başlar. Emircoaching olarak, " +
                "fiziksel potansiyelinizi ortaya çıkarmak için bilimsel temelli bir yaklaşım sunuyoruz.", heroBg),
            Page("iletisim", "Değişime İlk Adımı Atma Zamanı", "Zamanı", kanit, heroBg),
            Page("diyet-formu", "UZMANLARLA ÇALIŞMAK PROFESYONELLİK GEREKTİRİR", "PROFESYONELLİK", lorem, heroBg),
            Page("diyet-plani-detay", "UZMANLARLA ÇALIŞMAK PROFESYONELLİK GEREKTİRİR", "PROFESYONELLİK", lorem, heroBg),
            Page("gizlilik-politikasi", "Gizlilik Politikası", null,
                "Bu politika, EMIRCOACHING platformunu kullanan bireylerin kişisel verilerinin toplanması, işlenmesi, " +
                "saklanması ve korunmasına ilişkin temel ilkeleri ve yasal yükümlülükleri düzenler.", null));
    }

    // Front-end diyet-plani-detay.html
    private static void SeedDietProgram(ModelBuilder builder)
    {
        builder.Entity<DietProgram>().HasData(new DietProgram
        {
            Id = 1,
            Title = "Kişiye Özel Diyet Planı",
            Price = 1950m,
            DurationWeeks = 4,
            DietitianTitle = "Uzm. Dr.",
            DietitianFullName = "Ceyda Güler",
            DietitianPhotoPath = "/images/uzman-ceyda.jpg",
            IsActive = true,
            CreatedAt = SeedDate
        });

        builder.Entity<DietProgramStep>().HasData(
            new DietProgramStep
            {
                Id = 1, SortOrder = 1, Icon = DietProgramStepIcon.FormuDoldur, Title = "Formu Doldur",
                Description = "Hedeflerini ve beslenme alışkanlıklarını içeren detaylı formu doldur.", CreatedAt = SeedDate
            },
            new DietProgramStep
            {
                Id = 2, SortOrder = 2, Icon = DietProgramStepIcon.PlanHazirlanir, Title = "Planın Hazırlanır",
                Description = "Uzman koçumuz verilerini analiz ederek sana özel planını 24 saat içinde hazırlar.",
                CreatedAt = SeedDate
            },
            new DietProgramStep
            {
                Id = 3, SortOrder = 3, Icon = DietProgramStepIcon.WhatsApp, Title = "WhatsApp'tan Ulaşılır",
                Description = "Planın sana WhatsApp üzerinden iletilir ve destek sürecimiz başlar.", CreatedAt = SeedDate
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
