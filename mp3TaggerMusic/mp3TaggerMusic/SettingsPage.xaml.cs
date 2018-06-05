using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mp3TaggerMusic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            var app = Application.Current as App;
            switchInfoMissing.IsToggled = app.OnlyCompleteMissingInfo;
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var app = Application.Current as App;
            app.OnlyCompleteMissingInfo = e.Value;
            Application.Current.SavePropertiesAsync();
        }
    }
}