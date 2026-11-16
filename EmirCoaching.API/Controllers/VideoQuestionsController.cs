using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/video-questions")]
public class VideoQuestionsController(IVideoQuestionService videoQuestionService) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VideoQuestionResponse>>> GetActive() => FromResult(await videoQuestionService.GetActiveAsync());
}
