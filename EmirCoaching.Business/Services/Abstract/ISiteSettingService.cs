using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

// Tek satırlık kayıt: oluşturma/silme yok, yalnızca okuma ve güncelleme
public interface ISiteSettingService
{
    Task<Result<SiteSettingResponse>> GetAsync();
    Task<Result> UpdateAsync(UpdateSiteSettingRequest request);
}
