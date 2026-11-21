namespace EmirCoaching.DataAccess.Seed;

// Front-end index.html koç bölümünün ilk metinleri; ilk admin oluşturulurken ve migration'da kullanılır
public static class CoachDefaults
{
    public const string Title = "Baş Antrenör";
    public const string Experience = "8+ yıl";
    public const string VideoCoverPath = "/images/hakkimda-video.png";

    public const string Story =
        "Gerçek değişim geçici motivasyonlarla değil, sürdürülebilir sistemlerle başlar. Emircoaching olarak, fiziksel " +
        "potansiyelinizi ortaya çıkarmak için bilimsel temelli bir yaklaşım sunuyoruz.\n\n" +
        "Buraya 2-3 satırlık kişisel hikaye gelecek: nasıl başladın, neden online koçluk, danışana neyi farklı veriyorsun.";
}
