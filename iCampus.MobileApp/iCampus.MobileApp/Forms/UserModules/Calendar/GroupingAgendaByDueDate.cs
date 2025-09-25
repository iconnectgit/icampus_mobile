using System.Collections.ObjectModel;

namespace iCampus.MobileApp.Forms.UserModules.Calendar;

public class GroupingAgendaByDueDate<K, T> : ObservableCollection<T>
{
    public K Key { get; private set; }

    public GroupingAgendaByDueDate(K key, IEnumerable<T> items)
    {
        Key = key;
        foreach (var item in items)
            this.Items.Add(item);
    }
}
