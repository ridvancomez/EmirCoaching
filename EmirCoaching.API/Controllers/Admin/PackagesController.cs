using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

[Route("api/v1/admin/packages")]
public class PackagesController(IPackageService packageService) : AdminControllerBase
{
    // Pasifler dahil tüm paketler → 200
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        FromResult(await packageService.GetAllAsync());

    // Düzenleme formu: modüller, kalemler, öne çıkanlar → 200 / 404
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) =>
        FromResult(await packageService.GetByIdAsync(id));

    // → 201 / 400 / 409
    [HttpPost]
    public async Task<IActionResult> Create(CreatePackageRequest request) =>
        CreatedFromResult(await packageService.CreateAsync(request), nameof(GetById));

    // Modüller ve öne çıkanlar gönderilen listeyle değiştirilir → 204 / 400 / 404 / 409
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdatePackageRequest request) =>
        FromResult(await packageService.UpdateAsync(request with { Id = id }));

    // Soft delete (modüller ve kalemlerle birlikte) → 204 / 404
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        FromResult(await packageService.DeleteAsync(id));

    // Gövde: paketin tüm modül id'leri yeni sırayla, ör. [3, 1, 2] → 204 / 400 / 404
    [HttpPut("{id:int}/modules/order")]
    public async Task<IActionResult> ReorderModules(int id, List<int> moduleIds) =>
        FromResult(await packageService.ReorderModulesAsync(id, moduleIds));
}
