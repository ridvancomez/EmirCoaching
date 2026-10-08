// Paketler sayfası — hero filter bar
// Filtre animasyonu js/card-filter.js'teki ortak filterCards ile yapılır.
// Adresteki kategori (paketler.html?kategori=gumus|altin) sayfa açılırken seçili gelir;
// diyet sayfalarındaki Gümüş/Altın sekmeleri bu linklerle gelir.
document.addEventListener("DOMContentLoaded", () => {
  const tabs = document.querySelectorAll(".paketler-hero__tab");
  if (!tabs.length) return;

  const PARAM = "kategori";
  const cards = [...document.querySelectorAll("[data-category]")];

  const activate = (tab) => {
    tabs.forEach((other) => {
      const isActive = other === tab;
      other.classList.toggle("paketler-hero__tab--active", isActive);
      other.setAttribute("aria-selected", String(isActive));
    });
  };

  const matches = (category) => (card) => category === "tumu" || card.dataset.category === category;

  tabs.forEach((tab) => {
    tab.addEventListener("click", () => {
      activate(tab);
      filterCards(cards, matches(tab.dataset.tab));
    });
  });

  // Paylaşılan link: filtre animasyonsuz, doğrudan uygulanır
  const initial = new URLSearchParams(location.search).get(PARAM);
  const initialTab = [...tabs].find((tab) => tab.dataset.tab === initial);
  if (initialTab) {
    activate(initialTab);
    const show = matches(initial);
    cards.forEach((card) => {
      card.hidden = !show(card);
    });
  }
});
