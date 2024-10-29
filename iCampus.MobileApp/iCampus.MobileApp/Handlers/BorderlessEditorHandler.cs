using Microsoft.Maui.Handlers;
#if ANDROID
using Android.Graphics.Drawables;
using Android.Content;
#elif IOS
using UIKit;
#endif

namespace iCampus.MobileApp.Handlers;

public class BorderlessEditorHandler : EditorHandler
{
    public BorderlessEditorHandler()
    {
        Mapper.AppendToMapping("NoBorder", (handler, view) =>
        {
#if ANDROID
            if (handler.PlatformView is Android.Widget.EditText editText)
            {
                editText.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
            }
#elif IOS
            if (handler.PlatformView is UIKit.UITextView textView)
                textView.Layer.BorderWidth = 0;
#endif
        });
    }
}