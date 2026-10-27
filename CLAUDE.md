# Backend Kuralları — EmirCoaching API

## Genel Prensipler

N-Tier mimarisi (Entities → DataAccess → Business → API) kesinleşti. Sapmaya kapalıdır.
Tüm iş mantığı Business katmanında, veri erişimi DataAccess katmanında, HTTP sadece API'de.
Web projesi yalnızca statik siteyi sunar; veriyi API'den alır.
Entity asla HTTP yanıtında dışarı çıkmaz — DTO kullanılır.

---

## Çalışma Şekli

- **Arada kalırsan sor:** Bir istek birden fazla şekilde yorumlanabiliyorsa, mevcut tasarımla çelişiyorsa veya kapsamı belirsizse varsayım yapma. İşe başlamadan önce kullanıcıya sor.
- **Sadece isteneni değiştir:** Yalnızca açıkça istenen şey kodlanır. Örnek: "DietFormSubmission validation'ını ekle" deniyorsa başka entity'ler dokunulmaz. Ek iyileştirme gerekli görünüyorsa uygulamadan önce öner ve onay al.
- **Her kod değişikliğinde mimariye bak:** Katman sınırlarını ihlal etmiyor mu? Entity'nin yeri doğru mu? DTO mapping var mı? Async/await düzgün mü? Bu kontrol atlanmaz.

---

## Naming Conventions

- **Classes, Properties, Enums:** PascalCase (C# standard)
  - Entity: `BlogPost`, `ConsentRecord`, `DietFormSubmission`
  - DTO: `BlogPostDto`, `CreateBlogPostRequest`, `BlogPostResponse`
  - Enum: `ConsentType`, `PackageLocation`, `TransformationCategory`
  - Interface: `IRepository<T>`, `IBlogPostRepository`, `IPackageService`
  
- **Private/Internal Fields:** camelCase (`_dbContext`, `_uow`, `_logger`)

- **Methods:** PascalCase
  - Public: `GetByIdAsync()`, `CreateAsync()`, `UpdateAsync()`
  - Private: `ValidateInput()`, `MapToDto()`

- **Variables (local):** camelCase (`var result`, `int userId`)

---

## Katman Sorumluluğu

### **Entities** (`EmirCoaching.Entities`)
- Domain modelleri (BaseEntity, Package, BlogPost, vb.)
- Enum'lar (ConsentType, PackageLocation, vb.)
- İş kuralı değil; sadece veri şekli
- Navigation properties: gerekli olanlar

### **DataAccess** (`EmirCoaching.DataAccess`)
- `AppDbContext`, migration'lar
- `IRepository<T>` ve özel repository'ler
- Soft delete query filter (`IsDeleted == false`)
- Seed data
- **Önemli:** SaveChangesAsync override'ında Delete → soft delete dönüştürme

### **Business** (`EmirCoaching.Business`)
- Service interface'leri (`IPackageService`, vb.)
- DTO tanımları ve mapping (AutoMapper veya manuel)
- FluentValidation validator'ları
- `Result<T>` döndürme
- **Önemli:** Entity asla dışarı sızmaz

### **API** (`EmirCoaching.API`)
- Controller'lar, route'lar, HTTP cevapları
- Dependency Injection setup
- Auth (JWT), CORS, middleware
- Yüklenen dosyalar (`wwwroot/uploads`) burada durur; DB'de göreli yol tutulur, yanıtta `Storage:PublicBaseUrl` ile tam URL döner
  - Response'ta alan adı `*Url` (`CoverImageUrl`), Request'te `*Path` (`CoverImagePath`); çeviri `MappingProfile`'da `PublicUrlConverter` / `RequiredPublicUrlConverter` ile
  - Yalnızca `/uploads/...` yolları API adresini alır; sitenin kendi görselleri (`/images/...`) göreli kalır
  - Yükleme akışı: panel görseli önce `POST /api/v1/admin/uploads/images` ile yükler (multipart `file` + isteğe bağlı `folder`; jpg/jpeg/png/webp, en fazla 5 MB, içerik imzası kontrol edilir) → `{ path, url }`; sonra kayıt isteğinin `*Path` alanına yazar. PDF yalnızca yasal metin ucundan yüklenir
  - Görsel temizliği (`IUploadCleanupService`): görseli değişen veya silinen kaydın eski dosyası, silinmemiş başka hiçbir kayıt kullanmıyorsa kaydettikten **sonra** silinir (`UploadPaths.Of(entity)` eski yolları bulur). `CrudService`'te `ReleaseUploadsAsync` ezilir; kendi akışı olan serviste `DeleteUnusedAsync` doğrudan çağrılır. Yasal metin PDF'leri (eski sürümler dahil) hiç silinmez
  - Hangi dosyanın kullanıldığı `DataAccess/Repositories/Concrete/UploadReferenceRepository.cs`'ten okunur: **yeni bir `*Path` kolonu eklenince oraya da eklenir**, yoksa o kolonun dosyaları temizlikte silinir
  - Request'teki `*Path` alanı yanıttaki tam URL'yi de kabul eder: `Common/StoredPathJson.cs` (JSON TypeInfo modifier) `Dtos` namespace'indeki string `*Path` setter'larını `StorageOptions.ToStoredPath` ile göreli yola çevirir. Böylece panel GET'te aldığı `*Url` değerini PUT'ta aynen geri gönderebilir; başka alan adındaki URL doğrulamada reddedilir
- EF migration komutlarında startup project: `--startup-project EmirCoaching.API`
- **Önemli:** iş mantığı YOK; sadece Service çağırma

### **Web** (`EmirCoaching.Web`)
- Yalnızca statik site (`wwwroot`: HTML/CSS/JS); Business/DataAccess referansı yok
- Veri `fetch` ile API'den alınır

---

## API Design Standards

**Önemli:** Tüm endpoint'ler REST standartlarına uyar. Frontend ve gelecekteki mobil app aynı API'yi tüketecek.

### Endpoint Naming & Routing

Site (herkese açık) ve admin uçları ayrı controller'lardadır:

```csharp
// Site — Controllers/{Resource}Controller.cs: yalnızca okuma (aktif/yayında kayıtlar) + form gönderimi
GET    /api/v1/packages                       // aktif paketler
GET    /api/v1/packages/{slug}                // paket detay
GET    /api/v1/blog-posts?category=&page=&size=
POST   /api/v1/diet-form-submissions          // form → 204
POST   /api/v1/contact-messages
POST   /api/v1/newsletter-subscribers

// Admin — Controllers/Admin/{Resource}Controller.cs, AdminControllerBase'ten türer ([Authorize] tabanda)
GET    /api/v1/admin/packages                 // pasifler dahil
GET    /api/v1/admin/packages/{id}
POST   /api/v1/admin/packages                 // 201
PUT    /api/v1/admin/packages/{id}            // 204
DELETE /api/v1/admin/packages/{id}            // 204 (soft)
PUT    /api/v1/admin/packages/{id}/modules/order   // gövde: [3, 1, 2]

// Admin — diğer kaynaklar
// Tam CRUD (GET, GET {id}, POST, PUT {id}, DELETE {id}):
//   blog-categories, transformation-types, transformations (pasifler dahil), testimonials (?packageId=), faqs,
//   video-questions, site-stats, social-links, social-posts, diet-program-steps ("Nasıl Çalışır?" adımları)
GET    /api/v1/admin/blog-posts?categoryId=&isPublished=&page=&size=   // taslaklar dahil
POST   /api/v1/admin/blog-posts               // taslak; AuthorId yoksa giriş yapan kullanıcı
POST   /api/v1/admin/blog-posts/{id}/publish  // 204
POST   /api/v1/admin/blog-posts/{id}/unpublish // 204, taslağa döner (ilk yayın tarihi korunur)
GET|PUT /api/v1/admin/site-settings
GET    /api/v1/admin/page-contents            // GET|PUT /page-contents/{pageKey}
GET|PUT /api/v1/admin/diet-program
GET    /api/v1/admin/legal-documents/{type}   // + GET {type}/revisions
POST   /api/v1/admin/legal-documents/{type}/versions   // multipart: file (PDF) + note
GET    /api/v1/admin/contact-messages?status=&page=&size=        // + PATCH {id}/status, DELETE {id} (kalıcı)
GET    /api/v1/admin/diet-form-submissions?status=&page=&size=   // + PATCH {id}/status, DELETE {id} (kalıcı)
GET    /api/v1/admin/newsletter-subscribers?page=&size=          // + DELETE {id} (kalıcı)
GET    /api/v1/admin/login-attempts?result=&page=&size=          // giriş geçmişi
POST   /api/v1/admin/uploads/images           // multipart: file + folder → { path, url }

// Auth
POST   /api/v1/auth/login                     // Token al
POST   /api/v1/auth/refresh                   // Token refresh
```

**Convention:**
- lowercase, kebab-case; route controller'da açıkça yazılır (`[Route("api/v1/diet-form-submissions")]`)
- `/api/v1/` prefix (versioning), admin için `/api/v1/admin/`
- Resource adı entity'nin çoğulu (`DietFormSubmission` → `diet-form-submissions`); tek kayıtlı kaynaklar tekil (`site-settings`, `diet-program`)
- Pasif/taslak kavramı olan kaynaklarda (paket, değişim `IsActive`; blog `IsPublished`) site uçları yalnızca aktif/yayındakileri döner, pasif kaydın site detayı 404'tür; admin uçları hepsini döner. Form kayıtları (iletişim, diyet başvurusu, bülten) KVKK gereği **kalıcı** silinir: `IPersonalDataRepository<T>.DeletePermanentlyAsync` kaydı bağlı onay kayıtlarıyla birlikte `ExecuteDelete` ile siler (soft delete'e dönüşmez); transaction `UnitOfWorkExtensions.DeletePermanentlyAsync`'te
- Paket güncellemesinde modül, kalem ve öne çıkan maddeler, değişim güncellemesinde ölçümler numarasıyla eşitlenir (`Common/ChildListSync.cs`): istekte `Id` dolu olan güncellenir, boş olan eklenir, listede olmayan silinir; başka üst kayda ait veya tekrar eden `Id` 400 döner
- Site uçları slug'ı olan kaynakta slug (`/packages/{slug}`), olmayanda ID (`/transformations/{id}`), admin uçları her zaman ID ile çalışır; ID/slug path'te, filtreler query'de
- Admin controller'ları `AdminControllerBase`'ten türer (`EmirCoaching.API/Common/AdminControllerBase.cs`): `[Authorize]` ve `[Area("Admin")]` orada, controller'da tekrar yazılmaz. Sınıf adı site controller'ıyla aynı olabilir (`Controllers/Admin/PackagesController`); örnek: paketler
- Herkese açık POST (form) controller'ları `[EnableRateLimiting(RateLimitPolicies.Forms)]` taşır (IP başına dakikada 5); IP, `UseForwardedHeaders` sayesinde proxy arkasında da gerçek istemcinindir

### HTTP Methods & Status Codes

```csharp
// GET — Veri al
GET /api/v1/packages/1
Response: 200 OK + veri

// POST — Yeni kayıt oluştur
POST /api/v1/packages
Response: 201 Created + { "id": 3 } (+ Location header: /api/v1/packages/3)

// PUT — Var olan kaydı güncelle (full replace)
PUT /api/v1/packages/1
Response: 204 No Content

// PATCH — Kısmi güncelleme (gelecek)
// DELETE — Soft delete
DELETE /api/v1/packages/1
Response: 204 No Content

// Hata kodları (gövde: ProblemDetails):
400 Bad Request         — Doğrulama hatası, bozuk istek    (ResultErrorType.Validation)
401 Unauthorized        — JWT token yok/geçersiz, hatalı giriş (ResultErrorType.Unauthorized)
403 Forbidden           — Auth var ama permission yok
404 Not Found           — Kayıt bulunamadı                 (ResultErrorType.NotFound)
409 Conflict            — Çakışma (slug, platform), silme engeli (ResultErrorType.Conflict)
429 Too Many Requests   — İstek sınırı (RateLimitPolicies.Forms) veya iş kuralı sınırı (ResultErrorType.TooManyRequests)
500 Internal Server Error — Beklenmeyen hata (GlobalExceptionHandler)
```

### API Response Format

**Başarı → zarfsız veri, hata → `ProblemDetails` (RFC 9457).** `{ success, data, message }` gibi bir zarf kullanılmaz; başarı bilgisi HTTP durum kodundadır. İstemci `response.ok` ile kontrol eder.

```jsonc
// 200 — GET: doğrudan Response DTO'su (liste ise dizi)
{ "id": 1, "name": "Başlangıç Paketi", ... }

// 201 — POST: yeni kaydın id'si + Location header
{ "id": 3 }

// 204 — PUT / DELETE: gövde yok

// Hata — Content-Type: application/problem+json
// title: 400'de sabit "Gönderilen bilgiler geçersiz.", diğerlerinde servisin mesajı
// errors: her zaman mesaj listesi (string[])
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "Gönderilen bilgiler geçersiz.",
  "status": 400,
  "traceId": "00-...",
  "errors": ["'Ad' boş olmamalı.", "Fiyat 0'dan büyük olmalı."]
}
{ "type": "...", "title": "Paket bulunamadı.", "status": 404, "traceId": "00-...", "errors": ["Paket bulunamadı."] }
{ "type": "...", "title": "Beklenmeyen bir hata oluştu.", "status": 500, "traceId": "00-..." } // ayrıntı sızdırılmaz
```

- Tek format: servis hataları, model binding hataları (bozuk JSON, yanlış tip) ve 500 aynı `ProblemDetails` şeklinde döner. Ortak üretici `EmirCoaching.API/Common/ApiProblem.cs`; model binding için `Program.cs` → `InvalidModelStateResponseFactory`.
- URL'ler küçük harf (`LowercaseUrls`); route'lar controller'da açıkça yazılır (`[Route("api/v1/diet-forms")]`).

**Controller'da:** Her controller `ApiControllerBase`'ten türer (`EmirCoaching.API/Common/ApiControllerBase.cs`). `Result` → HTTP çevirisi yalnızca oradaki yardımcılarla yapılır; controller'da `if (!result.IsSuccess)` yazılmaz.

```csharp
[Route("api/v1/faqs")]
public class FaqsController(IFaqService faqService) : ApiControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) =>
        FromResult(await faqService.GetByIdAsync(id));                              // 200 / 404

    [HttpPost]
    public async Task<IActionResult> Create(CreateFaqRequest request) =>
        CreatedFromResult(await faqService.CreateAsync(request), nameof(GetById));  // 201 / 400 / 409

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateFaqRequest request) =>
        FromResult(await faqService.UpdateAsync(request with { Id = id }));         // 204 / 400 / 404 / 409

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        FromResult(await faqService.DeleteAsync(id));                               // 204 / 404 / 409
}
```

- PUT'ta id route'tan alınır ve `request with { Id = id }` ile gövdedekinin üzerine yazılır.

### Pagination & Filtering

Sayfalı listeler `PagedResult<T>` döner (`EmirCoaching.Business/Common/PagedResult.cs`); `size` en fazla 100.

```jsonc
// GET /api/v1/blog-posts?page=1&size=10&category=beslenme
{
  "items": [ { "id": 1, "title": "...", ... }, ... ],
  "totalCount": 25,
  "page": 1,
  "size": 10,
  "totalPages": 3
}
```

**Service'te:** sorgu repository'de (`ToPagedListAsync`), servis yalnızca map'ler:
```csharp
public async Task<Result<PagedResult<BlogPostResponse>>> GetPagedAsync(string? categorySlug, int page, int size)
{
    var posts = await unitOfWork.BlogPosts.GetPublishedPagedAsync(categorySlug, page, size);
    return Result<PagedResult<BlogPostResponse>>.Success(mapper.MapPaged<BlogPost, BlogPostResponse>(posts));
}
```

### API Versioning

URL'de versioning:

```csharp
// appsettings.json (RoutePrefix)
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PackagesController : ControllerBase { }

// Deprecated endpoint gerekirse:
[ApiVersion("1.0", Deprecated = true)]
[Route("api/v{version:apiVersion}/old-packages")]
public class OldPackagesController : ControllerBase { }
```

### CORS Policy

```csharp
// Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000", "https://emircoaching.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

app.UseCors("AllowFrontend");
```

### Error Handling Middleware

Servislerde try/catch yazılmaz. Yakalanmayan her hata `EmirCoaching.API/Middleware/GlobalExceptionHandler.cs` içinde **Error** seviyesinde loglanır ve istemciye ayrıntı sızdırmadan standart `ProblemDetails` (500) döner.

```csharp
// Program.cs
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
...
app.UseExceptionHandler();
```

### API Documentation (Swagger/OpenAPI)

```csharp
// Program.cs
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EmirCoaching API",
        Version = "v1.0",
        Description = "REST API for coaching platform"
    });

    // JWT auth header
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
});

app.UseSwagger();
app.UseSwaggerUI();
```

---

## Result<T> Pattern

Her Service methodu `Result` (veri yoksa) veya `Result<T>` döner — okumalar dahil. Sınıf: `EmirCoaching.Business/Common/Result.cs`.

```csharp
public enum ResultErrorType { None, Validation, NotFound, Conflict, TooManyRequests, Unauthorized } // → 400 / 404 / 409 / 429 / 401

// Result:    IsSuccess, ErrorType, Errors
// Result<T>: + Value
Result.Success();                      Result<int>.Success(id);
Result.Invalid(errors);                Result<T>.Invalid("Mesaj");
Result.NotFound("Paket bulunamadı.");  Result<T>.Conflict("Bu slug kullanılıyor.");
Result.TooManyRequests("...");         // ör. aynı IP'den 24 saatte 3'ten fazla bülten aboneliği
Result<T>.Failure(digerResult);        // başarısız Result'ı aynı hata türüyle Result<T>'ye taşır
```

- Bulunamadı `null` ile değil `Result.NotFound` ile bildirilir; mesaj servisten gelir.
- `Result` HTTP'ye çıkmaz: controller `ErrorType`'a göre durum kodunu tek bir yardımcıyla üretir.

---

## FluentValidation

Her Request için validator; dosyalar entity başına (`Validation/{Entity}Validators.cs`). Ortak kurallar `ValidationRules` içinde (`Slug()`, `Phone()`, `HttpsUrl()`, `YouTubeUrl()`, `SitePath()`, `Rating()`).

**Kullanıcıya giden her mesaj Türkçedir** (alan adları dahil):
- Hazır mesajlar `Validation/TurkishValidation.cs`'te (kütüphanenin kendi çevirisi hatalı olduğu için ezilir); alan başına tek mesaj (`CascadeMode.Stop`).
- Alan adları `Validation/FieldNames.cs`'te: **yeni bir Request alanı eklenince buraya Türkçe adı da eklenir** (yoksa mesajda `PhoneNumber` gibi property adı görünür).
- Servise ulaşmayan hatalar (bozuk JSON, yanlış tip, eksik alan, 415) `API/Common/TurkishModelBinding.cs`'te; kullanıcının girdiği değer mesaja yazılmaz.
- Identity mesajları `Validation/TurkishIdentityErrorDescriber.cs`'te.

```csharp
public class CreateFaqRequestValidator : AbstractValidator<CreateFaqRequest>
{
    public CreateFaqRequestValidator()
    {
        RuleFor(x => x.Question).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Group).IsInEnum();
    }
}

// Update = Create kuralları + Id > 0
public class UpdateFaqRequestValidator()
    : UpdateRequestValidator<UpdateFaqRequest, CreateFaqRequest>(new CreateFaqRequestValidator());
```

Validator'lar DI'dan enjekte edilir (`new` ile oluşturulmaz):
```csharp
var errors = await createValidator.GetErrorsAsync(request);
if (errors.Count > 0)
    return Result<int>.Invalid(errors);
```

---

## Logging (Serilog)

API projesinde kurulu (`Serilog.AspNetCore`, `Serilog.Sinks.MSSqlServer`). Ayarlar `appsettings.json` → `Serilog` bölümünde; konsol + ayrı log veritabanı (`ConnectionStrings:LogConnection` → `EmirCoachingLog`, `Logs` tablosu otomatik oluşur). İstek logları `UseSerilogRequestLogging()` ile.

Service'te `ILogger<{Service}>` enjekte edilir:
```csharp
logger.LogInformation("Paket {Id} oluşturuldu", package.Id);
logger.LogWarning("Paket {Slug} bulunamadı", slug);
```

**Log seviyeleri:**
- **Information:** başarılı yazma işlemi (oluşturma/güncelleme/silme, form alındı)
- **Warning:** bulunamadı, doğrulama/iş kuralı reddi
- **Error:** beklenmeyen hata — yalnızca `GlobalExceptionHandler`'da (servislerde try/catch yok)
- Kişisel/sağlık verisi loga yazılmaz; yalnızca kayıt numarası
- Log mesajları Türkçe yazılır. Framework'ün kendi uyarıları ve exception ayrıntıları (.NET'in metinleri) İngilizce kalır; bunlar çevrilemez

---

## Async/Await (Zorunlu)

**Kural:** Her DB, HTTP, I/O call async olmalı. Sync yok.

```csharp
// ✓ Doğru
public async Task<Result<PackageResponse>> GetByIdAsync(int id)
{
    var package = await unitOfWork.Packages.GetByIdAsync(id);
    return Result<PackageResponse>.Success(mapper.Map<PackageResponse>(package));
}

// ✗ Yanlış — sync call
public async Task<Result<PackageResponse>> GetByIdAsync(int id)
{
    var package = unitOfWork.Packages.GetById(id); // ❌ sync
    return Result<PackageResponse>.Success(mapper.Map<PackageResponse>(package));
}

// ✗ Yanlış — .Result (deadlock riski)
var package = unitOfWork.Packages.GetByIdAsync(id).Result; // ❌ deadlock
```

---

## Entity Design Rules

- **BaseEntity:** `Id`, `CreatedAt`, `UpdatedAt?`, `IsDeleted`, `DeletedAt?` zorunlu
- **Soft Delete:** `SaveChangesAsync()` override'ında Delete → soft delete
- **Global Filter:** `DbContext.OnModelCreating()` → `.HasQueryFilter(e => !e.IsDeleted)`
- **Navigation:** `Include()` explicit, lazy loading riski yok
- **Query Optimization:** Complex select'lerde `Select(x => new Dto {...})` ile entity tam yükleme

---

## Repository Pattern

- **IRepository<T>:** GetByIdAsync, GetAllAsync, AddAsync, Update, Remove (IQueryable dışarı çıkmaz)
- **Özel Repository:** `IBlogPostRepository` gibi — complex query'ler burada
- **UnitOfWork:** `SaveChangesAsync()`, transaction metodları

```csharp
public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}
```

### Tutarlı isimlendirme DRY'dan önce gelir

- **Her entity kendi repository'sini alır**, içi boş olsa bile: `I{Entity}Repository` + `{Entity}Repository`.
- **UoW property adı = DbSet adı:** `uow.Testimonials`, `uow.DietFormSubmissions`. Bir kayda tek yoldan erişilir; `Repository<T>()` gibi generic erişim yok.
- **Tekrar eden kod abstract tabanlarda durur:** `Repository<T>`, `SortedRepository<T>`, `ActiveSortedRepository<T>`, `SlugRepository<T>` ve `Extensions/QueryableExtensions`. Generic tipler UoW'de veya servislerde dışarı açılmaz.
- **Repository'si olmayanlar** üst kaydı üzerinden yönetilir; `IUnitOfWork`'ün başında listelidir.
- **Tek istisna** `uow.UploadReferences`: bir entity'ye değil, tüm `*Path` kolonlarına bakan salt okunur sorgu (görsel temizliği).
- **Aynı kural Business ve API katmanında da geçerli:** her kaynağın kendi servisi/controller'ı olur, ortak mantık taban sınıfa çekilir.

```csharp
public interface ITestimonialRepository : IActiveSortedRepository<Testimonial>;
public class TestimonialRepository(AppDbContext context)
    : ActiveSortedRepository<Testimonial>(context), ITestimonialRepository;
```

---

## Service Pattern

- **İsim:** `I{Entity}Service` + `{Entity}Service` (`IBlogPostService`, `IDietFormSubmissionService`)
- **DTO:** `Create{Entity}Request`, `Update{Entity}Request : Create{Entity}Request, IUpdateRequest`, `{Entity}Response` (`Dtos/{Entity}Dtos.cs`). Entity asla dışarı çıkmaz.
- **Dönüş:** her metot `Result` / `Result<T>`
- **Hata:** try/catch yok; beklenmeyen hata `GlobalExceptionHandler`'a gider. Transaction açan yerde yalnızca `catch { Rollback; throw; }`
- **DI:** primary constructor (`IUnitOfWork`, `IMapper`, `IValidator<>`, `ILogger<>`)

### Basit CRUD: `CrudService` tabanı

Ek iş akışı olmayan içerikler (SSS, sayaç, yorum, video, sosyal link/post, blog kategorisi, dönüşüm tipi) `CrudService<TEntity, TResponse, TCreateRequest, TUpdateRequest>`'ten türer. Taban; doğrulama → `CheckAsync` → kaydet → log akışını ve NotFound'u yönetir. Türeyen servis yalnızca farklı olanı ezer:

```csharp
public interface IBlogCategoryService
    : ICrudService<BlogCategoryResponse, CreateBlogCategoryRequest, UpdateBlogCategoryRequest>;

public class BlogCategoryService(IUnitOfWork unitOfWork, IMapper mapper,
    IValidator<CreateBlogCategoryRequest> createValidator, IValidator<UpdateBlogCategoryRequest> updateValidator,
    ILogger<BlogCategoryService> logger)
    : CrudService<BlogCategory, BlogCategoryResponse, CreateBlogCategoryRequest, UpdateBlogCategoryRequest>(
        unitOfWork.BlogCategories, unitOfWork, mapper, createValidator, updateValidator, logger), IBlogCategoryService
{
    // slug/platform çakışması, ilişkili kayıt kontrolü (id güncellemede dolu)
    protected override async Task<Result> CheckAsync(CreateBlogCategoryRequest request, int? id) =>
        await UnitOfWork.BlogCategories.SlugExistsAsync(request.Slug, id)
            ? Result.Conflict("Bu slug başka bir kategoride kullanılıyor.")
            : Result.Success();

    // silmeyi engelleyen durum
    protected override async Task<Result> CheckDeleteAsync(BlogCategory category) => ...;
}
```

- Türeyen sınıfta constructor parametresi yerine tabanın `UnitOfWork` / `Mapper` / `Logger` property'leri kullanılır.
- Admin listesinin sırası `ListAsync` ezilerek verilir (`GetOrderedAsync`).
- Kendi akışı olan servisler (Package, BlogPost, Transformation, formlar, tek kayıtlılar) tabandan türemez ama aynı kurallara uyar.

---

## JWT & Authentication

Yalnızca admin paneli giriş yapar. Kullanıcılar ASP.NET Identity (`UserManager<AppUser>`) ile yönetilir; ilk admin `SeedData.SeedAdminUserAsync` ile user-secrets'tan oluşur (`AdminUser:Email`, `AdminUser:Password`, `AdminUser:FullName`).

- **Access token:** 15 dk, yanıt gövdesinde döner; istemci bellekte tutar ve `Authorization: Bearer ...` ile gönderir.
- **Refresh token:** 7 gün, HttpOnly + Secure + SameSite=Strict cookie (`refresh_token`, path `/api/v1/auth`). DB'de yalnızca SHA-256 özeti (`RefreshTokens` tablosu). Her kullanımda iptal edilip yenisi verilir; iptal edilmiş bir token tekrar gelirse kullanıcının tüm oturumları kapatılır.
- **Ayarlar:** `appsettings.json` → `Jwt` (Issuer, Audience, süreler); `Jwt:SecretKey` yalnızca user-secrets / ortam değişkeni, en az 32 bayt — yoksa uygulama açılmaz.
- **Koruma:** 5 hatalı şifrede Identity lockout; `/auth/login` IP başına dakikada 5 istek (`RateLimitPolicies.Login`). Hatalı e-posta ve şifre aynı mesajı alır.
- **Yetki:** `[Authorize]` varsayılan politikası = giriş yapmış + `Admin` rolü. 401/403 de `ProblemDetails` formatında döner.

```
POST /api/v1/auth/login    { email, password } → 200 { accessToken, expiresAt } + cookie / 400 / 401 / 429
POST /api/v1/auth/refresh  (cookie)            → 200 { accessToken, expiresAt } + yeni cookie / 401
POST /api/v1/auth/logout   (cookie)            → 204, refresh token iptal, cookie silinir
GET  /api/v1/auth/me       [Authorize]         → 200 { id, email, fullName, roles }
```

| Katman | Dosya |
|---|---|
| Entities | `Identity/RefreshToken.cs` |
| DataAccess | `IRefreshTokenRepository` → `uow.RefreshTokens` |
| Business | `AuthService` (giriş/yenileme/çıkış), `JwtTokenService` (token üretimi), `Common/JwtOptions.cs`, `Dtos/AuthDtos.cs` |
| API | `AuthController`, `Program.cs` (JwtBearer, varsayılan politika, CORS `AllowCredentials`) |

Controller'da kullanıcı kimliği: `CurrentUserId` (`ApiControllerBase`, token'daki `sub`).

**Giriş geçmişi:** `AuthService.LoginAsync` her denemeyi `LoginAttempts` tablosuna yazar (sonuç: Basarili / KullaniciYok / HataliSifre / Kilitli, IP, tarayıcı, eşleştiyse kullanıcı). Girilen şifre ve bulunamayan e-posta **asla** yazılmaz. Panelde `GET /api/v1/admin/login-attempts`.

### Günlük bakım

`API/BackgroundJobs/MaintenanceWorker.cs` uygulama açıldıktan 1 dk sonra ve günde bir kez `IMaintenanceService.RunAsync`'i çalıştırır (hata Error olarak loglanır, uygulama durmaz). Kalıcı silinenler:
- Hiçbir kayda bağlı olmayan, 24 saatten eski yüklemeler (panelde yüklenip kaydedilmeyen görseller dahil)
- 90 günden eski giriş geçmişi
- Süresi 30 günden önce dolmuş refresh token'lar (iptal edilenler süresi dolana kadar tutulur ki tekrar kullanım yakalansın)

---

## Unit Testing — Baştan Sona Kılavuz

Unit test henüz yapılmamış. Burada sistematik şekilde başlayacağız.

### Framework & Tools Seçimi

- **xUnit:** test framework (industry standard, Microsoft tarafından desteklenen)
- **Moq:** dependency mocking (IRepository, IUnitOfWork, ILogger mock'lanır)
- **FluentAssertions:** assertion yazımı ve okunabilirliği (Assert.AreEqual yerine `.Should().Be()`)

### Proje Yapısı

```
EmirCoaching.Tests/
├── Services/
│   ├── PackageServiceTests.cs
│   ├── BlogPostServiceTests.cs
├── Repositories/
│   ├── PackageRepositoryTests.cs
├── Validators/
│   ├── CreateBlogPostRequestValidatorTests.cs
```

### Kurulum: xUnit + Moq + FluentAssertions

**Package.csproj'a ekle:**
```xml
<ItemGroup>
    <PackageReference Include="xunit" Version="2.6.0" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.0" />
    <PackageReference Include="Moq" Version="4.20.0" />
    <PackageReference Include="FluentAssertions" Version="6.12.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="8.0.0" />
</ItemGroup>
```

### AAA Pattern (Arrange-Act-Assert)

Her test 3 bölümde yazılır:

```csharp
[Fact]
public async Task GetByIdAsync_WithValidId_ReturnsSuccessResult()
{
    // Arrange (setup)
    int packageId = 1;
    var package = new Package { Id = packageId, Name = "Test", IsDeleted = false };
    _mockRepository.Setup(x => x.GetByIdAsync(packageId)).ReturnsAsync(package);

    // Act (test et)
    var result = await _service.GetByIdAsync(packageId);

    // Assert (kontrol et)
    result.Success.Should().BeTrue();
    result.Data.Should().NotBeNull();
    _mockRepository.Verify(x => x.GetByIdAsync(packageId), Times.Once);
}
```

### Örnek: Service Tests (xUnit + Moq + FluentAssertions)

```csharp
public class PackageServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<IRepository<Package>> _mockRepository;
    private readonly Mock<ILogger<PackageService>> _mockLogger;
    private readonly IMapper _mapper;
    private readonly PackageService _service;

    public PackageServiceTests()
    {
        // Mock'ları kur
        _mockUow = new Mock<IUnitOfWork>();
        _mockRepository = new Mock<IRepository<Package>>();
        _mockLogger = new Mock<ILogger<PackageService>>();
        
        // AutoMapper setup
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        // UnitOfWork'e mock'u bağla
        _mockUow.Setup(x => x.Repository<Package>())
            .Returns(_mockRepository.Object);

        // Service'i yarat
        _service = new PackageService(_mockUow.Object, _mockLogger.Object, _mapper);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsSuccessResult()
    {
        // Arrange
        var package = new Package
        {
            Id = 1,
            Name = "Başlangıç Paketi",
            Price = 3450,
            IsDeleted = false
        };
        _mockRepository.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(package);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Name.Should().Be("Başlangıç Paketi");
        _mockRepository.Verify(x => x.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsFailureResult()
    {
        // Arrange
        _mockRepository.Setup(x => x.GetByIdAsync(999))
            .ReturnsAsync((Package)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Contain("not found");
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_OnDatabaseException_ReturnsFailureAndLogsError()
    {
        // Arrange
        _mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("An error occurred");
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = new CreatePackageRequest { Name = "Yeni Paket", Price = 5000 };
        var validator = new CreatePackageRequestValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsValid)
        {
            var newPackage = new Package { Id = 2, Name = request.Name, Price = request.Price };
            _mockRepository.Setup(x => x.AddAsync(It.IsAny<Package>()))
                .Returns(Task.CompletedTask);
            _mockUow.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);
        }

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateAsync_WithInvalidRequest_ReturnsValidationError()
    {
        // Arrange — boş name ile geçersiz request
        var request = new CreatePackageRequest { Name = "", Price = 5000 };

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
}
```

### FluentAssertions Sık Kullanılan Assertion'lar

```csharp
// Boolean
result.Success.Should().BeTrue();
result.Success.Should().BeFalse();

// Null checks
result.Data.Should().BeNull();
result.Data.Should().NotBeNull();

// String checks
result.Message.Should().Be("Expected message");
result.Message.Should().Contain("not found");
result.Message.Should().StartWith("Error:");

// Collection checks
result.Errors.Should().NotBeEmpty();
result.Errors.Should().HaveCount(2);
result.Errors.Should().Contain("Validation error");

// Numeric
result.Data.Id.Should().Be(1);
result.Data.Price.Should().BeGreaterThan(0);

// Type checks
result.Data.Should().BeOfType<PackageResponse>();
```

### Test Naming Convention

`[MethodName]_[Scenario]_[ExpectedResult]`

✓ `CreateAsync_WithValidRequest_ReturnsSuccess`
✓ `GetByIdAsync_WithInvalidId_ReturnsFailure`
✓ `DeleteAsync_OnDatabaseError_LogsErrorAndReturnsFailure`
✗ `Test1`, `ShouldWork`, `TestCreate`

### Her Service Method'u İçin Minimum Senaryolar

```
✓ Happy Path: valid input → success + correct data
✓ Not Found: id/entity yok → failure + proper message
✓ Validation Error: invalid input → failure + error list
✓ Exception: DB/service error → failure + logged error
✓ Authorization (varsa): unauthorized user → failure + 401
```

### Mock Kurulum Patterns

**Repository Mock'u:**
```csharp
_mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
    .ReturnsAsync((int id) => new Package { Id = id });
```

**Multiple Calls:**
```csharp
_mockRepository.SetupSequence(x => x.GetByIdAsync(It.IsAny<int>()))
    .ReturnsAsync(new Package { Id = 1 })
    .ReturnsAsync((Package)null); // 2. call null döner
```

**Verify — mock'un çağrılıp çağrılmadığını kontrol et:**
```csharp
_mockRepository.Verify(x => x.GetByIdAsync(1), Times.Once);
_mockRepository.Verify(x => x.Update(It.IsAny<Package>()), Times.Never);
```

### Integration Test vs Unit Test

**Unit Test:** Database mock'lanır, sadece service logic test edilir (bu döküman)
**Integration Test:** Gerçek DB (InMemory EF Core), tüm katmanlar çalışır (sonraya)

---

## Kod Kalitesi

- **Class/Method Boyutu:** Service methodu 50 satır üstü → refactor et
- **Tekrar:** Aynı validation/mapping logic 2+ yerde → helper method
- **Magic Numbers:** Yoksa constant tanımla (`const int PAGE_SIZE = 10;`)
- **Comments:** Complex logic'e 1-2 satır açıklama (Türkçe veya İngilizce, tutarlı)

---

## Documentation

### Public Method'lar

```csharp
/// <summary>
/// Verilen ID'ye göre paketi getirir.
/// </summary>
/// <param name="id">Paket ID'si</param>
/// <returns>Paket veri transfer nesnesi ve başarı durumu</returns>
public async Task<Result<PackageResponse>> GetByIdAsync(int id)
{
    // ...
}
```

### Complex Logic

```csharp
public async Task<Result<PackageResponse>> CreateAsync(CreatePackageRequest request)
{
    // Validation ve DTO'dan entity'ye mapping
    var validator = new CreatePackageRequestValidator();
    var validationResult = await validator.ValidateAsync(request);
    if (!validationResult.IsValid)
        return Result<PackageResponse>.FailureResult("Validation failed", 
            validationResult.Errors.Select(e => e.ErrorMessage).ToList());

    // Database'e kaydet
    var entity = new Package { /* ... */ };
    await _uow.Repository<Package>().AddAsync(entity);
    await _uow.SaveChangesAsync();

    return Result<PackageResponse>.SuccessResult(_mapper.Map<PackageResponse>(entity));
}
```

---

## Commit Messages

**Standart format:**
```
<type>(<scope>): <subject>

<body (opsiyonel)>
```

Types: `feat`, `fix`, `refactor`, `docs`, `test`, `chore`

**Önemli: Commit mesajında hiçbir Claude trailer yok.** Ne `Co-Authored-By: Claude`, ne `Claude-Session:`. Tüm commit'ler sadece senin kendi adınla gitsin.

Örnekler:
```
feat(package): add soft delete logic to Package entity

fix(blog): return DTO instead of entity in GetByIdAsync

refactor(validation): extract FluentValidation to separate class

test(package-service): add unit tests for GetByIdAsync
```

---

## Implementation Checklist

Entity ekle → Repository ekle → Service ekle → Validator ekle → Controller ekle → Test yaz

```
[ ] Entity tanımlandı (Entities katmanında)
[ ] Migration'lar created/updated
[ ] Repository interface + implementation
[ ] Service interface + implementation
[ ] DTO + FluentValidator
[ ] Controller endpoint'leri
[ ] Unit test'ler yazıldı (minimum 4 scenario)
[ ] Async/await doğru
[ ] Error handling + logging
[ ] Code review
```

---

## Mimariye Sadık Kalma Kontrol Listesi

- [ ] Entity'nin yeri doğru (Entities katmanı)
- [ ] DB access sadece Repository'de (Business'te direkt query YOK)
- [ ] Entity asla HTTP response'da (DTO kullan)
- [ ] Service'ten Result<T> dönüyor (bool/null değil)
- [ ] Controller'da `await` (sync call yok)
- [ ] Logger context var (userId/orderId gibi)
- [ ] Test'ler mock kullanıyor (gerçek DB değil)
- [ ] Test methodu 1 şey test ediyor (multiple assertions → separate test)
