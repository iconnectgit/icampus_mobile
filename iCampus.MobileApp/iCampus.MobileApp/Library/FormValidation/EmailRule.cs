namespace iCampus.MobileApp.Library.FormValidation;

public class EmailRule<T> : IValidationRule<T>
{
    public string ValidationMessage { get; set; }

    public bool Check(T value)
    {
        if (value == null)
        {
            return false;
        }
        try
        {
            var str = value as string;
            System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(str);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}