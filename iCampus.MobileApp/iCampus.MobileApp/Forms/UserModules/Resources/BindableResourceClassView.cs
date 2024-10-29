using System.ComponentModel;

namespace iCampus.MobileApp.Forms.UserModules.Resources;

[Serializable]
public class BindableResourceClassView : INotifyPropertyChanged
{
    public BindableResourceClassView()
    {
    }
    public int ResourceClassId
    {
        get;
        set;
    }

    public int ClassId
    {
        get;
        set;
    }

    public int ResourceId
    {
        get;
        set;
    }

    public int GradeId
    {
        get;
        set;
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null)
            return;

        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}