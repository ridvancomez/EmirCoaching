// Blog sayfası — kategori filtresi
// Pill'ler gerçek linktir (blog.html?kategori=...): paylaşılabilir, JS yoksa da çalışır.
// JS tıklamayı yakalar, sayfa yenilenmeden "Tüm Yazılar" kartlarını
// js/card-filter.js'teki filterCards ile süzer ve adresi history.pushState ile günceller.
// Tasarımda "Tümü" pill'i yok: aktif pill'e tekrar tıklamak filtreyi kaldırır.
// Öne çıkan yazı filtreden etkilenmez (her zaman görünür).
document.addEventListener("DOMContentLoaded", () => {
  const PARAM = "kategori";
  const pills = [...document.querySelectorAll(".blog-filters__pill")];
  const cards = [...document.querySelectorAll(".blog-card")];
  const emptyMessage = document.querySelector(".blog-posts__empty");
  if (!pills.length || !cards.length) return;

  const categories = pills.map((pill) => pill.dataset.category);

  // Adresteki kategori; geçersiz veya yoksa null (tüm yazılar)
  const categoryFromUrl = () => {
    const value = new URLSearchParams(location.search).get(PARAM);
    return categories.includes(value) ? value : null;
  };

  const applyFilter = (category, animate) => {
    pills.forEach((pill) => {
      const isActive = pill.dataset.category === category;
      pill.classList.toggle("blog-filters__pill--active", isActive);
      if (isActive) pill.setAttribute("aria-current", "page");
      else pill.removeAttribute("aria-current");
    });

    const shouldShow = (card) => !category || card.dataset.category === category;
    if (animate) {
      filterCards(cards, shouldShow);
    } else {
      cards.forEach((card) => {
        card.hidden = !shouldShow(card);
      });
    }
    if (emptyMessage) emptyMessage.hidden = cards.some(shouldShow);
  };

  pills.forEach((pill) => {
    pill.addEventListener("click", (event) => {
      // Yeni sekmede açma (Ctrl/Cmd/Shift/orta tık) tarayıcıya bırakılır
      if (event.button !== 0 || event.ctrlKey || event.metaKey || event.shiftKey || event.altKey) return;
      event.preventDefault();

      const isDeselect = pill.classList.contains("blog-filters__pill--active");
      const category = isDeselect ? null : pill.dataset.category;

      const url = new URL(location.href);
      if (category) url.searchParams.set(PARAM, category);
      else url.searchParams.delete(PARAM);
      history.pushState({ category }, "", url);

      applyFilter(category, true);
    });
  });

  // Geri / ileri tuşları filtreyi değiştirir
  window.addEventListener("popstate", () => applyFilter(categoryFromUrl(), true));

  // Paylaşılan link (blog.html?kategori=...) filtre seçili açılır
  applyFilter(categoryFromUrl(), false);
});
