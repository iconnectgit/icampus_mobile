namespace iCampus.MobileApp.Library.FormValidation;

public interface IValidationRule<T>
{
    string ValidationMessage { get; set; }
    bool Check(T value);
}