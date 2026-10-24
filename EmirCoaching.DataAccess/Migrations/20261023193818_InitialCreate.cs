using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmirCoaching.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AvatarPath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DietFormSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HeightCm = table.Column<decimal>(type: "decimal(5,1)", precision: 5, scale: 1, nullable: false),
                    CurrentWeight = table.Column<decimal>(type: "decimal(5,1)", precision: 5, scale: 1, nullable: false),
                    TargetWeight = table.Column<decimal>(type: "decimal(5,1)", precision: 5, scale: 1, nullable: false),
                    MealsPerDay = table.Column<int>(type: "int", nullable: false),
                    Allergies = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AvoidedFoods = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Supplements = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SleepHours = table.Column<int>(type: "int", nullable: true),
                    ActivityLevel = table.Column<int>(type: "int", nullable: false),
                    WaterLiters = table.Column<decimal>(type: "decimal(4,1)", precision: 4, scale: 1, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietFormSubmissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Faqs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Group = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faqs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LegalDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ContentHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsletterSubscribers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsletterSubscribers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Subtitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DurationMonths = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Location = table.Column<int>(type: "int", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MetaTitle = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(170)", maxLength: 170, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WhatsApp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Instagram = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    LinkedIn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Twitter = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    WorkingHours = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Suffix = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteStats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transformations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    DurationWeeks = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Quote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Story = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeforeImagePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AfterImagePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    BeforeSquarePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    AfterSquarePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false),
                    AlignBottom = table.Column<bool>(type: "bit", nullable: false),
                    ShowOnHome = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transformations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ContentHtml = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoverImagePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ReadingMinutes = table.Column<int>(type: "int", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    MetaTitle = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(170)", maxLength: 170, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: true),
                    RecommendedPackageId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPosts_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BlogPosts_BlogCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "BlogCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlogPosts_Packages_RecommendedPackageId",
                        column: x => x.RecommendedPackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ContactMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactMessages_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PackageHighlights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageHighlights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageHighlights_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackageModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IconKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageModules_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Testimonials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testimonials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Testimonials_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TransformationMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    TransformationId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransformationMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransformationMetrics_Transformations_TransformationId",
                        column: x => x.TransformationId,
                        principalTable: "Transformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConsentRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConsentType = table.Column<int>(type: "int", nullable: false),
                    IsGranted = table.Column<bool>(type: "bit", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    GrantedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IysStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LegalDocumentId = table.Column<int>(type: "int", nullable: true),
                    ContactMessageId = table.Column<int>(type: "int", nullable: true),
                    DietFormSubmissionId = table.Column<int>(type: "int", nullable: true),
                    NewsletterSubscriberId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsentRecords", x => x.Id);
                    table.CheckConstraint("CK_ConsentRecords_SingleOwner", "(CASE WHEN [ContactMessageId] IS NULL THEN 0 ELSE 1 END + CASE WHEN [DietFormSubmissionId] IS NULL THEN 0 ELSE 1 END + CASE WHEN [NewsletterSubscriberId] IS NULL THEN 0 ELSE 1 END) = 1");
                    table.ForeignKey(
                        name: "FK_ConsentRecords_ContactMessages_ContactMessageId",
                        column: x => x.ContactMessageId,
                        principalTable: "ContactMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsentRecords_DietFormSubmissions_DietFormSubmissionId",
                        column: x => x.DietFormSubmissionId,
                        principalTable: "DietFormSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsentRecords_LegalDocuments_LegalDocumentId",
                        column: x => x.LegalDocumentId,
                        principalTable: "LegalDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsentRecords_NewsletterSubscribers_NewsletterSubscriberId",
                        column: x => x.NewsletterSubscriberId,
                        principalTable: "NewsletterSubscribers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PackageModuleItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    LockedLabel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageModuleItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageModuleItems_PackageModules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "PackageModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 1, "5c1f6b0e-2f4e-4c55-9a51-3b7c7d0b6a11", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "BlogCategories",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "IsDeleted", "Name", "Slug", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Beslenme", "beslenme", null },
                    { 2, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Hipertrofi", "hipertrofi", null },
                    { 3, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Kuvvet", "kuvvet", null },
                    { 4, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Mentalite", "mentalite", null },
                    { 5, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "Toparlanma", "toparlanma", null }
                });

            migrationBuilder.InsertData(
                table: "LegalDocuments",
                columns: new[] { "Id", "ContentHtml", "CreatedAt", "DeletedAt", "FilePath", "IsCurrent", "IsDeleted", "PublishedAt", "Title", "Type", "UpdatedAt", "Version" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "/yasal-metinler/kvkk-aydinlatma-metni.pdf", true, false, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), "KVKK Aydınlatma Metni", 0, null, 1 },
                    { 2, null, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "/yasal-metinler/acik-riza-metni.pdf", true, false, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), "Açık Rıza Metni", 1, null, 1 },
                    { 3, null, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "/yasal-metinler/cerez-politikasi.pdf", true, false, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), "Çerez Politikası", 2, null, 1 },
                    { 4, null, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "/yasal-metinler/iptal-ve-iade-kosullari.pdf", true, false, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), "İptal ve İade Koşulları", 3, null, 1 },
                    { 5, "<section class=\"gizlilik-bolum\" aria-labelledby=\"genel-bilgilendirme\">\n  <h2 id=\"genel-bilgilendirme\" class=\"gizlilik-bolum__title\">\n    <span class=\"gizlilik-bolum__no\">1.</span> Genel Bilgilendirme\n  </h2>\n  <p>\n    EMIRCOACHING (\"Şirket\", \"Biz\" veya \"Bize\"), 6698 sayılı Kişisel Verilerin Korunması Kanunu (\"KVKK\") ve\n    ilgili ikincil mevzuat kapsamında Veri Sorumlusu sıfatıyla hareket etmektedir. Bu politika,\n    hizmetlerimizi kullanırken sağladığınız bilgilerin nasıl yönetildiğini açıklamaktadır.\n  </p>\n</section>\n\n<section class=\"gizlilik-bolum\" aria-labelledby=\"veri-sorumlusu\">\n  <h2 id=\"veri-sorumlusu\" class=\"gizlilik-bolum__title\">\n    <span class=\"gizlilik-bolum__no\">2.</span> Veri Sorumlusu Kimliği\n  </h2>\n  <p>6698 sayılı KVKK uyarınca muhatabınız olan veri sorumlusunun bilgileri aşağıdadır:</p>\n  <dl class=\"gizlilik-liste gizlilik-liste--tanim\">\n    <div class=\"gizlilik-liste__item\">\n      <dt>Unvan:</dt>\n      <dd>EMIRCOACHING Performans Teknolojileri A.Ş.</dd>\n    </div>\n    <div class=\"gizlilik-liste__item\">\n      <dt>MERSİS No:</dt>\n      <dd>0123456789012345</dd>\n    </div>\n    <div class=\"gizlilik-liste__item\">\n      <dt>Adres:</dt>\n      <dd>Levent Mah. Büyükdere Cad. No:199, Şişli / İstanbul</dd>\n    </div>\n  </dl>\n</section>\n\n<section class=\"gizlilik-bolum\" aria-labelledby=\"toplanan-veriler\">\n  <h2 id=\"toplanan-veriler\" class=\"gizlilik-bolum__title\">\n    <span class=\"gizlilik-bolum__no\">3.</span> Toplanan Kişisel Veriler\n  </h2>\n  <p>\n    Platformumuz üzerinden sunduğumuz hizmetlerin doğası gereği aşağıdaki veri kategorilerinde kişisel veri\n    işlenmektedir:\n  </p>\n  <!-- CKEditor çıktısı: düz tablo (<figure class=\"table\"><table>…). İlk satır \"başlık satırı\"\n       (thead), ilk sütun \"başlık sütunu\" (satır <th>) yapılabilir. Bileşen sınıfları, dar ekrandaki\n       kart etiketleri (data-label) ve ARIA rolleri js/blog-detay.js ile otomatik eklenir;\n       \"Tablo özellikleri\" eklentisi (sabit genişlik/renk) kapalı olmalı. -->\n  <figure class=\"table\">\n    <table>\n      <thead>\n        <tr>\n          <th>Veri Kategorisi</th>\n          <th>Örnek</th>\n          <th>Amaç</th>\n          <th>Saklama Süresi</th>\n        </tr>\n      </thead>\n      <tbody>\n        <tr>\n          <th>Kimlik Verisi</th>\n          <td>Ad, Soyad, TCKN</td>\n          <td>Sözleşme Kurulumu</td>\n          <td>10 Yıl</td>\n        </tr>\n        <tr>\n          <th>İletişim Verisi</th>\n          <td>E-posta, Telefon</td>\n          <td>Müşteri Desteği</td>\n          <td>10 Yıl</td>\n        </tr>\n        <tr>\n          <th>Sağlık Verisi (Özel Nitelikli)</th>\n          <td>Boy, Kilo, Geçmiş Sakatlıklar</td>\n          <td>Program Optimizasyonu</td>\n          <td>Üyelik + 2 Yıl</td>\n        </tr>\n      </tbody>\n    </table>\n  </figure>\n</section>\n\n<section class=\"gizlilik-bolum\" aria-labelledby=\"isleme-amaclari\">\n  <h2 id=\"isleme-amaclari\" class=\"gizlilik-bolum__title\">\n    <span class=\"gizlilik-bolum__no\">4.</span> Verilerin İşlenme Amaçları\n  </h2>\n  <p>Toplanan kişisel verileriniz aşağıdaki amaçlarla işlenmektedir:</p>\n  <ul class=\"gizlilik-liste\">\n    <li class=\"gizlilik-liste__item\">Kişiselleştirilmiş antrenman ve beslenme programlarının oluşturulması.</li>\n    <li class=\"gizlilik-liste__item\">Kullanıcı deneyiminin iyileştirilmesi ve uygulama hatalarının giderilmesi.</li>\n    <li class=\"gizlilik-liste__item\">Yasal yükümlülüklerin yerine getirilmesi (Fatura düzenlenmesi vb.).</li>\n  </ul>\n</section>\n\n<section class=\"gizlilik-bolum\" aria-labelledby=\"veri-sahibi-haklari\">\n  <h2 id=\"veri-sahibi-haklari\" class=\"gizlilik-bolum__title\">\n    <span class=\"gizlilik-bolum__no\">8.</span> Veri Sahibinin Hakları\n  </h2>\n  <p>\n    KVKK Madde 11 uyarınca, kişisel verileriniz üzerinde belirli haklara sahipsiniz. Bu haklar veri işlemeyi\n    öğrenme, düzeltme, silme talep etme haklarını içerir.\n  </p>\n  <div class=\"gizlilik-callout\" role=\"note\" aria-labelledby=\"basvuru-hakki\">\n    <iconify-icon icon=\"mdi:information-outline\" mode=\"svg\" noobserver class=\"gizlilik-callout__icon\" aria-hidden=\"true\"></iconify-icon>\n    <div class=\"gizlilik-callout__body\">\n      <p id=\"basvuru-hakki\" class=\"gizlilik-callout__title\">Başvuru Hakkı</p>\n      <p class=\"gizlilik-callout__text\">\n        Taleplerinizi sistemimizde kayıtlı bulunan elektronik posta adresiniz üzerinden\n        <a href=\"mailto:kvkk@emircoaching.com\" class=\"gizlilik-callout__link\" title=\"kvkk@emircoaching.com adresine e-posta gönder\">kvkk@emircoaching.com</a>\n        adresine iletebilirsiniz. Başvurunuz en geç 30 gün içinde ücretsiz olarak sonuçlandırılacaktır.\n      </p>\n    </div>\n  </div>\n</section>\n", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), "Gizlilik Politikası", 5, null, 1 }
                });

            migrationBuilder.InsertData(
                table: "Packages",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "DurationMonths", "IsActive", "IsDeleted", "Location", "MetaDescription", "MetaTitle", "Name", "Price", "ShortDescription", "Slug", "SortOrder", "Subtitle", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, true, false, 0, "Emir Coaching Başlangıç & Deneme: 3 aylık program. Antrenman, beslenme, takip ve koçluk modülleriyle toplam 15 kalem içerik.", "Başlangıç & Deneme Paket Detayı | Emir Coaching", "Başlangıç & Deneme", 3450m, "3 aylık program: antrenman, beslenme, takip ve koçluk modülleri.", "baslangic-deneme", 1, "3 Aylık Başlangıç Programı", null },
                    { 2, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 6, true, false, 0, "Emir Coaching Dönüşüm Protokolü: 6 aylık program. Antrenman, beslenme, takip ve koçluk modülleriyle toplam 15 kalem içerik.", "Dönüşüm Protokolü Paket Detayı | Emir Coaching", "Dönüşüm Protokolü", 8250m, "6 aylık program: antrenman, beslenme, takip ve koçluk modülleri.", "donusum-protokolu", 2, "6 Aylık Dönüşüm Programı", null },
                    { 3, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 12, true, false, 0, "Emir Coaching VIP Yaşam Tarzı: 12 aylık program. Antrenman, beslenme, takip ve koçluk modülleriyle toplam 15 kalem içerik.", "VIP Yaşam Tarzı Paket Detayı | Emir Coaching", "VIP Yaşam Tarzı", 14900m, "12 aylık program: antrenman, beslenme, takip ve koçluk modülleri.", "vip-yasam-tarzi", 3, "12 Aylık Yaşam Tarzı Programı", null },
                    { 4, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, true, false, 1, "Emir Coaching Başlangıç & Deneme (Evde): evde antrenmanla 3 aylık program. Antrenman, beslenme, takip ve koçluk modülleriyle toplam 15 kalem içerik.", "Başlangıç & Deneme (Evde) Paket Detayı | Emir Coaching", "Başlangıç & Deneme", 3450m, "Evde antrenmanla 3 aylık program: antrenman, beslenme, takip ve koçluk modülleri.", "baslangic-deneme-evde", 1, "3 Aylık Başlangıç Programı", null },
                    { 5, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 6, true, false, 1, "Emir Coaching Dönüşüm Protokolü (Evde): evde antrenmanla 6 aylık program. Antrenman, beslenme, takip ve koçluk modülleriyle toplam 15 kalem içerik.", "Dönüşüm Protokolü (Evde) Paket Detayı | Emir Coaching", "Dönüşüm Protokolü", 8250m, "Evde antrenmanla 6 aylık program: antrenman, beslenme, takip ve koçluk modülleri.", "donusum-protokolu-evde", 2, "6 Aylık Dönüşüm Programı", null },
                    { 6, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 12, true, false, 1, "Emir Coaching VIP Yaşam Tarzı (Evde): evde antrenmanla 12 aylık program. Antrenman, beslenme, takip ve koçluk modülleriyle toplam 15 kalem içerik.", "VIP Yaşam Tarzı (Evde) Paket Detayı | Emir Coaching", "VIP Yaşam Tarzı", 14900m, "Evde antrenmanla 12 aylık program: antrenman, beslenme, takip ve koçluk modülleri.", "vip-yasam-tarzi-evde", 3, "12 Aylık Yaşam Tarzı Programı", null }
                });

            migrationBuilder.InsertData(
                table: "SiteSettings",
                columns: new[] { "Id", "Address", "CreatedAt", "DeletedAt", "Email", "Facebook", "Instagram", "IsDeleted", "LinkedIn", "Phone", "Twitter", "UpdatedAt", "WhatsApp", "WorkingHours" },
                values: new object[] { 1, "Levent Mah. Büyükdere Cad. No:199, Şişli / İstanbul", new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "info@emircoaching.com", null, "https://instagram.com/", false, null, "+90 545 371 25 15", null, null, "905453712515", "Pazartesi - Cumartesi: 08:00 - 22:00 | Pazar: 10:00 - 20:00" });

            migrationBuilder.InsertData(
                table: "PackageHighlights",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "IsDeleted", "PackageId", "SortOrder", "Text", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 1, 1, "Kişiye Özel Planlama", null },
                    { 2, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 1, 2, "Haftalık Düzenli Takip", null },
                    { 3, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 1, 3, "7/24 WhatsApp Desteği", null },
                    { 4, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 1, 4, "Detaylı Gelişim Raporları", null },
                    { 5, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 2, 1, "Kişiye Özel Planlama", null },
                    { 6, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 2, 2, "Haftalık Düzenli Takip", null },
                    { 7, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 2, 3, "7/24 WhatsApp Desteği", null },
                    { 8, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 2, 4, "Detaylı Gelişim Raporları", null },
                    { 9, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 3, 1, "Kişiye Özel Planlama", null },
                    { 10, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 3, 2, "Haftalık Düzenli Takip", null },
                    { 11, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 3, 3, "7/24 WhatsApp Desteği", null },
                    { 12, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 3, 4, "Detaylı Gelişim Raporları", null },
                    { 13, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 4, 1, "Kişiye Özel Planlama", null },
                    { 14, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 4, 2, "Haftalık Düzenli Takip", null },
                    { 15, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 4, 3, "7/24 WhatsApp Desteği", null },
                    { 16, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 4, 4, "Detaylı Gelişim Raporları", null },
                    { 17, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 5, 1, "Kişiye Özel Planlama", null },
                    { 18, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 5, 2, "Haftalık Düzenli Takip", null },
                    { 19, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 5, 3, "7/24 WhatsApp Desteği", null },
                    { 20, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 5, 4, "Detaylı Gelişim Raporları", null },
                    { 21, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 6, 1, "Kişiye Özel Planlama", null },
                    { 22, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 6, 2, "Haftalık Düzenli Takip", null },
                    { 23, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 6, 3, "7/24 WhatsApp Desteği", null },
                    { 24, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 6, 4, "Detaylı Gelişim Raporları", null }
                });

            migrationBuilder.InsertData(
                table: "PackageModules",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "IconKey", "IsDeleted", "PackageId", "SortOrder", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "antrenman", false, 1, 1, "Antrenman", null },
                    { 2, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "beslenme", false, 1, 2, "Beslenme", null },
                    { 3, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "takip-olcum", false, 1, 3, "Takip & Ölçüm", null },
                    { 4, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "kocluk", false, 1, 4, "Koçluk", null },
                    { 5, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "antrenman", false, 2, 1, "Antrenman", null },
                    { 6, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "beslenme", false, 2, 2, "Beslenme", null },
                    { 7, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "takip-olcum", false, 2, 3, "Takip & Ölçüm", null },
                    { 8, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "kocluk", false, 2, 4, "Koçluk", null },
                    { 9, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "antrenman", false, 3, 1, "Antrenman", null },
                    { 10, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "beslenme", false, 3, 2, "Beslenme", null },
                    { 11, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "takip-olcum", false, 3, 3, "Takip & Ölçüm", null },
                    { 12, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "kocluk", false, 3, 4, "Koçluk", null },
                    { 13, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "antrenman", false, 4, 1, "Antrenman", null },
                    { 14, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "beslenme", false, 4, 2, "Beslenme", null },
                    { 15, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "takip-olcum", false, 4, 3, "Takip & Ölçüm", null },
                    { 16, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "kocluk", false, 4, 4, "Koçluk", null },
                    { 17, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "antrenman", false, 5, 1, "Antrenman", null },
                    { 18, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "beslenme", false, 5, 2, "Beslenme", null },
                    { 19, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "takip-olcum", false, 5, 3, "Takip & Ölçüm", null },
                    { 20, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "kocluk", false, 5, 4, "Koçluk", null },
                    { 21, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "antrenman", false, 6, 1, "Antrenman", null },
                    { 22, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "beslenme", false, 6, 2, "Beslenme", null },
                    { 23, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "takip-olcum", false, 6, 3, "Takip & Ölçüm", null },
                    { 24, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "kocluk", false, 6, 4, "Koçluk", null }
                });

            migrationBuilder.InsertData(
                table: "PackageModuleItems",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "IsDeleted", "LockedLabel", "ModuleId", "SortOrder", "State", "Text", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 1, 1, 0, "Kişiye Özel Antrenman Planlaması", null },
                    { 2, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 1, 2, 0, "Haftalık Program Güncellemesi", null },
                    { 3, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 1, 3, 0, "Hareket Tekniği Videoları", null },
                    { 4, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 1, 4, 0, "Sakatlık Uyarlaması", null },
                    { 5, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 2, 1, 0, "Kalorisi Hesaplanmış Beslenme Planı", null },
                    { 6, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 2, 2, 0, "Öğün Şablonları", null },
                    { 7, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 2, 3, 0, "Dışarıda Yeme Rehberi", null },
                    { 8, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 2, 4, 0, "Supplement Planlaması", null },
                    { 9, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 3, 1, 0, "Haftalık Durum Değerlendirmesi", null },
                    { 10, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 3, 2, 0, "Vücut Kompozisyonu Takibi", null },
                    { 11, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 3, 3, 0, "Aylık Detaylı Rapor", null },
                    { 12, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 4, 1, 0, "Birebir WhatsApp İletişimi", null },
                    { 13, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 4, 2, 0, "Anlık Program Revizeleri", null },
                    { 14, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 4, 3, 0, "Kan Tahlili Analizi & Revize", null },
                    { 15, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "6 ve 12 Aylık Pakette", 4, 4, 2, "Yüz Yüze Zoom Görüşmesi", null },
                    { 16, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 5, 1, 0, "Kişiye Özel Antrenman Planlaması", null },
                    { 17, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 5, 2, 0, "Haftalık Program Güncellemesi", null },
                    { 18, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 5, 3, 0, "Hareket Tekniği Videoları", null },
                    { 19, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 5, 4, 0, "Sakatlık Uyarlaması", null },
                    { 20, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 6, 1, 0, "Kalorisi Hesaplanmış Beslenme Planı", null },
                    { 21, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 6, 2, 0, "Öğün Şablonları", null },
                    { 22, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 6, 3, 0, "Dışarıda Yeme Rehberi", null },
                    { 23, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 6, 4, 0, "Supplement Planlaması", null },
                    { 24, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 7, 1, 0, "Haftalık Durum Değerlendirmesi", null },
                    { 25, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 7, 2, 0, "Vücut Kompozisyonu Takibi", null },
                    { 26, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 7, 3, 0, "Aylık Detaylı Rapor", null },
                    { 27, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 8, 1, 0, "Birebir WhatsApp İletişimi", null },
                    { 28, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 8, 2, 0, "Anlık Program Revizeleri", null },
                    { 29, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 8, 3, 0, "Kan Tahlili Analizi & Revize", null },
                    { 30, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 8, 4, 1, "Yüz Yüze Zoom Görüşmesi", null },
                    { 31, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 9, 1, 0, "Kişiye Özel Antrenman Planlaması", null },
                    { 32, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 9, 2, 0, "Haftalık Program Güncellemesi", null },
                    { 33, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 9, 3, 0, "Hareket Tekniği Videoları", null },
                    { 34, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 9, 4, 0, "Sakatlık Uyarlaması", null },
                    { 35, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 10, 1, 0, "Kalorisi Hesaplanmış Beslenme Planı", null },
                    { 36, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 10, 2, 0, "Öğün Şablonları", null },
                    { 37, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 10, 3, 0, "Dışarıda Yeme Rehberi", null },
                    { 38, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 10, 4, 0, "Supplement Planlaması", null },
                    { 39, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 11, 1, 0, "Haftalık Durum Değerlendirmesi", null },
                    { 40, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 11, 2, 0, "Vücut Kompozisyonu Takibi", null },
                    { 41, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 11, 3, 0, "Aylık Detaylı Rapor", null },
                    { 42, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 12, 1, 0, "Birebir WhatsApp İletişimi", null },
                    { 43, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 12, 2, 0, "Anlık Program Revizeleri", null },
                    { 44, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 12, 3, 0, "Kan Tahlili Analizi & Revize", null },
                    { 45, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 12, 4, 1, "Yüz Yüze Zoom Görüşmesi", null },
                    { 46, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 13, 1, 0, "Kişiye Özel Antrenman Planlaması", null },
                    { 47, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 13, 2, 0, "Haftalık Program Güncellemesi", null },
                    { 48, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 13, 3, 0, "Hareket Tekniği Videoları", null },
                    { 49, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 13, 4, 0, "Sakatlık Uyarlaması", null },
                    { 50, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 14, 1, 0, "Kalorisi Hesaplanmış Beslenme Planı", null },
                    { 51, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 14, 2, 0, "Öğün Şablonları", null },
                    { 52, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 14, 3, 0, "Dışarıda Yeme Rehberi", null },
                    { 53, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 14, 4, 0, "Supplement Planlaması", null },
                    { 54, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 15, 1, 0, "Haftalık Durum Değerlendirmesi", null },
                    { 55, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 15, 2, 0, "Vücut Kompozisyonu Takibi", null },
                    { 56, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 15, 3, 0, "Aylık Detaylı Rapor", null },
                    { 57, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 16, 1, 0, "Birebir WhatsApp İletişimi", null },
                    { 58, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 16, 2, 0, "Anlık Program Revizeleri", null },
                    { 59, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 16, 3, 0, "Kan Tahlili Analizi & Revize", null },
                    { 60, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "6 ve 12 Aylık Pakette", 16, 4, 2, "Yüz Yüze Zoom Görüşmesi", null },
                    { 61, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 17, 1, 0, "Kişiye Özel Antrenman Planlaması", null },
                    { 62, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 17, 2, 0, "Haftalık Program Güncellemesi", null },
                    { 63, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 17, 3, 0, "Hareket Tekniği Videoları", null },
                    { 64, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 17, 4, 0, "Sakatlık Uyarlaması", null },
                    { 65, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 18, 1, 0, "Kalorisi Hesaplanmış Beslenme Planı", null },
                    { 66, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 18, 2, 0, "Öğün Şablonları", null },
                    { 67, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 18, 3, 0, "Dışarıda Yeme Rehberi", null },
                    { 68, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 18, 4, 0, "Supplement Planlaması", null },
                    { 69, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 19, 1, 0, "Haftalık Durum Değerlendirmesi", null },
                    { 70, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 19, 2, 0, "Vücut Kompozisyonu Takibi", null },
                    { 71, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 19, 3, 0, "Aylık Detaylı Rapor", null },
                    { 72, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 20, 1, 0, "Birebir WhatsApp İletişimi", null },
                    { 73, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 20, 2, 0, "Anlık Program Revizeleri", null },
                    { 74, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 20, 3, 0, "Kan Tahlili Analizi & Revize", null },
                    { 75, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 20, 4, 1, "Yüz Yüze Zoom Görüşmesi", null },
                    { 76, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 21, 1, 0, "Kişiye Özel Antrenman Planlaması", null },
                    { 77, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 21, 2, 0, "Haftalık Program Güncellemesi", null },
                    { 78, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 21, 3, 0, "Hareket Tekniği Videoları", null },
                    { 79, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 21, 4, 0, "Sakatlık Uyarlaması", null },
                    { 80, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 22, 1, 0, "Kalorisi Hesaplanmış Beslenme Planı", null },
                    { 81, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 22, 2, 0, "Öğün Şablonları", null },
                    { 82, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 22, 3, 0, "Dışarıda Yeme Rehberi", null },
                    { 83, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 22, 4, 0, "Supplement Planlaması", null },
                    { 84, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 23, 1, 0, "Haftalık Durum Değerlendirmesi", null },
                    { 85, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 23, 2, 0, "Vücut Kompozisyonu Takibi", null },
                    { 86, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 23, 3, 0, "Aylık Detaylı Rapor", null },
                    { 87, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 24, 1, 0, "Birebir WhatsApp İletişimi", null },
                    { 88, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 24, 2, 0, "Anlık Program Revizeleri", null },
                    { 89, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 24, 3, 0, "Kan Tahlili Analizi & Revize", null },
                    { 90, new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 24, 4, 1, "Yüz Yüze Zoom Görüşmesi", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BlogCategories_Slug",
                table: "BlogCategories",
                column: "Slug",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_AuthorId",
                table: "BlogPosts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_CategoryId",
                table: "BlogPosts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_IsPublished_PublishedAt",
                table: "BlogPosts",
                columns: new[] { "IsPublished", "PublishedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_RecommendedPackageId",
                table: "BlogPosts",
                column: "RecommendedPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_Slug",
                table: "BlogPosts",
                column: "Slug",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ConsentRecords_ContactMessageId",
                table: "ConsentRecords",
                column: "ContactMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsentRecords_DietFormSubmissionId",
                table: "ConsentRecords",
                column: "DietFormSubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsentRecords_LegalDocumentId",
                table: "ConsentRecords",
                column: "LegalDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsentRecords_NewsletterSubscriberId",
                table: "ConsentRecords",
                column: "NewsletterSubscriberId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessages_PackageId",
                table: "ContactMessages",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessages_Status",
                table: "ContactMessages",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DietFormSubmissions_Status",
                table: "DietFormSubmissions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_LegalDocuments_Type_Current",
                table: "LegalDocuments",
                column: "Type",
                unique: true,
                filter: "[IsCurrent] = 1 AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_LegalDocuments_Type_Version",
                table: "LegalDocuments",
                columns: new[] { "Type", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewsletterSubscribers_Email",
                table: "NewsletterSubscribers",
                column: "Email",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_PackageHighlights_PackageId",
                table: "PackageHighlights",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageModuleItems_ModuleId",
                table: "PackageModuleItems",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageModules_PackageId",
                table: "PackageModules",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_Slug",
                table: "Packages",
                column: "Slug",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_PackageId",
                table: "Testimonials",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_TransformationMetrics_TransformationId",
                table: "TransformationMetrics",
                column: "TransformationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BlogPosts");

            migrationBuilder.DropTable(
                name: "ConsentRecords");

            migrationBuilder.DropTable(
                name: "Faqs");

            migrationBuilder.DropTable(
                name: "PackageHighlights");

            migrationBuilder.DropTable(
                name: "PackageModuleItems");

            migrationBuilder.DropTable(
                name: "SiteSettings");

            migrationBuilder.DropTable(
                name: "SiteStats");

            migrationBuilder.DropTable(
                name: "Testimonials");

            migrationBuilder.DropTable(
                name: "TransformationMetrics");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "BlogCategories");

            migrationBuilder.DropTable(
                name: "ContactMessages");

            migrationBuilder.DropTable(
                name: "DietFormSubmissions");

            migrationBuilder.DropTable(
                name: "LegalDocuments");

            migrationBuilder.DropTable(
                name: "NewsletterSubscribers");

            migrationBuilder.DropTable(
                name: "PackageModules");

            migrationBuilder.DropTable(
                name: "Transformations");

            migrationBuilder.DropTable(
                name: "Packages");
        }
    }
}
