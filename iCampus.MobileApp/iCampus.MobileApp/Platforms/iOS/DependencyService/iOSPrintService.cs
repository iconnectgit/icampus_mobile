using CoreGraphics;
using iCampus.MobileApp.Forms;
using UIKit;

namespace iCampus.MobileApp.DependencyService;

public class iOSPrintService : IPrintService
{
    public bool PrintImage(Rect printButtonBounds)
    {
        try
        {
            // Get the current window's root view
            var window = UIApplication.SharedApplication.KeyWindow;
            if (window == null)
                throw new Exception("KeyWindow is null. Unable to capture the view.");

            var rootView = window.RootViewController.View;
            if (rootView == null)
                throw new Exception("Root view is null. Unable to capture the view.");

            // Define the area to capture
            var width = (nfloat)rootView.Frame.Width;
            var height = (nfloat)(rootView.Frame.Height - printButtonBounds.Y);
            var topOffset = (nfloat)(printButtonBounds.Bottom - 10);
            var captureRect = new CGRect(0, -topOffset, width, height);

            // Render the view into a UIImage
            UIGraphics.BeginImageContextWithOptions(captureRect.Size, false, 0);
            rootView.DrawViewHierarchy(captureRect, true);
            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            if (image == null)
                throw new Exception("Failed to generate image from view.");

            // Convert the image to NSData
            var imageData = image.AsPNG();
            if (imageData == null)
                throw new Exception("Failed to convert UIImage to PNG data.");

            AppSettings.Current.TaxReceiptStream = new MemoryStream(imageData.ToArray());


            // Configure the print job
            var printInfo = UIPrintInfo.PrintInfo;
            printInfo.OutputType = UIPrintInfoOutputType.General;
            printInfo.JobName = "TaxReceipt";

            var printController = UIPrintInteractionController.SharedPrintController;
            printController.PrintInfo = printInfo;
            printController.PrintingItem = image;

            // Show the print dialog
            printController.Present(true, (handler, completed, error) =>
            {
                if (!completed && error != null)
                {
                    System.Diagnostics.Debug.WriteLine($"PrintImage error: {error.LocalizedDescription}");
                }
            });

            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"PrintImage error: {ex.Message}");
            return false;
        }
    }
}