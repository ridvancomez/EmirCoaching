using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmirCoaching.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedFrontSampleContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BlogPosts",
                columns: new[] { "Id", "AuthorId", "CategoryId", "ContentHtml", "CoverImageCaption", "CoverImagePath", "CreatedAt", "DeletedAt", "IsDeleted", "IsFeatured", "IsPublished", "MetaDescription", "MetaTitle", "PublishedAt", "ReadingMinutes", "RecommendedPackageId", "Slug", "Summary", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, null, 1, "<section aria-label=\"Giriş\" class=\"makale__section\" id=\"giris\">\n<p>Fitness dünyasında en çok duyulan kavramlardan biri şüphesiz \"kalori açığı\"dır. İster yağ yakmak, ister kilo vermek isteyin, hedefinize ulaşmanın matematiksel temeli bu kavrama dayanır. Ancak mesele sadece \"az yemek\" değildir.</p>\n<p>Vücudunuz bir makine gibi çalışır ve bu makinenin çalışması için enerjiye (kaloriye) ihtiyacı vardır. Eğer bu makineye harcadığından daha az enerji verirseniz, aradaki farkı kapatmak için depoladığı enerjiyi (yağı) kullanmaya başlar. İşte bu sürece kalori açığı diyoruz.</p>\n</section>\n<section aria-labelledby=\"bmh-title\" class=\"makale__section\" id=\"bazal-metabolizma-hizi\">\n<h2 class=\"makale__title\" id=\"bmh-title\">Bazal Metabolizma Hızı (BMH) Nedir?</h2>\n<p>Kalori açığı hesaplamaya başlamadan önce bilmeniz gereken ilk değer Bazal Metabolizma Hızınızdır (BMR). Bu, vücudunuzun tam dinlenme halindeyken (hiçbir şey yapmadan yattığınızda) hayatta kalmak için harcadığı enerjidir.</p>\n<div class=\"makale-callout\">\n<p class=\"makale-callout__title\"><iconify-icon aria-hidden=\"true\" class=\"makale-callout__icon\" icon=\"mdi:calculator-variant-outline\" mode=\"svg\" noobserver></iconify-icon> Mifflin-St Jeor Formülü (Erkekler için)</p>\n<pre class=\"makale-callout__formula\"><code>BMH = (10 × Ağırlık) + (6.25 × Boy) − (5 × Yaş) + 5</code></pre>\n</div>\n</section>\n<section aria-labelledby=\"acik-title\" class=\"makale__section\" id=\"dogru-acik-miktari\">\n<h2 class=\"makale__title\" id=\"acik-title\">Doğru Açık Miktarı Ne Olmalı?</h2>\n<p>En sık yapılan hata, sonucu hızlandırmak için aşırı büyük bir kalori açığı yaratmaktır. Bu durum kas kaybına, enerji düşüklüğüne ve metabolizmanın yavaşlamasına neden olur.</p>\n<ul class=\"makale__list\">\n<li><strong>Hafif Açık (250-300 kcal):</strong> Yavaş ama çok sürdürülebilir. Maksimum kas koruması sağlar.</li>\n<li><strong>Orta Açık (500 kcal):</strong> Haftada yaklaşık 0.5 kg yağ kaybı sağlar. Çoğu kişi için idealdir.</li>\n<li><strong>Yüksek Açık (750+ kcal):</strong> Sadece ciddi obezite durumlarında ve uzman kontrolünde uygulanmalıdır.</li>\n</ul>\n<blockquote class=\"makale__quote\">\n<p>\"Hızlı kayıp, kalıcı kayıp değildir. Hedefiniz maraton koşmak olmalı, depar atmak değil.\"</p>\n</blockquote>\n</section>\n<section aria-labelledby=\"takip-title\" class=\"makale__section\" id=\"nasil-takip-edecegiz\">\n<h2 class=\"makale__title\" id=\"takip-title\">Nasıl Takip Edeceğiz?</h2>\n<p>Hesaplamayı yaptıktan sonra süreci yönetmek işin pratik kısmıdır. Şunlara dikkat etmelisiniz:</p>\n<ol class=\"makale__list makale__list--ordered\">\n<li>Mutfağınıza hassas bir dijital tartı alın. Göz kararı porsiyonlar genellikle yanıltıcıdır.</li>\n<li>Güvenilir bir kalori takip uygulaması indirin ve her öğünü kaydedin.</li>\n<li>Sıvı kalorileri (meyve suları, şekerli kahveler) gözden kaçırmayın.</li>\n<li>Haftalık tartılın ve ölçü alın; günlük dalgalanmalara takılmayın.</li>\n</ol>\n<div class=\"makale-callout makale-callout--dikkat\" role=\"note\">\n<p class=\"makale-callout__title\"><iconify-icon aria-hidden=\"true\" class=\"makale-callout__icon\" icon=\"mdi:alert-outline\" mode=\"svg\" noobserver></iconify-icon> Dikkat</p>\n<p class=\"makale-callout__text\">Kadınlar 1200 kcal, erkekler 1500 kcal altına kesinlikle inmemelidir. Bu değerlerin altı, mikro besin eksikliklerine ve hormonal bozukluklara yol açar.</p>\n</div>\n</section>", "Doğru hesaplanmış bir makro planı, başarının anahtarıdır.", "/images/blog-makro-besinler.jpg", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, false, true, null, "Kalori Açığı Nedir, Nasıl Doğru Hesaplanır? | Emir Coaching Blog", new DateTime(2024, 10, 24, 9, 0, 0, 0, DateTimeKind.Utc), 8, 2, "kalori-acigi-nedir", "Kilo vermenin temel kuralı kalori açığı yaratmaktır. Ancak doğru hesaplanmayan bir kalori açığı metabolizmanızı yavaşlatabilir ve kas kaybına neden olabilir.", "Kalori Açığı Nedir, Nasıl Doğru Hesaplanır?", null },
                    { 2, null, 1, "<section aria-label=\"Giriş\" class=\"makale__section\" id=\"giris\">\n<p>Fitness dünyasında en çok duyulan kavramlardan biri şüphesiz \"kalori açığı\"dır. İster yağ yakmak, ister kilo vermek isteyin, hedefinize ulaşmanın matematiksel temeli bu kavrama dayanır. Ancak mesele sadece \"az yemek\" değildir.</p>\n<p>Vücudunuz bir makine gibi çalışır ve bu makinenin çalışması için enerjiye (kaloriye) ihtiyacı vardır. Eğer bu makineye harcadığından daha az enerji verirseniz, aradaki farkı kapatmak için depoladığı enerjiyi (yağı) kullanmaya başlar. İşte bu sürece kalori açığı diyoruz.</p>\n</section>\n<section aria-labelledby=\"bmh-title\" class=\"makale__section\" id=\"bazal-metabolizma-hizi\">\n<h2 class=\"makale__title\" id=\"bmh-title\">Bazal Metabolizma Hızı (BMH) Nedir?</h2>\n<p>Kalori açığı hesaplamaya başlamadan önce bilmeniz gereken ilk değer Bazal Metabolizma Hızınızdır (BMR). Bu, vücudunuzun tam dinlenme halindeyken (hiçbir şey yapmadan yattığınızda) hayatta kalmak için harcadığı enerjidir.</p>\n<div class=\"makale-callout\">\n<p class=\"makale-callout__title\"><iconify-icon aria-hidden=\"true\" class=\"makale-callout__icon\" icon=\"mdi:calculator-variant-outline\" mode=\"svg\" noobserver></iconify-icon> Mifflin-St Jeor Formülü (Erkekler için)</p>\n<pre class=\"makale-callout__formula\"><code>BMH = (10 × Ağırlık) + (6.25 × Boy) − (5 × Yaş) + 5</code></pre>\n</div>\n</section>\n<section aria-labelledby=\"acik-title\" class=\"makale__section\" id=\"dogru-acik-miktari\">\n<h2 class=\"makale__title\" id=\"acik-title\">Doğru Açık Miktarı Ne Olmalı?</h2>\n<p>En sık yapılan hata, sonucu hızlandırmak için aşırı büyük bir kalori açığı yaratmaktır. Bu durum kas kaybına, enerji düşüklüğüne ve metabolizmanın yavaşlamasına neden olur.</p>\n<ul class=\"makale__list\">\n<li><strong>Hafif Açık (250-300 kcal):</strong> Yavaş ama çok sürdürülebilir. Maksimum kas koruması sağlar.</li>\n<li><strong>Orta Açık (500 kcal):</strong> Haftada yaklaşık 0.5 kg yağ kaybı sağlar. Çoğu kişi için idealdir.</li>\n<li><strong>Yüksek Açık (750+ kcal):</strong> Sadece ciddi obezite durumlarında ve uzman kontrolünde uygulanmalıdır.</li>\n</ul>\n<blockquote class=\"makale__quote\">\n<p>\"Hızlı kayıp, kalıcı kayıp değildir. Hedefiniz maraton koşmak olmalı, depar atmak değil.\"</p>\n</blockquote>\n</section>\n<section aria-labelledby=\"takip-title\" class=\"makale__section\" id=\"nasil-takip-edecegiz\">\n<h2 class=\"makale__title\" id=\"takip-title\">Nasıl Takip Edeceğiz?</h2>\n<p>Hesaplamayı yaptıktan sonra süreci yönetmek işin pratik kısmıdır. Şunlara dikkat etmelisiniz:</p>\n<ol class=\"makale__list makale__list--ordered\">\n<li>Mutfağınıza hassas bir dijital tartı alın. Göz kararı porsiyonlar genellikle yanıltıcıdır.</li>\n<li>Güvenilir bir kalori takip uygulaması indirin ve her öğünü kaydedin.</li>\n<li>Sıvı kalorileri (meyve suları, şekerli kahveler) gözden kaçırmayın.</li>\n<li>Haftalık tartılın ve ölçü alın; günlük dalgalanmalara takılmayın.</li>\n</ol>\n<div class=\"makale-callout makale-callout--dikkat\" role=\"note\">\n<p class=\"makale-callout__title\"><iconify-icon aria-hidden=\"true\" class=\"makale-callout__icon\" icon=\"mdi:alert-outline\" mode=\"svg\" noobserver></iconify-icon> Dikkat</p>\n<p class=\"makale-callout__text\">Kadınlar 1200 kcal, erkekler 1500 kcal altına kesinlikle inmemelidir. Bu değerlerin altı, mikro besin eksikliklerine ve hormonal bozukluklara yol açar.</p>\n</div>\n</section>", null, "/images/blog-makro-besinler.jpg", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, true, true, null, null, new DateTime(2026, 3, 12, 9, 0, 0, 0, DateTimeKind.Utc), 8, 2, "makro-besinlerin-gucu", "Vücudunuzun bir spor araba olduğunu düşünün. Sadece kalori saymak yetmez, yakıtın kalitesi belirleyicidir. Protein, karb ve yağ dengesini optimize etmenin yolları.", "Makro Besinlerin Gücü: Performans İçin Yakıtınızı Doğru Seçin", null },
                    { 3, null, 2, "<section aria-label=\"Giriş\" class=\"makale__section\" id=\"giris\">\n<p>Fitness dünyasında en çok duyulan kavramlardan biri şüphesiz \"kalori açığı\"dır. İster yağ yakmak, ister kilo vermek isteyin, hedefinize ulaşmanın matematiksel temeli bu kavrama dayanır. Ancak mesele sadece \"az yemek\" değildir.</p>\n<p>Vücudunuz bir makine gibi çalışır ve bu makinenin çalışması için enerjiye (kaloriye) ihtiyacı vardır. Eğer bu makineye harcadığından daha az enerji verirseniz, aradaki farkı kapatmak için depoladığı enerjiyi (yağı) kullanmaya başlar. İşte bu sürece kalori açığı diyoruz.</p>\n</section>\n<section aria-labelledby=\"bmh-title\" class=\"makale__section\" id=\"bazal-metabolizma-hizi\">\n<h2 class=\"makale__title\" id=\"bmh-title\">Bazal Metabolizma Hızı (BMH) Nedir?</h2>\n<p>Kalori açığı hesaplamaya başlamadan önce bilmeniz gereken ilk değer Bazal Metabolizma Hızınızdır (BMR). Bu, vücudunuzun tam dinlenme halindeyken (hiçbir şey yapmadan yattığınızda) hayatta kalmak için harcadığı enerjidir.</p>\n<div class=\"makale-callout\">\n<p class=\"makale-callout__title\"><iconify-icon aria-hidden=\"true\" class=\"makale-callout__icon\" icon=\"mdi:calculator-variant-outline\" mode=\"svg\" noobserver></iconify-icon> Mifflin-St Jeor Formülü (Erkekler için)</p>\n<pre class=\"makale-callout__formula\"><code>BMH = (10 × Ağırlık) + (6.25 × Boy) − (5 × Yaş) + 5</code></pre>\n</div>\n</section>\n<section aria-labelledby=\"acik-title\" class=\"makale__section\" id=\"dogru-acik-miktari\">\n<h2 class=\"makale__title\" id=\"acik-title\">Doğru Açık Miktarı Ne Olmalı?</h2>\n<p>En sık yapılan hata, sonucu hızlandırmak için aşırı büyük bir kalori açığı yaratmaktır. Bu durum kas kaybına, enerji düşüklüğüne ve metabolizmanın yavaşlamasına neden olur.</p>\n<ul class=\"makale__list\">\n<li><strong>Hafif Açık (250-300 kcal):</strong> Yavaş ama çok sürdürülebilir. Maksimum kas koruması sağlar.</li>\n<li><strong>Orta Açık (500 kcal):</strong> Haftada yaklaşık 0.5 kg yağ kaybı sağlar. Çoğu kişi için idealdir.</li>\n<li><strong>Yüksek Açık (750+ kcal):</strong> Sadece ciddi obezite durumlarında ve uzman kontrolünde uygulanmalıdır.</li>\n</ul>\n<blockquote class=\"makale__quote\">\n<p>\"Hızlı kayıp, kalıcı kayıp değildir. Hedefiniz maraton koşmak olmalı, depar atmak değil.\"</p>\n</blockquote>\n</section>\n<section aria-labelledby=\"takip-title\" class=\"makale__section\" id=\"nasil-takip-edecegiz\">\n<h2 class=\"makale__title\" id=\"takip-title\">Nasıl Takip Edeceğiz?</h2>\n<p>Hesaplamayı yaptıktan sonra süreci yönetmek işin pratik kısmıdır. Şunlara dikkat etmelisiniz:</p>\n<ol class=\"makale__list makale__list--ordered\">\n<li>Mutfağınıza hassas bir dijital tartı alın. Göz kararı porsiyonlar genellikle yanıltıcıdır.</li>\n<li>Güvenilir bir kalori takip uygulaması indirin ve her öğünü kaydedin.</li>\n<li>Sıvı kalorileri (meyve suları, şekerli kahveler) gözden kaçırmayın.</li>\n<li>Haftalık tartılın ve ölçü alın; günlük dalgalanmalara takılmayın.</li>\n</ol>\n<div class=\"makale-callout makale-callout--dikkat\" role=\"note\">\n<p class=\"makale-callout__title\"><iconify-icon aria-hidden=\"true\" class=\"makale-callout__icon\" icon=\"mdi:alert-outline\" mode=\"svg\" noobserver></iconify-icon> Dikkat</p>\n<p class=\"makale-callout__text\">Kadınlar 1200 kcal, erkekler 1500 kcal altına kesinlikle inmemelidir. Bu değerlerin altı, mikro besin eksikliklerine ve hormonal bozukluklara yol açar.</p>\n</div>\n</section>", null, "/images/blog-mekanik-gerilim.jpg", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, false, true, null, null, new DateTime(2026, 3, 10, 9, 0, 0, 0, DateTimeKind.Utc), 5, 2, "mekanik-gerilim", "Kas geliştirmek istiyorsanız antrenmanlarınızda mekanik gerilimi nasıl maksimize edeceğinizi öğrenmelisiniz.", "Mekanik Gerilim: Kas Gelişiminin Gerçek Sırrı", null },
                    { 4, null, 5, "<section aria-label=\"Giriş\" class=\"makale__section\" id=\"giris\">\n<p>Fitness dünyasında en çok duyulan kavramlardan biri şüphesiz \"kalori açığı\"dır. İster yağ yakmak, ister kilo vermek isteyin, hedefinize ulaşmanın matematiksel temeli bu kavrama dayanır. Ancak mesele sadece \"az yemek\" değildir.</p>\n<p>Vücudunuz bir makine gibi çalışır ve bu makinenin çalışması için enerjiye (kaloriye) ihtiyacı vardır. Eğer bu makineye harcadığından daha az enerji verirseniz, aradaki farkı kapatmak için depoladığı enerjiyi (yağı) kullanmaya başlar. İşte bu sürece kalori açığı diyoruz.</p>\n</section>\n<section aria-labelledby=\"bmh-title\" class=\"makale__section\" id=\"bazal-metabolizma-hizi\">\n<h2 class=\"makale__title\" id=\"bmh-title\">Bazal Metabolizma Hızı (BMH) Nedir?</h2>\n<p>Kalori açığı hesaplamaya başlamadan önce bilmeniz gereken ilk değer Bazal Metabolizma Hızınızdır (BMR). Bu, vücudunuzun tam dinlenme halindeyken (hiçbir şey yapmadan yattığınızda) hayatta kalmak için harcadığı enerjidir.</p>\n<div class=\"makale-callout\">\n<p class=\"makale-callout__title\"><iconify-icon aria-hidden=\"true\" class=\"makale-callout__icon\" icon=\"mdi:calculator-variant-outline\" mode=\"svg\" noobserver></iconify-icon> Mifflin-St Jeor Formülü (Erkekler için)</p>\n<pre class=\"makale-callout__formula\"><code>BMH = (10 × Ağırlık) + (6.25 × Boy) − (5 × Yaş) + 5</code></pre>\n</div>\n</section>\n<section aria-labelledby=\"acik-title\" class=\"makale__section\" id=\"dogru-acik-miktari\">\n<h2 class=\"makale__title\" id=\"acik-title\">Doğru Açık Miktarı Ne Olmalı?</h2>\n<p>En sık yapılan hata, sonucu hızlandırmak için aşırı büyük bir kalori açığı yaratmaktır. Bu durum kas kaybına, enerji düşüklüğüne ve metabolizmanın yavaşlamasına neden olur.</p>\n<ul class=\"makale__list\">\n<li><strong>Hafif Açık (250-300 kcal):</strong> Yavaş ama çok sürdürülebilir. Maksimum kas koruması sağlar.</li>\n<li><strong>Orta Açık (500 kcal):</strong> Haftada yaklaşık 0.5 kg yağ kaybı sağlar. Çoğu kişi için idealdir.</li>\n<li><strong>Yüksek Açık (750+ kcal):</strong> Sadece ciddi obezite durumlarında ve uzman kontrolünde uygulanmalıdır.</li>\n</ul>\n<blockquote class=\"makale__quote\">\n<p>\"Hızlı kayıp, kalıcı kayıp değildir. Hedefiniz maraton koşmak olmalı, depar atmak değil.\"</p>\n</blockquote>\n</section>\n<section aria-labelledby=\"takip-title\" class=\"makale__section\" id=\"nasil-takip-edecegiz\">\n<h2 class=\"makale__title\" id=\"takip-title\">Nasıl Takip Edeceğiz?</h2>\n<p>Hesaplamayı yaptıktan sonra süreci yönetmek işin pratik kısmıdır. Şunlara dikkat etmelisiniz:</p>\n<ol class=\"makale__list makale__list--ordered\">\n<li>Mutfağınıza hassas bir dijital tartı alın. Göz kararı porsiyonlar genellikle yanıltıcıdır.</li>\n<li>Güvenilir bir kalori takip uygulaması indirin ve her öğünü kaydedin.</li>\n<li>Sıvı kalorileri (meyve suları, şekerli kahveler) gözden kaçırmayın.</li>\n<li>Haftalık tartılın ve ölçü alın; günlük dalgalanmalara takılmayın.</li>\n</ol>\n<div class=\"makale-callout makale-callout--dikkat\" role=\"note\">\n<p class=\"makale-callout__title\"><iconify-icon aria-hidden=\"true\" class=\"makale-callout__icon\" icon=\"mdi:alert-outline\" mode=\"svg\" noobserver></iconify-icon> Dikkat</p>\n<p class=\"makale-callout__text\">Kadınlar 1200 kcal, erkekler 1500 kcal altına kesinlikle inmemelidir. Bu değerlerin altı, mikro besin eksikliklerine ve hormonal bozukluklara yol açar.</p>\n</div>\n</section>", null, "/images/blog-uyku-kas-gelisimi.jpg", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, false, true, null, null, new DateTime(2026, 3, 5, 9, 0, 0, 0, DateTimeKind.Utc), 6, 2, "uyku-ve-kas-gelisimi", "Antrenmanda yıkılan kaslar yatakta inşa edilir. Kaliteli bir uykunun anabolik hormonlara etkisi.", "Uyku ve Kas Gelişimi Arasındaki Bilimsel Bağ", null },
                    { 5, null, 3, "<section aria-label=\"Giriş\" class=\"makale__section\" id=\"giris\">\n<p>Fitness dünyasında en çok duyulan kavramlardan biri şüphesiz \"kalori açığı\"dır. İster yağ yakmak, ister kilo vermek isteyin, hedefinize ulaşmanın matematiksel temeli bu kavrama dayanır. Ancak mesele sadece \"az yemek\" değildir.</p>\n<p>Vücudunuz bir makine gibi çalışır ve bu makinenin çalışması için enerjiye (kaloriye) ihtiyacı vardır. Eğer bu makineye harcadığından daha az enerji verirseniz, aradaki farkı kapatmak için depoladığı enerjiyi (yağı) kullanmaya başlar. İşte bu sürece kalori açığı diyoruz.</p>\n</section>\n<section aria-labelledby=\"bmh-title\" class=\"makale__section\" id=\"bazal-metabolizma-hizi\">\n<h2 class=\"makale__title\" id=\"bmh-title\">Bazal Metabolizma Hızı (BMH) Nedir?</h2>\n<p>Kalori açığı hesaplamaya başlamadan önce bilmeniz gereken ilk değer Bazal Metabolizma Hızınızdır (BMR). Bu, vücudunuzun tam dinlenme halindeyken (hiçbir şey yapmadan yattığınızda) hayatta kalmak için harcadığı enerjidir.</p>\n<div class=\"makale-callout\">\n<p class=\"makale-callout__title\"><iconify-icon aria-hidden=\"true\" class=\"makale-callout__icon\" icon=\"mdi:calculator-variant-outline\" mode=\"svg\" noobserver></iconify-icon> Mifflin-St Jeor Formülü (Erkekler için)</p>\n<pre class=\"makale-callout__formula\"><code>BMH = (10 × Ağırlık) + (6.25 × Boy) − (5 × Yaş) + 5</code></pre>\n</div>\n</section>\n<section aria-labelledby=\"acik-title\" class=\"makale__section\" id=\"dogru-acik-miktari\">\n<h2 class=\"makale__title\" id=\"acik-title\">Doğru Açık Miktarı Ne Olmalı?</h2>\n<p>En sık yapılan hata, sonucu hızlandırmak için aşırı büyük bir kalori açığı yaratmaktır. Bu durum kas kaybına, enerji düşüklüğüne ve metabolizmanın yavaşlamasına neden olur.</p>\n<ul class=\"makale__list\">\n<li><strong>Hafif Açık (250-300 kcal):</strong> Yavaş ama çok sürdürülebilir. Maksimum kas koruması sağlar.</li>\n<li><strong>Orta Açık (500 kcal):</strong> Haftada yaklaşık 0.5 kg yağ kaybı sağlar. Çoğu kişi için idealdir.</li>\n<li><strong>Yüksek Açık (750+ kcal):</strong> Sadece ciddi obezite durumlarında ve uzman kontrolünde uygulanmalıdır.</li>\n</ul>\n<blockquote class=\"makale__quote\">\n<p>\"Hızlı kayıp, kalıcı kayıp değildir. Hedefiniz maraton koşmak olmalı, depar atmak değil.\"</p>\n</blockquote>\n</section>\n<section aria-labelledby=\"takip-title\" class=\"makale__section\" id=\"nasil-takip-edecegiz\">\n<h2 class=\"makale__title\" id=\"takip-title\">Nasıl Takip Edeceğiz?</h2>\n<p>Hesaplamayı yaptıktan sonra süreci yönetmek işin pratik kısmıdır. Şunlara dikkat etmelisiniz:</p>\n<ol class=\"makale__list makale__list--ordered\">\n<li>Mutfağınıza hassas bir dijital tartı alın. Göz kararı porsiyonlar genellikle yanıltıcıdır.</li>\n<li>Güvenilir bir kalori takip uygulaması indirin ve her öğünü kaydedin.</li>\n<li>Sıvı kalorileri (meyve suları, şekerli kahveler) gözden kaçırmayın.</li>\n<li>Haftalık tartılın ve ölçü alın; günlük dalgalanmalara takılmayın.</li>\n</ol>\n<div class=\"makale-callout makale-callout--dikkat\" role=\"note\">\n<p class=\"makale-callout__title\"><iconify-icon aria-hidden=\"true\" class=\"makale-callout__icon\" icon=\"mdi:alert-outline\" mode=\"svg\" noobserver></iconify-icon> Dikkat</p>\n<p class=\"makale-callout__text\">Kadınlar 1200 kcal, erkekler 1500 kcal altına kesinlikle inmemelidir. Bu değerlerin altı, mikro besin eksikliklerine ve hormonal bozukluklara yol açar.</p>\n</div>\n</section>", null, "/images/blog-deadlift-biyomekanigi.jpg", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, false, true, null, null, new DateTime(2026, 3, 1, 9, 0, 0, 0, DateTimeKind.Utc), 10, 2, "deadlift-biyomekanigi", "Kral egzersiz deadlift'i anatominize uygun şekilde nasıl optimize edebilirsiniz?", "Deadlift Biyomekaniği: Güvenli ve Ağır Kaldırmak", null },
                    { 6, null, 2, "<section aria-label=\"Giriş\" class=\"makale__section\" id=\"giris\">\n<p>Fitness dünyasında en çok duyulan kavramlardan biri şüphesiz \"kalori açığı\"dır. İster yağ yakmak, ister kilo vermek isteyin, hedefinize ulaşmanın matematiksel temeli bu kavrama dayanır. Ancak mesele sadece \"az yemek\" değildir.</p>\n<p>Vücudunuz bir makine gibi çalışır ve bu makinenin çalışması için enerjiye (kaloriye) ihtiyacı vardır. Eğer bu makineye harcadığından daha az enerji verirseniz, aradaki farkı kapatmak için depoladığı enerjiyi (yağı) kullanmaya başlar. İşte bu sürece kalori açığı diyoruz.</p>\n</section>\n<section aria-labelledby=\"bmh-title\" class=\"makale__section\" id=\"bazal-metabolizma-hizi\">\n<h2 class=\"makale__title\" id=\"bmh-title\">Bazal Metabolizma Hızı (BMH) Nedir?</h2>\n<p>Kalori açığı hesaplamaya başlamadan önce bilmeniz gereken ilk değer Bazal Metabolizma Hızınızdır (BMR). Bu, vücudunuzun tam dinlenme halindeyken (hiçbir şey yapmadan yattığınızda) hayatta kalmak için harcadığı enerjidir.</p>\n<div class=\"makale-callout\">\n<p class=\"makale-callout__title\"><iconify-icon aria-hidden=\"true\" class=\"makale-callout__icon\" icon=\"mdi:calculator-variant-outline\" mode=\"svg\" noobserver></iconify-icon> Mifflin-St Jeor Formülü (Erkekler için)</p>\n<pre class=\"makale-callout__formula\"><code>BMH = (10 × Ağırlık) + (6.25 × Boy) − (5 × Yaş) + 5</code></pre>\n</div>\n</section>\n<section aria-labelledby=\"acik-title\" class=\"makale__section\" id=\"dogru-acik-miktari\">\n<h2 class=\"makale__title\" id=\"acik-title\">Doğru Açık Miktarı Ne Olmalı?</h2>\n<p>En sık yapılan hata, sonucu hızlandırmak için aşırı büyük bir kalori açığı yaratmaktır. Bu durum kas kaybına, enerji düşüklüğüne ve metabolizmanın yavaşlamasına neden olur.</p>\n<ul class=\"makale__list\">\n<li><strong>Hafif Açık (250-300 kcal):</strong> Yavaş ama çok sürdürülebilir. Maksimum kas koruması sağlar.</li>\n<li><strong>Orta Açık (500 kcal):</strong> Haftada yaklaşık 0.5 kg yağ kaybı sağlar. Çoğu kişi için idealdir.</li>\n<li><strong>Yüksek Açık (750+ kcal):</strong> Sadece ciddi obezite durumlarında ve uzman kontrolünde uygulanmalıdır.</li>\n</ul>\n<blockquote class=\"makale__quote\">\n<p>\"Hızlı kayıp, kalıcı kayıp değildir. Hedefiniz maraton koşmak olmalı, depar atmak değil.\"</p>\n</blockquote>\n</section>\n<section aria-labelledby=\"takip-title\" class=\"makale__section\" id=\"nasil-takip-edecegiz\">\n<h2 class=\"makale__title\" id=\"takip-title\">Nasıl Takip Edeceğiz?</h2>\n<p>Hesaplamayı yaptıktan sonra süreci yönetmek işin pratik kısmıdır. Şunlara dikkat etmelisiniz:</p>\n<ol class=\"makale__list makale__list--ordered\">\n<li>Mutfağınıza hassas bir dijital tartı alın. Göz kararı porsiyonlar genellikle yanıltıcıdır.</li>\n<li>Güvenilir bir kalori takip uygulaması indirin ve her öğünü kaydedin.</li>\n<li>Sıvı kalorileri (meyve suları, şekerli kahveler) gözden kaçırmayın.</li>\n<li>Haftalık tartılın ve ölçü alın; günlük dalgalanmalara takılmayın.</li>\n</ol>\n<div class=\"makale-callout makale-callout--dikkat\" role=\"note\">\n<p class=\"makale-callout__title\"><iconify-icon aria-hidden=\"true\" class=\"makale-callout__icon\" icon=\"mdi:alert-outline\" mode=\"svg\" noobserver></iconify-icon> Dikkat</p>\n<p class=\"makale-callout__text\">Kadınlar 1200 kcal, erkekler 1500 kcal altına kesinlikle inmemelidir. Bu değerlerin altı, mikro besin eksikliklerine ve hormonal bozukluklara yol açar.</p>\n</div>\n</section>", null, "/images/blog-mekanik-gerilim-2.jpg", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, false, true, null, null, new DateTime(2026, 3, 10, 9, 0, 0, 0, DateTimeKind.Utc), 5, 2, "mekanik-gerilim-2", "Kas geliştirmek istiyorsanız antrenmanlarınızda mekanik gerilimi nasıl maksimize edeceğinizi öğrenmelisiniz.", "Mekanik Gerilim: Kas Gelişiminin Gerçek Sırrı", null },
                    { 7, null, 5, "<section aria-label=\"Giriş\" class=\"makale__section\" id=\"giris\">\n<p>Fitness dünyasında en çok duyulan kavramlardan biri şüphesiz \"kalori açığı\"dır. İster yağ yakmak, ister kilo vermek isteyin, hedefinize ulaşmanın matematiksel temeli bu kavrama dayanır. Ancak mesele sadece \"az yemek\" değildir.</p>\n<p>Vücudunuz bir makine gibi çalışır ve bu makinenin çalışması için enerjiye (kaloriye) ihtiyacı vardır. Eğer bu makineye harcadığından daha az enerji verirseniz, aradaki farkı kapatmak için depoladığı enerjiyi (yağı) kullanmaya başlar. İşte bu sürece kalori açığı diyoruz.</p>\n</section>\n<section aria-labelledby=\"bmh-title\" class=\"makale__section\" id=\"bazal-metabolizma-hizi\">\n<h2 class=\"makale__title\" id=\"bmh-title\">Bazal Metabolizma Hızı (BMH) Nedir?</h2>\n<p>Kalori açığı hesaplamaya başlamadan önce bilmeniz gereken ilk değer Bazal Metabolizma Hızınızdır (BMR). Bu, vücudunuzun tam dinlenme halindeyken (hiçbir şey yapmadan yattığınızda) hayatta kalmak için harcadığı enerjidir.</p>\n<div class=\"makale-callout\">\n<p class=\"makale-callout__title\"><iconify-icon aria-hidden=\"true\" class=\"makale-callout__icon\" icon=\"mdi:calculator-variant-outline\" mode=\"svg\" noobserver></iconify-icon> Mifflin-St Jeor Formülü (Erkekler için)</p>\n<pre class=\"makale-callout__formula\"><code>BMH = (10 × Ağırlık) + (6.25 × Boy) − (5 × Yaş) + 5</code></pre>\n</div>\n</section>\n<section aria-labelledby=\"acik-title\" class=\"makale__section\" id=\"dogru-acik-miktari\">\n<h2 class=\"makale__title\" id=\"acik-title\">Doğru Açık Miktarı Ne Olmalı?</h2>\n<p>En sık yapılan hata, sonucu hızlandırmak için aşırı büyük bir kalori açığı yaratmaktır. Bu durum kas kaybına, enerji düşüklüğüne ve metabolizmanın yavaşlamasına neden olur.</p>\n<ul class=\"makale__list\">\n<li><strong>Hafif Açık (250-300 kcal):</strong> Yavaş ama çok sürdürülebilir. Maksimum kas koruması sağlar.</li>\n<li><strong>Orta Açık (500 kcal):</strong> Haftada yaklaşık 0.5 kg yağ kaybı sağlar. Çoğu kişi için idealdir.</li>\n<li><strong>Yüksek Açık (750+ kcal):</strong> Sadece ciddi obezite durumlarında ve uzman kontrolünde uygulanmalıdır.</li>\n</ul>\n<blockquote class=\"makale__quote\">\n<p>\"Hızlı kayıp, kalıcı kayıp değildir. Hedefiniz maraton koşmak olmalı, depar atmak değil.\"</p>\n</blockquote>\n</section>\n<section aria-labelledby=\"takip-title\" class=\"makale__section\" id=\"nasil-takip-edecegiz\">\n<h2 class=\"makale__title\" id=\"takip-title\">Nasıl Takip Edeceğiz?</h2>\n<p>Hesaplamayı yaptıktan sonra süreci yönetmek işin pratik kısmıdır. Şunlara dikkat etmelisiniz:</p>\n<ol class=\"makale__list makale__list--ordered\">\n<li>Mutfağınıza hassas bir dijital tartı alın. Göz kararı porsiyonlar genellikle yanıltıcıdır.</li>\n<li>Güvenilir bir kalori takip uygulaması indirin ve her öğünü kaydedin.</li>\n<li>Sıvı kalorileri (meyve suları, şekerli kahveler) gözden kaçırmayın.</li>\n<li>Haftalık tartılın ve ölçü alın; günlük dalgalanmalara takılmayın.</li>\n</ol>\n<div class=\"makale-callout makale-callout--dikkat\" role=\"note\">\n<p class=\"makale-callout__title\"><iconify-icon aria-hidden=\"true\" class=\"makale-callout__icon\" icon=\"mdi:alert-outline\" mode=\"svg\" noobserver></iconify-icon> Dikkat</p>\n<p class=\"makale-callout__text\">Kadınlar 1200 kcal, erkekler 1500 kcal altına kesinlikle inmemelidir. Bu değerlerin altı, mikro besin eksikliklerine ve hormonal bozukluklara yol açar.</p>\n</div>\n</section>", null, "/images/blog-uyku-kas-gelisimi-2.jpg", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, false, true, null, null, new DateTime(2026, 3, 5, 9, 0, 0, 0, DateTimeKind.Utc), 6, 2, "uyku-ve-kas-gelisimi-2", "Antrenmanda yıkılan kaslar yatakta inşa edilir. Kaliteli bir uykunun anabolik hormonlara etkisi.", "Uyku ve Kas Gelişimi Arasındaki Bilimsel Bağ", null },
                    { 8, null, 3, "<section aria-label=\"Giriş\" class=\"makale__section\" id=\"giris\">\n<p>Fitness dünyasında en çok duyulan kavramlardan biri şüphesiz \"kalori açığı\"dır. İster yağ yakmak, ister kilo vermek isteyin, hedefinize ulaşmanın matematiksel temeli bu kavrama dayanır. Ancak mesele sadece \"az yemek\" değildir.</p>\n<p>Vücudunuz bir makine gibi çalışır ve bu makinenin çalışması için enerjiye (kaloriye) ihtiyacı vardır. Eğer bu makineye harcadığından daha az enerji verirseniz, aradaki farkı kapatmak için depoladığı enerjiyi (yağı) kullanmaya başlar. İşte bu sürece kalori açığı diyoruz.</p>\n</section>\n<section aria-labelledby=\"bmh-title\" class=\"makale__section\" id=\"bazal-metabolizma-hizi\">\n<h2 class=\"makale__title\" id=\"bmh-title\">Bazal Metabolizma Hızı (BMH) Nedir?</h2>\n<p>Kalori açığı hesaplamaya başlamadan önce bilmeniz gereken ilk değer Bazal Metabolizma Hızınızdır (BMR). Bu, vücudunuzun tam dinlenme halindeyken (hiçbir şey yapmadan yattığınızda) hayatta kalmak için harcadığı enerjidir.</p>\n<div class=\"makale-callout\">\n<p class=\"makale-callout__title\"><iconify-icon aria-hidden=\"true\" class=\"makale-callout__icon\" icon=\"mdi:calculator-variant-outline\" mode=\"svg\" noobserver></iconify-icon> Mifflin-St Jeor Formülü (Erkekler için)</p>\n<pre class=\"makale-callout__formula\"><code>BMH = (10 × Ağırlık) + (6.25 × Boy) − (5 × Yaş) + 5</code></pre>\n</div>\n</section>\n<section aria-labelledby=\"acik-title\" class=\"makale__section\" id=\"dogru-acik-miktari\">\n<h2 class=\"makale__title\" id=\"acik-title\">Doğru Açık Miktarı Ne Olmalı?</h2>\n<p>En sık yapılan hata, sonucu hızlandırmak için aşırı büyük bir kalori açığı yaratmaktır. Bu durum kas kaybına, enerji düşüklüğüne ve metabolizmanın yavaşlamasına neden olur.</p>\n<ul class=\"makale__list\">\n<li><strong>Hafif Açık (250-300 kcal):</strong> Yavaş ama çok sürdürülebilir. Maksimum kas koruması sağlar.</li>\n<li><strong>Orta Açık (500 kcal):</strong> Haftada yaklaşık 0.5 kg yağ kaybı sağlar. Çoğu kişi için idealdir.</li>\n<li><strong>Yüksek Açık (750+ kcal):</strong> Sadece ciddi obezite durumlarında ve uzman kontrolünde uygulanmalıdır.</li>\n</ul>\n<blockquote class=\"makale__quote\">\n<p>\"Hızlı kayıp, kalıcı kayıp değildir. Hedefiniz maraton koşmak olmalı, depar atmak değil.\"</p>\n</blockquote>\n</section>\n<section aria-labelledby=\"takip-title\" class=\"makale__section\" id=\"nasil-takip-edecegiz\">\n<h2 class=\"makale__title\" id=\"takip-title\">Nasıl Takip Edeceğiz?</h2>\n<p>Hesaplamayı yaptıktan sonra süreci yönetmek işin pratik kısmıdır. Şunlara dikkat etmelisiniz:</p>\n<ol class=\"makale__list makale__list--ordered\">\n<li>Mutfağınıza hassas bir dijital tartı alın. Göz kararı porsiyonlar genellikle yanıltıcıdır.</li>\n<li>Güvenilir bir kalori takip uygulaması indirin ve her öğünü kaydedin.</li>\n<li>Sıvı kalorileri (meyve suları, şekerli kahveler) gözden kaçırmayın.</li>\n<li>Haftalık tartılın ve ölçü alın; günlük dalgalanmalara takılmayın.</li>\n</ol>\n<div class=\"makale-callout makale-callout--dikkat\" role=\"note\">\n<p class=\"makale-callout__title\"><iconify-icon aria-hidden=\"true\" class=\"makale-callout__icon\" icon=\"mdi:alert-outline\" mode=\"svg\" noobserver></iconify-icon> Dikkat</p>\n<p class=\"makale-callout__text\">Kadınlar 1200 kcal, erkekler 1500 kcal altına kesinlikle inmemelidir. Bu değerlerin altı, mikro besin eksikliklerine ve hormonal bozukluklara yol açar.</p>\n</div>\n</section>", null, "/images/blog-deadlift-biyomekanigi-2.jpg", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, false, true, null, null, new DateTime(2026, 3, 1, 9, 0, 0, 0, DateTimeKind.Utc), 10, 2, "deadlift-biyomekanigi-2", "Kral egzersiz deadlift'i anatominize uygun şekilde nasıl optimize edebilirsiniz?", "Deadlift Biyomekaniği: Güvenli ve Ağır Kaldırmak", null }
                });

            migrationBuilder.InsertData(
                table: "BlogTags",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "IsDeleted", "Name", "Slug", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "beslenme", "beslenme", null },
                    { 2, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "kilo-verme", "kilo-verme", null },
                    { 3, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "makro", "makro", null },
                    { 4, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "kalori-acigi", "kalori-acigi", null }
                });

            migrationBuilder.InsertData(
                table: "SiteStats",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "IsDeleted", "Label", "SortOrder", "Suffix", "UpdatedAt", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Yıl Deneyim", 1, "+", null, "8" },
                    { 2, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Danışan", 2, "+", null, "100" },
                    { 3, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Başarı", 3, null, null, "%95" }
                });

            migrationBuilder.InsertData(
                table: "SocialPosts",
                columns: new[] { "Id", "Caption", "CreatedAt", "DeletedAt", "ExternalId", "Hashtag", "ImagePath", "IsActive", "IsDeleted", "LikeCount", "Platform", "PostedAt", "SortOrder", "Title", "UpdatedAt", "Url" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "/images/home_youtube1.png", true, false, null, 2, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Kardionun Önemi", null, "https://youtube.com/" },
                    { 2, null, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "/images/home_instagram1.png", true, false, 4891, 1, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null, "https://instagram.com/" },
                    { 3, null, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "/images/home_instagram2.png", true, false, 4891, 1, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null, "https://instagram.com/" },
                    { 4, null, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "/images/home_instagram3.png", true, false, 4891, 1, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null, "https://instagram.com/" }
                });

            migrationBuilder.InsertData(
                table: "Testimonials",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "IsActive", "IsDeleted", "Name", "PackageId", "Rating", "ShowOnHome", "SortOrder", "Text", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "A.K.", 2, 5m, true, 1, "İlk ayda 6 kilo verdim ama asıl kazancım, neyi neden yaptığımı öğrenmek oldu.", null },
                    { 2, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "M.Y.", 1, 5m, true, 2, "Programı hayatıma göre kurdu; ilk kez bir beslenme planını üç ay boyunca bırakmadan sürdürebildim.", null },
                    { 3, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "E.D.", 3, 5m, true, 3, "Her hafta form kontrolü olunca bırakmak aklıma bile gelmedi. Bir yıl sonra bambaşka biriyim.", null },
                    { 4, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Ahmet Y.", 1, 5m, false, 4, "Dönüşüm Protokolü ile hayatım değişti. Beslenme ve antrenman konusunda öğrendiklerim paha biçilemez. İletişim kalitesi mükemmel.", null },
                    { 5, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Caner K.", 1, 5m, false, 5, "Daha önce birçok program denedim ama hiçbiri bu kadar detaylı ve takip odaklı değildi. Hedefime ulaştım ve korumaya devam ediyorum.", null },
                    { 6, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Burak M.", 1, 5m, false, 6, "Sakatlık geçmişim olmasına rağmen bana özel hazırlanan programla hiçbir sorun yaşamadım. Gerçekten kişiye özel bir hizmet.", null },
                    { 7, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Ahmet Y.", 2, 5m, false, 4, "Dönüşüm Protokolü ile hayatım değişti. Beslenme ve antrenman konusunda öğrendiklerim paha biçilemez. İletişim kalitesi mükemmel.", null },
                    { 8, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Caner K.", 2, 5m, false, 5, "Daha önce birçok program denedim ama hiçbiri bu kadar detaylı ve takip odaklı değildi. Hedefime ulaştım ve korumaya devam ediyorum.", null },
                    { 9, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Burak M.", 2, 5m, false, 6, "Sakatlık geçmişim olmasına rağmen bana özel hazırlanan programla hiçbir sorun yaşamadım. Gerçekten kişiye özel bir hizmet.", null },
                    { 10, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Ahmet Y.", 3, 5m, false, 4, "Dönüşüm Protokolü ile hayatım değişti. Beslenme ve antrenman konusunda öğrendiklerim paha biçilemez. İletişim kalitesi mükemmel.", null },
                    { 11, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Caner K.", 3, 5m, false, 5, "Daha önce birçok program denedim ama hiçbiri bu kadar detaylı ve takip odaklı değildi. Hedefime ulaştım ve korumaya devam ediyorum.", null },
                    { 12, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Burak M.", 3, 5m, false, 6, "Sakatlık geçmişim olmasına rağmen bana özel hazırlanan programla hiçbir sorun yaşamadım. Gerçekten kişiye özel bir hizmet.", null },
                    { 13, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Ahmet Y.", 4, 5m, false, 4, "Dönüşüm Protokolü ile hayatım değişti. Beslenme ve antrenman konusunda öğrendiklerim paha biçilemez. İletişim kalitesi mükemmel.", null },
                    { 14, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Caner K.", 4, 5m, false, 5, "Daha önce birçok program denedim ama hiçbiri bu kadar detaylı ve takip odaklı değildi. Hedefime ulaştım ve korumaya devam ediyorum.", null },
                    { 15, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Burak M.", 4, 5m, false, 6, "Sakatlık geçmişim olmasına rağmen bana özel hazırlanan programla hiçbir sorun yaşamadım. Gerçekten kişiye özel bir hizmet.", null },
                    { 16, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Ahmet Y.", 5, 5m, false, 4, "Dönüşüm Protokolü ile hayatım değişti. Beslenme ve antrenman konusunda öğrendiklerim paha biçilemez. İletişim kalitesi mükemmel.", null },
                    { 17, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Caner K.", 5, 5m, false, 5, "Daha önce birçok program denedim ama hiçbiri bu kadar detaylı ve takip odaklı değildi. Hedefime ulaştım ve korumaya devam ediyorum.", null },
                    { 18, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Burak M.", 5, 5m, false, 6, "Sakatlık geçmişim olmasına rağmen bana özel hazırlanan programla hiçbir sorun yaşamadım. Gerçekten kişiye özel bir hizmet.", null },
                    { 19, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Ahmet Y.", 6, 5m, false, 4, "Dönüşüm Protokolü ile hayatım değişti. Beslenme ve antrenman konusunda öğrendiklerim paha biçilemez. İletişim kalitesi mükemmel.", null },
                    { 20, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Caner K.", 6, 5m, false, 5, "Daha önce birçok program denedim ama hiçbiri bu kadar detaylı ve takip odaklı değildi. Hedefime ulaştım ve korumaya devam ediyorum.", null },
                    { 21, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Burak M.", 6, 5m, false, 6, "Sakatlık geçmişim olmasına rağmen bana özel hazırlanan programla hiçbir sorun yaşamadım. Gerçekten kişiye özel bir hizmet.", null }
                });

            migrationBuilder.InsertData(
                table: "TransformationTypes",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "IsDeleted", "Name", "Slug", "SortOrder", "UpdatedAt" },
                values: new object[] { 4, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Form Koruma", "form-koruma", 4, null });

            migrationBuilder.InsertData(
                table: "Transformations",
                columns: new[] { "Id", "AfterImagePath", "AfterSquarePath", "Age", "AlignBottom", "BeforeImagePath", "BeforeSquarePath", "ClientName", "CreatedAt", "DeletedAt", "DurationWeeks", "IsActive", "IsDeleted", "Quote", "Section", "ShowOnHome", "SortOrder", "Story", "TransformationTypeId", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 1, "/images/hakkimda-degisim-1-after.jpg", "/images/hakkimda-degisim-1-after-kare.jpg", 28, false, "/images/hakkimda-degisim-1-before.jpg", "/images/hakkimda-degisim-1-before-kare.jpg", "Mert", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 12, true, false, "Sadece fiziksel değil, mental olarak da büyük bir değişim yaşadım. Disiplin kelimesinin anlamını öğrendim.", 0, true, 1, "Programa başladığımda 98 kiloydum ve merdiven çıkarken bile nefes nefese kalıyordum. Yıllarca farklı diyetler denemiş, her seferinde birkaç hafta sonra bırakmıştım.\n\nİlk haftalarda en zorlandığım şey düzen oldu: öğünleri tartmak, antrenmanları kaçırmamak, uykuyu ciddiye almak. Haftalık kontrollerde planım küçük adımlarla güncellendi; hiçbir zaman aç kalmadım.\n\n12 haftanın sonunda 12 kilo verdim, yağ oranım %8 düştü ve kas kütlemi korudum. Ama asıl kazancım disiplin oldu: artık antrenmana motivasyon beklemeden gidiyorum.", 1, null, 2026 },
                    { 2, "/images/hakkimda-degisim-1-after.jpg", "/images/hakkimda-degisim-1-after-kare.jpg", 32, false, "/images/hakkimda-degisim-1-before.jpg", "/images/hakkimda-degisim-1-before-kare.jpg", "Can", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 16, true, false, "Kilo alamıyordum, yıllardır aynı formdaydım. Doğru yükleme ve beslenme planı ile tabularımı yıktım.", 0, true, 2, "Yıllardır spor salonuna gidiyordum ama tartıdaki rakam hiç değişmiyordu. Ne kadar yesem de kilo alamadığımı düşünüyordum; aslında ne yediğimi hiç ölçmemiştim.\n\nKoçumla önce gerçek kalori ihtiyacımı hesapladık. Antrenmanlarımı ilerleyici yüklemeye göre yeniden kurduk ve her hafta ağırlıkları kayıt altına aldık.\n\n16 haftada 8 kilo aldım ve bunun 6.5 kilosu kas kütlesiydi; yağ oranım sabit kaldı. Aynada gördüğüm değişimden daha önemlisi, kendime olan güvenimin geri gelmesi oldu.", 2, null, 2026 },
                    { 3, "/images/hakkimda-degisim-2-after.jpg", "/images/hakkimda-degisim-2-after-kare.jpg", 26, true, "/images/hakkimda-degisim-2-before.jpg", "/images/hakkimda-degisim-2-before-kare.jpg", "Elif", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 20, true, false, "Sürekli diyet yapıp bırakıyordum. Sürdürülebilir bir sistemin ne demek olduğunu burada anladım.", 0, true, 3, "Sürekli diyet yapıp bırakıyordum. Her yeni programın ilk haftasında çok heyecanlı olur, sonra kısıtlamalardan sıkılıp eski alışkanlıklarıma dönerdim.\n\nBu kez yasaklar yerine sürdürülebilir bir sistem kurduk: sevdiğim yemekler planın içindeydi, antrenmanlarım kondisyonumu adım adım artıracak şekilde ilerledi.\n\n20 haftanın sonunda 8 kilo verdim, yağ oranım %6 azaldı ve 1 kilo kas kazandım. Artık merdivenleri koşarak çıkıyorum ve bir diyete değil, bir yaşam tarzına bağlıyım.", 3, null, 2026 },
                    { 4, "/images/hakkimda-degisim-1-after.jpg", "/images/hakkimda-degisim-1-after-kare.jpg", 28, false, "/images/hakkimda-degisim-1-before.jpg", "/images/hakkimda-degisim-1-before-kare.jpg", "Mert", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 12, true, false, "Sadece fiziksel değil, mental olarak da büyük bir değişim yaşadım. Disiplin kelimesinin anlamını öğrendim.", 0, false, 4, "Programa başladığımda 98 kiloydum ve merdiven çıkarken bile nefes nefese kalıyordum. Yıllarca farklı diyetler denemiş, her seferinde birkaç hafta sonra bırakmıştım.\n\nİlk haftalarda en zorlandığım şey düzen oldu: öğünleri tartmak, antrenmanları kaçırmamak, uykuyu ciddiye almak. Haftalık kontrollerde planım küçük adımlarla güncellendi; hiçbir zaman aç kalmadım.\n\n12 haftanın sonunda 12 kilo verdim, yağ oranım %8 düştü ve kas kütlemi korudum. Ama asıl kazancım disiplin oldu: artık antrenmana motivasyon beklemeden gidiyorum.", 1, null, 2026 },
                    { 5, "/images/hakkimda-degisim-1-after.jpg", "/images/hakkimda-degisim-1-after-kare.jpg", 32, false, "/images/hakkimda-degisim-1-before.jpg", "/images/hakkimda-degisim-1-before-kare.jpg", "Can", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 16, true, false, "Kilo alamıyordum, yıllardır aynı formdaydım. Doğru yükleme ve beslenme planı ile tabularımı yıktım.", 0, false, 5, "Yıllardır spor salonuna gidiyordum ama tartıdaki rakam hiç değişmiyordu. Ne kadar yesem de kilo alamadığımı düşünüyordum; aslında ne yediğimi hiç ölçmemiştim.\n\nKoçumla önce gerçek kalori ihtiyacımı hesapladık. Antrenmanlarımı ilerleyici yüklemeye göre yeniden kurduk ve her hafta ağırlıkları kayıt altına aldık.\n\n16 haftada 8 kilo aldım ve bunun 6.5 kilosu kas kütlesiydi; yağ oranım sabit kaldı. Aynada gördüğüm değişimden daha önemlisi, kendime olan güvenimin geri gelmesi oldu.", 2, null, 2026 },
                    { 6, "/images/hakkimda-degisim-2-after.jpg", "/images/hakkimda-degisim-2-after-kare.jpg", 26, true, "/images/hakkimda-degisim-2-before.jpg", "/images/hakkimda-degisim-2-before-kare.jpg", "Elif", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 20, true, false, "Sürekli diyet yapıp bırakıyordum. Sürdürülebilir bir sistemin ne demek olduğunu burada anladım.", 0, false, 6, "Sürekli diyet yapıp bırakıyordum. Her yeni programın ilk haftasında çok heyecanlı olur, sonra kısıtlamalardan sıkılıp eski alışkanlıklarıma dönerdim.\n\nBu kez yasaklar yerine sürdürülebilir bir sistem kurduk: sevdiğim yemekler planın içindeydi, antrenmanlarım kondisyonumu adım adım artıracak şekilde ilerledi.\n\n20 haftanın sonunda 8 kilo verdim, yağ oranım %6 azaldı ve 1 kilo kas kazandım. Artık merdivenleri koşarak çıkıyorum ve bir diyete değil, bir yaşam tarzına bağlıyım.", 3, null, 2026 },
                    { 7, "/images/hakkimda-degisim-2-after.jpg", "/images/hakkimda-degisim-2-after-kare.jpg", 26, true, "/images/hakkimda-degisim-2-before.jpg", "/images/hakkimda-degisim-2-before-kare.jpg", "Elif", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 20, true, false, "Sürekli diyet yapıp bırakıyordum. Sürdürülebilir bir sistemin ne demek olduğunu burada anladım.", 0, false, 7, "Sürekli diyet yapıp bırakıyordum. Her yeni programın ilk haftasında çok heyecanlı olur, sonra kısıtlamalardan sıkılıp eski alışkanlıklarıma dönerdim.\n\nBu kez yasaklar yerine sürdürülebilir bir sistem kurduk: sevdiğim yemekler planın içindeydi, antrenmanlarım kondisyonumu adım adım artıracak şekilde ilerledi.\n\n20 haftanın sonunda 8 kilo verdim, yağ oranım %6 azaldı ve 1 kilo kas kazandım. Artık merdivenleri koşarak çıkıyorum ve bir diyete değil, bir yaşam tarzına bağlıyım.", 3, null, 2026 },
                    { 8, "/images/hakkimda-degisim-2-after.jpg", "/images/hakkimda-degisim-2-after-kare.jpg", 29, false, "/images/hakkimda-degisim-2-before.jpg", "/images/hakkimda-degisim-2-before-kare.jpg", "Zeynep", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 16, true, false, "Doğumdan sonra eski formuma dönemeyeceğimi düşünüyordum.", 1, false, 1, "Doğumdan sonra eski formuma dönemeyeceğimi düşünüyordum. Gün içinde vakit bulmak zordu, bu yüzden antrenmanlarım evde 40 dakikaya sığacak şekilde planlandı.\n\nBeslenmede yasaklar yerine porsiyon ve öğün düzeni üzerinde çalıştık. Haftalık kontrollerde ölçülerim düzenli takip edildi, takıldığım haftalarda plan küçük dokunuşlarla güncellendi.\n\n16 haftanın sonunda 9 kilo verdim ve yağ oranım %7 düştü. En önemlisi, enerjim geri geldi ve bu düzeni artık zorlanmadan sürdürebiliyorum.", 1, null, 2026 },
                    { 10, "/images/hakkimda-degisim-2-after.jpg", "/images/hakkimda-degisim-2-after-kare.jpg", 27, false, "/images/hakkimda-degisim-2-before.jpg", "/images/hakkimda-degisim-2-before-kare.jpg", "Selin", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 20, true, false, "Ofiste uzun saatler oturmak ve düzensiz yemek yüzünden birkaç yılda ciddi kilo almıştım.", 1, false, 3, "Ofiste uzun saatler oturmak ve düzensiz yemek yüzünden birkaç yılda ciddi kilo almıştım. Daha önce denediğim sert diyetlerin hepsi kısa sürede bitmişti.\n\nBu sefer acele etmedik: kalori açığı küçük tutuldu, adım hedefi ve haftada üç antrenman eklendi. Zor haftalarda koçumla konuşup planı birlikte ayarladık.\n\n20 haftanın sonunda 14 kilo verdim ve yağ oranım %10 düştü. Verdiğim kiloları geri almadan korumak benim için en büyük başarı oldu.", 1, null, 2026 },
                    { 11, "/images/hakkimda-degisim-2-after.jpg", "/images/hakkimda-degisim-2-after-kare.jpg", 31, false, "/images/hakkimda-degisim-2-before.jpg", "/images/hakkimda-degisim-2-before-kare.jpg", "Derya", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 14, true, false, "Çok zayıf görünmekten ve çabuk yorulmaktan şikayetçiydim.", 1, false, 4, "Çok zayıf görünmekten ve çabuk yorulmaktan şikayetçiydim. Spora başlamak istiyordum ama ağırlık çalışmaktan çekiniyordum.\n\nTemel hareketlerin tekniğini öğrenerek başladık, yükleri haftadan haftaya kontrollü artırdık. Öğünlerimi atlamamam için pratik bir beslenme planı hazırlandı.\n\n14 haftada yağ oranımı koruyarak 3 kilo kas kazandım. Artık hem daha dinç hissediyorum hem de duruşum tamamen değişti.", 2, null, 2026 }
                });

            migrationBuilder.InsertData(
                table: "BlogPostTags",
                columns: new[] { "BlogPostId", "BlogTagId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 }
                });

            migrationBuilder.InsertData(
                table: "TransformationMetrics",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "IsDeleted", "Label", "SortOrder", "TransformationId", "UpdatedAt", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Ağırlık", 1, 1, null, "-12 KG" },
                    { 2, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Yağ Oranı", 2, 1, null, "-%8" },
                    { 3, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Kas Kütlesi", 3, 1, null, "+2 KG" },
                    { 4, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Ağırlık", 1, 2, null, "+8 KG" },
                    { 5, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Yağ Oranı", 2, 2, null, "Sabit" },
                    { 6, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Kas Kütlesi", 3, 2, null, "+6.5 KG" },
                    { 7, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Ağırlık", 1, 3, null, "-8 KG" },
                    { 8, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Yağ Oranı", 2, 3, null, "-%6" },
                    { 9, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Kas Kütlesi", 3, 3, null, "+1 KG" },
                    { 10, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Ağırlık", 1, 4, null, "-12 KG" },
                    { 11, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Yağ Oranı", 2, 4, null, "-%8" },
                    { 12, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Kas Kütlesi", 3, 4, null, "+2 KG" },
                    { 13, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Ağırlık", 1, 5, null, "+8 KG" },
                    { 14, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Yağ Oranı", 2, 5, null, "Sabit" },
                    { 15, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Kas Kütlesi", 3, 5, null, "+6.5 KG" },
                    { 16, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Ağırlık", 1, 6, null, "-8 KG" },
                    { 17, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Yağ Oranı", 2, 6, null, "-%6" },
                    { 18, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Kas Kütlesi", 3, 6, null, "+1 KG" },
                    { 19, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Ağırlık", 1, 7, null, "-8 KG" },
                    { 20, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Yağ Oranı", 2, 7, null, "-%6" },
                    { 21, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Kas Kütlesi", 3, 7, null, "+1 KG" },
                    { 22, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Ağırlık", 1, 8, null, "-9 KG" },
                    { 23, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Yağ Oranı", 2, 8, null, "-%7" },
                    { 24, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Kas Kütlesi", 3, 8, null, "+1 KG" },
                    { 28, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Ağırlık", 1, 10, null, "-14 KG" },
                    { 29, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Yağ Oranı", 2, 10, null, "-%10" },
                    { 30, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Kas Kütlesi", 3, 10, null, "+1.5 KG" },
                    { 31, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Ağırlık", 1, 11, null, "+2 KG" },
                    { 32, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Yağ Oranı", 2, 11, null, "Sabit" },
                    { 33, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Kas Kütlesi", 3, 11, null, "+3 KG" }
                });

            migrationBuilder.InsertData(
                table: "Transformations",
                columns: new[] { "Id", "AfterImagePath", "AfterSquarePath", "Age", "AlignBottom", "BeforeImagePath", "BeforeSquarePath", "ClientName", "CreatedAt", "DeletedAt", "DurationWeeks", "IsActive", "IsDeleted", "Quote", "Section", "ShowOnHome", "SortOrder", "Story", "TransformationTypeId", "UpdatedAt", "Year" },
                values: new object[] { 9, "/images/hakkimda-degisim-2-after.jpg", "/images/hakkimda-degisim-2-after-kare.jpg", 34, false, "/images/hakkimda-degisim-2-before.jpg", "/images/hakkimda-degisim-2-before-kare.jpg", "Ayşe", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 12, true, false, "Kilom normaldi ama vücudum sıkı değildi; yıllardır sadece kardiyo yapıyordum.", 1, false, 2, "Kilom normaldi ama vücudum sıkı değildi; yıllardır sadece kardiyo yapıyordum. Hedefim tartıdaki sayıdan çok aynadaki görüntüydü.\n\nPrograma ağırlık antrenmanı ve yeterli protein ekledik. İlk haftalarda tartı neredeyse hiç değişmedi, ama mezura ölçüleri her hafta biraz daha azaldı.\n\n12 haftada 3 kilo verip 2 kilo kas kazandım. Kıyafetlerim artık farklı oturuyor ve kendimi hiç olmadığım kadar güçlü hissediyorum.", 4, null, 2026 });

            migrationBuilder.InsertData(
                table: "TransformationMetrics",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "IsDeleted", "Label", "SortOrder", "TransformationId", "UpdatedAt", "Value" },
                values: new object[,]
                {
                    { 25, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Ağırlık", 1, 9, null, "-3 KG" },
                    { 26, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Yağ Oranı", 2, 9, null, "-%4" },
                    { 27, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Kas Kütlesi", 3, 9, null, "+2 KG" }
                });

            // Admin bu migration'dan önce oluşmuşsa: seed yazılar koça bağlanır, yazar kartı alanları boşsa doldurulur
            // (yeni kurulumda aynı işi SeedData.SeedAdminUserAsync yapar)
            migrationBuilder.Sql("""
                DECLARE @coachId int = (SELECT TOP 1 Id FROM AspNetUsers WHERE IsCoach = 1 ORDER BY Id);
                IF @coachId IS NOT NULL
                BEGIN
                    UPDATE BlogPosts SET AuthorId = @coachId WHERE AuthorId IS NULL;
                    UPDATE AspNetUsers SET Bio = N'10 yılı aşkın tecrübesiyle yüzlerce kişinin fiziksel ve mental dönüşümüne rehberlik etmiş, bilimsel temelli antrenman ve beslenme prensiplerini savunan baş antrenör.'
                        WHERE Id = @coachId AND (Bio IS NULL OR Bio = N'');
                    UPDATE AspNetUsers SET AvatarPath = N'/images/yazar-ibrahim-emir.jpg'
                        WHERE Id = @coachId AND (AvatarPath IS NULL OR AvatarPath = N'');
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BlogPostTags",
                keyColumns: new[] { "BlogPostId", "BlogTagId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "BlogPostTags",
                keyColumns: new[] { "BlogPostId", "BlogTagId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "BlogPostTags",
                keyColumns: new[] { "BlogPostId", "BlogTagId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "BlogPostTags",
                keyColumns: new[] { "BlogPostId", "BlogTagId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "SiteStats",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SiteStats",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SiteStats",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SocialPosts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SocialPosts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SocialPosts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SocialPosts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Testimonials",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "TransformationMetrics",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BlogTags",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BlogTags",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BlogTags",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BlogTags",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Transformations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Transformations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Transformations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Transformations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Transformations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Transformations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Transformations",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Transformations",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Transformations",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Transformations",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Transformations",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "TransformationTypes",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
