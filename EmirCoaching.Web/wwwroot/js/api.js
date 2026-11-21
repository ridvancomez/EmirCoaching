// API adresi ve ortak GET yardımcısı. Site verisini EmirCoaching.API'den alır.
// Geliştirmede API https://localhost:7267'de çalışır; yayında api.<alan adı> varsayılır
// (yayına alma ayarlarında gerekirse yalnızca burası değişir).
(function () {
  var host = location.hostname;
  var isLocal = host === "localhost" || host === "127.0.0.1";
  var base = isLocal ? "https://localhost:7267" : "https://api." + host.replace(/^www\./, "");

  // Başarıda JSON, 404'te null döner; diğer hatalarda reddeder (sayfa statik içeriği korur)
  function get(path) {
    return fetch(base + "/api/v1" + path, { headers: { Accept: "application/json" } }).then(function (response) {
      if (response.status === 404) return null;
      if (!response.ok) throw new Error("API " + response.status);
      return response.json();
    });
  }

  window.EmirApi = { base: base, get: get };
})();
