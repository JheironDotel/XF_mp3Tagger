using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mp3TaggerMusic.Intefaces
{
    public interface IProgressInterface
    {
        void Show(string title = "Loading");
        void Hide();
    }
}
