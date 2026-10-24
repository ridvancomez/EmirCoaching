namespace EmirCoaching.DataAccess.Seed;

// Kaynak: front-end gizlilik-politikasi.html, article.gizlilik-body__content içindeki bölümler (1-5).
internal static class GizlilikPolitikasiContent
{
    public const string Html = """
<section class="gizlilik-bolum" aria-labelledby="genel-bilgilendirme">
  <h2 id="genel-bilgilendirme" class="gizlilik-bolum__title">
    <span class="gizlilik-bolum__no">1.</span> Genel Bilgilendirme
  </h2>
  <p>
    EMIRCOACHING ("Şirket", "Biz" veya "Bize"), 6698 sayılı Kişisel Verilerin Korunması Kanunu ("KVKK") ve
    ilgili ikincil mevzuat kapsamında Veri Sorumlusu sıfatıyla hareket etmektedir. Bu politika,
    hizmetlerimizi kullanırken sağladığınız bilgilerin nasıl yönetildiğini açıklamaktadır.
  </p>
</section>

<section class="gizlilik-bolum" aria-labelledby="veri-sorumlusu">
  <h2 id="veri-sorumlusu" class="gizlilik-bolum__title">
    <span class="gizlilik-bolum__no">2.</span> Veri Sorumlusu Kimliği
  </h2>
  <p>6698 sayılı KVKK uyarınca muhatabınız olan veri sorumlusunun bilgileri aşağıdadır:</p>
  <dl class="gizlilik-liste gizlilik-liste--tanim">
    <div class="gizlilik-liste__item">
      <dt>Unvan:</dt>
      <dd>EMIRCOACHING Performans Teknolojileri A.Ş.</dd>
    </div>
    <div class="gizlilik-liste__item">
      <dt>MERSİS No:</dt>
      <dd>0123456789012345</dd>
    </div>
    <div class="gizlilik-liste__item">
      <dt>Adres:</dt>
      <dd>Levent Mah. Büyükdere Cad. No:199, Şişli / İstanbul</dd>
    </div>
  </dl>
</section>

<section class="gizlilik-bolum" aria-labelledby="toplanan-veriler">
  <h2 id="toplanan-veriler" class="gizlilik-bolum__title">
    <span class="gizlilik-bolum__no">3.</span> Toplanan Kişisel Veriler
  </h2>
  <p>
    Platformumuz üzerinden sunduğumuz hizmetlerin doğası gereği aşağıdaki veri kategorilerinde kişisel veri
    işlenmektedir:
  </p>
  <!-- CKEditor çıktısı: düz tablo (<figure class="table"><table>…). İlk satır "başlık satırı"
       (thead), ilk sütun "başlık sütunu" (satır <th>) yapılabilir. Bileşen sınıfları, dar ekrandaki
       kart etiketleri (data-label) ve ARIA rolleri js/blog-detay.js ile otomatik eklenir;
       "Tablo özellikleri" eklentisi (sabit genişlik/renk) kapalı olmalı. -->
  <figure class="table">
    <table>
      <thead>
        <tr>
          <th>Veri Kategorisi</th>
          <th>Örnek</th>
          <th>Amaç</th>
          <th>Saklama Süresi</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <th>Kimlik Verisi</th>
          <td>Ad, Soyad, TCKN</td>
          <td>Sözleşme Kurulumu</td>
          <td>10 Yıl</td>
        </tr>
        <tr>
          <th>İletişim Verisi</th>
          <td>E-posta, Telefon</td>
          <td>Müşteri Desteği</td>
          <td>10 Yıl</td>
        </tr>
        <tr>
          <th>Sağlık Verisi (Özel Nitelikli)</th>
          <td>Boy, Kilo, Geçmiş Sakatlıklar</td>
          <td>Program Optimizasyonu</td>
          <td>Üyelik + 2 Yıl</td>
        </tr>
      </tbody>
    </table>
  </figure>
</section>

<section class="gizlilik-bolum" aria-labelledby="isleme-amaclari">
  <h2 id="isleme-amaclari" class="gizlilik-bolum__title">
    <span class="gizlilik-bolum__no">4.</span> Verilerin İşlenme Amaçları
  </h2>
  <p>Toplanan kişisel verileriniz aşağıdaki amaçlarla işlenmektedir:</p>
  <ul class="gizlilik-liste">
    <li class="gizlilik-liste__item">Kişiselleştirilmiş antrenman ve beslenme programlarının oluşturulması.</li>
    <li class="gizlilik-liste__item">Kullanıcı deneyiminin iyileştirilmesi ve uygulama hatalarının giderilmesi.</li>
    <li class="gizlilik-liste__item">Yasal yükümlülüklerin yerine getirilmesi (Fatura düzenlenmesi vb.).</li>
  </ul>
</section>

<section class="gizlilik-bolum" aria-labelledby="veri-sahibi-haklari">
  <h2 id="veri-sahibi-haklari" class="gizlilik-bolum__title">
    <span class="gizlilik-bolum__no">8.</span> Veri Sahibinin Hakları
  </h2>
  <p>
    KVKK Madde 11 uyarınca, kişisel verileriniz üzerinde belirli haklara sahipsiniz. Bu haklar veri işlemeyi
    öğrenme, düzeltme, silme talep etme haklarını içerir.
  </p>
  <div class="gizlilik-callout" role="note" aria-labelledby="basvuru-hakki">
    <iconify-icon icon="mdi:information-outline" mode="svg" noobserver class="gizlilik-callout__icon" aria-hidden="true"></iconify-icon>
    <div class="gizlilik-callout__body">
      <p id="basvuru-hakki" class="gizlilik-callout__title">Başvuru Hakkı</p>
      <p class="gizlilik-callout__text">
        Taleplerinizi sistemimizde kayıtlı bulunan elektronik posta adresiniz üzerinden
        <a href="mailto:kvkk@emircoaching.com" class="gizlilik-callout__link" title="kvkk@emircoaching.com adresine e-posta gönder">kvkk@emircoaching.com</a>
        adresine iletebilirsiniz. Başvurunuz en geç 30 gün içinde ücretsiz olarak sonuçlandırılacaktır.
      </p>
    </div>
  </div>
</section>

""";
}
