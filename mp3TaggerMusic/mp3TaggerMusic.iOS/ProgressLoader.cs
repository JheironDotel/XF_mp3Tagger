using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using mp3TaggerMusic.Intefaces;
using BigTed;
using mp3TaggerMusic.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(ProgressLoader))]
namespace mp3TaggerMusic.iOS
{
    public class ProgressLoader : IProgressInterface
    {
        public ProgressLoader()
        {
        }

        public void Hide()
        {
            BTProgressHUD.Dismiss();
        }

        public void Show(string title = "Loading")
        {
            BTProgressHUD.Show(title, maskType: ProgressHUD.MaskType.Black);
        }
    }
}