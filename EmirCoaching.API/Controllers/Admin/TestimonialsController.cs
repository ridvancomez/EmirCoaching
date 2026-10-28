using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

// Yorumlar
[Route("api/v1/admin/testimonials")]
public class TestimonialsController(ITestimonialService testimonialService) : AdminControllerBase
{
    // Pasifler dahil, sitedeki sırayla; ?packageId= ile tek paketin yorumları → 200
    [HttpGet]
    public async Task<IActionResult> GetAll(int? packageId) =>
        FromResult(await testimonialService.GetAllAsync(packageId));

    // → 200 / 404
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) =>
        FromResult(await testimonialService.GetByIdAsync(id));

    // → 201 / 400 / 409
    [HttpPost]
    public async Task<IActionResult> Create(CreateTestimonialRequest request) =>
        CreatedFromResult(await testimonialService.CreateAsync(request), nameof(GetById));

    // → 204 / 400 / 404 / 409
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateTestimonialRequest request) =>
        FromResult(await testimonialService.UpdateAsync(request with { Id = id }));

    // Soft delete → 204 / 404
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        FromResult(await testimonialService.DeleteAsync(id));

    // Ya hepsi ya hiçbiri: biri bulunamaz veya silinemezse hiçbiri silinmez; gövde: [3, 7, 12] → 204 / 400 / 404 / 409
    [HttpPost("bulk-delete")]
    public async Task<IActionResult> DeleteMany(List<int> ids) =>
        FromResult(await testimonialService.DeleteManyAsync(ids));
}
