using Android.Graphics;
using AndroidX.Print;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms;

namespace Gipa.MobileApp.DependencyService;

public class AndroidPrintService : IPrintService
{
    public bool PrintImage(MemoryStream memoryStream)
    {
        try
        {
            AppSettings.Current.TaxReceiptStream = new MemoryStream(memoryStream.ToArray());

            var activity = Platform.CurrentActivity;
            var photoPrinter = new PrintHelper(activity);
            photoPrinter.ScaleMode = PrintHelper.ScaleModeFit;

            using var printBitmap = BitmapFactory.DecodeStream(AppSettings.Current.TaxReceiptStream);
            if (printBitmap == null)
                throw new Exception("Failed to decode the image stream for printing.");

            photoPrinter.PrintBitmap("TaxReceipt", printBitmap);

            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"PrintImage error: {ex.Message}");
            return false;
        }
    }
}