using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class DietProgramService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<UpdateDietProgramRequest> validator,
    IUploadCleanupService uploadCleanup,
    ILogger<DietProgramService> logger) : IDietProgramService
{
    private const int ProgramId = 1; // SeedData ile oluşturulan tek kayıt
    private const string ProgramNotFound = "Diyet programı bulunamadı.";

    public async Task<Result<DietProgramResponse>> GetActiveAsync()
    {
        var program = await unitOfWork.DietPrograms.GetActiveAsync();
        return program is null
            ? Result<DietProgramResponse>.NotFound(ProgramNotFound)
            : Result<DietProgramResponse>.Success(mapper.Map<DietProgramResponse>(program));
    }

    public async Task<Result<DietProgramResponse>> GetAsync()
    {
        var program = await unitOfWork.DietPrograms.GetByIdAsync(ProgramId);
        return program is null
            ? Result<DietProgramResponse>.NotFound(ProgramNotFound)
            : Result<DietProgramResponse>.Success(mapper.Map<DietProgramResponse>(program));
    }

    public async Task<Result> UpdateAsync(UpdateDietProgramRequest request)
    {
        var errors = await validator.GetErrorsAsync(request);
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var program = await unitOfWork.DietPrograms.GetByIdAsync(ProgramId);
        if (program is null)
            return Result.NotFound(ProgramNotFound);

        var oldUploads = UploadPaths.Of(program);
        mapper.Map(request, program);

        await unitOfWork.SaveChangesAsync();
        await uploadCleanup.DeleteUnusedAsync(oldUploads.Except(UploadPaths.Of(program)));

        logger.LogInformation("Diyet programı güncellendi");
        return Result.Success();
    }
}
