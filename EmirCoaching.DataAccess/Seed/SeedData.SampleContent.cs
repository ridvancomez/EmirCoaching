using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Seed;

// Front-end'deki (EmirCoachingFront) tasarım örnek içerikleri: yorumlar, değişimler, blog yazıları, sayaçlar,
// sosyal gönderiler. Panel ilk açılışta sitenin aynısını göstersin diye birebir alındı; müşteri gerçekleriyle değiştirir.
public static partial class SeedData
{
    // Paket seed sırası: 1–3 salonda (Başlangıç, Dönüşüm, VIP), 4–6 evde
    private const int DonusumPackageId = 2;
    private const int PackageCount = 6;

    private static readonly DateTime SamplePostedAt = new(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc);

    private static void SeedSampleContent(ModelBuilder builder)
    {
        SeedTestimonials(builder);
        SeedTransformations(builder);
        SeedBlogPosts(builder);
        SeedSiteStats(builder);
        SeedSocialPosts(builder);
    }

    // index.html "reviews" (ana sayfa) + paket-detay.html "yorum-card" (her paket detayında aynı üç yorum)
    private static void SeedTestimonials(ModelBuilder builder)
    {
        var id = 0;
        Testimonial Review(string name, string text, int packageId, int sortOrder, bool showOnHome) => new()
        {
            Id = ++id, Name = name, Text = text, Rating = 5, PackageId = packageId, SortOrder = sortOrder,
            IsActive = true, ShowOnHome = showOnHome, CreatedAt = SeedDate
        };

        var testimonials = new List<Testimonial>
        {
            Review("A.K.", "İlk ayda 6 kilo verdim ama asıl kazancım, neyi neden yaptığımı öğrenmek oldu.",
                DonusumPackageId, 1, showOnHome: true),
            Review("M.Y.", "Programı hayatıma göre kurdu; ilk kez bir beslenme planını üç ay boyunca bırakmadan sürdürebildim.", 1, 2,
                showOnHome: true),
            Review("E.D.", "Her hafta form kontrolü olunca bırakmak aklıma bile gelmedi. Bir yıl sonra bambaşka biriyim.",
                3, 3, showOnHome: true)
        };

        (string Name, string Text)[] packageReviews =
        [
            ("Ahmet Y.", "Dönüşüm Protokolü ile hayatım değişti. Beslenme ve antrenman konusunda öğrendiklerim paha " +
                         "biçilemez. İletişim kalitesi mükemmel."),
            ("Caner K.", "Daha önce birçok program denedim ama hiçbiri bu kadar detaylı ve takip odaklı değildi. " +
                         "Hedefime ulaştım ve korumaya devam ediyorum."),
            ("Burak M.", "Sakatlık geçmişim olmasına rağmen bana özel hazırlanan programla hiçbir sorun yaşamadım. " +
                         "Gerçekten kişiye özel bir hizmet.")
        ];

        for (var packageId = 1; packageId <= PackageCount; packageId++)
        {
            for (var r = 0; r < packageReviews.Length; r++)
                testimonials.Add(Review(packageReviews[r].Name, packageReviews[r].Text, packageId, r + 4, showOnHome: false));
        }

        builder.Entity<Testimonial>().HasData(testimonials);
    }

    // degisimler.html: 7 kaydırmalı kart (Carousel) + 4 önce/sonra galerisi (Galeri)
    private static void SeedTransformations(ModelBuilder builder)
    {
        const int yagKaybi = 1, kasKazanimi = 2, kondisyon = 3, formKoruma = 4;
        const string set1 = "/images/hakkimda-degisim-1-", set2 = "/images/hakkimda-degisim-2-";

        var mert = new Sample("Mert", 28, 12, yagKaybi, set1, AlignBottom: false,
            "Sadece fiziksel değil, mental olarak da büyük bir değişim yaşadım. Disiplin kelimesinin anlamını öğrendim.",
            [
                "Programa başladığımda 98 kiloydum ve merdiven çıkarken bile nefes nefese kalıyordum. Yıllarca farklı diyetler denemiş, her seferinde birkaç hafta sonra bırakmıştım.",
                "İlk haftalarda en zorlandığım şey düzen oldu: öğünleri tartmak, antrenmanları kaçırmamak, uykuyu ciddiye almak. Haftalık kontrollerde planım küçük adımlarla güncellendi; hiçbir zaman aç kalmadım.",
                "12 haftanın sonunda 12 kilo verdim, yağ oranım %8 düştü ve kas kütlemi korudum. Ama asıl kazancım disiplin oldu: artık antrenmana motivasyon beklemeden gidiyorum."
            ], ["-12 KG", "-%8", "+2 KG"]);
        var can = new Sample("Can", 32, 16, kasKazanimi, set1, AlignBottom: false,
            "Kilo alamıyordum, yıllardır aynı formdaydım. Doğru yükleme ve beslenme planı ile tabularımı yıktım.",
            [
                "Yıllardır spor salonuna gidiyordum ama tartıdaki rakam hiç değişmiyordu. Ne kadar yesem de kilo alamadığımı düşünüyordum; aslında ne yediğimi hiç ölçmemiştim.",
                "Koçumla önce gerçek kalori ihtiyacımı hesapladık. Antrenmanlarımı ilerleyici yüklemeye göre yeniden kurduk ve her hafta ağırlıkları kayıt altına aldık.",
                "16 haftada 8 kilo aldım ve bunun 6.5 kilosu kas kütlesiydi; yağ oranım sabit kaldı. Aynada gördüğüm değişimden daha önemlisi, kendime olan güvenimin geri gelmesi oldu."
            ], ["+8 KG", "Sabit", "+6.5 KG"]);
        var elif = new Sample("Elif", 26, 20, kondisyon, set2, AlignBottom: true,
            "Sürekli diyet yapıp bırakıyordum. Sürdürülebilir bir sistemin ne demek olduğunu burada anladım.",
            [
                "Sürekli diyet yapıp bırakıyordum. Her yeni programın ilk haftasında çok heyecanlı olur, sonra kısıtlamalardan sıkılıp eski alışkanlıklarıma dönerdim.",
                "Bu kez yasaklar yerine sürdürülebilir bir sistem kurduk: sevdiğim yemekler planın içindeydi, antrenmanlarım kondisyonumu adım adım artıracak şekilde ilerledi.",
                "20 haftanın sonunda 8 kilo verdim, yağ oranım %6 azaldı ve 1 kilo kas kazandım. Artık merdivenleri koşarak çıkıyorum ve bir diyete değil, bir yaşam tarzına bağlıyım."
            ], ["-8 KG", "-%6", "+1 KG"]);

        // Galeri kayıtlarının sitede alıntısı yok; zorunlu alan olduğu için hikâyenin ilk cümlesi kullanıldı
        Sample[] galeri =
        [
            new("Zeynep", 29, 16, yagKaybi, set2, AlignBottom: false,
                "Doğumdan sonra eski formuma dönemeyeceğimi düşünüyordum.",
                [
                    "Doğumdan sonra eski formuma dönemeyeceğimi düşünüyordum. Gün içinde vakit bulmak zordu, bu yüzden antrenmanlarım evde 40 dakikaya sığacak şekilde planlandı.",
                    "Beslenmede yasaklar yerine porsiyon ve öğün düzeni üzerinde çalıştık. Haftalık kontrollerde ölçülerim düzenli takip edildi, takıldığım haftalarda plan küçük dokunuşlarla güncellendi.",
                    "16 haftanın sonunda 9 kilo verdim ve yağ oranım %7 düştü. En önemlisi, enerjim geri geldi ve bu düzeni artık zorlanmadan sürdürebiliyorum."
                ], ["-9 KG", "-%7", "+1 KG"]),
            new("Ayşe", 34, 12, formKoruma, set2, AlignBottom: false,
                "Kilom normaldi ama vücudum sıkı değildi; yıllardır sadece kardiyo yapıyordum.",
                [
                    "Kilom normaldi ama vücudum sıkı değildi; yıllardır sadece kardiyo yapıyordum. Hedefim tartıdaki sayıdan çok aynadaki görüntüydü.",
                    "Programa ağırlık antrenmanı ve yeterli protein ekledik. İlk haftalarda tartı neredeyse hiç değişmedi, ama mezura ölçüleri her hafta biraz daha azaldı.",
                    "12 haftada 3 kilo verip 2 kilo kas kazandım. Kıyafetlerim artık farklı oturuyor ve kendimi hiç olmadığım kadar güçlü hissediyorum."
                ], ["-3 KG", "-%4", "+2 KG"]),
            new("Selin", 27, 20, yagKaybi, set2, AlignBottom: false,
                "Ofiste uzun saatler oturmak ve düzensiz yemek yüzünden birkaç yılda ciddi kilo almıştım.",
                [
                    "Ofiste uzun saatler oturmak ve düzensiz yemek yüzünden birkaç yılda ciddi kilo almıştım. Daha önce denediğim sert diyetlerin hepsi kısa sürede bitmişti.",
                    "Bu sefer acele etmedik: kalori açığı küçük tutuldu, adım hedefi ve haftada üç antrenman eklendi. Zor haftalarda koçumla konuşup planı birlikte ayarladık.",
                    "20 haftanın sonunda 14 kilo verdim ve yağ oranım %10 düştü. Verdiğim kiloları geri almadan korumak benim için en büyük başarı oldu."
                ], ["-14 KG", "-%10", "+1.5 KG"]),
            new("Derya", 31, 14, kasKazanimi, set2, AlignBottom: false,
                "Çok zayıf görünmekten ve çabuk yorulmaktan şikayetçiydim.",
                [
                    "Çok zayıf görünmekten ve çabuk yorulmaktan şikayetçiydim. Spora başlamak istiyordum ama ağırlık çalışmaktan çekiniyordum.",
                    "Temel hareketlerin tekniğini öğrenerek başladık, yükleri haftadan haftaya kontrollü artırdık. Öğünlerimi atlamamam için pratik bir beslenme planı hazırlandı.",
                    "14 haftada yağ oranımı koruyarak 3 kilo kas kazandım. Artık hem daha dinç hissediyorum hem de duruşum tamamen değişti."
                ], ["+2 KG", "Sabit", "+3 KG"])
        ];

        // Sitedeki kart sırası birebir (tasarımda kartlar tekrar ediyor)
        Sample[] carousel = [mert, can, elif, mert, can, elif, elif];

        var transformations = new List<Transformation>();
        var metrics = new List<TransformationMetric>();
        string[] metricLabels = ["Ağırlık", "Yağ Oranı", "Kas Kütlesi"];

        void Add(Sample s, TransformationSection section, int sortOrder, bool showOnHome)
        {
            var id = transformations.Count + 1;
            transformations.Add(new Transformation
            {
                Id = id, ClientName = s.Name, Age = s.Age, DurationWeeks = s.Weeks, TransformationTypeId = s.TypeId,
                Quote = s.Quote, Story = string.Join("\n\n", s.Story),
                BeforeImagePath = $"{s.ImageSet}before.jpg", AfterImagePath = $"{s.ImageSet}after.jpg",
                BeforeSquarePath = $"{s.ImageSet}before-kare.jpg", AfterSquarePath = $"{s.ImageSet}after-kare.jpg",
                Year = 2026, AlignBottom = s.AlignBottom, Section = section, ShowOnHome = showOnHome, IsActive = true,
                SortOrder = sortOrder, CreatedAt = SeedDate
            });

            for (var m = 0; m < s.Metrics.Length; m++)
            {
                metrics.Add(new TransformationMetric
                {
                    Id = metrics.Count + 1, TransformationId = id, Label = metricLabels[m], Value = s.Metrics[m],
                    SortOrder = m + 1, CreatedAt = SeedDate
                });
            }
        }

        // Ana sayfadaki kaydırmalı bölüm için ilk üç farklı danışan işaretli
        for (var i = 0; i < carousel.Length; i++)
            Add(carousel[i], TransformationSection.Carousel, i + 1, showOnHome: i < 3);
        for (var i = 0; i < galeri.Length; i++)
            Add(galeri[i], TransformationSection.Galeri, i + 1, showOnHome: false);

        builder.Entity<Transformation>().HasData(transformations);
        builder.Entity<TransformationMetric>().HasData(metrics);
    }

    private sealed record Sample(string Name, int Age, int Weeks, int TypeId, string ImageSet, bool AlignBottom,
        string Quote, string[] Story, string[] Metrics);

    // blog.html (öne çıkan + 6 kart; tasarımda 3 yazı iki kez) ve blog-detay.html (Kalori Açığı)
    private static void SeedBlogPosts(ModelBuilder builder)
    {
        const int beslenme = 1, hipertrofi = 2, kuvvet = 3, toparlanma = 5;
        const string mekanik = "Mekanik Gerilim: Kas Gelişiminin Gerçek Sırrı";
        const string mekanikOzet = "Kas geliştirmek istiyorsanız antrenmanlarınızda mekanik gerilimi nasıl maksimize edeceğinizi öğrenmelisiniz.";
        const string uyku = "Uyku ve Kas Gelişimi Arasındaki Bilimsel Bağ";
        const string uykuOzet = "Antrenmanda yıkılan kaslar yatakta inşa edilir. Kaliteli bir uykunun anabolik hormonlara etkisi.";
        const string deadlift = "Deadlift Biyomekaniği: Güvenli ve Ağır Kaldırmak";
        const string deadliftOzet = "Kral egzersiz deadlift'i anatominize uygun şekilde nasıl optimize edebilirsiniz?";

        var id = 0;
        BlogPost Post(string title, string slug, string summary, int categoryId, string cover, int minutes,
            DateTime publishedAt, bool featured = false) => new()
        {
            Id = ++id, Title = title, Slug = slug, Summary = summary, ContentHtml = BlogSampleContent.Html,
            CategoryId = categoryId, CoverImagePath = $"/images/{cover}", ReadingMinutes = minutes,
            PublishedAt = publishedAt, IsPublished = true, IsFeatured = featured,
            RecommendedPackageId = DonusumPackageId, CreatedAt = SeedDate
        };

        static DateTime Day(int year, int month, int day) => new(year, month, day, 9, 0, 0, DateTimeKind.Utc);

        var kalori = Post("Kalori Açığı Nedir, Nasıl Doğru Hesaplanır?", "kalori-acigi-nedir",
            "Kilo vermenin temel kuralı kalori açığı yaratmaktır. Ancak doğru hesaplanmayan bir kalori açığı " +
            "metabolizmanızı yavaşlatabilir ve kas kaybına neden olabilir.",
            beslenme, "blog-makro-besinler.jpg", 8, Day(2024, 10, 24));
        kalori.CoverImageCaption = "Doğru hesaplanmış bir makro planı, başarının anahtarıdır.";
        kalori.MetaTitle = "Kalori Açığı Nedir, Nasıl Doğru Hesaplanır? | Emir Coaching Blog";

        var posts = new[]
        {
            kalori,
            Post("Makro Besinlerin Gücü: Performans İçin Yakıtınızı Doğru Seçin", "makro-besinlerin-gucu",
                "Vücudunuzun bir spor araba olduğunu düşünün. Sadece kalori saymak yetmez, yakıtın kalitesi " +
                "belirleyicidir. Protein, karb ve yağ dengesini optimize etmenin yolları.",
                beslenme, "blog-makro-besinler.jpg", 8, Day(2026, 3, 12), featured: true),
            Post(mekanik, "mekanik-gerilim", mekanikOzet, hipertrofi, "blog-mekanik-gerilim.jpg", 5, Day(2026, 3, 10)),
            Post(uyku, "uyku-ve-kas-gelisimi", uykuOzet, toparlanma, "blog-uyku-kas-gelisimi.jpg", 6, Day(2026, 3, 5)),
            Post(deadlift, "deadlift-biyomekanigi", deadliftOzet, kuvvet, "blog-deadlift-biyomekanigi.jpg", 10, Day(2026, 3, 1)),
            Post(mekanik, "mekanik-gerilim-2", mekanikOzet, hipertrofi, "blog-mekanik-gerilim-2.jpg", 5, Day(2026, 3, 10)),
            Post(uyku, "uyku-ve-kas-gelisimi-2", uykuOzet, toparlanma, "blog-uyku-kas-gelisimi-2.jpg", 6, Day(2026, 3, 5)),
            Post(deadlift, "deadlift-biyomekanigi-2", deadliftOzet, kuvvet, "blog-deadlift-biyomekanigi-2.jpg", 10, Day(2026, 3, 1))
        };

        builder.Entity<BlogPost>().HasData(posts);

        // blog-detay.html etiketleri (Kalori Açığı yazısı)
        string[] tags = ["beslenme", "kilo-verme", "makro", "kalori-acigi"];
        builder.Entity<BlogTag>().HasData(tags.Select((t, i) => new BlogTag
        {
            Id = i + 1, Name = t, Slug = t, CreatedAt = SeedDate
        }));
        builder.Entity<BlogPostTag>().HasData(tags.Select((_, i) => new BlogPostTag
        {
            BlogPostId = kalori.Id, BlogTagId = i + 1
        }));
    }

    // hakkimda.html hero: "8+ Yıl Deneyim · 100+ Danışan · %95 Başarı"
    private static void SeedSiteStats(ModelBuilder builder)
    {
        builder.Entity<SiteStat>().HasData(
            new SiteStat { Id = 1, Value = "8", Suffix = "+", Label = "Yıl Deneyim", SortOrder = 1, CreatedAt = SeedDate },
            new SiteStat { Id = 2, Value = "100", Suffix = "+", Label = "Danışan", SortOrder = 2, CreatedAt = SeedDate },
            new SiteStat { Id = 3, Value = "%95", Label = "Başarı", SortOrder = 3, CreatedAt = SeedDate });
    }

    // index.html sosyal bölümü: 1 YouTube videosu + 3 Instagram gönderisi (Instagram entegrasyonu gelene kadar elle)
    private static void SeedSocialPosts(ModelBuilder builder)
    {
        builder.Entity<SocialPost>().HasData(
            new SocialPost
            {
                Id = 1, Platform = SocialPlatform.YouTube, Title = "Kardionun Önemi", ImagePath = "/images/home_youtube1.png",
                Url = "https://youtube.com/", PostedAt = SamplePostedAt, SortOrder = 1, IsActive = true, CreatedAt = SeedDate
            },
            Instagram(2, "home_instagram1.png"),
            Instagram(3, "home_instagram2.png"),
            Instagram(4, "home_instagram3.png"));

        static SocialPost Instagram(int id, string image) => new()
        {
            Id = id, Platform = SocialPlatform.Instagram, ImagePath = $"/images/{image}", Url = "https://instagram.com/",
            LikeCount = 4891, PostedAt = SamplePostedAt, SortOrder = id, IsActive = true, CreatedAt = SeedDate
        };
    }
}
