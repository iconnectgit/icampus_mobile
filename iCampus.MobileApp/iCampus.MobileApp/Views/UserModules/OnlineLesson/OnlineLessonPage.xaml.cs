using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCampus.MobileApp.Forms;
using iCampus.MobileApp.Forms.UserModules.OnlineLesson;
using iCampus.MobileApp.Forms.UserModules.OnlineLesson.TreeView;

namespace iCampus.MobileApp.Views.UserModules.OnlineLesson;

public partial class OnlineLessonPage : ContentPage
{
    public OnlineLessonPage()
    {
        InitializeComponent();
        MessagingCenter.Unsubscribe<OnlineLessonForm, Tuple<IEnumerable<TreeViewNode>, IEnumerable<TreeViewNode>>>("OnlineLessonForm", "StudentChanged");
        MessagingCenter.Subscribe<OnlineLessonForm, Tuple<IEnumerable<TreeViewNode>, IEnumerable<TreeViewNode>>>("OnlineLessonForm", "StudentChanged", (sender, subjectList) =>
        {
            if (subjectList != null)
            {
                TheTreeViewRequired.ClearTree();
                TheTreeViewElective.ClearTree();
                TheTreeViewRequired.RootNodes = subjectList.Item1?.ToList();
                TheTreeViewElective.RootNodes = subjectList.Item2?.ToList();
                
                TheTreeViewRequired.ForceLayout();
                TheTreeViewElective.ForceLayout();
                ForceLayout(); 
            }
        });
    }

    protected override bool OnBackButtonPressed()
    {
        var currentViewModel = BindingContext as ViewModelBase;

        if (currentViewModel != null)
        {
            currentViewModel.HandleMenuSelectionOnBack();
        }
        return true;
    }
}