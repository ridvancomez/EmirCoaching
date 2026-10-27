// Paket Detay — modül sayıları DOM'dan hesaplanır (hardcode edilmez)
// Her kartın "X Modül" rozeti o karttaki madde sayısını, alttaki
// "Toplam X kalem içerik" barı dört kartın toplamını gösterir. Bir madde
// (ör. pakete özel Zoom satırı) basılmazsa sayılar kendiliğinden düzelir.
// Kilitli satır (--kilitli: "6 ve 12 Aylık Pakette") pakete dahil değildir, sayılmaz.
document.addEventListener("DOMContentLoaded", () => {
  const grid = document.querySelector(".paket-detay__grid");
  if (!grid) return;

  let total = 0;
  grid.querySelectorAll(".modul-card").forEach((card) => {
    const count = card.querySelectorAll(".modul-card__item:not(.modul-card__item--kilitli)").length;
    const badge = card.querySelector(".modul-card__badge");
    if (badge) badge.textContent = `${count} Modül`;
    total += count;
  });

  const totalCount = document.querySelector(".paket-detay__total-count");
  if (totalCount) totalCount.textContent = total;
});
