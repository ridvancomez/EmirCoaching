(function () {
  "use strict";

  var lockedModalCount = 0;

  function lockBodyScroll() {
    lockedModalCount++;
    document.body.style.overflow = "hidden";
  }

  function unlockBodyScroll() {
    lockedModalCount = Math.max(0, lockedModalCount - 1);

    if (lockedModalCount === 0) {
      document.body.style.overflow = "";
    }
  }

  function initPrivacyModal() {
    var modal = document.getElementById("privacy-modal");
    var triggers = document.querySelectorAll("[data-privacy-trigger]");

    if (!modal || !triggers.length) {
      return;
    }

    var closeEls = modal.querySelectorAll("[data-privacy-close]");

    function openModal() {
      modal.classList.add("privacy-modal--visible");
      modal.setAttribute("aria-hidden", "false");
      lockBodyScroll();

      requestAnimationFrame(function () {
        requestAnimationFrame(function () {
          modal.classList.add("privacy-modal--open");
        });
      });
    }

    function closeModal() {
      modal.classList.remove("privacy-modal--open");
      modal.setAttribute("aria-hidden", "true");
      unlockBodyScroll();

      setTimeout(function () {
        modal.classList.remove("privacy-modal--visible");
      }, 300);
    }

    triggers.forEach(function (trigger) {
      trigger.addEventListener("click", function (event) {
        event.preventDefault();
        openModal();
      });
    });

    closeEls.forEach(function (el) {
      el.addEventListener("click", function () {
        closeModal();
      });
    });
  }

  function openContactSuccessModal() {
    var modal = document.getElementById("contact-success-modal");

    if (!modal) {
      return;
    }

    modal.classList.add("contact-success-modal--visible");
    modal.setAttribute("aria-hidden", "false");
    lockBodyScroll();

    requestAnimationFrame(function () {
      requestAnimationFrame(function () {
        modal.classList.add("contact-success-modal--open");
      });
    });
  }

  function closeContactSuccessModal() {
    var modal = document.getElementById("contact-success-modal");

    if (!modal) {
      return;
    }

    modal.classList.remove("contact-success-modal--open");
    modal.setAttribute("aria-hidden", "true");
    unlockBodyScroll();

    setTimeout(function () {
      modal.classList.remove("contact-success-modal--visible");
    }, 300);
  }

  function initContactSuccessModal() {
    var modal = document.getElementById("contact-success-modal");

    if (!modal) {
      return;
    }

    var closeEls = modal.querySelectorAll("[data-contact-success-close]");

    closeEls.forEach(function (el) {
      el.addEventListener("click", function () {
        closeContactSuccessModal();
      });
    });
  }

  function initContactModal() {
    var modal = document.getElementById("contact-modal");
    var triggers = document.querySelectorAll("[data-contact-trigger]");

    if (!modal || !triggers.length) {
      return;
    }

    var overlay = modal.querySelector("[data-contact-close]");
    var form = modal.querySelector("#contact-form");
    var selects = modal.querySelectorAll(".contact-modal__select");

    function openModal() {
      modal.classList.add("contact-modal--visible");
      modal.setAttribute("aria-hidden", "false");
      lockBodyScroll();

      requestAnimationFrame(function () {
        requestAnimationFrame(function () {
          modal.classList.add("contact-modal--open");
        });
      });
    }

    function closeModal() {
      modal.classList.remove("contact-modal--open");
      modal.setAttribute("aria-hidden", "true");
      unlockBodyScroll();

      setTimeout(function () {
        modal.classList.remove("contact-modal--visible");
      }, 300);
    }

    triggers.forEach(function (trigger) {
      trigger.addEventListener("click", function (event) {
        event.preventDefault();
        openModal();
      });
    });

    if (overlay) {
      overlay.addEventListener("click", function () {
        closeModal();
      });
    }

    selects.forEach(function (select) {
      var wrap = select.closest(".contact-modal__select-wrap");

      if (!wrap) {
        return;
      }

      select.addEventListener("focus", function () {
        wrap.classList.add("contact-modal__select-wrap--open");
      });

      select.addEventListener("blur", function () {
        wrap.classList.remove("contact-modal__select-wrap--open");
      });

      select.addEventListener("change", function () {
        wrap.classList.remove("contact-modal__select-wrap--open");
      });
    });

    if (form) {
      form.addEventListener("submit", function (event) {
        event.preventDefault();
        closeModal();
        openContactSuccessModal();
        form.reset();
      });
    }
  }

  function initMobileDrawer() {
    var checkbox = document.getElementById("navbar-toggle");
    var toggle = document.querySelector(".navbar__toggle");
    var drawer = document.querySelector(".mobile-drawer");
    var overlay = document.querySelector(".mobile-drawer-overlay");

    if (!checkbox || !toggle || !drawer || !overlay) {
      return;
    }

    var submenuToggle = drawer.querySelector(".mobile-drawer__submenu-toggle");
    var submenu = drawer.querySelector(".mobile-drawer__submenu");
    var mobileQuery = window.matchMedia("(max-width: 999px)");
    var isOpen = false;

    // Label, gizli checkbox'ı tetikler; klavye erişimi için buton gibi davranmasını sağla
    toggle.setAttribute("role", "button");
    toggle.setAttribute("tabindex", "0");
    toggle.setAttribute("aria-controls", "mobile-drawer");
    toggle.setAttribute("aria-expanded", "false");

    function setSubmenu(open) {
      if (!submenuToggle || !submenu) {
        return;
      }

      submenuToggle.setAttribute("aria-expanded", open ? "true" : "false");
      submenu.classList.toggle("mobile-drawer__submenu--open", open);
      submenu.style.maxHeight = open ? submenu.scrollHeight + "px" : "0";
    }

    function setDrawer(open) {
      if (open === isOpen) {
        return;
      }

      isOpen = open;
      checkbox.checked = open;
      toggle.classList.toggle("navbar__toggle--active", open);
      toggle.setAttribute("aria-expanded", open ? "true" : "false");
      drawer.classList.toggle("mobile-drawer--open", open);
      drawer.setAttribute("aria-hidden", open ? "false" : "true");
      overlay.classList.toggle("mobile-drawer-overlay--visible", open);
      document.body.classList.toggle("mobile-drawer-is-open", open);

      if (open) {
        lockBodyScroll();
      } else {
        unlockBodyScroll();
        setSubmenu(false);
      }
    }

    checkbox.addEventListener("change", function () {
      setDrawer(checkbox.checked);
    });

    toggle.addEventListener("keydown", function (event) {
      if (event.key === "Enter" || event.key === " ") {
        event.preventDefault();
        setDrawer(!isOpen);
      }
    });

    overlay.addEventListener("click", function () {
      setDrawer(false);
    });

    document.addEventListener("keydown", function (event) {
      if (event.key === "Escape" && isOpen) {
        setDrawer(false);
        toggle.focus();
      }
    });

    if (submenuToggle) {
      submenuToggle.addEventListener("click", function () {
        setSubmenu(submenuToggle.getAttribute("aria-expanded") !== "true");
      });
    }

    // Masaüstüne geçilirse drawer açık kalıp body scroll'u kilitlemesin
    mobileQuery.addEventListener("change", function (event) {
      if (!event.matches) {
        setDrawer(false);
      }
    });
  }

  // Kaydırma animasyonu: bölüm başlıkları ve kartlar görünür alana girince bir kez
  // aşağıdan süzülerek belirir (CSS: .reveal / .is-visible, @keyframes fade-up).
  // Açılışta zaten ekranda olanlar animasyonsuz kalır (yanıp sönme olmaz);
  // aynı kapsayıcıdaki kardeşler 80ms arayla gelir. Modal ve footer kapsam dışı.
  var REVEAL_SELECTORS = [
    // Bölüm başlıkları
    ".social__title", ".program__title", ".story__title", ".faq__title", ".cta__title",
    ".paket-detay__heading", ".blog-posts__title", ".tum-degisimler__title",
    ".perde-arkasi__title", ".sayilar__title", ".iletisim-sosyal__title",
    ".nasil-calisir__title", ".program-detay__title", ".beslenme__title",
    ".hata-404-linkler__title", ".gizlilik-belgeler__title",
    // Kartlar ve bloklar
    ".program__left", ".program__right", ".story__row", ".qa__slider", ".faq__item",
    ".paketler-card", ".beslenme__box", ".modul-card", ".paket-detay__total", ".fiyat-karti",
    ".uygunluk__card", ".yorum-card", ".blog-featured__card", ".blog-card",
    ".oneri-card", ".yazar-bio", ".sosyal-card", ".sayilar__grid",
    ".program-detay__card", ".nasil-calisir__step", ".hata-404-linkler__card",
    ".gizlilik-bolum", ".gizlilik-belgeler"
  ];
  var REVEAL_STAGGER = 80;
  var REVEAL_MAX_STEPS = 3;

  function initReveal() {
    var reducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;

    if (reducedMotion || !("IntersectionObserver" in window)) {
      return;
    }

    var main = document.querySelector("main");

    if (!main) {
      return;
    }

    var elements = Array.prototype.slice.call(main.querySelectorAll(REVEAL_SELECTORS.join(",")))
      .filter(function (el) {
        // Ekranda olanlar (ör. hero altı) ve iç içe eşleşmelerin içtekileri atlanır
        var rect = el.getBoundingClientRect();
        var inView = rect.top < window.innerHeight && rect.bottom > 0;
        var nested = el.parentElement && el.parentElement.closest(".reveal");

        if (inView || nested) {
          return false;
        }

        el.classList.add("reveal");
        return true;
      });

    if (!elements.length) {
      return;
    }

    // Aynı kapsayıcıdaki kardeşler sırayla gelir
    elements.forEach(function (el) {
      var siblings = Array.prototype.filter.call(el.parentElement.children, function (child) {
        return child.classList.contains("reveal");
      });
      var step = Math.min(siblings.indexOf(el), REVEAL_MAX_STEPS);
      el.style.setProperty("--reveal-delay", step * REVEAL_STAGGER + "ms");
    });

    document.documentElement.classList.add("has-reveal");

    var observer = new IntersectionObserver(function (entries) {
      entries.forEach(function (entry) {
        if (!entry.isIntersecting) {
          return;
        }

        var el = entry.target;
        observer.unobserve(el);
        el.classList.add("is-visible");

        el.addEventListener("animationend", function onEnd(event) {
          if (event.target !== el) {
            return;
          }

          el.removeEventListener("animationend", onEnd);
          el.classList.remove("reveal", "is-visible");
          el.style.removeProperty("--reveal-delay");
        });
      });
    }, { rootMargin: "0px 0px -10% 0px" });

    elements.forEach(function (el) {
      observer.observe(el);
    });
  }

  document.addEventListener("DOMContentLoaded", function () {
    initReveal();
    initMobileDrawer();
    initPrivacyModal();
    initContactModal();
    initContactSuccessModal();
  });

  // Sayfaya özel scriptlerin (ör. diyet-formu.js) ortak başarı modalını kullanabilmesi için
  window.EmirCoaching = window.EmirCoaching || {};
  window.EmirCoaching.openSuccessModal = openContactSuccessModal;
})();
