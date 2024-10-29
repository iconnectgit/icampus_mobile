using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCampus.MobileApp.Forms.UserModules.ReportCard;

namespace iCampus.MobileApp.Views.UserModules.ReportCard;

public partial class ReportCardFilterPage : ContentPage
{
    public ReportCardFilterPage()
    {
        InitializeComponent();
    }
    protected override bool OnBackButtonPressed()
    {
        MessagingCenter.Send<ReportCardForm>((ReportCardForm)this.BindingContext, "ReportFilterBackClicked");
        return base.OnBackButtonPressed();
    }
}