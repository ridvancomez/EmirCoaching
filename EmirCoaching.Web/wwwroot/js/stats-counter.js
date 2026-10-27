// Ortak stats bar sayacı (Anasayfa, Değişimler)
// Sayılar 0'dan hedef değere artar; stats bar ekrana girdiğinde (%30 görünür) bir kez çalışır.
(function () {
  "use strict";

  var DURATION = 1500;

  function easeOutCubic(t) {
    return 1 - Math.pow(1 - t, 3);
  }

  function animateValue(el) {
    var raw = el.textContent.trim();
    var match = raw.match(/\d+/);

    if (!match) {
      return;
    }

    var target = parseInt(match[0], 10);
    var suffix = raw.slice(match.index + match[0].length);
    var prefix = raw.slice(0, match.index);
    var startTime = null;

    function step(timestamp) {
      if (startTime === null) {
        startTime = timestamp;
      }

      var elapsed = timestamp - startTime;
      var progress = Math.min(elapsed / DURATION, 1);
      var eased = easeOutCubic(progress);
      var current = Math.round(eased * target);

      el.textContent = prefix + current + suffix;

      if (progress < 1) {
        requestAnimationFrame(step);
      }
    }

    requestAnimationFrame(step);
  }

  function initStatsCountUp() {
    var statsBar = document.querySelector(".hero__stats");

    if (!statsBar) {
      return;
    }

    var valueEls = statsBar.querySelectorAll(".hero__stat-value");

    if (!valueEls.length) {
      return;
    }

    if (!("IntersectionObserver" in window)) {
      valueEls.forEach(animateValue);
      return;
    }

    var observer = new IntersectionObserver(
      function (entries, obs) {
        entries.forEach(function (entry) {
          if (entry.isIntersecting) {
            valueEls.forEach(animateValue);
            obs.unobserve(entry.target);
          }
        });
      },
      { threshold: 0.3 }
    );

    observer.observe(statsBar);
  }

  document.addEventListener("DOMContentLoaded", initStatsCountUp);
})();
