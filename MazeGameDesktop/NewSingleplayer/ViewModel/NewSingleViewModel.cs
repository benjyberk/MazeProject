using MazeGameDesktop.NewSingleplayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace MazeGameDesktop.NewSingleplayer.ViewModel
{
    class NewSingleViewModel : INewSingleViewModel
    {
        private INewSingleModel model;
        private Client client;

        public event PropertyChangedEventHandler PropertyChanged;

        public NewSingleViewModel(INewSingleModel model)
        {
            Close = false;
            this.model = model;
            model.PropertyChanged += ModelUpdated;
            model.UpdateDefaultValues();
            client = new MazeGameDesktop.Client();
            client.PropertyChanged += ModelUpdated;
            client.start();
        }

        private void UpdateListeners(PropertyChangedEventArgs e)
        {
            Debug.WriteLine("Received update");
            PropertyChanged?.Invoke(this, e);
        }

        private void ModelUpdated(object sender, PropertyChangedEventArgs e)
        {
            if (sender == model)
            {
                if (e.PropertyName == "Rows")
                {
                    Rows = model.Rows;
                    UpdateListeners(e);
                }

                else if (e.PropertyName == "Columns")
                {
                    Columns = model.Columns;
                    UpdateListeners(e);
                }
                else
                {
                    JObject parse = JObject.Parse(e.PropertyName);
                    if (parse["ErrorType"] != null)
                    {
                        client.PropertyChanged -= ModelUpdated;
                        client.stop();
                        MessageBox.Show("Error in connection with server!");
                        Close = true;
                    }
                }
            }
        }

        public bool Close { set; get; }

        private int _Columns;
        public int Columns
        {
            get
            {
                return _Columns;
            }
            set
            {
                _Columns = value;
            }
        }

        private int _Rows;
        public int Rows
        {
            get
            {
                return _Rows;
            }
            set
            {
                _Rows = value;
            }
        }

        public void StartGameClicked(object sender, RoutedEventArgs e)
        {
            client.PropertyChanged -= ModelUpdated;
            client.stop();
            Close = true;
            Debug.WriteLine("Start game clicked!");
        }
    }
}
