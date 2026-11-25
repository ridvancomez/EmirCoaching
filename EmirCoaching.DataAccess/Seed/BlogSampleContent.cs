namespace EmirCoaching.DataAccess.Seed;

// Front-end blog-detay.html makale gövdesi (etiketler ve yazar kartı ayrı alanlardan gelir).
// Sitedeki tüm blog kartları şu an bu sayfayı açtığı için örnek yazıların hepsi bu gövdeyi taşır; panelden değiştirilir.
public static class BlogSampleContent
{
    public const string Html = """
        <section aria-label="Giriş" class="makale__section" id="giris">
        <p>Fitness dünyasında en çok duyulan kavramlardan biri şüphesiz "kalori açığı"dır. İster yağ yakmak, ister kilo vermek isteyin, hedefinize ulaşmanın matematiksel temeli bu kavrama dayanır. Ancak mesele sadece "az yemek" değildir.</p>
        <p>Vücudunuz bir makine gibi çalışır ve bu makinenin çalışması için enerjiye (kaloriye) ihtiyacı vardır. Eğer bu makineye harcadığından daha az enerji verirseniz, aradaki farkı kapatmak için depoladığı enerjiyi (yağı) kullanmaya başlar. İşte bu sürece kalori açığı diyoruz.</p>
        </section>
        <section aria-labelledby="bmh-title" class="makale__section" id="bazal-metabolizma-hizi">
        <h2 class="makale__title" id="bmh-title">Bazal Metabolizma Hızı (BMH) Nedir?</h2>
        <p>Kalori açığı hesaplamaya başlamadan önce bilmeniz gereken ilk değer Bazal Metabolizma Hızınızdır (BMR). Bu, vücudunuzun tam dinlenme halindeyken (hiçbir şey yapmadan yattığınızda) hayatta kalmak için harcadığı enerjidir.</p>
        <div class="makale-callout">
        <p class="makale-callout__title"><iconify-icon aria-hidden="true" class="makale-callout__icon" icon="mdi:calculator-variant-outline" mode="svg" noobserver></iconify-icon> Mifflin-St Jeor Formülü (Erkekler için)</p>
        <pre class="makale-callout__formula"><code>BMH = (10 × Ağırlık) + (6.25 × Boy) − (5 × Yaş) + 5</code></pre>
        </div>
        </section>
        <section aria-labelledby="acik-title" class="makale__section" id="dogru-acik-miktari">
        <h2 class="makale__title" id="acik-title">Doğru Açık Miktarı Ne Olmalı?</h2>
        <p>En sık yapılan hata, sonucu hızlandırmak için aşırı büyük bir kalori açığı yaratmaktır. Bu durum kas kaybına, enerji düşüklüğüne ve metabolizmanın yavaşlamasına neden olur.</p>
        <ul class="makale__list">
        <li><strong>Hafif Açık (250-300 kcal):</strong> Yavaş ama çok sürdürülebilir. Maksimum kas koruması sağlar.</li>
        <li><strong>Orta Açık (500 kcal):</strong> Haftada yaklaşık 0.5 kg yağ kaybı sağlar. Çoğu kişi için idealdir.</li>
        <li><strong>Yüksek Açık (750+ kcal):</strong> Sadece ciddi obezite durumlarında ve uzman kontrolünde uygulanmalıdır.</li>
        </ul>
        <blockquote class="makale__quote">
        <p>"Hızlı kayıp, kalıcı kayıp değildir. Hedefiniz maraton koşmak olmalı, depar atmak değil."</p>
        </blockquote>
        </section>
        <section aria-labelledby="takip-title" class="makale__section" id="nasil-takip-edecegiz">
        <h2 class="makale__title" id="takip-title">Nasıl Takip Edeceğiz?</h2>
        <p>Hesaplamayı yaptıktan sonra süreci yönetmek işin pratik kısmıdır. Şunlara dikkat etmelisiniz:</p>
        <ol class="makale__list makale__list--ordered">
        <li>Mutfağınıza hassas bir dijital tartı alın. Göz kararı porsiyonlar genellikle yanıltıcıdır.</li>
        <li>Güvenilir bir kalori takip uygulaması indirin ve her öğünü kaydedin.</li>
        <li>Sıvı kalorileri (meyve suları, şekerli kahveler) gözden kaçırmayın.</li>
        <li>Haftalık tartılın ve ölçü alın; günlük dalgalanmalara takılmayın.</li>
        </ol>
        <div class="makale-callout makale-callout--dikkat" role="note">
        <p class="makale-callout__title"><iconify-icon aria-hidden="true" class="makale-callout__icon" icon="mdi:alert-outline" mode="svg" noobserver></iconify-icon> Dikkat</p>
        <p class="makale-callout__text">Kadınlar 1200 kcal, erkekler 1500 kcal altına kesinlikle inmemelidir. Bu değerlerin altı, mikro besin eksikliklerine ve hormonal bozukluklara yol açar.</p>
        </div>
        </section>
        """;
}
