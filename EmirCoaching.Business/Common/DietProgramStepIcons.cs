using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Common;

// DietProgramStepIcon → sitedeki Iconify ikon adı (diyet-plani-detay.html "Nasıl Çalışır?" bölümünden)
public static class DietProgramStepIcons
{
    private static readonly Dictionary<DietProgramStepIcon, string> Names = new()
    {
        [DietProgramStepIcon.FormuDoldur] = "mdi:clipboard-text",
        [DietProgramStepIcon.PlanHazirlanir] = "mdi:silverware-fork-knife",
        [DietProgramStepIcon.WhatsApp] = "mdi:message-text"
    };

    public static string NameOf(DietProgramStepIcon icon) => Names.GetValueOrDefault(icon, string.Empty);
}
