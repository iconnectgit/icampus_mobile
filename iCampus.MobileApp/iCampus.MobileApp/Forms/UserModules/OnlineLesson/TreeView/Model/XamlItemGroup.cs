namespace iCampus.MobileApp.Forms.UserModules.OnlineLesson.TreeView.Model;

public class XamlItemGroup
{
    public List<XamlItemGroup> Children { get; } = new List<XamlItemGroup>();
    public List<XamlItem> XamlItems { get; } = new List<XamlItem>();
    public string Name { get; set; }
}

[Serializable]
public class XamlItem
{
    public string Key { get; set; }
}