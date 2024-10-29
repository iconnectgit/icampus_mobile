namespace iCampus.MobileApp.Forms.UserModules.ChequeReplacement;

public class BindablePdcReplacementDataViewModel 
{
    public IEnumerable<BindablePdcDataView> PendingAndBouncedPdcList { get; set; }
    public IEnumerable<BindablePdcDataView> PdcHistoryList { get; set; }
    public BindablePaymentSettingsView PaymentSettings { get; set; }
    public int AcademicYear { get; set; }
    public BindableChequeReplacementSettingView ChequeReplacementSettings { get; set; }
    public bool AllChequePaymentSynchronized { get; set; }
    public IEnumerable<BindablePdcDataView> ChequePaymentList { get; set; }

        
}