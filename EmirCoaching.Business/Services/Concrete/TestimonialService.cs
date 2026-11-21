using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class TestimonialService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateTestimonialRequest> createValidator,
    IValidator<UpdateTestimonialRequest> updateValidator,
    ILogger<TestimonialService> logger)
    : CrudService<Testimonial, TestimonialResponse, CreateTestimonialRequest, UpdateTestimonialRequest>(
        unitOfWork.Testimonials, unitOfWork, mapper, createValidator, updateValidator, logger), ITestimonialService
{
    public async Task<Result<IReadOnlyList<TestimonialResponse>>> GetAllAsync(int? packageId) =>
        Result<IReadOnlyList<TestimonialResponse>>.Success(
            Mapper.Map<List<TestimonialResponse>>(await UnitOfWork.Testimonials.GetOrderedAsync(packageId)));

    public async Task<Result<IReadOnlyList<TestimonialResponse>>> GetHomeAsync() =>
        Result<IReadOnlyList<TestimonialResponse>>.Success(
            Mapper.Map<List<TestimonialResponse>>(await UnitOfWork.Testimonials.GetHomeAsync()));

    protected override Task<IReadOnlyList<Testimonial>> ListAsync() => UnitOfWork.Testimonials.GetOrderedAsync();

    protected override async Task<Result> CheckAsync(CreateTestimonialRequest request, int? id) =>
        await UnitOfWork.Packages.GetByIdAsync(request.PackageId) is null
            ? Result.Invalid("Seçilen paket bulunamadı.")
            : Result.Success();
}
