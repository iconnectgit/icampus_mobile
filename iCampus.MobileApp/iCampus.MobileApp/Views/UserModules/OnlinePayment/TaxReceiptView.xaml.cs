using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms;
using Splat;

namespace iCampus.MobileApp.Views.UserModules.OnlinePayment;

public partial class TaxReceiptView : ContentPage
{
    public TaxReceiptView()
    {
        InitializeComponent();
    }
    
    void screenShot_SwipeRight(System.Object sender, System.EventArgs e)
    {
        MessagingCenter.Send("", "ScrollViewRightSwipeTaxReceipt");

    }

    private void TapGestureRecognizer_OnTapped(object? sender, TappedEventArgs e)
    {
        var printButtonFrameBounds = PrintButtonFrame.Bounds;
        var printServices = Locator.Current.GetService<IPrintService>();
        if (printServices != null)
        {
            var success = printServices.PrintImage(printButtonFrameBounds);
            if (success)
                System.Diagnostics.Debug.WriteLine("Bitmap generated and printed successfully.");
            else
                System.Diagnostics.Debug.WriteLine("Failed to generate and print bitmap.");
        }
    }
}