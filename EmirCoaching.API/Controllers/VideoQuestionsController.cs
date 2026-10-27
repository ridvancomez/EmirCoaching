using EmirCoaching.API.Common;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/video-questions")]
public class VideoQuestionsController(IVideoQuestionService videoQuestionService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetActive() => FromResult(await videoQuestionService.GetActiveAsync());
}
