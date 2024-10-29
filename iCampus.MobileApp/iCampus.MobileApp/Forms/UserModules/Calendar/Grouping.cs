using System.Collections.ObjectModel;

namespace iCampus.MobileApp.Forms.UserModules.Calendar;

public class Grouping<K, T> : ObservableCollection<T>
{
    public K Key { get; private set; }
    public T ParentItem { get; private set; }
    public int ChildListItemCount { get; private set; }
    public Grouping(K key, T parent, IEnumerable<T> items)
    {
        Key = key;
        ParentItem = parent;
        ChildListItemCount = items.Count();
        foreach (var item in items)
        {
            this.Items.Add(item);
        }
               
    }
}