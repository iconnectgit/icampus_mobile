using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCampus.MobileApp.Forms;
using iCampus.MobileApp.Forms.UserModules.ReportCard;
using iCampus.Portal.ViewModels;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;


namespace iCampus.MobileApp.Views.UserModules.ReportCard;

public partial class ReportCardPage : ContentPage
{
    public ReportCardPage()
    {
        InitializeComponent();
        ReportCardWebView.Navigating += WebView_Navigating;
        ReportCardButton.Loaded += (s, e) => AlignButtonText(ReportCardButton);
        SkillReportCardButton.Loaded += (s, e) => AlignButtonText(SkillReportCardButton);
        MessagingCenter.Subscribe<ReportCardSettingsView, bool>(this, "ReportCardData", (res, isStudentChange) =>
        {
            try
            {
                // if (isStudentChange)
                // {
                //     if (beamReportCardTab.ItemSource.Count > 0)
                //     {
                //         if (beamReportCardTab.ItemSource.Count > 1)
                //         {
                //             beamReportCardTab.RemoveTab(1);
                //         }
                //         beamReportCardTab.RemoveTab(0);
                //     }
                //
                //     beamReportCardTab.SelectedTabIndex = 0;
                //
                //     if (res.IsMarksReportCardEnabled)
                //         beamReportCardTab.AddTab(beamMarkTab, 0, true);
                //
                //     if (res.IsSkillsReportCardEnabled)
                //     {
                //         beamReportCardTab.AddTab(beamSkillTab, beamReportCardTab.ItemSource.Count, true);
                //     }
                //
                //     if (beamReportCardTab.ItemSource.Count >= 1)
                //         beamReportCardTab.SelectFirst();
                // }
            }
            catch (Exception ex)
            {
            }
        });
    }

    // private void reportCardTab_PositionChanged(object sender, Xam.Plugin.TabView.PositionChangedEventArgs e)
    // {
    //     try
    //     {
    //             MessagingCenter.Send<ReportCardForm, int>((ReportCardForm)this.BindingContext, "TabPosition", beamReportCardTab.SelectedTabIndex);
    //     }
    //     catch(Exception ex)
    //     {
    //
    //     }
    // }

    protected override bool OnBackButtonPressed()
    {
        var currentViewModel = BindingContext as ViewModelBase;

        if (currentViewModel != null) currentViewModel.HandleMenuSelectionOnBack();
        return true;
    }

    // private async void OnWebViewNavigated(object sender, WebNavigatedEventArgs e)
    // {
    //     if (sender is WebView webView)
    //     {
    //         try
    //         {
    //             // Execute JavaScript to get the height of the content
    //             string heightStr = await webView.EvaluateJavaScriptAsync(
    //                 "document.body.scrollHeight.toString();"
    //             );
    //
    //             if (int.TryParse(heightStr, out int contentHeight))
    //             {
    //                 // Update WebView Height Request based on content height
    //                 webView.HeightRequest = contentHeight;
    //             }
    //         }
    //         catch (Exception ex)
    //         {
    //             Console.WriteLine($"Error getting WebView height: {ex.Message}");
    //         }
    //     }
    // }
    private async void WebView_Navigating(object sender, WebNavigatingEventArgs e)
    {
        if (e.Url.StartsWith("http") || e.Url.StartsWith("https"))
        {
            e.Cancel = true;
            await Launcher.OpenAsync(e.Url);
        }
    }
    private void AlignButtonText(Button button)
    {
        if (button.Handler == null)
            return;
#if ANDROID
        var androidButton = button.Handler.PlatformView as Google.Android.Material.Button.MaterialButton;
        if (androidButton != null)
        {
            androidButton.TextAlignment = Android.Views.TextAlignment.ViewStart;
        }
#elif IOS
        var iosButton = button.Handler.PlatformView as UIKit.UIButton;
        if (iosButton != null)
        {
            iosButton.HorizontalAlignment = UIKit.UIControlContentHorizontalAlignment.Left;
            iosButton.TitleLabel.TextAlignment = UIKit.UITextAlignment.Left;
        }
#endif
    }
}