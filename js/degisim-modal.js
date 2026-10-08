// Değişim hikayesi popup'ı (Değişimler, Değişimler Detay)
// [data-hikaye-open] tetikleyicisi ("Hikayeyi Oku" / büyüt ikonu) kendi kartının verisini
// okur: önce/sonra görselleri, isim, etiketler, metrikler ve <template> içindeki uzun hikaye.
// Galeri görselleri (.degisim-galeri) aynı veriyi kendi <template class="degisim-galeri__hikaye">'sinde taşır.
// Aç/kapat kalıbı privacy/contact modallarıyla aynı (--visible / --open, aria-hidden,
// gövde kaydırma kilidi). Esc, overlay ve X ile kapanır; odak modalda tutulur ve
// kapanınca tetikleyen öğeye döner. Büyük slider ortak bileşendir (js/compare-slider.js).
document.addEventListener("DOMContentLoaded", () => {
  const modal = document.getElementById("degisim-modal");
  if (!modal) return;

  const box = modal.querySelector(".degisim-modal__box");
  const compare = modal.querySelector("[data-compare]");
  const [imgBefore, imgAfter] = compare.querySelectorAll(".compare__img");
  const range = compare.querySelector(".compare__range");
  const rangeLabel = compare.querySelector("label");
  const nameEl = modal.querySelector(".degisim-modal__name");
  const tagsEl = modal.querySelector(".degisim-modal__tags");
  const storyEl = modal.querySelector(".degisim-modal__story");
  const metricsEl = modal.querySelector(".degisim-modal__metrics");
  let lastFocus = null;
  let closeTimer = null;

  const focusables = () => [...box.querySelectorAll("button, [href], input, [tabindex]:not([tabindex='-1'])")]
    .filter((el) => !el.disabled && el.offsetParent !== null);

  // Kaynak: kart (.degisim-card) ya da galeri görseli (içindeki <template> veriyi taşır)
  const fill = (source) => {
    const data = source.querySelector("template.degisim-galeri__hikaye")?.content || source;
    const [srcBefore, srcAfter] = source.querySelectorAll(".compare__img");
    const name = data.querySelector(".degisim-card__name").textContent.trim();
    [[imgBefore, srcBefore], [imgAfter, srcAfter]].forEach(([img, src]) => {
      img.src = src.src;
      img.alt = src.alt;
      img.title = src.title;
      img.style.objectPosition = getComputedStyle(src).objectPosition; // kaynaktaki kırpım
    });
    rangeLabel.textContent = `${name}: öncesi ve sonrası karşılaştırma`;
    nameEl.textContent = name;
    tagsEl.innerHTML = data.querySelector(".degisim-card__tags").innerHTML;
    metricsEl.innerHTML = data.querySelector(".degisim-card__metrics").innerHTML;
    const story = data.querySelector(".degisim-card__story");
    storyEl.innerHTML = story ? story.innerHTML : data.querySelector(".degisim-card__quote").innerHTML;
  };

  const open = (trigger, source = trigger.closest(".degisim-card")) => {
    if (!source) return;
    clearTimeout(closeTimer);
    lastFocus = trigger;
    fill(source);
    modal.classList.add("degisim-modal--visible");
    modal.setAttribute("aria-hidden", "false");
    document.body.style.overflow = "hidden";
    // Slider ortada başlasın; görünür olunca etiket durumları yeniden hesaplanır
    range.value = 50;
    range.dispatchEvent(new Event("input"));
    requestAnimationFrame(() => requestAnimationFrame(() => {
      modal.classList.add("degisim-modal--open");
      modal.querySelector(".degisim-modal__close").focus();
    }));
  };

  const close = () => {
    if (!modal.classList.contains("degisim-modal--visible")) return;
    modal.classList.remove("degisim-modal--open");
    modal.setAttribute("aria-hidden", "true");
    document.body.style.overflow = "";
    closeTimer = setTimeout(() => modal.classList.remove("degisim-modal--visible"), 300);
    if (lastFocus) lastFocus.focus();
  };

  document.addEventListener("click", (event) => {
    const trigger = event.target.closest("[data-hikaye-open]");
    if (trigger) open(trigger);
  });
  // Görsele dokunuş (js/compare-slider.js data-compare-tap). Kartta odak kapanınca
  // "Hikayeyi Oku" butonuna, galeride görselin kaydırıcısına döner
  const galleryFigure = (el) => el.closest(".degisim-galeri [data-compare]");
  document.addEventListener("compare:tap", (event) => {
    const card = event.target.closest(".degisim-card");
    if (card) {
      open(card.querySelector("[data-hikaye-open]") || event.target, card);
      return;
    }
    const figure = galleryFigure(event.target);
    if (figure) open(figure.querySelector(".compare__range"), figure);
  });
  // Klavye: galeri kaydırıcısında Enter popup'ı açar
  document.addEventListener("keydown", (event) => {
    if (event.key !== "Enter" || !event.target.matches(".compare__range")) return;
    const figure = galleryFigure(event.target);
    if (figure) open(event.target, figure);
  });
  modal.querySelectorAll("[data-degisim-close]").forEach((el) => el.addEventListener("click", close));

  document.addEventListener("keydown", (event) => {
    if (!modal.classList.contains("degisim-modal--visible")) return;
    if (event.key === "Escape") {
      close();
      return;
    }
    // Odak kapanı: Tab modalın dışına çıkmaz
    if (event.key === "Tab") {
      const items = focusables();
      if (!items.length) return;
      const first = items[0];
      const last = items[items.length - 1];
      if (event.shiftKey && document.activeElement === first) {
        event.preventDefault();
        last.focus();
      } else if (!event.shiftKey && document.activeElement === last) {
        event.preventDefault();
        first.focus();
      }
    }
  });
});
