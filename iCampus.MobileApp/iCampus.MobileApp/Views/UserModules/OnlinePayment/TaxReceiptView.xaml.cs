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

    private async void TapGestureRecognizer_OnTapped(object? sender, TappedEventArgs e)
    {
        var result = await screenShot.CaptureAsync();
        using MemoryStream memoryStream = new();
        await result.CopyToAsync(memoryStream);
        
        var printServices = Locator.Current.GetService<IPrintService>();
        if (printServices != null)
        {
            var success = printServices.PrintImage(memoryStream);
            if (success)
                System.Diagnostics.Debug.WriteLine("Bitmap generated and printed successfully.");
            else
                System.Diagnostics.Debug.WriteLine("Failed to generate and print bitmap.");
        }
    }
}