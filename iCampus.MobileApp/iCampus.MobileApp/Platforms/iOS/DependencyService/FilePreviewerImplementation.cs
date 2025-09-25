using System.Text;
using Foundation;
using QuickLook;
using UIKit;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;

[assembly: Dependency(typeof(FilePreviewerImplementation))]

namespace iCampus.MobileApp.DependencyService;

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
                    { DataSource = new FilePreviewControllerDataSource(this) };
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
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return null;

                var data = await response.Content.ReadAsByteArrayAsync();

                // Detect content type
                var contentType = response.Content.Headers.ContentType?.MediaType ?? string.Empty;

                // Default to PDF
                string extension = ".pdf";

                if (contentType.Contains("msword"))
                    extension = ".doc";
                else if (contentType.Contains("officedocument.wordprocessingml.document"))
                    extension = ".docx";
                else if (contentType.Contains("excel"))
                    extension = ".xls";
                else if (contentType.Contains("spreadsheetml.sheet"))
                    extension = ".xlsx";
                else if (contentType.Contains("plain"))
                    extension = ".txt";
                else if (contentType.Contains("html"))
                    extension = ".html"; // in case server sends HTML

                // If URL already has a valid extension, prefer that
                string fileName = Path.GetFileName(new Uri(url).AbsolutePath);
                if (string.IsNullOrEmpty(Path.GetExtension(fileName)))
                {
                    fileName = Guid.NewGuid().ToString() + extension;
                }

                fileName = SanitizeFileName(fileName);

                var tempPath = Path.Combine(Path.GetTempPath(), fileName);

                File.WriteAllBytes(tempPath, data);
                
                var fileInfo = new FileInfo(tempPath);
                Console.WriteLine($"📂 File Path: {tempPath}");
                Console.WriteLine($"📏 File Size: {fileInfo.Length} bytes");

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
    [Export("previewItemURL")] public NSUrl PreviewItemURL => NSUrl.FromFilename(localFilePath);

    [Export("previewItemTitle")] public string PreviewItemTitle => Path.GetFileName(localFilePath);
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