#if ANDROID
using iCampus.MobileApp.Controls;
using Microsoft.Maui.Handlers;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace iCampus.MobileApp.Handlers;

public class SwipeGestureListviewHandler : ViewHandler<SwipeGestureListview, RecyclerView>
{
    private readonly CustomGestureListener _listener;
    private readonly GestureDetector _detector;

    public SwipeGestureListviewHandler(Context context) : base(PropertyMapper)
    {
        _listener = new CustomGestureListener();
        _detector = new GestureDetector(context, _listener);
    }

    protected override RecyclerView CreatePlatformView()
    {
        var recyclerView = new RecyclerView(Context);
        recyclerView.Touch += OnTouchEvent;
        return recyclerView;
    }


    private void OnTouchEvent(object sender, Android.Views.View.TouchEventArgs e)
    {
        _detector.OnTouchEvent(e.Event);
    }

    protected override void ConnectHandler(RecyclerView platformView)
    {
        base.ConnectHandler(platformView);

        _listener.OnSwipeLeft += (sender, e) => VirtualView?.OnSwipeLeft();
        _listener.OnSwipeRight += (sender, e) => VirtualView?.OnSwipeRight();
    }

    protected override void DisconnectHandler(RecyclerView platformView)
    {
        platformView.Touch -= OnTouchEvent;
        _listener.OnSwipeLeft -= (sender, e) => VirtualView?.OnSwipeLeft();
        _listener.OnSwipeRight -= (sender, e) => VirtualView?.OnSwipeRight();

        base.DisconnectHandler(platformView);
    }

    public static IPropertyMapper PropertyMapper =
        new PropertyMapper<SwipeGestureListview, SwipeGestureListviewHandler>(ViewMapper);
}

public class CustomGestureListener : GestureDetector.SimpleOnGestureListener
{
    private static readonly int SWIPE_THRESHOLD = 100;
    private static readonly int SWIPE_VELOCITY_THRESHOLD = 100;

    public event EventHandler OnSwipeLeft;
    public event EventHandler OnSwipeRight;

    public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
    {
        var diffX = e2.GetX() - e1.GetX();
        var diffY = e2.GetY() - e1.GetY();

        if (Math.Abs(diffX) > Math.Abs(diffY))
            if (Math.Abs(diffX) > SWIPE_THRESHOLD && Math.Abs(velocityX) > SWIPE_VELOCITY_THRESHOLD)
            {
                if (diffX > 0)
                    OnSwipeRight?.Invoke(this, EventArgs.Empty);
                else
                    OnSwipeLeft?.Invoke(this, EventArgs.Empty);
            }

        return base.OnFling(e1, e2, velocityX, velocityY);
    }
}

#elif IOS
using iCampus.MobileApp.Controls;
using Microsoft.Maui.Handlers;
using UIKit;
using CoreGraphics;
namespace iCampus.MobileApp.Handlers;
public class SwipeGestureListviewHandler : ViewHandler<SwipeGestureListview, UITableView>
    {
    public SwipeGestureListviewHandler() : base(PropertyMapper)
        {
        }

        protected override UITableView CreatePlatformView()
        {
            var tableView = new UITableView();

            // Check iOS version and remove section header top padding for iOS 15+
            if (UIDevice.CurrentDevice.CheckSystemVersion(15, 0))
            {
                tableView.SectionHeaderTopPadding = 0;
            }

            return tableView;
        }

        protected override void ConnectHandler(UITableView platformView)
        {
            base.ConnectHandler(platformView);

            // Connect event handlers and add swipe gestures
            VirtualView.ViewCellSizeChangedEvent += UpdateTableView;
            AddSwipeGestures(platformView);
        }

        private void UpdateTableView()
        {
            PlatformView.BeginUpdates();
            PlatformView.EndUpdates();
        }

        private void AddSwipeGestures(UITableView tableView)
        {
            var swipeRightGesture = new UISwipeGestureRecognizer(OnSwipeRight)
            {
                Direction = UISwipeGestureRecognizerDirection.Right
            };

            var swipeLeftGesture = new UISwipeGestureRecognizer(OnSwipeLeft)
            {
                Direction = UISwipeGestureRecognizerDirection.Left
            };

            tableView.AddGestureRecognizer(swipeRightGesture);
            tableView.AddGestureRecognizer(swipeLeftGesture);
        }

        private void OnSwipeRight()
        {
            VirtualView?.OnSwipeRight();
        }

        private void OnSwipeLeft()
        {
            VirtualView?.OnSwipeLeft();
        }

        protected override void DisconnectHandler(UITableView platformView)
        {
            base.DisconnectHandler(platformView);
            VirtualView.ViewCellSizeChangedEvent -= UpdateTableView;
        }
    public static IPropertyMapper PropertyMapper = new PropertyMapper<SwipeGestureListview, SwipeGestureListviewHandler>(ViewHandler.ViewMapper);
    }


#endif