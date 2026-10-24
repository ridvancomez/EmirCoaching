# Proje Kuralları — Emir Coaching

## Genel Prensipler

Bu proje saf HTML ve CSS ile yazılmaktadır.
Bootstrap, Tailwind veya herhangi bir CSS framework/kütüphanesi KULLANILMAYACAKTIR.
Tüm stillendirme elle yazılmış CSS ile yapılacaktır.

---

## SEO Kuralları

- Her sayfada `<title>` etiketi zorunludur.
- Her sayfada `<meta name="description">` etiketi zorunludur.
- Başlık hiyerarşisi doğru olmalıdır: her sayfada yalnızca bir `<h1>`, alt başlıklar sırasıyla `<h2>`, `<h3>` şeklinde devam etmelidir.
- Anlamlı ve açıklayıcı URL yapısı için dosya isimleri küçük harf, tire (-) ile ayrılmış olmalıdır.
- Yapısal içerik için semantik HTML etiketleri kullanılmalıdır: `<header>`, `<nav>`, `<main>`, `<section>`, `<article>`, `<footer>` vb.
- `<a>` etiketlerinde `title` attribute zorunludur.
- `<img>` etiketlerinde `alt` ve `title` attribute zorunludur.
- İçerik anahtar kelimeler açısından anlamlı olmalı; gereksiz `<div>` yığınından kaçınılmalıdır.

---

## Erişilebilirlik (Accessibility)

- Tüm form alanlarında `<label>` kullanılmalıdır.
- Renk kontrastı yeterli düzeyde olmalıdır (WCAG AA standardı).
- Klavye navigasyonu desteklenmelidir.

---

## Responsive Tasarım

- Tüm tasarımlar mobil, tablet ve masaüstü için uyumlu olmalıdır.
- CSS media query kullanılacaktır. Breakpoint'ler:
  - Mobil: max-width 768px
  - Tablet: 769px – 1024px
  - Masaüstü: 1025px ve üzeri
- Hedef: desktop tasarıma mümkün olduğunca sadık kalmak, ancak küçük ekranlarda göze hoş gelecek şekilde uyarlamak.
- Görseller responsive olmalı: `max-width: 100%`, `height: auto`.
- Esnek layout için `flexbox` veya `CSS grid` kullanılmalıdır.

---

## Tasarım Değerleri (Figma'dan alındı)

### Renkler
```css
--bg-dark:    #000636;           /* Ana koyu arka plan */
--accent:     #2b7eff;           /* Mavi vurgu rengi */
--text-white: #ffffff;
--text-muted: rgba(255,255,255,0.8);
--navbar-bg:  #ffffff;
```

### Fontlar
- **Geologica** — SemiBold (600): Navbar aktif link, genel başlıklar
- **Geologica** — ExtraLight (200): Navbar pasif linkler
- **Geologica** — Black (900) + uppercase: Vurgu kelimeleri (örn. GÜÇLÜ)
- **Inter** — Regular (400): Body metin, açıklamalar

Google Fonts import:
```html
<link href="https://fonts.googleapis.com/css2?family=Geologica:wght@200;600;900&family=Inter&display=swap" rel="stylesheet">
```

### Navbar
- Yükseklik: 72px
- Border-radius: 16px
- Sol ve sağ margin: 100px
- Arka plan: #ffffff
- Pozisyon: fixed (sayfanın üstünde sabit kalır)

### Hero / H1
- Font: Geologica SemiBold, 60px, beyaz
- Vurgu kelimesi: Geologica Black, uppercase, #2b7eff
- Body: Inter Regular, 16px, line-height 24px, rgba(255,255,255,0.8)

---

## Micro-interaction Kuralları

- Tüm tıklanabilir elemanlarda (`a`, `button`) transition zorunludur:
  `transition: all 0.3s ease;`
- Hover efektleri subtle ve göze yormayan olmalıdır:
  `opacity`, `color`, `transform: translateY(-2px)` veya `box-shadow` tercih edilir.
- Hiçbir element aniden değişmemeli, her geçiş smooth olmalıdır.
- Animasyon süreleri: hover için `0.2s–0.3s`, sayfa elementleri için max `0.5s`.
- `animation: keyframes` kullanılacaksa sadece anlam taşıyan yerlerde kullanılmalıdır
  (örn. hero başlığı fade-in, stats sayaç animasyonu). Gereksiz animasyondan kaçınılmalıdır.

---

## Kod Kalitesi

- CSS class isimleri anlamlı ve BEM benzeri bir yapıda olmalıdır.
- Gereksiz tekrar eden koddan kaçınılmalıdır.
- Yorum satırları Türkçe veya İngilizce olabilir, ancak tutarlı olunmalıdır.
- Her section ayrı ve okunabilir biçimde organize edilmelidir.
