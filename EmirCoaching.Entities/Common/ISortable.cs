namespace EmirCoaching.Entities.Common;

/// <summary>Admin'in belirlediği sırayla listelenen kayıtlar.</summary>
public interface ISortable
{
    int SortOrder { get; set; }
}
