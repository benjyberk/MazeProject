using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MazeGameDesktop.NewSingleplayer.Model
{
    /// <summary>
    /// The Single Player Window Model only handles providing the default values for Rows and Columns
    /// </summary>
    class NewSingleModel : INewSingleModel, INotifyPropertyChanged
    {
        /// <summary>
        /// The Properties use the 'Notifier' getters and setters
        /// </summary>
        private int _Columns;
        private MazeGameDesktop.Client client;

        public int Columns
        {
            set
            {
                _Columns = value;
                NotifyChanged("Columns");
            }
            get
            {
                return _Columns;
            }
        }

        /// <summary>
        /// The Properties use the 'Notifier' getters and setters
        /// </summary>
        private int _Rows;
        public int Rows
        {
            set
            {
                _Rows = value;
                NotifyChanged("Rows");
            }
            get
            {
                return _Rows;
            }
        }

        public NewSingleModel()
        {
            client = new MazeGameDesktop.Client();
            client.PropertyChanged += ClientUpdated;
            client.start();
        }

        public void UpdateDefaultValues()
        {
            Rows = Properties.Settings.Default.DefaultRows;
            Columns = Properties.Settings.Default.DefaultCols;
        }

        // The Event needed to implement the interface
        public event PropertyChangedEventHandler PropertyChanged;

        // A shortcut method for calling 'Property changed'
        private void NotifyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        
        private void ClientUpdated(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        public void Stop()
        {
            client?.stop();
        }

        public void GenerateMaze(string name, int rows, int cols)
        {
            if (client.IsRunning())
            {
                client.sendData(String.Format("generate {0} {1} {2}", name, rows, cols));
            } else
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Error.makeError("Socket Error")));
            }
        }
    }
}
