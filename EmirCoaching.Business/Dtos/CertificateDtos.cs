using EmirCoaching.Business.Common;

namespace EmirCoaching.Business.Dtos;

public record CertificateResponse
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string IssuerFullName { get; init; } = string.Empty;
    public string? Detail { get; init; }
    public int Year { get; init; }
    public string LogoUrl { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
}

public record CreateCertificateRequest
{
    public string Title { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;         // kartta görünen kısa ad ("FMI")
    public string IssuerFullName { get; init; } = string.Empty; // logonun alt/title metni
    public string? Detail { get; init; }                        // isteğe bağlı ek bilgi ("EREPS Seviye 4")
    public int Year { get; init; }
    public string LogoPath { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
}

public record UpdateCertificateRequest : CreateCertificateRequest, IUpdateRequest
{
    public int Id { get; init; }
}
