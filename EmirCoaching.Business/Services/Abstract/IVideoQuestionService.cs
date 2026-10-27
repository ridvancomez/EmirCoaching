using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

public interface IVideoQuestionService
    : ICrudService<VideoQuestionResponse, CreateVideoQuestionRequest, UpdateVideoQuestionRequest>
{
    // Sitede gösterilenler: aktif, SortOrder sırasıyla
    Task<Result<IReadOnlyList<VideoQuestionResponse>>> GetActiveAsync();
}
