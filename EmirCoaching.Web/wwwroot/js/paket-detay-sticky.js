// Paket Detay (Alternatif 2) — ekrandan uzun fiyat kartı alttan yapışır
// Kartın yüksekliği --fiyat-karti-h olarak CSS'e verilir; CSS sticky üst değerini
// min(navbar altı, ekran yüksekliği − kart yüksekliği − 16px) yapar. Böylece kart
// ekrana sığmıyorsa önce sonuna kadar kayar, alt kenarı (WhatsApp butonu)
// görününce yapışır.
document.addEventListener("DOMContentLoaded", () => {
  const card = document.querySelector(".fiyat-karti");
  if (!card || !("ResizeObserver" in window)) return;

  const update = () => card.style.setProperty("--fiyat-karti-h", `${card.offsetHeight}px`);
  new ResizeObserver(update).observe(card);
  update();
});
