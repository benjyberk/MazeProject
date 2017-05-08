using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGameDesktop.Settings
{
    public interface ISettingsModel
    {
        string IP { get; set; }
        string Port { get; set; }
        int DefaultRows { get; set; }
        int DefaultColumns { get; set; }
        int Algorithm { get; set; }

        void SaveSettings();
    }
}
