using Foundation;
using QuickLook;
using UIKit;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using iCampus.MobileApp.DependencyService;
using Microsoft.Maui.Controls; // Required for Dependency attribute

[assembly: Dependency(typeof(CS.MobileApp.DependencyService.FilePreviewerImplementation))]

namespace CS.MobileApp.DependencyService
{
    public class FilePreviewerImplementation : NSObject, IFilePreviewer, IQLPreviewItem
    {
        private string localFilePath;

        public async Task PreviewFile(string fileUrl)
        {
            try
            {
                // Download file locally
                localFilePath = await DownloadFileAsync(fileUrl);

                if (!string.IsNullOrEmpty(localFilePath))
                {
                    // Instantiate QLPreviewController
                    var previewController = new QLPreviewController
                    {
                        DataSource = new FilePreviewControllerDataSource(this)
                    };

                    // Present the QLPreviewController
                    var viewController = UIApplication.SharedApplication.KeyWindow?.RootViewController;
                    viewController?.PresentViewController(previewController, true, null);
                }
                else
                {
                    Console.WriteLine("Failed to download file.");
                }
            }
            catch (Exception ex)
            {
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
