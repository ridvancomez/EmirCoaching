using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface IBlogPostRepository : ISlugRepository<BlogPost>
{
    Task<PagedList<BlogPost>> GetPublishedPagedAsync(string? categorySlug, int page, int size);
    Task<BlogPost?> GetBySlugAsync(string slug);
    Task<IReadOnlyList<BlogPost>> GetRelatedAsync(int id, int count);

    // Blog sayfasındaki "Öne Çıkan": kategorinin işaretli yazısı, yoksa en son yayınlanan.
    // Kategori verilmezse işaretlilerden en son yayınlanan, işaretli yoksa en son yayınlanan yazı.
    Task<BlogPost?> GetFeaturedForDisplayAsync(string? categorySlug);

    // Admin listesi: taslaklar dahil, yeniden eskiye; kategori ve yayın durumuyla filtrelenebilir
    Task<PagedList<BlogPost>> GetPagedAsync(int? categoryId, bool? isPublished, int page, int size);

    // Admin düzenleme formu: taslak olsa da döner (kategori, yazar, önerilen paket, etiketler dahil)
    Task<BlogPost?> GetDetailByIdAsync(int id);

    // Admin: kategorinin işaretli öne çıkan yazısı (takip edilen kayıt)
    Task<BlogPost?> GetFeaturedAsync(int categoryId);

    // Admin güncellemesi için etiket bağlantıları dahil, takip edilen kayıt
    Task<BlogPost?> GetByIdWithTagsAsync(int id);

    // Var olan etiketler (takip edilen); bulunamayan slug'lar için servis yeni etiket oluşturur
    Task<List<BlogTag>> GetTagsBySlugsAsync(IReadOnlyCollection<string> slugs);

    // Kategori silinmeden önce: taslaklar dahil (silinmemiş) yazı var mı
    Task<bool> AnyInCategoryAsync(int categoryId);
}
