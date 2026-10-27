namespace EmirCoaching.Business.Common;

// Aynı numaranın farklı yazımları tek biçime iner (mükerrer başvuru kontrolü için):
// "0555 111 22 33", "+90 555 111 22 33", "5551112233" -> "05551112233"
public static class PhoneHelper
{
    public static string Normalize(string phone)
    {
        var digits = new string(phone.Where(char.IsAsciiDigit).ToArray());
        return digits switch
        {
            { Length: 12 } when digits.StartsWith("90") => "0" + digits[2..],
            { Length: 10 } when digits.StartsWith('5') => "0" + digits,
            _ when phone.TrimStart().StartsWith('+') => "+" + digits, // yurt dışı numara
            _ => digits
        };
    }
}
