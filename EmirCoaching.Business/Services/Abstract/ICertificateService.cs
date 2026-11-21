using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

public interface ICertificateService : ICrudService<CertificateResponse, CreateCertificateRequest, UpdateCertificateRequest>
{
    // Site: aktif sertifikalar, SortOrder'a göre (ana sayfa koç bölümü ve Hakkımda)
    Task<Result<IReadOnlyList<CertificateResponse>>> GetActiveAsync();
}
