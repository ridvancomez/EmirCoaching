using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

// Tek kayıt: oluşturma/silme yok
public interface IDietProgramService
{
    // Diyet planı detay sayfası; program pasifse NotFound
    Task<Result<DietProgramResponse>> GetActiveAsync();

    // Admin: pasif olsa da döner
    Task<Result<DietProgramResponse>> GetAsync();
    Task<Result> UpdateAsync(UpdateDietProgramRequest request);
}
