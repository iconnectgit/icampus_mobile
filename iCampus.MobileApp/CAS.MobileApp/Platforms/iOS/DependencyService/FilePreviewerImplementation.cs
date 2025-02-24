using Foundation;
using QuickLook;
using UIKit;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using Microsoft.Maui.Controls; // Required for Dependency attribute

[assembly: Dependency(typeof(CAS.MobileApp.DependencyService.FilePreviewerImplementation))]

namespace CAS.MobileApp.DependencyService
{
    public class FilePreviewerImplementation : NSObject, IFilePreviewer, IQLPreviewItem
    {
        private string localFilePath;

        public async Task PreviewFile(string fileUrl)
        {
            try
            {
                await ApiHelper.ShowProcessingIndicatorPopup();

                localFilePath = await DownloadFileAsync(fileUrl);

                await ApiHelper.HideProcessingIndicatorPopup(); 

                if (!string.IsNullOrEmpty(localFilePath))
                {
                    var previewController = new QLPreviewController
                    {
                        DataSource = new FilePreviewControllerDataSource(this)
                    };

                    var viewController = UIApplication.SharedApplication.KeyWindow?.RootViewController;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        viewController?.PresentViewController(previewController, true, null);
                    });
                }
                else
                {
                    Console.WriteLine("Failed to download file.");
                }
            }
            catch (Exception ex)
            {
                await ApiHelper.HideProcessingIndicatorPopup();
                Console.WriteLine($"Exception in PreviewFile: {ex.Message}");
            }
        }


        // Async method to download file from URL
        private async Task<string> DownloadFileAsync(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var data = await client.GetByteArrayAsync(url);
                    var fileName = Path.GetFileName(url);
                    fileName = SanitizeFileName(fileName);
                    var tempPath = Path.Combine(Path.GetTempPath(), fileName);

                    File.WriteAllBytes(tempPath, data);
                    return tempPath;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in DownloadFileAsync: {ex.Message}");
                return null;
            }
        }
        
        private string SanitizeFileName(string fileName)
        {
            fileName = Uri.UnescapeDataString(fileName);

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }

            fileName = fileName.Normalize(NormalizationForm.FormC).Trim();

            if (fileName.Length > 100)
                fileName = fileName.Substring(0, 100);

            return fileName;
        }

        // Required for IQLPreviewItem
        [Export("previewItemURL")]
        public NSUrl PreviewItemURL => NSUrl.FromFilename(localFilePath);

        [Export("previewItemTitle")]
        public string PreviewItemTitle => Path.GetFileName(localFilePath);
    }

    // QLPreviewControllerDataSource class to provide data to QLPreviewController
    public class FilePreviewControllerDataSource : QLPreviewControllerDataSource
    {
        private readonly FilePreviewerImplementation previewItem;

        public FilePreviewControllerDataSource(FilePreviewerImplementation previewItem)
        {
            this.previewItem = previewItem;
        }

        public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index)
        {
            return previewItem;
        }

        public override nint PreviewItemCount(QLPreviewController controller)
        {
            return 1;
        }
    }
}
