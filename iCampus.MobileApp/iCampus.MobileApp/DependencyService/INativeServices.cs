namespace iCampus.MobileApp.DependencyService;

public interface INativeServices
{
    void ChangeStatusBarColor(int r, int g, int b);
    void DownloadAndPreviewFile(string path, string fileId);
    void KillProcess();
    void ShowAlert(string title, string message);
    void ShowAlertWithTwoButtons(string title, string message, string yesBtn, string noBtn, Action<bool> result);
    void GetDeviceID(Action<string> result);
    void SetToolBarColor(string color);
    void CheckLocationPermission(Action<bool> result);
    void NotificationToggled(Action<bool> result = null);
    Task CheckNotificationSetting();
    bool SystemVersionCheck();
    Task<string> GetDownloadFolderPathAsync();
}