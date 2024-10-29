using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCampus.MobileApp.Views.UserModules.OnlinePayment;

public partial class TaxReceiptView : ContentPage
{
    public TaxReceiptView()
    {
        InitializeComponent();
    }

    void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
    {
        
    }
    
    void screenShot_SwipeRight(System.Object sender, System.EventArgs e)
    {
        MessagingCenter.Send("", "ScrollViewRightSwipeTaxReceipt");

    }
}