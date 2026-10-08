// Diyet formu — istemci tarafı doğrulama ve başarı modalı (backend yok)
document.addEventListener("DOMContentLoaded", () => {
  const form = document.getElementById("diyet-form");
  if (!form) return;

  const ERROR_CLASS = "diyet-form__field--error";

  const messageFor = (control) => {
    if (control.validity.valueMissing) {
      return control.tagName === "SELECT" ? "Lütfen bir seçim yapın." : "Bu alan zorunludur.";
    }
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

  const controls = [...form.querySelectorAll("input, select, textarea")];

  // Kullanıcı düzeltirken hata anında kalksın
  controls.forEach((control) => {
    const event = control.tagName === "SELECT" ? "change" : "input";
    control.addEventListener(event, () => {
      if (control.closest(`.${ERROR_CLASS}`) && control.checkValidity()) clearError(control);
    });
  });

  form.addEventListener("submit", (event) => {
    event.preventDefault();

    const invalid = controls.filter((control) => !control.checkValidity());
    controls.forEach(clearError);
    invalid.forEach(showError);

    if (invalid.length) {
      invalid[0].focus();
      return;
    }

    form.reset();
    if (window.EmirCoaching && window.EmirCoaching.openSuccessModal) {
      window.EmirCoaching.openSuccessModal();
    }
  });
});
