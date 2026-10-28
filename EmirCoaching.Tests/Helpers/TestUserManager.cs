using EmirCoaching.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace EmirCoaching.Tests.Helpers;

// UserManager arayüz değil, sınıf: Moq onu yalnızca kurucusuna bir IUserStore vererek taklit edebilir.
// Metotları virtual olduğu için Setup ile ezilebilir; ezilmeyenler varsayılan (null/false) döner.
public static class TestUserManager
{
    public static Mock<UserManager<AppUser>> Create() =>
        new(Mock.Of<IUserStore<AppUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);
}
