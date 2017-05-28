using MazeGameDesktop.NewMultiplayer.Model;
using MazeLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MazeGameDesktop.NewMultiplayer.ViewModel
{
    class NewMultiViewModel : INewMultiViewModel
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public string Name { get; set; }
        public bool Close { set; get; }
        public ObservableCollection<string> GameList { get; set; }

        public event CloseFunc CloseEvent;
        public event PropertyChangedEventHandler PropertyChanged;

        private bool Open;
        private INewMultiModel model;

        public NewMultiViewModel(INewMultiModel model)
        {
            Name = "Default Name";
            Open = true;
            GameList = new ObservableCollection<string>();
            this.model = model;
            model.PropertyChanged += ModelUpdated;
            model.ServerMessageEvent += ServerEventHandle;
            model.UpdateDefaultValues();
            System.Threading.Thread.Sleep(250);
            model.GetList();
        }

        private void ServerEventHandle(string update)
        {
            update = update.Trim('\n');
            JObject parse = null;
            JArray array = null;

            if (update.StartsWith("["))
            {
                array = JArray.Parse(update);
            }
            else
            {
                parse = JObject.Parse(update);
            }

            if (parse != null && parse["ErrorType"] != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CloseEvent(true, parse["ErrorType"].ToString());
                });
                model.Stop();
            }
            else if (array != null)
            {
                List<string> ParseList = null;

                ParseList = JsonConvert.DeserializeObject<List<string>>(update);

                foreach (string unit in ParseList)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        GameList.Add(unit);
                    });
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GameList"));
                model.ServerMessageEvent -= ServerEventHandle;
            }
        }

        private void UpdateListeners(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        private void ModelUpdated(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine("Received Update: {0}", e.PropertyName, "");
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
            }
        }


        public void StartGameClicked(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CloseEvent(false, "");
            });
            model.StartMaze(Name, Rows, Columns);
        }

        public void JoinGameClicked(string inputName)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CloseEvent(false, "");
            });
            model.JoinGame(inputName);
        }
    }
}
