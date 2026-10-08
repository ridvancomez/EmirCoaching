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

  document.addEventListener("DOMContentLoaded", function () {
    initProgramSlider();
    initStoryCarousel();
    initPrivacyToggles();
    initQaResponsiveSlides(initQaCarousel());
    initFaqAccordion();
  });
})();
