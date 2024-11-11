namespace iCampus.MobileApp.DependencyService;

public interface IFilePreviewer
{
    Task PreviewFile(string filePath);
}