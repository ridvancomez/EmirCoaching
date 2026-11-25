// Yasal metin sayfası (gizlilik-politikasi.html?tur=kvkk). İçerik panelde editörle girilir ve API'den gelir:
// başlık, sürüm, tarihler, metin, içindekiler ve revizyon geçmişi buradan doldurulur.
// API'ye ulaşılamazsa sayfadaki statik Gizlilik Politikası metni olduğu gibi kalır.
// api.js ve blog-detay.js'ten (initToc, initContentTable) SONRA yüklenir.
(function () {
  // URL'deki tür → API'deki LegalDocumentType adı
  var TURLER = {
    gizlilik: "Gizlilik",
    kvkk: "Kvkk",
    "acik-riza": "AcikRiza",
    cerez: "Cerez",
    "iptal-iade": "IptalIade",
    "ticari-ileti": "TicariIleti",
    "mesafeli-satis": "MesafeliSatis"
  };

  var params = new URLSearchParams(location.search);
  var tur = TURLER[params.get("tur")] ? params.get("tur") : "gizlilik";
  var type = TURLER[tur];

  var article = document.querySelector("[data-legal-content]");
  if (!article || !window.EmirApi) return;

  function formatDate(iso) {
    return new Date(iso).toLocaleDateString("tr-TR", { day: "2-digit", month: "long", year: "numeric" });
  }

  function shortDate(iso) {
    return new Date(iso).toLocaleDateString("tr-TR", { day: "2-digit", month: "2-digit", year: "numeric" });
  }

  function setTime(el, iso) {
    if (!el) return;
    el.innerHTML = "";
    var time = document.createElement("time");
    time.dateTime = iso.slice(0, 10);
    time.textContent = formatDate(iso);
    el.appendChild(time);
  }

  // "Genel Bilgilendirme" → "genel-bilgilendirme" (içindekiler bağlantısı için)
  function slugify(text, used) {
    var map = { ç: "c", ğ: "g", ı: "i", ö: "o", ş: "s", ü: "u" };
    var slug = text.toLocaleLowerCase("tr-TR").replace(/[çğıöşü]/g, function (c) { return map[c]; })
      .replace(/[^a-z0-9]+/g, "-").replace(/^-+|-+$/g, "") || "bolum";
    var unique = slug, i = 2;
    while (used[unique]) unique = slug + "-" + i++;
    used[unique] = true;
    return unique;
  }

  function renderHero(doc, revisions) {
    document.title = doc.title + " | Emir Coaching";
    document.querySelectorAll("[data-legal-title]").forEach(function (el) {
      el.textContent = doc.title;
      if (el.tagName === "A") el.setAttribute("title", doc.title);
    });
    var lead = document.querySelector("[data-legal-lead]");
    if (lead && type !== "Gizlilik") lead.hidden = true; // giriş metni yalnızca Gizlilik Politikası'na ait

    var version = document.querySelector("[data-legal-version]");
    if (version) version.textContent = "v" + doc.version;
    // Yürürlük: bu sürümün yayın tarihi; son güncelleme: en yeni revizyon
    setTime(document.querySelector("[data-legal-effective]"), doc.publishedAt);
    setTime(document.querySelector("[data-legal-updated]"), revisions.length ? revisions[0].revisedAt : doc.publishedAt);

    var download = document.querySelector("[data-legal-download]");
    if (download) {
      if (doc.fileUrl) {
        download.href = doc.fileUrl;
        download.setAttribute("title", doc.title + " PDF olarak indir");
      } else {
        download.hidden = true; // panelde girilen metinlerin PDF'i yok
      }
    }
  }

  function renderContent(doc) {
    var wrapper = document.createElement("div");
    wrapper.className = "yasal-icerik";
    if (doc.contentHtml) {
      wrapper.innerHTML = doc.contentHtml;
    } else if (doc.fileUrl) {
      // İlk sürümler PDF; metin panelden girilene kadar PDF bağlantısı gösterilir
      var p = document.createElement("p");
      var a = document.createElement("a");
      a.href = doc.fileUrl;
      a.target = "_blank";
      a.rel = "noopener";
      a.textContent = doc.title + " (PDF)";
      p.append("Bu metnin güncel hali PDF olarak yayında: ", a);
      wrapper.appendChild(p);
    }

    // Revizyon bölümü korunur; önceki metin kaldırılıp yenisi başa eklenir
    var revision = article.querySelector(".gizlilik-revizyon");
    Array.prototype.slice.call(article.children).forEach(function (child) {
      if (child !== revision) child.remove();
    });
    article.insertBefore(wrapper, revision);

    // Sayfa açılışında blog-detay.js tabloları bileşene çevirir; API'den gelen içerik sonradan geldiği için burada tekrarlanır
    if (typeof initContentTable === "function") wrapper.querySelectorAll("table").forEach(initContentTable);
    return wrapper;
  }

  // İçindekiler başlıklardan (h2) yeniden kurulur; eski dinleyiciler kopyalanarak atılır
  function renderToc(wrapper) {
    var nav = document.querySelector(".gizlilik-icindekiler");
    if (!nav) return;
    var headings = wrapper.querySelectorAll("h2");
    if (!headings.length) {
      nav.hidden = true;
      return;
    }

    var used = {};
    var fresh = nav.cloneNode(true);
    var list = fresh.querySelector(".gizlilik-icindekiler__list");
    list.innerHTML = "";
    headings.forEach(function (h, i) {
      if (!h.id) h.id = slugify(h.textContent, used);
      else used[h.id] = true;
      if (!h.classList.contains("gizlilik-bolum__title")) h.classList.add("yasal-icerik__baslik");
      var text = h.textContent.replace(/^\s*\d+\.\s*/, "").trim(); // "1. Genel" → "Genel" (numara ayrıca yazılır)
      var li = document.createElement("li");
      var a = document.createElement("a");
      a.href = "#" + h.id;
      a.className = "gizlilik-icindekiler__link";
      a.title = text + " bölümüne git";
      var no = document.createElement("span");
      no.className = "gizlilik-icindekiler__no";
      no.textContent = String(i + 1).padStart(2, "0");
      a.append(no, " " + text);
      li.appendChild(a);
      list.appendChild(li);
    });
    nav.replaceWith(fresh);
    if (typeof initToc === "function") initToc(fresh);
  }

  function renderRevisions(revisions) {
    var list = document.querySelector(".gizlilik-revizyon__list");
    if (!list) return;
    list.innerHTML = "";
    revisions.forEach(function (r) {
      var li = document.createElement("li");
      var time = document.createElement("time");
      time.dateTime = r.revisedAt.slice(0, 10);
      time.textContent = "[" + shortDate(r.revisedAt) + "]";
      li.append(time, " v" + r.version + " - " + r.note);
      list.appendChild(li);
    });
    list.closest(".gizlilik-revizyon").hidden = revisions.length === 0;
  }

  // Kenar çubuğunda açık olan belge işaretlenir
  document.querySelectorAll(".gizlilik-belgeler__link").forEach(function (link) {
    if (new URL(link.href).searchParams.get("tur") === tur) link.setAttribute("aria-current", "page");
  });

  Promise.all([
    EmirApi.get("/legal-documents/" + type),
    EmirApi.get("/legal-documents/" + type + "/revisions")
  ]).then(function (results) {
    var doc = results[0];
    var revisions = results[1] || [];
    if (!doc) {
      // Henüz yayınlanmamış metin (ör. Mesafeli Satış Sözleşmesi)
      if (type !== "Gizlilik") {
        renderHero({ title: "Belge henüz yayınlanmadı", version: 0, publishedAt: new Date().toISOString() }, []);
        var empty = renderContent({});
        empty.innerHTML = "<p>Bu metin hazırlanıyor. Sorularınız için legal@emircoaching.com adresine yazabilirsiniz.</p>";
        renderToc(empty);
        renderRevisions([]);
      }
      return;
    }
    renderHero(doc, revisions);
    renderToc(renderContent(doc));
    renderRevisions(revisions);
  }).catch(function () {
    // API'ye ulaşılamadı: statik metin kalır
  });
})();
