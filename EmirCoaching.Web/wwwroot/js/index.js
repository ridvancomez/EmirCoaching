(function () {
  "use strict";

  function generateDots(container, count, dotClass, activeClass, onSelect) {
    if (!container) {
      return [];
    }

    container.innerHTML = "";

    var dots = [];

    var makeHandler = function (index) {
      return function () {
        onSelect(index);
      };
    };

    for (var i = 0; i < count; i++) {
      var dot = document.createElement("button");
      dot.type = "button";
      dot.className = dotClass + (i === 0 ? " " + activeClass : "");
      dot.title = (i + 1) + ". slide'a git";
      dot.setAttribute("aria-label", (i + 1) + ". slide'a git");
      dot.addEventListener("click", makeHandler(i));

      container.appendChild(dot);
      dots.push(dot);
    }

    return dots;
  }

  function initProgramSlider() {
    var wrapper = document.querySelector(".program__right");

    if (!wrapper) {
      return;
    }

    var track = wrapper.querySelector(".program__slider-track");

    if (!track || !track.children.length) {
      return;
    }

    var slides = track.children;
    var lastIndex = slides.length - 1;
    var prevBtn = wrapper.querySelector(".program__slider-arrow--prev");
    var nextBtn = wrapper.querySelector(".program__slider-arrow--next");
    var dotsContainer = wrapper.querySelector(".program__slider-dots");
    var currentIndex = 0;

    function goToSlide(index) {
      if (index < 0) {
        index = 0;
      } else if (index > lastIndex) {
        index = lastIndex;
      }

      currentIndex = index;
      track.style.transform = "translateX(-" + currentIndex * 100 + "%)";

      dots.forEach(function (dot, i) {
        dot.classList.toggle("program__slider-dot--active", i === currentIndex);
      });
    }

    var dots = generateDots(
      dotsContainer,
      slides.length,
      "program__slider-dot",
      "program__slider-dot--active",
      goToSlide
    );

    if (prevBtn) {
      prevBtn.addEventListener("click", function () {
        goToSlide(currentIndex - 1);
      });
    }

    if (nextBtn) {
      nextBtn.addEventListener("click", function () {
        goToSlide(currentIndex + 1);
      });
    }
  }

  // Paket kartı ön/arka yüz (≤931px): fiyat kartı ön, "Bu Pakette Neler Var" arka yüz.
  // data-program-flip butonları kartı çevirir; görünmeyen yüz inert olur (klavye ve ekran
  // okuyucu atlar), odak diğer yüzün butonuna geçer. Daha geniş ekranda kartlar yan yana.
  function initProgramFlip() {
    var slides = document.querySelectorAll(".program__slide");

    if (!slides.length) {
      return;
    }

    var mobile = window.matchMedia("(max-width: 931px)");

    function sync(slide) {
      var flipped = slide.classList.contains("program__slide--arka");

      slide.querySelector(".program__card--pricing").inert = mobile.matches && flipped;
      slide.querySelector(".program__card--details").inert = mobile.matches && !flipped;
    }

    Array.prototype.forEach.call(slides, function (slide) {
      slide.addEventListener("click", function (event) {
        if (!event.target.closest("[data-program-flip]")) {
          return;
        }

        var flipped = slide.classList.toggle("program__slide--arka");
        var target = slide.querySelector(
          flipped ? ".program__flip-btn--back" : ".program__flip-btn:not(.program__flip-btn--back)"
        );

        sync(slide);

        if (target) {
          target.focus({ preventScroll: true });
        }
      });

      sync(slide);
    });

    mobile.addEventListener("change", function () {
      Array.prototype.forEach.call(slides, sync);
    });
  }

  // Blok blok kaydırma (yalnızca ana sayfa, 1024–1920px): her tekerlek hareketi bir sonraki /
  // önceki bölüme geçer. Ekrana sığan bölüm navbar'ın altındaki alanda ortalanır; ekrandan uzun
  // bölümün (ör. SSS) içinde normal kaydırılır, sonuna gelince bir sonraki hareket diğer bölüme
  // geçer. Klavye, dokunmatik ve bağlantılar normal çalışır; popup açıkken devre dışıdır.
  function initSectionSnap() {
    var blocks = document.querySelectorAll("main > section, body > footer");

    if (!blocks.length) {
      return;
    }

    var desktop = window.matchMedia("(min-width: 1024px) and (max-width: 1920px)");
    var reduce = window.matchMedia("(prefers-reduced-motion: reduce)");
    var navbar = document.querySelector(".navbar");
    var GAP = 200; // ms: bu süreden kısa aralıklı tekerlek olayları aynı hareket sayılır
    var EPS = 2;
    // Uzun bölümün sonuna gelince alt kenarı da görünsün diye 40px daha inilir, sonra sonraki bölüme geçilir
    var END_ROOM = 40;
    var busy = false;
    var settling = false;
    var lastNative = false;
    var lastWheel = 0;
    var endTimer;

    function maxScroll() {
      return document.documentElement.scrollHeight - window.innerHeight;
    }

    function clamp(y) {
      return Math.max(0, Math.min(y, maxScroll()));
    }

    // Her bölüm için iniş (aşağı) ve çıkış (yukarı) konumları
    function stops() {
      // Üste hizalanan bölüm navbar'a yapışmasın diye altında 24px boşluk bırakılır
      var nav = (navbar ? navbar.getBoundingClientRect().bottom : 0) + 24;
      var room = window.innerHeight - nav;

      return Array.prototype.map.call(blocks, function (el, i) {
        var r = el.getBoundingClientRect();
        var top = r.top + window.pageYOffset;

        if (i === 0) {
          var heroTall = r.height - window.innerHeight > window.innerHeight * 0.25;
          return { top: 0, bottom: heroTall ? clamp(top + r.height - window.innerHeight + END_ROOM) : 0, tall: heroTall };
        }

        if (r.height <= room) {
          var center = clamp(top - nav - (room - r.height) / 2);
          return { top: center, bottom: center, tall: false };
        }

        // Alandan en fazla %25 taşan bölüm tek durakla gösterilir: başlık görünsün diye üstü
        // navbar'ın altına hizalanır, kesilen kısım bölümün alt iç boşluğudur.
        if (r.height - room <= room * 0.25) {
          return { top: clamp(top - nav), bottom: clamp(top - nav), tall: false };
        }

        return { top: clamp(top - nav), bottom: clamp(top + r.height - window.innerHeight + END_ROOM), tall: true };
      });
    }

    function canScrollInside(node, dy) {
      while (node && node !== document.body && node.nodeType === 1) {
        var oy = window.getComputedStyle(node).overflowY;

        if ((oy === "auto" || oy === "scroll") && node.scrollHeight > node.clientHeight) {
          if (dy > 0 ? node.scrollTop + node.clientHeight < node.scrollHeight - 1 : node.scrollTop > 0) {
            return true;
          }
        }

        node = node.parentNode;
      }

      return false;
    }

    function finish() {
      clearTimeout(endTimer);
      window.removeEventListener("scrollend", finish);
      busy = false;
    }

    function go(y) {
      busy = true;
      settling = true;
      window.addEventListener("scrollend", finish);
      endTimer = setTimeout(finish, 1000);
      window.scrollTo({ top: y, behavior: reduce.matches ? "auto" : "smooth" });
    }

    window.addEventListener("wheel", function (event) {
      if (!desktop.matches || event.ctrlKey || document.body.style.overflow === "hidden" ||
          Math.abs(event.deltaY) <= Math.abs(event.deltaX) || canScrollInside(event.target, event.deltaY)) {
        return;
      }

      var now = Date.now();
      var gap = now - lastWheel;
      lastWheel = now;

      if (busy || (settling && gap < GAP)) {
        event.preventDefault();
        return;
      }

      settling = false;

      var down = event.deltaY > 0;
      var dy = Math.abs(event.deltaY) * (event.deltaMode === 1 ? 16 : event.deltaMode === 2 ? window.innerHeight : 1);
      var y = window.pageYOffset;
      var list = stops();
      var i;

      // Uzun bölümün içindeysek normal kaydır; sınırı aşacaksa sınırda dur
      for (i = 0; i < list.length; i++) {
        var s = list[i];

        if (s.tall && y >= s.top - EPS && y <= s.bottom + EPS) {
          var edge = down ? s.bottom : s.top;
          var left = down ? edge - y : y - edge;

          if (left > EPS) {
            lastNative = true;

            if (dy > left) {
              event.preventDefault();
              window.scrollTo({ top: edge, behavior: reduce.matches ? "auto" : "smooth" });
            }

            return;
          }
        }
      }

      event.preventDefault();

      // Uzun bölümü sürekli kaydırırken sona gelindiyse önce dur; yeni hareket diğer bölüme geçer
      if (lastNative && gap < GAP) {
        return;
      }

      lastNative = false;

      var target = null;

      for (i = 0; i < list.length; i++) {
        if (down && list[i].top > y + EPS && (target === null || list[i].top < target)) {
          target = list[i].top;
        } else if (!down && list[i].bottom < y - EPS && (target === null || list[i].bottom > target)) {
          target = list[i].bottom;
        }
      }

      if (target !== null) {
        go(target);
      }
    }, { passive: false });
  }

  function initStoryCarousel() {
    var wrapper = document.querySelector(".story__carousel-col");

    if (!wrapper) {
      return;
    }

    var track = wrapper.querySelector(".story__carousel-track");

    if (!track || !track.children.length) {
      return;
    }

    var slides = track.children;
    var total = slides.length;
    var prevBtn = wrapper.querySelector(".story__carousel-arrow--prev");
    var nextBtn = wrapper.querySelector(".story__carousel-arrow--next");
    var dotsContainer = wrapper.querySelector(".story__carousel-dots");
    var currentIndex = 0;
    var STACK_CLASSES = [
      "story__slide--active",
      "story__slide--stack-2",
      "story__slide--stack-3",
      "story__slide--hidden"
    ];

    function updateStack() {
      Array.prototype.forEach.call(slides, function (slide, i) {
        var relative = (i - currentIndex + total) % total;

        slide.classList.remove.apply(slide.classList, STACK_CLASSES);

        if (relative === 0) {
          slide.classList.add("story__slide--active");
        } else if (relative === 1) {
          slide.classList.add("story__slide--stack-2");
        } else if (relative === 2) {
          slide.classList.add("story__slide--stack-3");
        } else {
          slide.classList.add("story__slide--hidden");
        }
      });

      dots.forEach(function (dot, i) {
        dot.classList.toggle("story__carousel-dot--active", i === currentIndex);
      });
    }

    function goToSlide(index) {
      currentIndex = ((index % total) + total) % total;
      updateStack();
    }

    var dots = generateDots(
      dotsContainer,
      total,
      "story__carousel-dot",
      "story__carousel-dot--active",
      goToSlide
    );

    if (prevBtn) {
      prevBtn.addEventListener("click", function () {
        goToSlide(currentIndex - 1);
      });
    }

    if (nextBtn) {
      nextBtn.addEventListener("click", function () {
        goToSlide(currentIndex + 1);
      });
    }

    updateStack();
  }

  function initPrivacyToggles() {
    var toggles = document.querySelectorAll(".story__privacy-toggle");

    toggles.forEach(function (toggle) {
      toggle.addEventListener("click", function () {
        var isPressed = toggle.getAttribute("aria-pressed") === "true";
        toggle.setAttribute("aria-pressed", String(!isPressed));

        var slide = toggle.closest(".story__slide");
        var img = slide ? slide.querySelector(".story__slide-img") : null;

        if (img) {
          img.classList.toggle("story__slide-img--revealed", !isPressed);
        }
      });
    });
  }

  function initQaCarousel() {
    var wrapper = document.querySelector(".qa__slider");

    if (!wrapper) {
      return null;
    }

    var track = wrapper.querySelector(".qa__slider-track");

    if (!track) {
      return null;
    }

    var nav = document.querySelector(".qa__nav");
    var prevBtn = nav ? nav.querySelector(".qa__arrow--prev") : null;
    var nextBtn = nav ? nav.querySelector(".qa__arrow--next") : null;
    var dotsContainer = nav ? nav.querySelector(".qa__dots") : null;
    var currentIndex = 0;
    var lastIndex = 0;
    var dots = [];

    function goToSlide(index) {
      if (index < 0) {
        index = 0;
      } else if (index > lastIndex) {
        index = lastIndex;
      }

      currentIndex = index;
      track.style.transform = "translateX(-" + currentIndex * 100 + "%)";

      dots.forEach(function (dot, i) {
        dot.classList.toggle("qa__dot--active", i === currentIndex);
      });
    }

    function rebuild() {
      var slideCount = track.children.length;
      lastIndex = Math.max(slideCount - 1, 0);
      currentIndex = 0;
      track.style.transform = "translateX(0%)";
      dots = generateDots(dotsContainer, slideCount, "qa__dot", "qa__dot--active", goToSlide);
    }

    if (prevBtn) {
      prevBtn.addEventListener("click", function () {
        goToSlide(currentIndex - 1);
      });
    }

    if (nextBtn) {
      nextBtn.addEventListener("click", function () {
        goToSlide(currentIndex + 1);
      });
    }

    rebuild();

    return { rebuild: rebuild };
  }

  function initQaResponsiveSlides(qaCarousel) {
    var track = document.querySelector(".qa__slider-track");

    if (!track || !track.children.length || !qaCarousel) {
      return;
    }

    var originalMarkup = track.innerHTML;
    var mql = window.matchMedia("(max-width: 810px)");
    var isSplit = null;

    function splitIntoSingleCardSlides() {
      var slides = Array.prototype.slice.call(track.children);
      var fragment = document.createDocumentFragment();

      slides.forEach(function (slide) {
        var cards = Array.prototype.slice.call(slide.children);

        cards.forEach(function (card) {
          var singleSlide = document.createElement("div");
          singleSlide.className = slide.className;
          singleSlide.appendChild(card);
          fragment.appendChild(singleSlide);
        });
      });

      track.innerHTML = "";
      track.appendChild(fragment);
    }

    function restoreOriginalSlides() {
      track.innerHTML = originalMarkup;
    }

    function applyLayout(shouldSplit) {
      if (shouldSplit === isSplit) {
        return;
      }

      if (shouldSplit) {
        splitIntoSingleCardSlides();
      } else {
        restoreOriginalSlides();
      }

      isSplit = shouldSplit;
      qaCarousel.rebuild();
    }

    applyLayout(mql.matches);

    if (typeof mql.addEventListener === "function") {
      mql.addEventListener("change", function (event) {
        applyLayout(event.matches);
      });
    } else if (typeof mql.addListener === "function") {
      mql.addListener(function (event) {
        applyLayout(event.matches);
      });
    }

    window.addEventListener("resize", function () {
      applyLayout(window.innerWidth < 811);
    });
  }

  function initFaqAccordion() {
    var items = document.querySelectorAll(".faq__item");

    if (!items.length) {
      return;
    }

    function setIcon(item, iconName) {
      var icon = item.querySelector(".faq__item-toggle iconify-icon");

      if (icon) {
        icon.setAttribute("icon", iconName);
      }
    }

    function openItem(item) {
      var header = item.querySelector(".faq__item-header");
      var body = item.querySelector(".faq__item-body");

      item.classList.add("faq__item--open");

      if (header) {
        header.setAttribute("aria-expanded", "true");
      }

      if (body) {
        body.style.maxHeight = body.scrollHeight + "px";
      }

      setIcon(item, "fa6-solid:minus");
    }

    function closeItem(item) {
      var header = item.querySelector(".faq__item-header");
      var body = item.querySelector(".faq__item-body");

      item.classList.remove("faq__item--open");

      if (header) {
        header.setAttribute("aria-expanded", "false");
      }

      if (body) {
        body.style.maxHeight = "0px";
      }

      setIcon(item, "fa6-solid:plus");
    }

    items.forEach(function (item) {
      var header = item.querySelector(".faq__item-header");

      if (!header) {
        return;
      }

      header.addEventListener("click", function () {
        var isOpen = item.classList.contains("faq__item--open");

        items.forEach(function (other) {
          if (other !== item) {
            closeItem(other);
          }
        });

        if (isOpen) {
          closeItem(item);
        } else {
          openItem(item);
        }
      });
    });

    items.forEach(function (item) {
      if (item.classList.contains("faq__item--open")) {
        openItem(item);
      } else {
        closeItem(item);
      }
    });
  }

  // Danışan yorumları: tek yorum görünür, yandaki oklar ve alttaki noktalar arasında geçiş.
  // Son yorumdan sonra başa döner. Kendiliğinden 6 sn'de bir ilerler; fare üstündeyken,
  // klavye odağı içindeyken, sekme arka plandayken ve "hareketi azalt" tercihinde durur.
  function initReviewsCarousel() {
    var root = document.querySelector("[data-reviews]");

    if (!root) {
      return;
    }

    var track = root.querySelector("[data-reviews-track]");
    var items = track ? track.children : [];

    if (!items.length) {
      return;
    }

    var count = items.length;
    var currentIndex = 0;

    function goTo(index) {
      currentIndex = (index + count) % count;
      track.style.transform = "translateX(-" + currentIndex * 100 + "%)";

      Array.prototype.forEach.call(items, function (item, i) {
        item.inert = i !== currentIndex;
      });

      dots.forEach(function (dot, i) {
        dot.classList.toggle("home-dot--active", i === currentIndex);
      });
    }

    var dots = generateDots(
      root.querySelector("[data-reviews-dots]"),
      count,
      "home-dot",
      "home-dot--active",
      goTo
    );

    dots.forEach(function (dot, i) {
      dot.title = (i + 1) + ". yoruma git";
      dot.setAttribute("aria-label", (i + 1) + ". yoruma git");
    });

    var AUTOPLAY_MS = 6000;
    var reduceMotion = window.matchMedia("(prefers-reduced-motion: reduce)");
    var timer = null;
    var hovered = false;
    var focused = false;

    function stop() {
      clearInterval(timer);
      timer = null;
    }

    // Her çağrıda sayaç baştan başlar; elle geçişten hemen sonra kaymasın
    function start() {
      stop();

      if (hovered || focused || document.hidden || reduceMotion.matches) {
        return;
      }

      timer = setInterval(function () {
        goTo(currentIndex + 1);
      }, AUTOPLAY_MS);
    }

    root.querySelector("[data-reviews-prev]").addEventListener("click", function () {
      goTo(currentIndex - 1);
      start();
    });

    root.querySelector("[data-reviews-next]").addEventListener("click", function () {
      goTo(currentIndex + 1);
      start();
    });

    dots.forEach(function (dot) {
      dot.addEventListener("click", start);
    });

    // Yalnızca fare: dokunmatikte dokunuş "hover" sayılıp dönüşü kalıcı durdurmasın
    root.addEventListener("pointerenter", function (event) {
      if (event.pointerType === "mouse") {
        hovered = true;
        stop();
      }
    });

    root.addEventListener("pointerleave", function (event) {
      if (event.pointerType === "mouse") {
        hovered = false;
        start();
      }
    });

    // Klavye odağı (focus-visible); fareyle tıklanan ok odak alsa da dönüş durmaz
    root.addEventListener("focusin", function (event) {
      if (event.target.matches(":focus-visible")) {
        focused = true;
        stop();
      }
    });

    root.addEventListener("focusout", function (event) {
      if (!root.contains(event.relatedTarget)) {
        focused = false;
        start();
      }
    });

    document.addEventListener("visibilitychange", start);

    if (reduceMotion.addEventListener) {
      reduceMotion.addEventListener("change", start);
    }

    goTo(0);
    start();
  }

  document.addEventListener("DOMContentLoaded", function () {
    initReviewsCarousel();
    initProgramSlider();
    initProgramFlip();
    initStoryCarousel();
    initPrivacyToggles();
    initQaResponsiveSlides(initQaCarousel());
    initFaqAccordion();
    initSectionSnap();
  });
})();
