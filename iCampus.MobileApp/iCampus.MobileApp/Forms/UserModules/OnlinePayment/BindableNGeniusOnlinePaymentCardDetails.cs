namespace iCampus.MobileApp.Forms.UserModules.OnlinePayment;

[Serializable]
public class BindableNGeniusOnlinePaymentCardDetails
{
    public BindableNGeniusOnlinePaymentCardDetails()
    {
    }
    public string OrderId
    {
        get;
        set;
    }

    public string Amount
    {
        get;
        set;
    }

    public string PAN
    {
        get;
        set;
    }

    public string Expiry
    {
        get;
        set;
    }

    public string CVV
    {
        get;
        set;
    }

    public string CardHolderName
    {
        get;
        set;
    }
}