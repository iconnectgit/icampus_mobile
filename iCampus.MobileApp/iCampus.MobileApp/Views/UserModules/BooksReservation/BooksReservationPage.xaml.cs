using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Webkit;
using iCampus.MobileApp.Forms;
using iCampus.MobileApp.Forms.UserModules.BooksReservation;
using CheckBox = InputKit.Shared.Controls.CheckBox;

namespace iCampus.MobileApp.Views.UserModules.BooksReservation;

public partial class BooksReservationPage : ContentPage
{
    public BooksReservationPage()
    {
        InitializeComponent();
        // MessagingCenter.Subscribe<string>(this, "BooksListViewTopScroll", async(arg) =>
        // {
        //     await Task.Delay(1);
        //     await booksListView.ScrollToAsync(0, 0, true);
        // });
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
    void CheckBox_CheckChanged(object? sender, EventArgs e)
    {
        if (sender != null)
        {
            var checkBox = (CheckBox)sender;
            BindableStudentBookMasterView bindableStudentBookMasterView = new BindableStudentBookMasterView();
            bindableStudentBookMasterView.IsChecked = checkBox.IsChecked;
            bindableStudentBookMasterView.StudentBookMasterId = ((BindableStudentBookMasterView)checkBox.CommandParameter).StudentBookMasterId;
            MessagingCenter.Send<BindableStudentBookMasterView>(bindableStudentBookMasterView, "BooksSelection");
        }
    }

    void NoUnderlineEntry_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if(sender!=null)
        {
            Entry entry = (Entry)sender;
            BindableStudentBookMasterView bindableStudentBookMasterView = new BindableStudentBookMasterView();
            bindableStudentBookMasterView =(BindableStudentBookMasterView)entry.ReturnCommandParameter;
            if(bindableStudentBookMasterView!=null)
            {
                MessagingCenter.Send<BindableStudentBookMasterView>(bindableStudentBookMasterView, "UpdateAmountOnQuantityChanged");
            }
        }
    }
}