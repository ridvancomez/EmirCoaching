using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

// "Nasıl Çalışır?" adımları sitede de GetAllAsync ile (SortOrder sırasıyla) gelir
public interface IDietProgramStepService
    : ICrudService<DietProgramStepResponse, CreateDietProgramStepRequest, UpdateDietProgramStepRequest>;
