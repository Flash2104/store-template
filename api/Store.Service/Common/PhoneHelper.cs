using System.Text.RegularExpressions;

namespace Store.Service.Common;

public class PhoneHelper
{
    public static string CleanPhone(string phone)
    {
        Regex rgx = new Regex("[^0-9]");
        return rgx.Replace(phone, "");
    }
}