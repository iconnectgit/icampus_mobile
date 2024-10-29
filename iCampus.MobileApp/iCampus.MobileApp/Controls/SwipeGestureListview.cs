namespace iCampus.MobileApp.Controls;

public class SwipeGestureListview : ListView
{
    public event EventHandler SwipeRight;
    public void OnSwipeRight() =>
        SwipeRight?.Invoke(this, EventArgs.Empty);

    public event EventHandler SwipeLeft;
    public void OnSwipeLeft() =>
        SwipeLeft?.Invoke(this, EventArgs.Empty);

    public void ForceNativeTableUpdate()
    {
        ViewCellSizeChangedEvent?.Invoke();
    }

    public event Action ViewCellSizeChangedEvent;
}