// Paket kartı slider'ı (Ana Sayfa, Paketler): sayfadaki her [data-pkg-slider] için ayrı çalışır.
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

  // Paket kartları (≤810px): CSS yatay scroll-snap şeridi kurar; JS sayaç, oklar ve noktaları
  // kaydırma konumuna bağlar. Geniş ekranda kartlar yan yana durur, bu kontroller gizlidir.
  // Aynı anda iki kart görünen ekranlarda (tablet dikey) şerit yalnızca iki konuma kayabilir;
  // noktalar ve oklar kartlara değil bu erişilebilen konumlara (sayfalara) göre kurulur,
  // sayaç görünen kart aralığını gösterir (ör. "1–2 / 3").
  function initPackageSlider(slider) {
    var track = slider.querySelector("[data-pkg-track]");
    var cards = track ? track.children : [];

    if (!cards.length) {
      return;
    }

    var counter = slider.querySelector("[data-pkg-current]");
    var dotsContainer = slider.querySelector("[data-pkg-dots]");
    var prevBtn = slider.querySelector("[data-pkg-prev]");
    var nextBtn = slider.querySelector("[data-pkg-next]");
    var pages = [];
    var dots = [];
    var currentPage = 0;
    var startPlaced = false;

    // Her kartın başına kaydırma konumu; en sona sığanlar aynı (son) konumda birleşir
    function measurePages() {
      var start = cards[0].offsetLeft;
      var max = track.scrollWidth - track.clientWidth;
      var list = [];

      for (var i = 0; i < cards.length; i++) {
        var pos = Math.min(cards[i].offsetLeft - start, max);

        if (!list.length || pos - list[list.length - 1] > 2) {
          list.push(pos);
        }
      }

      return list;
    }

    function goToPage(index) {
      index = Math.max(0, Math.min(pages.length - 1, index));
      track.scrollTo({ left: pages[index], behavior: "smooth" });
    }

    function buildDots() {
      dots = generateDots(dotsContainer, pages.length, "home-dot", "home-dot--active", goToPage);

      dots.forEach(function (dot, i) {
        dot.title = (i + 1) + ". konuma git";
        dot.setAttribute("aria-label", (i + 1) + ". konuma git");
      });
    }

    // Görünür alanda tamamen duran ilk ve son kart. Ölçüler offset tabanlı: filtre animasyonu
    // (scale) sürerken de doğru sonuç verir.
    function visibleRange() {
      var style = getComputedStyle(track);
      var padLeft = parseFloat(style.paddingLeft) || 0;
      var padRight = parseFloat(style.paddingRight) || 0;
      var viewLeft = track.scrollLeft;
      var viewRight = viewLeft + track.clientWidth - padLeft - padRight;
      var start = cards[0].offsetLeft;
      var first = -1;
      var last = -1;

      for (var i = 0; i < cards.length; i++) {
        var left = cards[i].offsetLeft - start;
        var right = left + cards[i].offsetWidth;

        if (left >= viewLeft - 2 && right <= viewRight + 2) {
          if (first === -1) {
            first = i;
          }

          last = i;
        }
      }

      return first === -1 ? [0, 0] : [first, last];
    }

    function sync() {
      var x = track.scrollLeft;
      var index = 0;

      for (var i = 1; i < pages.length; i++) {
        if (Math.abs(pages[i] - x) < Math.abs(pages[index] - x)) {
          index = i;
        }
      }

      currentPage = index;

      if (counter) {
        var range = visibleRange();
        counter.textContent = range[0] === range[1] ? range[0] + 1 : (range[0] + 1) + "–" + (range[1] + 1);
      }

      dots.forEach(function (dot, i) {
        dot.classList.toggle("home-dot--active", i === index);
      });

      if (prevBtn) {
        prevBtn.disabled = index === 0;
      }

      if (nextBtn) {
        nextBtn.disabled = index === pages.length - 1;
      }
    }

    // Açılışta rozetli kart ("En Çok Tercih Edilen") görünür: o kartın tamamen göründüğü ilk konum.
    // Slider kurulu değilse (kartlar yan yana) ya da grup gizliyse ölçü yok; görünür olunca yapılır.
    function placeStart() {
      // Gizlenen grubun kaydırması sıfırlanır; tekrar görünürken yine rozetli karttan açılsın
      if (track.clientWidth === 0) {
        startPlaced = false;
        return;
      }

      if (startPlaced || pages.length < 2) {
        return;
      }

      startPlaced = true;

      var featured = track.querySelector(".pkg__badge");
      var card = featured ? featured.closest(".pkg") : null;

      if (!card) {
        return;
      }

      var style = getComputedStyle(track);
      var view = track.clientWidth - (parseFloat(style.paddingLeft) || 0) - (parseFloat(style.paddingRight) || 0);
      var left = card.offsetLeft - cards[0].offsetLeft;
      var right = left + card.offsetWidth;

      for (var i = 0; i < pages.length; i++) {
        if (left >= pages[i] - 2 && right <= pages[i] + view + 2) {
          track.scrollTo({ left: pages[i], behavior: "instant" });
          return;
        }
      }
    }

    function rebuild() {
      var next = measurePages();

      if (next.length !== pages.length) {
        pages = next;
        buildDots();
      } else {
        pages = next;
      }

      placeStart();
      sync();
    }

    if (prevBtn) {
      prevBtn.addEventListener("click", function () {
        goToPage(currentPage - 1);
      });
    }

    if (nextBtn) {
      nextBtn.addEventListener("click", function () {
        goToPage(currentPage + 1);
      });
    }

    track.addEventListener("scroll", sync, { passive: true });
    window.addEventListener("resize", rebuild);

    // Filtreyle gizli gruptan görünür olunca (Paketler: Tümü / Evde / Salonda) yeniden ölç
    if (window.ResizeObserver) {
      new ResizeObserver(rebuild).observe(track);
    }

    rebuild();
  }

  document.addEventListener("DOMContentLoaded", function () {
    Array.prototype.forEach.call(document.querySelectorAll("[data-pkg-slider]"), initPackageSlider);
  });
})();
