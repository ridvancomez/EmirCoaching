# EmirCoaching — Backend API

Kişisel antrenörlük hizmeti için N-Tier mimaride geliştirilen ASP.NET Core backend.

## Teknoloji Stack

- **Framework:** ASP.NET Core 10.0
- **ORM:** Entity Framework Core
- **Veritabanı:** MSSQL
- **Validasyon:** FluentValidation
- **Mimari:** N-Tier (Entities → DataAccess → Business → Web)

## Proje Yapısı

EmirCoaching/
├── EmirCoaching.Entities/      # Domain modelleri, BaseEntity, Identity
├── EmirCoaching.DataAccess/    # AppDbContext, Repository/UnitOfWork, migrations
├── EmirCoaching.Business/      # Servisler, DTO'lar, FluentValidation validators
└── EmirCoaching.Web/           # Controllers, Views, Admin paneli
## Mimari Kararlar

- **Repository + Unit of Work Pattern** — `IRepository<T>` generic repository, servis katmanında işlem bütünlüğü için `IUnitOfWork` (transaction desteğiyle)
- **Soft Delete** — `BaseEntity.IsDeleted` üzerinden global query filter; kayıtlar fiziksel silinmez
- **Result Pattern** — Servis katmanı `Result<T>` döner, exception yerine kontrollü hata yönetimi
- **DTO + FluentValidation** — Entity'ler doğrudan dışa açılmaz, her DTO için ayrı validator

## Domain Modeli (Özet)

15+ entity ile gerçek bir iş modeli:
- **Package** — 3 paket × 2 lokasyon (Evde/Salonda) = 6 kayıt; modüller ve highlight'larla ilişkili
- **Transformation** — önce/sonra müşteri dönüşüm kayıtları, kategori bazlı (yağ kaybı, kas kazanımı, form koruma)
- **DietFormSubmission** — diyet formu başvuruları, Türk telefon format validasyonu
- **ConsentRecord** — KVKK/açık rıza/ticari ileti onay kayıtları (IP, zaman damgası, İYS durumu ile)
- **BlogPost**, **Faq**, **Testimonial**, **LegalDocument** — içerik yönetimi

## Kurulum

```bash
git clone https://github.com/ridvancomez/EmirCoaching.git
cd EmirCoaching
dotnet restore
dotnet ef database update --project EmirCoaching.DataAccess --startup-project EmirCoaching.Web
dotnet run --project EmirCoaching.Web
```

## Durum

🚧 Aktif geliştirme aşamasında.