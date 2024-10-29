using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCampus.MobileApp.Views.UserModules.Communication;

public partial class SendMessagePage : ContentPage
{
    public SendMessagePage()
    {
        InitializeComponent();
        MessagingCenter.Subscribe<string>(this, "SearchUnfocus", (res) =>
        {
            beamSearchText.Unfocus();
        });
    }
}