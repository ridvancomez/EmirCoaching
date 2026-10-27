// Değişimler — "Tüm Değişimler" carousel
// Oklar birer kart kaydırır; her nokta bir konumdur (konum sayısı = kart − görünen + 1).
// Görünen kart sayısı CSS'teki --per-view'den okunur (genişliğe göre 3 / 2 / 1).
// Dokunmatikte yatay kaydırma hareketi de kartları kaydırır; önce/sonra görselinin
// üzerindeki sürükleme bileşene bırakılır (js/compare-slider.js).
document.addEventListener("DOMContentLoaded", () => {
  document.querySelectorAll("[data-carousel]").forEach((carousel) => {
    const viewport = carousel.querySelector(".degisim-carousel__viewport");
    const track = carousel.querySelector(".degisim-carousel__track");
    // Filtrede gizlenen kartlar ([hidden]) kaydırmaya katılmaz
    const allCards = () => [...track.children];
    const cards = () => allCards().filter((card) => !card.hidden);
    const prev = carousel.querySelector("[data-carousel-prev]");
    const next = carousel.querySelector("[data-carousel-next]");
    const dotsWrap = carousel.querySelector("[data-carousel-dots]");
    let index = 0;

    const perView = () => parseInt(getComputedStyle(carousel).getPropertyValue("--per-view"), 10) || 1;
    const positions = () => Math.max(1, cards().length - perView() + 1);

    const renderDots = () => {
      const count = positions();
      dotsWrap.innerHTML = "";
      for (let i = 0; i < count; i++) {
        const dot = document.createElement("button");
        dot.type = "button";
        dot.className = "degisim-carousel__dot";
        dot.setAttribute("aria-label", `${i + 1}. konuma git`);
        dot.title = `${i + 1}. konuma git`;
        dot.addEventListener("click", () => goTo(i));
        dotsWrap.appendChild(dot);
      }
    };

    const update = () => {
      const count = positions();
      index = Math.min(index, count - 1);
      const list = cards();
      const step = list[1] ? list[1].offsetLeft - list[0].offsetLeft : 0;
      track.style.transform = `translateX(${-index * step}px)`;
      prev.disabled = index === 0;
      next.disabled = index >= count - 1;
      [...dotsWrap.children].forEach((dot, i) => {
        const isActive = i === index;
        dot.classList.toggle("degisim-carousel__dot--active", isActive);
        if (isActive) dot.setAttribute("aria-current", "true");
        else dot.removeAttribute("aria-current");
      });
      // Görünmeyen kartlar klavye sırasından çıkar
      allCards().forEach((card) => {
        const i = list.indexOf(card);
        card.inert = i < index || i >= index + perView();
      });
    };

    const goTo = (i) => {
      index = Math.max(0, Math.min(i, positions() - 1));
      update();
    };

    prev.addEventListener("click", () => goTo(index - 1));
    next.addEventListener("click", () => goTo(index + 1));

    // Basit dokunmatik kaydırma (önce/sonra görseli hariç)
    let startX = null;
    viewport.addEventListener("pointerdown", (event) => {
      if (event.pointerType === "mouse" || event.target.closest("[data-compare]")) return;
      startX = event.clientX;
    });
    viewport.addEventListener("pointerup", (event) => {
      if (startX === null) return;
      const dx = event.clientX - startX;
      startX = null;
      if (Math.abs(dx) > 50) goTo(index + (dx < 0 ? 1 : -1));
    });
    viewport.addEventListener("pointercancel", () => { startX = null; });

    let lastCount = positions();
    window.addEventListener("resize", () => {
      if (positions() !== lastCount) {
        lastCount = positions();
        renderDots();
      }
      update();
    });

    // Filtre/sıralama sonrası (degisimler-detay.js) başa dön ve yeniden hesapla
    carousel.addEventListener("degisim:refresh", () => {
      index = 0;
      lastCount = positions();
      renderDots();
      update();
    });

    renderDots();
    update();
  });
});
