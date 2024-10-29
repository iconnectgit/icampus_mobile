using System.ComponentModel;

namespace iCampus.MobileApp.Forms.UserModules.ChequeReplacement;

public class BindableChequeReplacementSettingView : INotifyPropertyChanged
{
    public int SettingsId { get; set; }
    public bool CanPayInSequenceOnly { get; set; }
    public bool EnableExtraCharges { get; set; }
    public decimal? ExtraCharges { get; set; }
    public decimal? ExtraChargesVatPercentage { get; set; }
    public int MindaysToPay { get; set; }
    public bool CountOnlyWorkingDays { get; set; }
    public string NotificationEmailAddresses { get; set; }
    public string NotificationEmailTemplate { get; set; }
    public string NotificationFailedEmailTemplate { get; set; }
    public bool IsExtraAmountEnabled { get; set; }
    public bool IsExtraAmountVatEnabled { get; set; }

    //public bool CanSendNotificationEmail(bool isTransactionSuccessful);

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null)
            return;

        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}