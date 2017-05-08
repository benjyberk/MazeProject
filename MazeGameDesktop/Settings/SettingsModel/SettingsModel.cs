using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGameDesktop.Settings
{
    /// <summary>
    /// The settings model practically handles the handover between saved variables and the
    /// properties file
    /// </summary>
    class SettingsModel : ISettingsModel
    {
        /// <summary>
        /// Below is the list of public properties used for data connecting between ViewModel and Model
        /// </summary>
        private int _Algorithm;
        public int Algorithm
        {
            get
            {
                return _Algorithm;
            }
            set
            {
                _Algorithm = value;
            }
        }

        private int _DefaultColumns;
        public int DefaultColumns
        {
            get
            {
                return _DefaultColumns;
            }
            set
            {
                _DefaultColumns = value;
            }
        }

        private int _DefaultRows;
        public int DefaultRows
        {
            get
            {
                return _DefaultRows;
            }
            set
            {
                _DefaultRows = value;
            }
        }

        private string _IP;
        public string IP
        {
            get
            {
                return _IP;
            }
            set
            {
                _IP = value;
            }
        }

        private string _Port;
        public string Port
        {
            get
            {
                return _Port;
            }
            set
            {
                _Port = value;
            }
        }

        /// <summary>
        /// The constructor is used to initialize values
        /// </summary>
        public SettingsModel()
        {
            Algorithm = Properties.Settings.Default.Algorithm;
            DefaultRows = Properties.Settings.Default.DefaultRows;
            DefaultColumns = Properties.Settings.Default.DefaultCols;
            Port = Properties.Settings.Default.Port;
            IP = Properties.Settings.Default.IP;

        }

        /// <summary>
        /// The settings held in private variables are saved in the Properties file.
        /// </summary>
        public void SaveSettings()
        {
            Properties.Settings.Default.Algorithm = _Algorithm;
            Properties.Settings.Default.DefaultCols = _DefaultColumns;
            Properties.Settings.Default.DefaultRows = _DefaultRows;
            Properties.Settings.Default.IP = _IP;
            Properties.Settings.Default.Port = _Port;

            // The changes are saved
            Properties.Settings.Default.Save();
        }
    }
}
