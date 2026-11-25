// Formların API'ye gönderimi. Sayfa verisi sunucuda (MVC) basılır; tarayıcı API'ye yalnızca form gönderir.
// API adresi layout'taki <meta name="api-base"> etiketinden okunur (appsettings: Api:PublicUrl).
(function () {
  var meta = document.querySelector('meta[name="api-base"]');
  var base = (meta ? meta.content : "").replace(/\/+$/, "");

  // Hata yanıtı ProblemDetails: errors[] Türkçe mesajlardır; yoksa title
  function problemMessages(response) {
    return response.json().then(function (problem) {
      if (problem && problem.errors && problem.errors.length) return problem.errors;
      return [problem && problem.title ? problem.title : "Bir hata oluştu. Lütfen tekrar deneyin."];
    }, function () {
      return ["Bir hata oluştu. Lütfen tekrar deneyin."];
    });
  }

  // Başarıda { ok: true }, hatada { ok: false, errors: [...] } döner (reddetmez)
  function post(path, body) {
    return fetch(base + "/api/v1" + path, {
      method: "POST",
      headers: { "Content-Type": "application/json", Accept: "application/json" },
      body: JSON.stringify(body)
    }).then(function (response) {
      if (response.ok) return { ok: true };
      if (response.status === 429) return { ok: false, errors: ["Çok fazla deneme yaptınız. Lütfen biraz sonra tekrar deneyin."] };
      return problemMessages(response).then(function (errors) { return { ok: false, errors: errors }; });
    }, function () {
      return { ok: false, errors: ["Sunucuya ulaşılamadı. İnternet bağlantınızı kontrol edip tekrar deneyin."] };
    });
  }

  // Formun altındaki bildirim satırı (gerekirse oluşturulur); boş mesaj gizler
  function feedback(form, messages, isSuccess) {
    var el = form.querySelector(".form-feedback");
    if (!el) {
      el = document.createElement("p");
      el.className = "form-feedback";
      el.setAttribute("role", "alert");
      form.appendChild(el);
    }
    el.classList.toggle("form-feedback--success", !!isSuccess);
    el.textContent = (messages || []).join(" ");
    el.hidden = !el.textContent;
  }

  // Gönderim sürerken düğme kilitlenir (çift gönderim olmasın)
  function send(form, path, body) {
    var buttons = form.querySelectorAll('button[type="submit"]');
    buttons.forEach(function (b) { b.disabled = true; });
    feedback(form, []);
    return post(path, body).then(function (result) {
      buttons.forEach(function (b) { b.disabled = false; });
      if (!result.ok) feedback(form, result.errors);
      return result.ok;
    });
  }

  window.EmirApi = { base: base, post: post, send: send, feedback: feedback };
})();
