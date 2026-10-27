namespace EmirCoaching.Entities.Common;

/// <summary>URL'de kullanılan, benzersiz slug'ı olan kayıtlar.</summary>
public interface ISluggable
{
    string Slug { get; set; }
}
