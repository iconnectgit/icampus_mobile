using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iCampus.MobileApp.Forms;
using iCampus.MobileApp.Forms.UserModules.ReportCard;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Views.UserModules.ReportCard;

public partial class ReportCardPage : ContentPage
{
    public ReportCardPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<ReportCardSettingsView,bool>(this, "ReportCardData", (res,isStudentChange) =>
            {
                try
                {
                        // if (isStudentChange)
                        // {
                        //     if (beamReportCardTab.ItemSource.Count > 0)
                        //     {
                        //         if (beamReportCardTab.ItemSource.Count > 1)
                        //         {
                        //             beamReportCardTab.RemoveTab(1);
                        //         }
                        //         beamReportCardTab.RemoveTab(0);
                        //     }
                        //
                        //     beamReportCardTab.SelectedTabIndex = 0;
                        //
                        //     if (res.IsMarksReportCardEnabled)
                        //         beamReportCardTab.AddTab(beamMarkTab, 0, true);
                        //
                        //     if (res.IsSkillsReportCardEnabled)
                        //     {
                        //         beamReportCardTab.AddTab(beamSkillTab, beamReportCardTab.ItemSource.Count, true);
                        //     }
                        //
                        //     if (beamReportCardTab.ItemSource.Count >= 1)
                        //         beamReportCardTab.SelectFirst();
                        // }
                }
                catch(Exception ex)
                {

                }
            });
        }

        // private void reportCardTab_PositionChanged(object sender, Xam.Plugin.TabView.PositionChangedEventArgs e)
        // {
        //     try
        //     {
        //             MessagingCenter.Send<ReportCardForm, int>((ReportCardForm)this.BindingContext, "TabPosition", beamReportCardTab.SelectedTabIndex);
        //     }
        //     catch(Exception ex)
        //     {
        //
        //     }
        // }

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