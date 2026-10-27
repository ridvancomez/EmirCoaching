namespace EmirCoaching.Business.Dtos;

// Yüklenen dosya: Path kayıt isteklerinde (*Path alanları) kullanılır, Url önizleme içindir.
public record UploadedFileResponse
{
    public string Path { get; init; } = string.Empty; // /uploads/images/....jpg
    public string Url { get; init; } = string.Empty;  // http://.../uploads/images/....jpg
}
