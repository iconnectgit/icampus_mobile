namespace CAS.MobileApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Delegate the initialization to iCampus.MobileApp.App
        var app = new iCampus.MobileApp.App();
        MainPage = app.MainPage;
    }
}