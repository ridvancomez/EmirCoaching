// İçindekiler: scrollspy, yumuşak kaydırma ve mobilde açılır-kapanır liste
// (Blog Detay ve Gizlilik Politikası). Sticky davranışı CSS'tedir; burada yalnızca aktif
// bölüm ve kaydırma yönetilir. Varsayılan bileşen .icindekiler (≤759px açılır-kapanır);
// başka bir BEM bloğu data-toc="blok-adi" ve data-toc-mobile="999" ile aynı mantığı kullanır.
const initToc = (toc) => {
  const block = toc.dataset.toc || "icindekiler";
  const toggle = toc.querySelector(`.${block}__toggle`);
  const list = toc.querySelector(`.${block}__list`);
  const links = [...toc.querySelectorAll(`.${block}__link`)];
  const activeClass = `${block}__link--active`;
  const sections = links.map((link) => document.querySelector(link.getAttribute("href")));
  // Bu genişlik ve altında içindekiler içeriğin üstünde açılır-kapanır; daha genişte hep açık
  const mobile = window.matchMedia(`(max-width: ${toc.dataset.tocMobile || 759}px)`);
  const reducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)");

  // Aktif bölüm: üst kenarı navbar'ın biraz altındaki çizgiyi geçmiş son bölüm
  const offset = () => {
    const navbar = document.querySelector(".navbar");
    return (navbar ? navbar.getBoundingClientRect().bottom : 0) + 40;
  };

  const setActive = (index) => {
    links.forEach((link, i) => {
      const isActive = i === index;
      link.classList.toggle(activeClass, isActive);
      if (isActive) link.setAttribute("aria-current", "location");
      else link.removeAttribute("aria-current");
    });
  };

  const updateActive = () => {
    const line = offset();
    let index = 0;
    sections.forEach((section, i) => {
      if (section && section.getBoundingClientRect().top <= line) index = i;
    });
    // Sayfa sonunda son bölüm aktif olsun (kısa son bölüm çizgiye ulaşamayabilir)
    const atBottom = window.innerHeight + window.scrollY >= document.documentElement.scrollHeight - 2;
    const last = sections[sections.length - 1];
    if (atBottom && last && last.getBoundingClientRect().top < window.innerHeight) index = sections.length - 1;
    setActive(index);
  };

  // Birkaç bölüm için hesap ucuz; doğrudan her scroll'da güncellenir
  window.addEventListener("scroll", updateActive, { passive: true });
  window.addEventListener("resize", updateActive);

  // Açılır-kapanır mod; daha genişte liste hep açık, buton devre dışı
  const setOpen = (open) => {
    toggle.setAttribute("aria-expanded", String(open));
    list.hidden = !open;
  };

  // Tıklayınca başlığa yumuşak kaydırma (navbar payı CSS scroll-margin-top ile)
  links.forEach((link, i) => {
    link.addEventListener("click", (event) => {
      const target = sections[i];
      if (!target) return;
      event.preventDefault();
      target.scrollIntoView({ behavior: reducedMotion.matches ? "auto" : "smooth", block: "start" });
      history.replaceState(null, "", link.getAttribute("href"));
      if (mobile.matches) setOpen(false);
    });
  });

  const applyMode = () => {
    toggle.disabled = !mobile.matches;
    setOpen(!mobile.matches);
  };

  toggle.addEventListener("click", () => {
    setOpen(toggle.getAttribute("aria-expanded") !== "true");
  });
  mobile.addEventListener("change", applyMode);

  applyMode();
  updateActive();
};

// Editör (CKEditor) tablosu: içerikteki düz <table> otomatik olarak tablo bileşenine
// (.gizlilik-tablo) dönüşür. CKEditor'ün <figure class="table"> sarmalayıcısı kullanılır,
// yoksa bir sarmalayıcı eklenir. Başlık satırındaki (thead) sütun adları her hücreye
// data-label olarak yazılır; dar alanda tablo kartlara döner ve etiketler değerin
// solunda görünür (CSS). display değişince tablo anlamı kaybolmasın diye ARIA rolleri eklenir.
const initContentTable = (table) => {
  if (table.classList.contains("gizlilik-tablo__table")) return;
  let wrapper = table.closest("figure.table");
  if (!wrapper) {
    wrapper = document.createElement("div");
    table.before(wrapper);
    wrapper.append(table);
  }
  wrapper.classList.add("gizlilik-tablo", "gizlilik-tablo--kart");
  wrapper.setAttribute("role", "region");
  wrapper.tabIndex = 0;
  // Bölge adı: tablonun içinde bulunduğu bölümün başlığı
  const heading = wrapper.closest("section")?.querySelector("h2, h3");
  wrapper.setAttribute("aria-label", heading ? heading.textContent.trim() : "Tablo");

  table.classList.add("gizlilik-tablo__table");
  table.setAttribute("role", "table");
  const headRow = table.tHead?.rows[table.tHead.rows.length - 1];
  const labels = headRow ? [...headRow.cells].map((cell) => cell.textContent.trim()) : [];
  [...table.querySelectorAll("thead, tbody, tfoot")].forEach((group) => group.setAttribute("role", "rowgroup"));
  [...table.rows].forEach((row) => {
    row.setAttribute("role", "row");
    const inHead = row.parentElement.tagName === "THEAD";
    [...row.cells].forEach((cell, i) => {
      if (inHead) {
        cell.setAttribute("role", "columnheader");
        cell.setAttribute("scope", "col");
      } else if (cell.tagName === "TH") {
        cell.setAttribute("role", "rowheader");
        cell.setAttribute("scope", "row");
      } else {
        cell.setAttribute("role", "cell");
        if (labels[i] && !cell.dataset.label) cell.dataset.label = labels[i];
      }
    });
  });
};

document.addEventListener("DOMContentLoaded", () => {
  document.querySelectorAll(".icindekiler, [data-toc]").forEach(initToc);
  document.querySelectorAll(".gizlilik-body__content table, .makale table").forEach(initContentTable);
});
