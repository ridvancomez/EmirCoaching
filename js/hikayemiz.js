// Hikayemiz — "Perdenin Arkası" tanıtım videosu modalı
// Video adresi oynat butonundaki data-video-src'den okunur (YouTube embed URL'si).
// Adres boşken modal "yakında" mesajını gösterir.
document.addEventListener("DOMContentLoaded", () => {
  const modal = document.getElementById("video-modal");
  const openers = document.querySelectorAll("[data-video-open]");
  if (!modal || !openers.length) return;

  const frame = modal.querySelector(".video-modal__frame");
  const placeholder = frame.innerHTML;
  const closers = modal.querySelectorAll("[data-video-close]");
  let lastFocus = null;

  const open = (opener) => {
    const src = opener.dataset.videoSrc;
    lastFocus = opener; // kapanınca odak oynat butonuna döner
    if (src) {
      frame.innerHTML = `<iframe class="video-modal__iframe" src="${src}${src.includes("?") ? "&" : "?"}autoplay=1"
        title="Emir Coaching tanıtım videosu" allow="autoplay; encrypted-media; picture-in-picture" allowfullscreen></iframe>`;
    }
    modal.classList.add("video-modal--visible");
    modal.setAttribute("aria-hidden", "false");
    document.body.style.overflow = "hidden";
    void modal.offsetWidth; // görünür hali çizilsin ki geçiş animasyonu çalışsın
    modal.classList.add("video-modal--open");
    modal.querySelector(".video-modal__close").focus();
  };

  const close = () => {
    modal.classList.remove("video-modal--open");
    modal.setAttribute("aria-hidden", "true");
    document.body.style.overflow = "";
    setTimeout(() => {
      modal.classList.remove("video-modal--visible");
      frame.innerHTML = placeholder; // iframe kaldırılınca video durur
    }, 300);
    if (lastFocus) lastFocus.focus();
  };

  openers.forEach((button) => button.addEventListener("click", () => open(button)));
  closers.forEach((el) => el.addEventListener("click", close));
  document.addEventListener("keydown", (event) => {
    if (event.key === "Escape" && modal.classList.contains("video-modal--visible")) close();
  });
});
