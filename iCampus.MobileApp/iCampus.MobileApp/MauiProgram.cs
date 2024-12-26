using System.Collections.ObjectModel;
using System.Globalization;
using Akavache;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using CommunityToolkit.Maui;
using FFImageLoading.Maui;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.Controls;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms;
using iCampus.MobileApp.Forms.UserModules;
using iCampus.MobileApp.Forms.UserModules.Attendance;
using iCampus.MobileApp.Forms.UserModules.BooksReservation;
using iCampus.MobileApp.Forms.UserModules.Calendar;
using iCampus.MobileApp.Forms.UserModules.Communication;
using iCampus.MobileApp.Forms.UserModules.DailyMarks;
using iCampus.MobileApp.Forms.UserModules.DataCollection;
using iCampus.MobileApp.Forms.UserModules.Exam;
using iCampus.MobileApp.Forms.UserModules.FinancialStatus;
using iCampus.MobileApp.Forms.UserModules.OnlinePayment;
using iCampus.MobileApp.Forms.UserModules.Registration;
using iCampus.MobileApp.Forms.UserModules.Resources;
using iCampus.MobileApp.Forms.UserModules.Survey;
using iCampus.MobileApp.Handlers;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.Portal.EditModels;
using iCampus.Portal.ViewModels;
using Microcharts.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.LifecycleEvents;
using Mopups.Hosting;
using Newtonsoft.Json;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Splat;

namespace iCampus.MobileApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("fa-regular-400.ttf", "FontAwesomeRegular");
                fonts.AddFont("fa-solid-900.ttf", "FontAwesomeSolid");
                fonts.AddFont("fontawesome-webfont.ttf", "FontAwesomeClassic");
                fonts.AddFont("Montserrat-SemiBold.ttf", "MontserratSemiBold");
                fonts.AddFont("Montserrat-Light.ttf", "MontserratLight");
                fonts.AddFont("Montserrat-Medium.ttf", "MontserratMedium");
                fonts.AddFont("Montserrat-Regular.ttf", "MontserratRegular");
            })
            .UseMauiCommunityToolkit()
            .UseFFImageLoading()
            .UseSkiaSharp()
            .ConfigureMopups()
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<NoUnderlineEntry, EntryHandler>();
                NoUnderlineEntryHandler.MapBorderlessEntry(EntryHandler.Mapper);
                handlers.AddHandler<BorderlessEditor, BorderlessEditorHandler>();
            });
        
        DatePickerHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
        });
        EditorHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
        });

        PickerHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
        });
        
        BlobCache.ApplicationName = "iCampus.MobileApp";
        BlobCache.EnsureInitialized();
        BlobCache.ForcedDateTimeKind = DateTimeKind.Local;
        CultureInfo defaultCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
        CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

#if ANDROID
        Locator.CurrentMutable.RegisterConstant<INativeServices>(new AndroidNativeServices());
        Locator.CurrentMutable.RegisterConstant<IPrintService>(new AndroidPrintService());
#elif IOS       
        Locator.CurrentMutable.RegisterConstant<INativeServices>(new iOSNativeServices());
        Locator.CurrentMutable.RegisterConstant<IPrintService>(new iOSPrintService());
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}