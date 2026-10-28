using AwesomeAssertions;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Services.Concrete;
using EmirCoaching.Entities.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace EmirCoaching.Tests.Services;

// Bağımlılığı yok (yalnızca ayarlar): mock gerekmez, gerçek token üretilip çözülür
public class JwtTokenServiceTests
{
    private static readonly JwtOptions Options = new()
    {
        Issuer = "emircoaching-test",
        Audience = "emircoaching-panel",
        SecretKey = new string('k', 32),
        AccessTokenMinutes = 15
    };

    private readonly JwtTokenService _service = new(Microsoft.Extensions.Options.Options.Create(Options));
    private readonly AppUser _user = new() { Id = 7, Email = "admin@emircoaching.com", FullName = "Emir" };

    [Fact]
    public async Task CreateAccessToken_ProducesTokenValidWithSameSettings()
    {
        // Act
        var token = _service.CreateAccessToken(_user, ["Admin"]);

        // Assert — API'nin JwtBearer doğrulamasıyla aynı parametreler
        var validation = await new JsonWebTokenHandler().ValidateTokenAsync(token.Token, new TokenValidationParameters
        {
            ValidIssuer = Options.Issuer,
            ValidAudience = Options.Audience,
            IssuerSigningKey = Options.GetSigningKey()
        });
        validation.IsValid.Should().BeTrue();
    }

    [Fact]
    public void CreateAccessToken_ContainsUserIdNameAndRoles()
    {
        // Act
        var token = new JsonWebToken(_service.CreateAccessToken(_user, ["Admin", "Editor"]).Token);

        // Assert
        token.Subject.Should().Be("7");
        token.GetClaim(JwtOptions.NameClaimType).Value.Should().Be("Emir");
        token.Claims.Where(c => c.Type == JwtOptions.RoleClaimType).Select(c => c.Value).Should().BeEquivalentTo(["Admin", "Editor"]);
    }

    [Fact]
    public void CreateAccessToken_ExpiresAfterConfiguredMinutes()
    {
        // Act
        var token = _service.CreateAccessToken(_user, []);

        // Assert
        token.ExpiresAt.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(15), TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task CreateAccessToken_WithDifferentKey_FailsValidation()
    {
        // Act
        var token = _service.CreateAccessToken(_user, ["Admin"]);

        // Assert — başka anahtarla imzalanmış sayılır
        var validation = await new JsonWebTokenHandler().ValidateTokenAsync(token.Token, new TokenValidationParameters
        {
            ValidIssuer = Options.Issuer,
            ValidAudience = Options.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(new byte[32])
        });
        validation.IsValid.Should().BeFalse();
    }

    [Fact]
    public void CreateRefreshToken_ReturnsDifferentValueEachTime()
    {
        _service.CreateRefreshToken().Should().NotBe(_service.CreateRefreshToken());
    }

    [Fact]
    public void HashRefreshToken_IsDeterministicAndHidesToken()
    {
        // Arrange
        var token = _service.CreateRefreshToken();

        // Act
        var hash = _service.HashRefreshToken(token);

        // Assert — aynı token hep aynı özeti verir (DB'de arama), özet token'ın kendisi değildir; SHA-256 = 64 hex
        hash.Should().Be(_service.HashRefreshToken(token));
        hash.Should().NotBe(token);
        hash.Should().HaveLength(64);
    }
}
