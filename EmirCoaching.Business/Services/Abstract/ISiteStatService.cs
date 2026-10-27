using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

// Sayaçlar sitede de GetAllAsync ile (SortOrder sırasıyla) gelir
public interface ISiteStatService : ICrudService<SiteStatResponse, CreateSiteStatRequest, UpdateSiteStatRequest>;
