using mp3TaggerMusic.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace mp3TaggerMusic
{
    public class BasePage : ContentPage
    {
        public BasePage()
        {          
        }

        public void Show()
        {
            DependencyService.Get<IProgressInterface>().Show();
        }

        public void Hide()
        {
            DependencyService.Get<IProgressInterface>().Hide();
        }
    }
}