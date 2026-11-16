using EmirCoaching.API.Common;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Entities.Common;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

[Route("api/v1/admin/diet-form-submissions")]
public class DietFormSubmissionsController(IDietFormSubmissionService dietFormSubmissionService) : AdminControllerBase
{
    private const int DefaultPageSize = 20;

    // Yeniden eskiye, duruma göre filtre: ?status=Yeni&page=1&size=20 → 200
    [HttpGet]
    public async Task<ActionResult<PagedResult<DietFormSubmissionResponse>>> GetAll(DietFormStatus? status, int page = 1, int size = DefaultPageSize) =>
        FromResult(await dietFormSubmissionService.GetPagedAsync(status, page, size));

    // Yeni dışındaki durumlarda kişi yeniden başvurabilir → 204 / 400 / 404
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateDietFormSubmissionStatusRequest request) =>
        FromResult(await dietFormSubmissionService.UpdateStatusAsync(id, request.Status));

    // KVKK: kayıt onay kayıtlarıyla birlikte kalıcı silinir → 204 / 404
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        FromResult(await dietFormSubmissionService.DeleteAsync(id));

    // KVKK: kalıcı silme, ya hepsi ya hiçbiri; gövde: [3, 7, 12] → 204 / 400 / 404
    [HttpPost("bulk-delete")]
    public async Task<IActionResult> DeleteMany(List<int> ids) =>
        FromResult(await dietFormSubmissionService.DeleteManyAsync(ids));
}
