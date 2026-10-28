using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Entities.Common;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

[Route("api/v1/admin/contact-messages")]
public class ContactMessagesController(IContactMessageService contactMessageService) : AdminControllerBase
{
    private const int DefaultPageSize = 20;

    // Yeniden eskiye, duruma göre filtre: ?status=Yeni&page=1&size=20 → 200
    [HttpGet]
    public async Task<IActionResult> GetAll(ContactMessageStatus? status, int page = 1, int size = DefaultPageSize) =>
        FromResult(await contactMessageService.GetPagedAsync(status, page, size));

    // → 204 / 400 / 404
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateContactMessageStatusRequest request) =>
        FromResult(await contactMessageService.UpdateStatusAsync(id, request.Status));

    // KVKK: kayıt onay kayıtlarıyla birlikte kalıcı silinir → 204 / 404
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        FromResult(await contactMessageService.DeleteAsync(id));

    // KVKK: kalıcı silme, ya hepsi ya hiçbiri; gövde: [3, 7, 12] → 204 / 400 / 404
    [HttpPost("bulk-delete")]
    public async Task<IActionResult> DeleteMany(List<int> ids) =>
        FromResult(await contactMessageService.DeleteManyAsync(ids));
}
