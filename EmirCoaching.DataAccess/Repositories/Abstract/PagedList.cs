namespace EmirCoaching.DataAccess.Repositories.Abstract;

public record PagedList<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int Size);
