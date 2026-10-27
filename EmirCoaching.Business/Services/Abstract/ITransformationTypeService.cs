using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

// Değişimler filtre sekmeleri GetAllAsync ile (SortOrder sırasıyla) listelenir
public interface ITransformationTypeService
    : ICrudService<TransformationTypeResponse, CreateTransformationTypeRequest, UpdateTransformationTypeRequest>;
