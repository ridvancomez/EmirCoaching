using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

// Değişim türleri
[Route("api/v1/admin/transformation-types")]
public class TransformationTypesController(ITransformationTypeService transformationTypeService) : AdminControllerBase
{
    // Pasifler dahil, sitedeki sırayla → 200
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        FromResult(await transformationTypeService.GetAllAsync());

    // → 200 / 404
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) =>
        FromResult(await transformationTypeService.GetByIdAsync(id));

    // → 201 / 400 / 409
    [HttpPost]
    public async Task<IActionResult> Create(CreateTransformationTypeRequest request) =>
        CreatedFromResult(await transformationTypeService.CreateAsync(request), nameof(GetById));

    // → 204 / 400 / 404 / 409
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateTransformationTypeRequest request) =>
        FromResult(await transformationTypeService.UpdateAsync(request with { Id = id }));

    // Soft delete → 204 / 404 / 409
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        FromResult(await transformationTypeService.DeleteAsync(id));

    // Ya hepsi ya hiçbiri: biri bulunamaz veya silinemezse hiçbiri silinmez; gövde: [3, 7, 12] → 204 / 400 / 404 / 409
    [HttpPost("bulk-delete")]
    public async Task<IActionResult> DeleteMany(List<int> ids) =>
        FromResult(await transformationTypeService.DeleteManyAsync(ids));
}
