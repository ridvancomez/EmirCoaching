// Diyet formu — 2 adımlı sihirbaz (Kişisel, İletişim), istemci tarafı doğrulama, taslak kaydı (backend yok)
// Akış: Paket Detay "Satın Al" (?paket=orta&yer=salonda) → bu form → "Bilgilerin alındı" → Shopier (yeni sekme)
document.addEventListener("DOMContentLoaded", () => {
  const form = document.getElementById("diyet-form");
  if (!form) return;

  const ERROR_CLASS = "diyet-form__field--error";
  const STORAGE_KEY = "emir-diyet-formu-taslak";
  const PAKET_ADLARI = {
    temel: "Başlangıç & Deneme",
    orta: "Dönüşüm Protokolü",
    premium: "VIP Yaşam Tarzı",
  };

  const messageFor = (control) => {
    if (control.validity.valueMissing) {
      if (control.type === "checkbox") return "Formu gönderebilmek için bu onayı vermelisiniz.";
      return control.tagName === "SELECT" ? "Lütfen bir seçim yapın." : "Bu alan zorunludur.";
    }
    if (control.validity.typeMismatch && control.type === "email") return "Lütfen geçerli bir e-posta adresi girin.";
    if (control.validity.patternMismatch && control.type === "tel") return "Lütfen geçerli bir telefon numarası girin.";
    if (control.validity.rangeUnderflow) return `En az ${control.min} olmalıdır.`;
    if (control.validity.rangeOverflow) return `En fazla ${control.max} olabilir.`;
    if (control.validity.stepMismatch) return "Lütfen geçerli bir değer girin.";
    if (control.validity.badInput) return "Lütfen bir sayı girin.";
    return "Lütfen bu alanı kontrol edin.";
  };

  const clearError = (control) => {
    const field = control.closest(".diyet-form__field");
    if (!field) return;
    field.classList.remove(ERROR_CLASS);
    control.removeAttribute("aria-invalid");
    control.removeAttribute("aria-describedby");
    const error = field.querySelector(".diyet-form__error");
    if (error) error.remove();
  };

  const showError = (control) => {
    clearError(control);
    const field = control.closest(".diyet-form__field");
    if (!field) return;
    const error = document.createElement("p");
    error.className = "diyet-form__error";
    error.id = `${control.id}-error`;
    error.textContent = messageFor(control);
    field.appendChild(error);
    field.classList.add(ERROR_CLASS);
    control.setAttribute("aria-invalid", "true");
    control.setAttribute("aria-describedby", error.id);
  };

  // Verilen kontrolleri doğrular; hatalıysa ilkine odaklanır
  const validate = (controls) => {
    const invalid = controls.filter((control) => !control.checkValidity());
    controls.forEach(clearError);
    invalid.forEach(showError);
    if (invalid.length) invalid[0].focus();
    return invalid.length === 0;
  };

  const controls = [...form.querySelectorAll("input, select, textarea")];

  // Kullanıcı düzeltirken hata anında kalksın
  controls.forEach((control) => {
    const event = control.tagName === "SELECT" || control.type === "checkbox" ? "change" : "input";
    control.addEventListener(event, () => {
      if (control.closest(`.${ERROR_CLASS}`) && control.checkValidity()) clearError(control);
    });
  });

  /* ---------- Taslak: sayfa yenilense de yazılanlar kaybolmasın (onay kutuları hariç) ---------- */
  const draftControls = controls.filter((control) => control.type !== "checkbox" && control.name);

  const readDraft = () => {
    try {
      return JSON.parse(localStorage.getItem(STORAGE_KEY)) || {};
    } catch (e) {
      return {};
    }
  };

  const saveDraft = () => {
    const data = {};
    draftControls.forEach((control) => {
      if (control.value) data[control.name] = control.value;
    });
    try {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(data));
    } catch (e) {
      /* depolama kapalıysa taslak tutulmaz */
    }
  };

  const clearDraft = () => {
    try {
      localStorage.removeItem(STORAGE_KEY);
    } catch (e) {
      /* yok say */
    }
  };

  const draft = readDraft();
  draftControls.forEach((control) => {
    if (draft[control.name] !== undefined) control.value = draft[control.name];
  });
  form.addEventListener("input", saveDraft);
  form.addEventListener("change", saveDraft);

  /* ---------- Paket: URL'den gelen seçim taslağın önüne geçer ---------- */
  const paketSelect = form.querySelector("#paket");
  const params = new URLSearchParams(location.search);
  if (paketSelect && params.get("paket")) {
    const yer = params.get("yer") === "evde" ? "evde" : "salonda";
    const value = `${params.get("paket")}-${yer}`;
    if (paketSelect.querySelector(`option[value="${value}"]`)) {
      paketSelect.value = value;
      saveDraft();
    }
  }

  /* ---------- Sihirbaz ---------- */
  const steps = [...form.querySelectorAll("fieldset[data-step]")];
  const stepItems = [...form.querySelectorAll(".diyet-form__step")];
  const consents = form.querySelector(".diyet-form__consents");
  const prevBtn = form.querySelector("[data-step-prev]");
  const nextBtn = form.querySelector("[data-step-next]");
  const submitBtn = form.querySelector('button[type="submit"]');
  let current = 0;

  form.classList.add("diyet-form__form--wizard");

  // Adım değişince formun başı navbar'ın altında görünsün
  const scrollToForm = () => {
    const top = form.getBoundingClientRect().top + window.scrollY - 110;
    if (window.scrollY > top) {
      const reduce = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
      window.scrollTo({ top, behavior: reduce ? "auto" : "smooth" });
    }
  };

  const showStep = (index, moveFocus) => {
    current = index;
    const last = index === steps.length - 1;
    steps.forEach((step, i) => {
      step.hidden = i !== index;
    });
    stepItems.forEach((item, i) => {
      item.classList.toggle("diyet-form__step--active", i === index);
      item.classList.toggle("diyet-form__step--done", i < index);
      if (i === index) item.setAttribute("aria-current", "step");
      else item.removeAttribute("aria-current");
    });
    if (consents) consents.hidden = !last;
    prevBtn.hidden = index === 0;
    nextBtn.hidden = last;
    submitBtn.hidden = !last;

    if (moveFocus) {
      const title = steps[index].querySelector(".diyet-form__title");
      title.setAttribute("tabindex", "-1");
      title.focus({ preventScroll: true });
      scrollToForm();
    }
  };

  // Bir adımın kontrolleri (1. adımda paket seçimi de doğrulanır)
  const stepControls = (index) => {
    const list = [...steps[index].querySelectorAll("input, select, textarea")];
    if (index === 0 && paketSelect) list.unshift(paketSelect);
    return list;
  };

  nextBtn.addEventListener("click", () => {
    if (validate(stepControls(current))) showStep(current + 1, true);
  });

  prevBtn.addEventListener("click", () => showStep(current - 1, true));

  showStep(0, false);

  /* ---------- Gönder → "Bilgilerin alındı" ---------- */
  const done = document.getElementById("diyet-form-done");

  form.addEventListener("submit", (event) => {
    event.preventDefault();

    // Ara adımlarda Enter "İleri" gibi davranır
    if (current < steps.length - 1) {
      nextBtn.click();
      return;
    }

    // Önceki adımlar tamam olmalı; değilse hatalı adıma dön
    for (let i = 0; i < steps.length - 1; i += 1) {
      if (stepControls(i).some((control) => !control.checkValidity())) {
        showStep(i, false);
        validate(stepControls(i));
        return;
      }
    }
    const lastControls = [...stepControls(steps.length - 1), ...consents.querySelectorAll("input")];
    if (!validate(lastControls)) return;

    // Backend: form verisi burada sunucuya gönderilir; Shopier linki yanıtla gelir
    const [kod, yer] = (paketSelect.value || "orta-salonda").split("-");
    const paketAdi = (PAKET_ADLARI[kod] || PAKET_ADLARI.orta) + (yer === "evde" ? " (Evde)" : "");

    clearDraft();
    form.reset();
    form.hidden = true;
    if (done) {
      const adEl = done.querySelector(".diyet-form__done-paket");
      if (adEl) adEl.textContent = paketAdi;
      done.hidden = false;
      done.focus({ preventScroll: true });
      const top = done.getBoundingClientRect().top + window.scrollY - 110;
      window.scrollTo({ top: Math.max(0, top) });
    }
  });
});
