using System.Net;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.RateLimiting;
using EmirCoaching.API.BackgroundJobs;
using EmirCoaching.API.Common;
using EmirCoaching.API.Middleware;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Mapping;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Business.Services.Concrete;
using EmirCoaching.Business.Validation;
using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.DataAccess.Seed;
using EmirCoaching.Entities.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Loglama: ayarlar appsettings "Serilog" bölümünden (konsol + MSSQL Logs tablosu)
builder.Services.AddSerilog((services, config) => config
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services));

// DataAccess
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<AppUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 10;
        options.Lockout.MaxFailedAccessAttempts = 5;
    })
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddErrorDescriber<TurkishIdentityErrorDescriber>(); // Identity hata mesajları Türkçe

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Business
builder.Services.Configure<StorageOptions>(builder.Configuration.GetSection(StorageOptions.SectionName));
builder.Services.AddAutoMapper(typeof(MappingProfile)); // PublicUrlConverter da buradan DI'a eklenir
builder.Services.AddValidatorsFromAssemblyContaining<CreatePackageRequestValidator>();
TurkishValidation.Configure(); // hazır mesajlar ve alan adları Türkçe

// Paketler
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<ITestimonialService, TestimonialService>();

// Blog
builder.Services.AddScoped<IBlogCategoryService, BlogCategoryService>();
builder.Services.AddScoped<IBlogPostService, BlogPostService>();

// Değişimler
builder.Services.AddScoped<ITransformationTypeService, TransformationTypeService>();
builder.Services.AddScoped<ITransformationService, TransformationService>();

// Site içeriği
builder.Services.AddScoped<IFaqService, FaqService>();
builder.Services.AddScoped<IVideoQuestionService, VideoQuestionService>();
builder.Services.AddScoped<ISiteStatService, SiteStatService>();
builder.Services.AddScoped<ISiteSettingService, SiteSettingService>();
builder.Services.AddScoped<ISocialLinkService, SocialLinkService>();
builder.Services.AddScoped<ISocialPostService, SocialPostService>();
builder.Services.AddScoped<IPageContentService, PageContentService>();
builder.Services.AddScoped<IDietProgramService, DietProgramService>();
builder.Services.AddScoped<IDietProgramStepService, DietProgramStepService>();
builder.Services.AddScoped<ILegalDocumentService, LegalDocumentService>();

// Formlar ve onaylar
builder.Services.AddScoped<IContactMessageService, ContactMessageService>();
builder.Services.AddScoped<IDietFormSubmissionService, DietFormSubmissionService>();
builder.Services.AddScoped<INewsletterSubscriberService, NewsletterSubscriberService>();
builder.Services.AddScoped<IConsentRecordService, ConsentRecordService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IUploadCleanupService, UploadCleanupService>();

// Kimlik doğrulama
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<ILoginAttemptService, LoginAttemptService>();

// Günlük bakım: kullanılmayan yüklemeler, eski giriş geçmişi ve oturum kayıtları
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();
builder.Services.AddHostedService<MaintenanceWorker>();

// API
builder.Services.AddControllers(TurkishModelBinding.Configure)
    // System.Text.Json'ın İngilizce ve iç tip adı içeren mesajları istemciye gitmez (TurkishModelBinding üretir)
    .AddJsonOptions(options => options.AllowInputFormatterExceptionMessages = false)
    .ConfigureApiBehaviorOptions(options =>
    {
        TurkishModelBinding.Configure(options);

        // Bozuk JSON / tip hatası da servis hatalarıyla aynı formatta döner
        options.InvalidModelStateResponseFactory = context => ApiProblem.Create(
            context.HttpContext,
            StatusCodes.Status400BadRequest,
            TurkishModelBinding.GetErrors(context.ModelState));
    });
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// İstekteki *Path alanlarına tam URL gelirse API adresi kırpılır (yanıttaki *Url aynen geri gönderilebilir)
builder.Services.AddOptions<Microsoft.AspNetCore.Mvc.JsonOptions>()
    .Configure<IOptions<StorageOptions>>((json, storage) =>
        json.JsonSerializerOptions.TypeInfoResolver =
            (json.JsonSerializerOptions.TypeInfoResolver ?? new DefaultJsonTypeInfoResolver())
            .WithAddedModifier(StoredPathJson.Modifier(storage.Value)));

// Proxy / hosting katmanı arkasında RemoteIpAddress proxy'nin IP'si olur; X-Forwarded-For'dan gerçek
// istemci IP'si alınır (onay kayıtları ve IP sınırları için). Varsayılan olarak yalnızca yerel proxy'ye güvenilir.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    foreach (var proxy in builder.Configuration.GetSection("ForwardedHeaders:KnownProxies").Get<string[]>() ?? [])
        options.KnownProxies.Add(IPAddress.Parse(proxy));
});

// Herkese açık formlarda spam/bot koruması: IP başına sabit pencere
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy(RateLimitPolicies.Forms, context => RateLimitPartition.GetFixedWindowLimiter(
        context.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 5, Window = TimeSpan.FromMinutes(1) }));
    options.AddPolicy(RateLimitPolicies.Login, context => RateLimitPartition.GetFixedWindowLimiter(
        context.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 5, Window = TimeSpan.FromMinutes(1) }));

    // 429 da diğer hatalarla aynı ProblemDetails formatında döner
    options.OnRejected = async (context, cancellationToken) =>
    {
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
            context.HttpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString();

        await ApiProblem.WriteAsync(context.HttpContext, StatusCodes.Status429TooManyRequests,
            ["Çok fazla istek gönderildi. Lütfen biraz sonra tekrar deneyin."]);
    };
});

// Kimlik doğrulama: JWT Bearer. Ayarlar "Jwt" bölümünden, SecretKey user-secrets'tan;
// anahtar yoksa / kısaysa uygulama açılmaz.
builder.Services.AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection(JwtOptions.SectionName))
    .Validate(o => Encoding.UTF8.GetByteCount(o.SecretKey) >= JwtOptions.MinSecretKeyBytes,
        $"Jwt:SecretKey en az {JwtOptions.MinSecretKeyBytes} bayt olmalı (dotnet user-secrets set \"Jwt:SecretKey\" ...).")
    .ValidateOnStart();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
    .Configure<IOptions<JwtOptions>>((bearer, jwtOptions) =>
    {
        var jwt = jwtOptions.Value;
        bearer.MapInboundClaims = false; // claim adları token'daki gibi kalır (sub, name, role)
        bearer.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = jwt.GetSigningKey(),
            ValidateIssuerSigningKey = true,
            NameClaimType = JwtOptions.NameClaimType,
            RoleClaimType = JwtOptions.RoleClaimType,
            ClockSkew = TimeSpan.FromSeconds(30)
        };

        // 401 / 403 de diğer hatalarla aynı ProblemDetails formatında
        bearer.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                return ApiProblem.WriteAsync(context.HttpContext, StatusCodes.Status401Unauthorized,
                    ["Bu işlem için giriş yapmalısınız."]);
            },
            OnForbidden = context => ApiProblem.WriteAsync(context.HttpContext, StatusCodes.Status403Forbidden,
                ["Bu işlem için yetkiniz yok."])
        };
    });

// [Authorize] = giriş yapmış Admin
builder.Services.AddAuthorizationBuilder()
    .SetDefaultPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireRole(SeedData.AdminRoleName)
        .Build());

// Beklenmeyen hatalar tek yerde loglanır ve ProblemDetails (500) olarak döner
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Statik site (EmirCoaching.Web) farklı origin'den çağırır
const string SiteCorsPolicy = "Site";
builder.Services.AddCors(options => options.AddPolicy(SiteCorsPolicy, policy =>
    policy.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [])
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials())); // refresh token cookie'si için
// API belgesi (/openapi/v1.json) ve Swagger arayüzü (/swagger) — yalnızca geliştirmede açık
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<OpenApiBearerTransformer>();
    options.AddOperationTransformer<OpenApiBearerTransformer>();
});

var app = builder.Build();

// Admin kullanıcısı (AdminUser:* ayarları user-secrets'tan okunur)
try
{
    await SeedData.SeedAdminUserAsync(app.Services);
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Admin seed başarısız. Veritabanı migrate edildi mi? (dotnet ef database update)");
}

app.UseForwardedHeaders(); // en başta: sonraki her şey gerçek istemci IP'sini görür
app.UseExceptionHandler();
app.UseSerilogRequestLogging(options =>
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} isteği {StatusCode} ile yanıtlandı ({Elapsed:0.0} ms)");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "EmirCoaching API v1");
        options.DocumentTitle = "EmirCoaching API";
    });
}

app.UseHttpsRedirection();

app.UseStaticFiles(); // wwwroot/uploads

app.UseCors(SiteCorsPolicy);

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Framework'ün İngilizce başlangıç mesajları kapalı (appsettings: Microsoft.Hosting.Lifetime = Warning)
app.Lifetime.ApplicationStarted.Register(() =>
    app.Logger.LogInformation("API başlatıldı: {Urls} ({Environment})", string.Join(", ", app.Urls), app.Environment.EnvironmentName));

app.Run();
