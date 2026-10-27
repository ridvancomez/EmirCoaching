// Değişimler Detay — kategori filtresi ve sıralama
// Filtre pill'leri karttaki data-category'ye göre gizler (Tümü = hepsi); sıralama kartları
// yeniden dizer (En Yeniler = HTML'deki sıra, En Eskiler = tersi). Carousel'e
// "degisim:refresh" olayıyla haber verilir (js/degisimler.js). Carousel'de
// data-filter-animate varsa filtre paketler.html'deki gibi animasyonludur.
document.addEventListener("DOMContentLoaded", () => {
  const carousel = document.querySelector("[data-carousel]");
  const track = carousel && carousel.querySelector(".degisim-carousel__track");
  const pills = [...document.querySelectorAll(".degisim-filtre__pill")];
  const sort = document.querySelector(".degisim-toolbar__sort-select");
  const empty = document.querySelector(".degisim-toolbar__empty");
  if (!track) return;

  const original = [...track.children]; // HTML sırası = en yeniden eskiye

  const refresh = () => carousel.dispatchEvent(new CustomEvent("degisim:refresh"));

  // data-filter-animate: paketler.html'deki filtre animasyonu (js/card-filter.js).
  // Önce carousel başa alınır (geçişsiz), kartlar yerinde küçülüp/büyüyüp kayar;
  // animasyon bitince carousel noktaları ve okları yeniden hesaplanır.
  const animate = carousel.hasAttribute("data-filter-animate") && typeof window.filterCards === "function";
  let settleTimer = null;
  const animateFilter = (shouldShow) => {
    track.style.transition = "none";
    track.style.transform = "translateX(0px)";
    void track.offsetWidth;
    original.forEach((card) => {
      card.inert = false;
    });
    window.filterCards(original, shouldShow);
    // Animasyonlar bitip gizlenen kartlar [hidden] olunca carousel güncellenir. "finish"
    // olayı beklenir (anim.finished promise'i card-filter'ın onfinish'inden önce çözülür;
    // kareler yavaşken kartlar henüz gizlenmemiş olurdu)
    const ended = original.flatMap((card) => card.getAnimations()).map((anim) => new Promise((resolve) => {
      anim.addEventListener("finish", resolve, { once: true });
      anim.addEventListener("cancel", resolve, { once: true });
    }));
    const run = (settleTimer = {});
    Promise.all(ended).then(() => {
      if (settleTimer !== run) return; // arada yeni filtre seçildiyse eskisini atla
      track.style.transition = "";
      refresh();
    });
  };

  pills.forEach((pill) => {
    pill.addEventListener("click", () => {
      const category = pill.dataset.filter;
      pills.forEach((other) => {
        const isActive = other === pill;
        other.classList.toggle("degisim-filtre__pill--active", isActive);
        other.setAttribute("aria-pressed", String(isActive));
      });
      const shouldShow = (card) => category === "tumu" || card.dataset.category === category;
      if (empty) empty.hidden = original.some(shouldShow);
      if (animate) {
        animateFilter(shouldShow);
        return;
      }
      original.forEach((card) => {
        card.hidden = !shouldShow(card);
      });
      refresh();
    });
  });

  if (sort) {
    sort.addEventListener("change", () => {
      const order = sort.value === "en-eskiler" ? [...original].reverse() : original;
      order.forEach((card) => track.appendChild(card));
      refresh();
    });
  }
});
