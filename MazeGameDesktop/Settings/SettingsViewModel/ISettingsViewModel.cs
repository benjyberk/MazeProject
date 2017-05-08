using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGameDesktop.Settings
{
    /// <summary>
    /// An Interface that defines the required properties and methods of the Settings window view-model
    /// </summary>
    public interface ISettingsViewModel
    {
        string IP { get; set; }
        string Port { get; set; }
        int DefaultRows { get; set; }
        int DefaultColumns { get; set; }
        int Algorithm { get; set; }

        void SaveSettings(SettingsWindow window);
        void Cancel(SettingsWindow window);
    }
}
