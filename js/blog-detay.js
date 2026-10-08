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

document.addEventListener("DOMContentLoaded", () => {
  document.querySelectorAll(".icindekiler, [data-toc]").forEach(initToc);
});
