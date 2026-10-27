using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class VideoQuestionService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateVideoQuestionRequest> createValidator,
    IValidator<UpdateVideoQuestionRequest> updateValidator,
    IUploadCleanupService uploadCleanup,
    ILogger<VideoQuestionService> logger)
    : CrudService<VideoQuestion, VideoQuestionResponse, CreateVideoQuestionRequest, UpdateVideoQuestionRequest>(
        unitOfWork.VideoQuestions, unitOfWork, mapper, createValidator, updateValidator, logger), IVideoQuestionService
{
    public async Task<Result<IReadOnlyList<VideoQuestionResponse>>> GetActiveAsync() =>
        Result<IReadOnlyList<VideoQuestionResponse>>.Success(
            Mapper.Map<List<VideoQuestionResponse>>(await UnitOfWork.VideoQuestions.GetActiveOrderedAsync()));

    protected override Task<IReadOnlyList<VideoQuestion>> ListAsync() => UnitOfWork.VideoQuestions.GetOrderedAsync();

    protected override Task ReleaseUploadsAsync(IEnumerable<string> paths) => uploadCleanup.DeleteUnusedAsync(paths);
}
