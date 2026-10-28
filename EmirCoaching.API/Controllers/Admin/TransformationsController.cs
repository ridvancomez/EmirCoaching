using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

[Route("api/v1/admin/transformations")]
public class TransformationsController(ITransformationService transformationService) : AdminControllerBase
{
    // Pasifler dahil, yeniden eskiye → 200
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        FromResult(await transformationService.GetAllAsync());

    // Düzenleme formu: hikâye ve ölçümler dahil (pasif olsa da) → 200 / 404
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) =>
        FromResult(await transformationService.GetByIdAsync(id));

    // → 201 / 400
    [HttpPost]
    public async Task<IActionResult> Create(CreateTransformationRequest request) =>
        CreatedFromResult(await transformationService.CreateAsync(request), nameof(GetById));

    // Ölçümler gönderilen listeyle değiştirilir → 204 / 400 / 404
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateTransformationRequest request) =>
        FromResult(await transformationService.UpdateAsync(request with { Id = id }));

    // Soft delete → 204 / 404
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        FromResult(await transformationService.DeleteAsync(id));

    // Ya hepsi ya hiçbiri: biri bulunamaz veya silinemezse hiçbiri silinmez; gövde: [3, 7, 12] → 204 / 400 / 404 / 409
    [HttpPost("bulk-delete")]
    public async Task<IActionResult> DeleteMany(List<int> ids) =>
        FromResult(await transformationService.DeleteManyAsync(ids));
}
