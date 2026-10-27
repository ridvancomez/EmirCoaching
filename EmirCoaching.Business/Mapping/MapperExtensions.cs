using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.DataAccess.Repositories.Abstract;

namespace EmirCoaching.Business.Mapping;

public static class MapperExtensions
{
    // Repository'nin sayfalı entity listesini sayfalı response'a çevirir
    public static PagedResult<TResponse> MapPaged<TEntity, TResponse>(this IMapper mapper, PagedList<TEntity> page) =>
        new(mapper.Map<List<TResponse>>(page.Items), page.TotalCount, page.Page, page.Size);
}
