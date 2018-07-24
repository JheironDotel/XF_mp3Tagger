using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mp3TaggerMusic.Intefaces
{
    public interface IConnectivityChecker
    {
        bool DeviceHasInternet();
        bool DeviceHasWifi();
        bool DeviceHasRoaming();
    }
}
