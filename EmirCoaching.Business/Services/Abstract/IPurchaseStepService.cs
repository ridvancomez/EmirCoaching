using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

// "Satın aldıktan sonra ne oluyor?" adımları sitede de GetAllAsync ile (SortOrder sırasıyla) gelir
public interface IPurchaseStepService
    : ICrudService<PurchaseStepResponse, CreatePurchaseStepRequest, UpdatePurchaseStepRequest>;
