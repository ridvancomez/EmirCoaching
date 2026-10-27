namespace EmirCoaching.Business.Common;

// Güncelleme isteklerinin ortak alanı (Update{Entity}Request : Create{Entity}Request, IUpdateRequest)
public interface IUpdateRequest
{
    int Id { get; }
}
