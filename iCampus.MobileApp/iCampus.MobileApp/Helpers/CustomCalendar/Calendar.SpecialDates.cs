namespace iCampus.MobileApp.Helpers.CustomCalendar;

public partial class Calendar : ContentView
{

    #region SpecialDates

    public static readonly BindableProperty SpecialDatesProperty =
        BindableProperty.Create(nameof(SpecialDates), typeof(ICollection<SpecialDate>), typeof(Calendar), new List<SpecialDate>(),
            propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar).ChangeCalendar(CalandarChanges.MaxMin));

    public ICollection<SpecialDate> SpecialDates
    {
        get { return (ICollection<SpecialDate>)GetValue(SpecialDatesProperty); }
        set { SetValue(SpecialDatesProperty, value); }
    }

    #endregion

    public void RaiseSpecialDatesChanged()
    {
        ChangeCalendar(CalandarChanges.MaxMin);
    }

    protected void SetButtonSpecial(CalendarButton button, SpecialDate special)
    {
        Device.BeginInvokeOnMainThread(() =>
        {
            // button.BackgroundPattern = special.BackgroundPattern;
            // button.BackgroundImage = special.BackgroundImage;
            // if (special.FontSize.HasValue) button.FontSize = special.FontSize.Value;
            // if (special.BorderWidth.HasValue) button.BorderWidth = special.BorderWidth.Value;
            // if (special.BorderColor.ValueHasValue) button.BorderColor = special.BorderColor;
            // if (special.BackgroundColor.HasValue) button.BackgroundColor = special.BackgroundColor;
            // if (special.TextColor.HasValue) button.TextColor = special.TextColor;
            // if (special.FontAttributes.HasValue) button.FontAttributes = special.FontAttributes.Value;
            // if (!string.IsNullOrEmpty(special.FontFamily)) button.FontFamily = special.FontFamily;
            // button.IsEnabled = special.Selectable;
        });
    }
}