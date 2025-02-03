using Foundation;
using iCampus.MobileApp.DependencyService;
using UIKit;

namespace CS.MobileApp.DependencyService;

public class iOSPrintService : IPrintService
{
    public bool PrintImage(MemoryStream memoryStream)
    {
        try
        {
            if (memoryStream == null || memoryStream.Length == 0)
                throw new ArgumentException("The provided memory stream is null or empty.");

            var imageData = NSData.FromArray(memoryStream.ToArray());
            if (imageData == null)
                throw new Exception("Failed to convert the memory stream to NSData.");

            var image = UIImage.LoadFromData(imageData);
            if (image == null)
                throw new Exception("Failed to create UIImage from NSData.");

            var printController = UIPrintInteractionController.SharedPrintController;
            if (printController == null)
                throw new Exception("Failed to get the shared print controller.");

            var printInfo = UIPrintInfo.PrintInfo;
            printInfo.OutputType = UIPrintInfoOutputType.General;
            printInfo.JobName = "TaxReceipt";

            printController.PrintInfo = printInfo;
            printController.PrintingItem = image;

            printController.Present(true, (controller, completed, error) =>
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