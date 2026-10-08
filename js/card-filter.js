// Ortak kart filtresi (Paketler, Blog)
// Isotope tarzı filtre animasyonu: gizlenen kart yerinde küçülüp kaybolur,
// kalan kartlar yeni yerlerine kayar, gelen kart büyüyerek belirir.
// Kartların ortak kabı (parentElement) position: relative olmalıdır.
window.filterCards = (() => {
  const DURATION = 400;
  const EASING = "ease";
  // Kap başına, animasyonu süren (gizlenmekte olan) kartlar
  const leavingByContainer = new WeakMap();

  const resetLeaving = (card) => {
    card.hidden = true;
    ["position", "left", "top", "width", "height", "margin"].forEach((prop) => {
      card.style[prop] = "";
    });
  };

  return (cards, shouldShow) => {
    if (!cards.length) return;
    const container = cards[0].parentElement;

    // Önceki filtrenin yarım kalan animasyonlarını bitir
    cards.forEach((card) => {
      card.getAnimations()
        .filter((animation) => animation.constructor === Animation)
        .forEach((animation) => animation.cancel());
    });
    (leavingByContainer.get(container) || []).forEach(resetLeaving);
    let leavingCards = [];
    leavingByContainer.set(container, leavingCards);

    // İlk konumlar (FLIP: First)
    const containerRect = container.getBoundingClientRect();
    const firstRects = new Map();
    cards.forEach((card) => {
      if (!card.hidden) firstRects.set(card, card.getBoundingClientRect());
    });

    // Gizlenecek kartlar akıştan çıkar ama bulundukları yerde kalır
    cards.forEach((card) => {
      if (card.hidden || shouldShow(card)) return;
      const rect = firstRects.get(card);
      Object.assign(card.style, {
        position: "absolute",
        left: `${rect.left - containerRect.left}px`,
        top: `${rect.top - containerRect.top}px`,
        width: `${rect.width}px`,
        height: `${rect.height}px`,
        margin: "0",
      });
      leavingCards.push(card);
    });

    const enteringCards = cards.filter((card) => card.hidden && shouldShow(card));
    enteringCards.forEach((card) => {
      card.hidden = false;
    });

    const options = { duration: DURATION, easing: EASING };

    cards.forEach((card) => {
      if (leavingCards.includes(card)) {
        card.animate(
          [
            { transform: "scale(1)", opacity: 1 },
            { transform: "scale(0.001)", opacity: 0 },
          ],
          options
        ).onfinish = () => {
          if (!leavingCards.includes(card)) return;
          resetLeaving(card);
          leavingCards.splice(leavingCards.indexOf(card), 1);
        };
      } else if (enteringCards.includes(card)) {
        card.animate(
          [
            { transform: "scale(0.001)", opacity: 0 },
            { transform: "scale(1)", opacity: 1 },
          ],
          options
        );
      } else if (firstRects.has(card)) {
        // Kalan kart: eski yerinden yeni yerine kayar (FLIP: Last, Invert, Play)
        const first = firstRects.get(card);
        const last = card.getBoundingClientRect();
        const dx = first.left - last.left;
        const dy = first.top - last.top;
        if (dx || dy) {
          card.animate(
            [
              { transform: `translate(${dx}px, ${dy}px)` },
              { transform: "translate(0, 0)" },
            ],
            options
          );
        }
      }
    });
  };
})();
