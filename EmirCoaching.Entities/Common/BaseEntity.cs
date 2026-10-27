namespace EmirCoaching.Entities.Common;

/// <summary>
/// Tüm tabloların ortak alanları. CreatedAt/UpdatedAt AppDbContext'te otomatik dolar;
/// silme işlemi soft delete'tir (IsDeleted = true, kayıt veritabanında kalır).
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
