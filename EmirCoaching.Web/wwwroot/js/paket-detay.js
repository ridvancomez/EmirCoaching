// Paket Detay — seçilen paketi gösterir (?paket=temel|orta|premium&yer=salonda|evde)
// Kartlardaki "Satın Al" bu sayfaya gelir; buradaki "Satın Al" seçimi forma taşır.
// Parametre yoksa / hatalıysa HTML'deki varsayılan (Dönüşüm Protokolü, Salonda) kalır.
// Backend notu: Sunucu bu alanları şablonda kendisi basacağı zaman bu dosya kaldırılabilir.
// paket-detay-moduller.js'ten ÖNCE yüklenir; modül sayıları Zoom satırı ayarlandıktan sonra hesaplanır.
(function () {
  var PAKETLER = {
    temel: { ad: "Başlangıç & Deneme", sure: 3, program: "Başlangıç Programı", zoom: false, fiyat: { salonda: "8.999", evde: "5.999" } },
    orta: { ad: "Dönüşüm Protokolü", sure: 6, program: "Dönüşüm Programı", zoom: true, fiyat: { salonda: "15.999", evde: "10.999" } },
    premium: { ad: "VIP Yaşam Tarzı", sure: 12, program: "Yaşam Tarzı Programı", zoom: true, fiyat: { salonda: "27.999", evde: "18.999" } }
  };

  var params = new URLSearchParams(location.search);
  var kod = PAKETLER[params.get("paket")] ? params.get("paket") : "orta";
  var yer = params.get("yer") === "evde" ? "evde" : "salonda";
  var p = PAKETLER[kod];
  var evde = yer === "evde";
  var adYer = p.ad + (evde ? " (Evde)" : "");

  function setText(sel, text) {
    var el = document.querySelector(sel);
    if (el) el.textContent = text;
  }

  // Sayfa başlığı ve açıklama
  document.title = adYer + " Paket Detayı | Emir Coaching — " + p.sure + " Aylık " + p.program;
  var desc = document.querySelector('meta[name="description"]');
  if (desc) {
    desc.setAttribute("content", "Emir Coaching " + adYer + ": " + (evde ? "evde antrenmanla " : "") + p.sure +
      " aylık program. Antrenman, beslenme, takip ve koçluk modülleri, WhatsApp ile 7/24 iletişim.");
  }

  // Evde paketler de mavi kalır (Paketler sayfasındaki karar). Yeşil tema istenirse:
  // if (evde) document.body.classList.add("tema-evde");

  // Hero
  setText(".paketler-hero__lead", "Hedeflerinize ulaşmanız için özel olarak tasarlanmış, bilimsel temellere dayanan " +
    p.sure + " aylık kapsamlı gelişim planı.");
  var heroImg = document.querySelector(".paketler-hero__bg");
  if (heroImg) heroImg.setAttribute("title", "Emir Coaching " + p.ad);
  var crumb = document.querySelector(".paketler-hero__breadcrumb-link[aria-current]");
  if (crumb) crumb.setAttribute("href", "paket-detay.html?paket=" + kod + "&yer=" + yer);

  // Fiyat kartı
  setText(".fiyat-karti__title", p.ad);
  setText(".fiyat-karti__subtitle", p.sure + " Aylık " + p.program + (evde ? " · Evde" : ""));
  setText(".fiyat-karti__price", "₺ " + p.fiyat[yer]);
  var buy = document.querySelector(".fiyat-karti__button");
  if (buy) {
    buy.setAttribute("href", "diyet-formu.html?paket=" + kod + "&yer=" + yer);
    buy.setAttribute("title", adYer + " paketi için bilgi formuna geç");
  }

  // Zoom satırı: 3 aylık pakette kilitli ("6 ve 12 Aylık Pakette"), sayılmaz
  if (!p.zoom) {
    var ozel = document.querySelector(".modul-card__item--ozel");
    if (ozel) {
      ozel.classList.remove("modul-card__item--ozel");
      ozel.classList.add("modul-card__item--kilitli");
      var ikon = ozel.querySelector(".modul-card__check");
      if (ikon) ikon.setAttribute("icon", "mdi:lock-outline");
      var etiket = ozel.querySelector(".modul-card__ozel-label");
      if (etiket) etiket.textContent = "6 ve 12 Aylık Pakette";
    }
  }
})();
