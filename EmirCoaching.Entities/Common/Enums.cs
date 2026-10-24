namespace EmirCoaching.Entities.Common;

// Tüm enum'lar veritabanında int olarak saklanır; mevcut değerler değiştirilmez.

public enum PackageLocation
{
    Salonda = 0,
    Evde = 1 // front-end'de body.tema-evde
}

public enum ModuleItemState
{
    Standart = 0,
    Ozel = 1,   // modul-card__item--ozel ("Pakete Özel")
    Kilitli = 2 // modul-card__item--kilitli (LockedLabel ile)
}

public enum TransformationCategory
{
    YagKaybi = 0,
    KasKazanimi = 1,
    FormKoruma = 2
}

public enum FaqGroup
{
    Genel = 0,
    Paket = 1,
    Diyet = 2
}

public enum LegalDocumentType
{
    Kvkk = 0,
    AcikRiza = 1,
    Cerez = 2,
    IptalIade = 3,
    TicariIleti = 4,
    Gizlilik = 5
}

public enum ContactMessageStatus
{
    Yeni = 0,
    Okundu = 1,
    Donuldu = 2
}

public enum DietFormStatus
{
    Yeni = 0,
    Okundu = 1,
    Donuldu = 2
}

public enum ActivityLevel
{
    Sedentary = 0,
    Light = 1,
    Moderate = 2,
    Active = 3,
    VeryActive = 4
}

public enum ConsentType
{
    Gizlilik = 0,
    AcikRizaSaglik = 1,
    TicariIleti = 2
}
