using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGameDesktop.Settings
{
    /// <summary>
    /// The settings ViewModel mostly handles saving values to the Application Properties
    /// Note that this does not use 'INotifiesObservers' because the model does not update the ViewModel
    /// on its own accord, only in response to direct user input.
    /// </summary>
    class SettingsViewModel : ISettingsViewModel
    {
        private ISettingsModel model;

        /// <summary>
        /// Below are the list of public properties used for Data-Binding with the GUI
        /// </summary>
        public int Algorithm
        {
            get
            {
                return model.Algorithm;
            }
            set
            {
                model.Algorithm = value;
            }
        }

        public int DefaultColumns
        {
            get
            {
                return model.DefaultColumns;
            }
            set
            {
                model.DefaultColumns = value;
            }
        }

        public int DefaultRows
        {
            get
            {
                return model.DefaultRows;
            }
            set
            {
                model.DefaultRows = value;
            }
        }

        public string IP
        {
            get
            {
                return model.IP;
            }
            set
            {
                model.IP = value;
            }
        }

        public string Port
        {
            get
            {
                return model.Port;
            }
            set
            {
                model.Port = value;
            }
        }

        /// <summary>
        /// On initalization, the 'Settings' ViewModel loads in data from properties. 
        /// </summary>
        /// <param name="model"></param>
        public SettingsViewModel(ISettingsModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// The Model handles saving of the changed settings and closes the window
        /// </summary>
        /// <param name="window">The settings windows</param>
        public void SaveSettings(SettingsWindow window)
        {
            model.SaveSettings();
            window.Close();
        }

        /// <summary>
        /// The window is closed when cancel is pressed
        /// </summary>
        /// <param name="window">The settings window</param>
        public void Cancel(SettingsWindow window)
        {
            Properties.Settings.Default.Reload();
            window.Close();
        }
    }
}
