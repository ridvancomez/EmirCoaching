using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

// Video sorular
[Route("api/v1/admin/video-questions")]
public class VideoQuestionsController(IVideoQuestionService videoQuestionService) : AdminControllerBase
{
    // Pasifler dahil, sitedeki sırayla → 200
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        FromResult(await videoQuestionService.GetAllAsync());

    // → 200 / 404
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) =>
        FromResult(await videoQuestionService.GetByIdAsync(id));

    // → 201 / 400 / 409
    [HttpPost]
    public async Task<IActionResult> Create(CreateVideoQuestionRequest request) =>
        CreatedFromResult(await videoQuestionService.CreateAsync(request), nameof(GetById));

    // → 204 / 400 / 404 / 409
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateVideoQuestionRequest request) =>
        FromResult(await videoQuestionService.UpdateAsync(request with { Id = id }));

    // Soft delete → 204 / 404
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        FromResult(await videoQuestionService.DeleteAsync(id));

    // Ya hepsi ya hiçbiri: biri bulunamaz veya silinemezse hiçbiri silinmez; gövde: [3, 7, 12] → 204 / 400 / 404 / 409
    [HttpPost("bulk-delete")]
    public async Task<IActionResult> DeleteMany(List<int> ids) =>
        FromResult(await videoQuestionService.DeleteManyAsync(ids));
}
