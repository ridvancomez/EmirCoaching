using System.Runtime.CompilerServices;
using EmirCoaching.Business.Validation;

namespace EmirCoaching.Tests.Helpers;

// API'nin Program.cs'teki ayarı: test derlemesi yüklenince bir kez çalışır, mesajlar ve alan adları Türkçe olur
internal static class TestSetup
{
    [ModuleInitializer]
    internal static void Initialize() => TurkishValidation.Configure();
}
