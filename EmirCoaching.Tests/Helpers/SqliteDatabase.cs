using EmirCoaching.DataAccess.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.Tests.Helpers;

// Repository testleri için bellekte gerçek bir ilişkisel veritabanı. Bağlantı açık kaldıkça veritabanı yaşar;
// her test sınıfı örneği kendi boş veritabanını alır (şema + seed verisi EnsureCreated ile).
// EF InMemory yerine SQLite: ExecuteDelete, unique/filtreli index ve FK kısıtları gerçekten çalışır.
public sealed class SqliteDatabase : IDisposable
{
    private readonly SqliteConnection _connection = new("DataSource=:memory:");

    public SqliteDatabase()
    {
        _connection.Open();
        using var context = CreateContext();
        context.Database.EnsureCreated();
    }

    // Her çağrı yeni bir context: kaydedip yeni context'le okumak, değişiklik takibinden bağımsız sonucu gösterir
    public AppDbContext CreateContext() =>
        new(new DbContextOptionsBuilder<AppDbContext>().UseSqlite(_connection).Options);

    public void Dispose() => _connection.Dispose();
}
