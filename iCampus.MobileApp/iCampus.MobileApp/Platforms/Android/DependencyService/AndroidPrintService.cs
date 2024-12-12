using Android.Graphics;
using Android.Views;
using AndroidX.Print;
using iCampus.MobileApp.Forms;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Path = System.IO.Path;
using Rect = Microsoft.Maui.Graphics.Rect;
using View = Microsoft.Maui.Controls.View;

namespace iCampus.MobileApp.DependencyService;

public class AndroidPrintService : IPrintService
{
    public bool PrintImage(Rect printButtonBounds)
    {
        try
        {
            // Get the current activity's ViewGroup
            var activity = Platform.CurrentActivity;
            if (activity == null)
                throw new Exception("Current activity is null. Ensure Platform.Init(this, bundle) is called in MainActivity.");

            var viewGroup = activity.Window?.DecorView as ViewGroup;
            if (viewGroup == null)
                throw new Exception("ViewGroup is null. Unable to capture the view.");

            // Generate the bitmap from the root view
            var subView = viewGroup.GetChildAt(0);
            int width = subView.Width;
            int topOffset = (int)printButtonBounds.Y * 7; 
            int height = subView.Height - topOffset;

            Bitmap bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(bitmap);
            canvas.Translate(0, -topOffset);
            subView.Draw(canvas);

            // Convert bitmap to MemoryStream
            using var stream = new MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            AppSettings.Current.TaxReceiptStream = new MemoryStream(stream.ToArray());

            // Print the image
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

