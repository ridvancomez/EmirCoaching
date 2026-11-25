namespace EmirCoaching.DataAccess.Seed;

// Front-end index.html koç bölümü ve blog yazar kartının ilk metinleri; ilk admin oluşturulurken ve migration'da kullanılır
public static class CoachDefaults
{
    public const string Title = "Baş Antrenör";
    public const string Experience = "8+ yıl";
    public const string VideoCoverPath = "/images/hakkimda-video.png";

    // blog-detay.html yazar kartı
    public const string AvatarPath = "/images/yazar-ibrahim-emir.jpg";
    public const string Bio =
        "10 yılı aşkın tecrübesiyle yüzlerce kişinin fiziksel ve mental dönüşümüne rehberlik etmiş, bilimsel temelli " +
        "antrenman ve beslenme prensiplerini savunan baş antrenör.";

    public const string Story =
        "Gerçek değişim geçici motivasyonlarla değil, sürdürülebilir sistemlerle başlar. Emircoaching olarak, fiziksel " +
        "potansiyelinizi ortaya çıkarmak için bilimsel temelli bir yaklaşım sunuyoruz.\n\n" +
        "Buraya 2-3 satırlık kişisel hikaye gelecek: nasıl başladın, neden online koçluk, danışana neyi farklı veriyorsun.";
}
