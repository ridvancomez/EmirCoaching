# Figma Design Tokens — Emir Coaching
Son güncelleme: 2026-09-30
Figma file: NTCQhmOtKxKlTZ5n5ms6oV

---

## Global Tokens (tüm sayfalarda geçerli)

### Renkler
```
--bg-dark:        #000636   Ana arka plan
--bg-card:        #070c1a   Kart arka planı
--bg-card-alt:    #111c33   Alternatif kart (beslenme section)
--accent:         #2B7EFF   Mavi vurgu (#1b6dff de kullanılıyor, aynı aile)
--accent-hover:   #1b6dff   Hover durumu
--text-white:     #ffffff
--text-muted:     rgba(255,255,255,0.8)
--text-label:     #C2C6D8   Form label, subtitle
--text-dark:      #000636   Koyu zemin üstü
--navbar-bg:      #ffffff
--whatsapp:       #25D366
--yellow-badge:   #ffea00   "EN ÇOK TERCİH EDİLEN" badge
```

### Fontlar
```
Geologica ExtraBold (800): Büyük başlıklar (SANA UYGUN PAKETI SEÇ)
Geologica Bold (700):      Section başlıkları, kart başlıkları, footer başlık
Geologica SemiBold (600):  Navbar aktif link, genel h2
Geologica Regular (400):   Fiyatlar
Geologica ExtraLight (200): Navbar pasif linkler
Geologica Thin (100):      Form açıklamaları, madde listesi

Inter Black (900):         Buton metni (MESAJI GÖNDER)
Inter Bold (700):          Filter buton, liste başlıkları
Inter SemiBold (600):      CTA buton metinleri (Hemen Başla, WhatsApp)
Inter Regular (400):       Body, placeholder, label, açıklama
```

### Navbar
```
width:         1236px (container)
height:        72px
border-radius: 16px
background:    #ffffff
position:      fixed
top:           50px
left/right:    100px margin (1440px canvas)
```

---

## Paketler Sayfası (node: 1:4801)

### Hero Section
```
Arka plan:     Karanlık sporcu görseli (image asset)
Gradient:      linear-gradient(90deg, rgba(0,0,0,0.2), rgba(0,0,0,0.2)), 
               linear-gradient(180deg, rgba(7,12,26,0.9) 0%, rgba(7,12,26,0.6) 50%, rgb(7,12,26) 100%)

Breadcrumb:    "ANA SAYFA / PAKETLER"
               Inter Regular, 12px, tracking: 1.2px, uppercase, beyaz
               Ayraç "/" rengi: #424655

Başlık:        "SANA UYGUN PAKETI SEÇ, DÖNÜŞÜMÜ BAŞLAT"
               Geologica ExtraBold, 56px, line-height: 61.6px, tracking: -2.24px
               uppercase, ortalı, beyaz
               "DÖNÜŞÜMÜ" kelimesi: #2B7EFF

Açıklama:      Inter Regular, 16px, line-height: 24px, rgba(255,255,255,0.8)
               width: 548px, ortalı
```

### Filter Bar (Tümü / Gümüş / Altın / Kişiye Özel Diyet Planı)
```
Container:     backdrop-blur: 10px, background: rgba(17,28,51,0.7)
               border: 1px solid rgba(255,255,255,0.08)
               border-radius: 9999px (pill), height: 58px, padding: 9px
               top: 595px

"Tümü" (aktif): background: rgba(47,52,68,0.5), border-radius: pill
                Inter Bold, 16px, beyaz, padding: 8px 24px

"Gümüş":       nokta rengi: #c0c7d1, Inter Regular, 16px, #C2C6D8
"Altın":        nokta rengi: #e3b341, Inter Regular, 16px, #C2C6D8

"Kişiye Özel Diyet Planı": background: rgba(43,126,255,0.2), border: 1px solid white
                            border-radius: 333px, padding: 4px 20px
                            Inter Regular, 16px, beyaz
```

### Paket Kartları (3 kart, container: 1230px, top: 729px)
```
Genel kart boyutu: ~393px x 541px
Kart iç padding:   35px
Border-radius:     17px
```

**Kart 1 — BAŞLANGIÇ PAKETİ (sol)**
```
background:  #070c1a
border:      1.068px solid rgba(27,109,255,0.3)
shadow:      0 0 16px 0 rgba(27,109,255,0.1)
Üst çizgi:   gradient #1b6dff ortada, şeffaf kenarlarda

Başlık:      "BAŞLANGIÇ PAKETI" — Geologica Bold, 25.6px, tracking: 1.28px, uppercase
Süre:        "SÜRE: 4 HAFTA" — Inter Regular, 12.8px, #C2C6D8
Fiyat:       "₺ 3.450" — Geologica Regular, 32px, tracking: -1.6px
Bölücü:      border-bottom: 1px solid rgba(255,255,255,0.1)

Liste:       Inter Regular, 14.9px, #C2C6D8, gap: 17px
             İkon: küçük check (SVG)

Satın Al:    border: 1px solid rgba(27,109,255,0.5)
             Geologica Bold, 12.8px, #1b6dff, tracking: 1.28px, uppercase
             border-radius: 8.5px, padding: 13.8px
```

**Kart 2 — DÖNÜŞÜM PAKETİ (orta, highlighted)**
```
background:  gradient from rgba(27,109,255,0.05) to #070c1a
border:      1.068px solid #1b6dff
shadow:      0 0 42.7px 0 rgba(27,109,255,0.3)
Üst badge:   "EN ÇOK TERCİH EDİLEN" — sağ üst köşe
             background: #ffea00, border-radius: 0 0 0 10.7px
             Inter Black, 10.7px, #070c1a, tracking: 1.07px, uppercase
             padding: 4.3px 12.8px

Başlık:      "DÖNÜŞÜM PAKETİ"
Süre rengi:  #1b6dff (diğerlerinde #C2C6D8)
Fiyat:       "₺ 8.250" — 38.4px (diğerlerinden büyük)
Bölücü:      border-bottom: 1px solid rgba(27,109,255,0.3)

Özel madde:  "7/24 WhatsApp İletişim Desteği"
             background: rgba(27,109,255,0.1), border: 1px solid rgba(27,109,255,0.2)
             padding: 9.6px, farklı check ikonu

Satın Al:    background: #1b6dff (dolu, diğerlerinde outline)
             Geologica Bold, beyaz, aynı tipografi
```

**Kart 3 — ELITE PAKET (sağ)**
```
BAŞLANGIÇ PAKETİ ile aynı stil
Süre: "SÜRE: 24 HAFTA", Fiyat: "₺ 14.900"
Liste: Supplement Protokolü (ekstra madde)
```

### Beslenme Section
```
Etiket:    "BESLENME PROGRAMI" — Inter Bold, 12.9px, #C2C6D8, tracking: 1.29px, uppercase
Başlık:    "HEDEFİNE UYGUN BESLEN, DÖNÜŞÜMÜ HIZLANDIR"
           Geologica ExtraBold, 42.9px, line-height: 51.5px, tracking: -0.86px
           uppercase, ortalı, beyaz — ikinci satır #1b6dff
Açıklama:  Inter Regular, 19.3px, line-height: 30px, #C2C6D8, ortalı

İçerik kutu: background: #111c33, border: 1.07px solid rgba(27,109,255,0.5)
             border-radius: 21.5px, padding: 1px (içte iki sütun)
Sol sütun:   Yemek görseli (image asset)
Sağ sütun:   padding: 51.5px
             Başlık: Inter Bold, 25.75px, tracking: -0.26px, uppercase, beyaz
             Liste: Inter Regular, 17.2px, line-height: 25.75px, #C2C6D8
             Buton: "DİYET PROGRAMI HAKKINDA DAHA FAZLA BİLGİ"
                    background: #070c1a, border: 1.07px solid #1b6dff
                    border-radius: 10728px (pill!), padding: 18px 26.8px
                    Inter Bold, 12.9px, beyaz, tracking: 1.29px, uppercase
```

### CTA Section (Hangi Paket?)
```
Başlık:    "HANGİ PAKETİN SANA UYGUN OLDUĞUNDAN EMİN DEĞİL MİSİN?"
           Geologica Black, 35.7px, line-height: 42.9px, tracking: 1.79px
           uppercase, ortalı, beyaz
Alt yazı:  Inter Regular, 16.7px, #C2C6D8, tracking: 1.67px, uppercase
Butonlar:  "Hemen Başla" (#2B7EFF) + "WhatsApp İletişim" (#25D366)
           aynı anasayfa buton stilleri
```

### Navbar — Paketler Sayfası Farkı
```
Menü linkleri: Ana Sayfa, Paketler, Hikayemiz, İletişim (4 link, anasayfadan farklı!)
Sağda: user ikonu + search ikonu (anasayfada yoktu)
Aktif: "Paketler" aktif görünüyor mu? Figma'da Ana Sayfa SemiBold, diğerleri ExtraLight
```

---

## Anasayfa (node: 1:3988) — Özet Token'lar

### Hero
```
H1:          Geologica SemiBold, 60px, beyaz
"GÜÇLÜ":     Geologica Black, uppercase, #2B7EFF
Body:        Inter Regular, 16px, line-height: 24px, rgba(255,255,255,0.8)

Hemen Başla: #2B7EFF, border-radius: 8px, padding: 19px 60px
             Inter SemiBold, 20px, beyaz
WhatsApp:    #25D366, aynı padding/radius
             Iconify whatsapp + "WhatsApp İletişim", gap: 10px
```

### Stats Bar
```
4 item: 100+/95%/5+/3+
Sayı:   Geologica Bold, 35px, beyaz
Label:  Inter Regular, 20px, #C2C6D8
Gap:    136px arası, padding: 157px sol/sağ
Bg:     rgba(255,255,255,0.1), backdrop-blur: 18px
        border: 1px solid rgba(255,255,255,0.15)
        border-radius: büyük (pill)
```
