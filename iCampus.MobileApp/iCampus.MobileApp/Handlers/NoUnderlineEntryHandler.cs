using iCampus.MobileApp.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace iCampus.MobileApp.Handlers;

public class NoUnderlineEntryHandler 
{
    public static void MapBorderlessEntry(IPropertyMapper mapper)
    {
        EntryHandler.Mapper.AppendToMapping(nameof(NoUnderlineEntry), (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.Background = null; // Removes border in Android
#elif IOS || MACCATALYST
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None; // Removes border in iOS
#endif
        });
    }
    

}