using EmirCoaching.Business.Mapping;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Business.Services.Concrete;
using EmirCoaching.Business.Validation;
using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.DataAccess.Seed;
using EmirCoaching.Entities.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Business
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddValidatorsFromAssemblyContaining<CreatePackageDtoValidator>();

builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<ITransformationService, TransformationService>();
builder.Services.AddScoped<IFaqService, FaqService>();
builder.Services.AddScoped<ISiteStatService, SiteStatService>();
builder.Services.AddScoped<ISiteSettingService, SiteSettingService>();
builder.Services.AddScoped<ILegalDocumentService, LegalDocumentService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IDietFormService, DietFormService>();
builder.Services.AddScoped<INewsletterService, NewsletterService>();
builder.Services.AddScoped<IConsentService, ConsentService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();

// Web
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseStaticFiles(); // wwwroot/uploads

app.UseAuthorization();

app.MapControllers();

app.Run();
