// Ortak önce/sonra karşılaştırma bileşeni (Hikayemiz, Değişimler)
// Kart üzerinde sürükleme (fare/dokunma) veya klavye (görünmez range) --pos'u
// değiştirir; üst katman ve bölücü bu değere göre kayar. Her kart bağımsızdır.
// Dokunmatikte touch-action: pan-y sayesinde dikey kaydırma bozulmaz.
// Etiketler kendi tarafına sığmazsa gizlenir: "öncesi" bölücünün solunda,
// "sonrası" bölücünün sağında ve kartın içinde kalmalı (etiket bölücüyle kaysa da,
// köşede sabit dursa da aynı kural).
window.initCompareSliders = (root = document) => {
  const LABEL_GAP = 8; // etiket ile bölücü / kart kenarı arasında kalması gereken en az boşluk

  root.querySelectorAll("[data-compare]").forEach((compare) => {
    if (compare.dataset.compareReady) return;
    compare.dataset.compareReady = "true";

    const range = compare.querySelector(".compare__range");
    const labelBefore = compare.querySelector(".compare__label--before");
    const labelAfter = compare.querySelector(".compare__label--after");

    const updateLabels = (percent) => {
      const width = compare.clientWidth;
      const pos = (width * percent) / 100;
      if (labelBefore) {
        const right = labelBefore.offsetLeft + labelBefore.offsetWidth;
        labelBefore.classList.toggle("compare__label--hidden", right > pos - LABEL_GAP);
      }
      if (labelAfter) {
        const left = labelAfter.offsetLeft;
        const right = left + labelAfter.offsetWidth;
        labelAfter.classList.toggle("compare__label--hidden", left < pos + LABEL_GAP || right > width - LABEL_GAP);
      }
    };

    const setPosition = (percent) => {
      const value = Math.round(Math.min(100, Math.max(0, percent)) * 10) / 10;
      compare.style.setProperty("--pos", `${value}%`);
      range.value = value;
      updateLabels(value);
    };

    const percentFromPointer = (event) => {
      const rect = compare.getBoundingClientRect();
      return ((event.clientX - rect.left) / rect.width) * 100;
    };

    // data-compare-tap: "popup" → sürükleme kaydırır, sürüklemeden kısa dokunuş
    // "compare:tap" olayı yayar (popup açılır); "only-popup" → dokunuş her zaman olay
    // yayar, işaretçiyle kaydırma olmaz (klavye kaydırıcısı çalışmaya devam eder).
    const tapMode = compare.dataset.compareTap;
    const DRAG_THRESHOLD = 6;
    let startX = 0;
    let dragging = false;

    compare.addEventListener("pointerdown", (event) => {
      // Bileşen içindeki butonlar (ör. büyüt) kaydırıcıyı oynatmaz
      if (event.button !== 0 || event.target.closest("button, a")) return;
      compare.setPointerCapture(event.pointerId);
      startX = event.clientX;
      dragging = false;
      if (!tapMode) setPosition(percentFromPointer(event));
    });

    compare.addEventListener("pointermove", (event) => {
      if (!compare.hasPointerCapture(event.pointerId) || tapMode === "only-popup") return;
      if (tapMode === "popup" && !dragging && Math.abs(event.clientX - startX) < DRAG_THRESHOLD) return;
      dragging = true;
      setPosition(percentFromPointer(event));
    });

    compare.addEventListener("pointerup", (event) => {
      if (!compare.hasPointerCapture(event.pointerId)) return;
      compare.releasePointerCapture(event.pointerId);
      if (tapMode && !dragging) compare.dispatchEvent(new CustomEvent("compare:tap", { bubbles: true }));
    });

    range.addEventListener("input", () => setPosition(Number(range.value)));
    window.addEventListener("resize", () => updateLabels(Number(range.value)));

    setPosition(Number(range.value));
  });
};

document.addEventListener("DOMContentLoaded", () => window.initCompareSliders());
