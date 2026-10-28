using EmirCoaching.Business.Common;

namespace EmirCoaching.Business.Services.Abstract;

// Basit içerik kayıtlarının ortak işlemleri; I{Entity}Service bunu genişletir, servisler doğrudan bunu kullanmaz.
public interface ICrudService<TResponse, in TCreateRequest, in TUpdateRequest>
{
    // Admin listesi (pasifler dahil)
    Task<Result<IReadOnlyList<TResponse>>> GetAllAsync();
    Task<Result<TResponse>> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(TCreateRequest request);
    Task<Result> UpdateAsync(TUpdateRequest request);
    Task<Result> DeleteAsync(int id);

    // Ya hepsi ya hiçbiri (bkz. BulkDelete)
    Task<Result> DeleteManyAsync(IReadOnlyCollection<int> ids);
}
