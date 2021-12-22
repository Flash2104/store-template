namespace Store.Service.Common;

public static class EmailHelper
{
    public static bool IsValidEmail(string email)
    {
        if (email.Trim().EndsWith("."))
        {
            return false; // suggested by @TK-421
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}