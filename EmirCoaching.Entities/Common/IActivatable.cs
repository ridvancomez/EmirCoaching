namespace EmirCoaching.Entities.Common;

/// <summary>Sitede gösterilip gösterilmeyeceği admin tarafından açılıp kapatılan kayıtlar.</summary>
public interface IActivatable
{
    bool IsActive { get; set; }
}
