using MazeGameDesktop.NewMultiplayer.Model;
using MazeLib;
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
            model.UpdateDefaultValues();
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
                else
                {
                    model.PropertyChanged -= ModelUpdated;
                    JObject parse = JObject.Parse(e.PropertyName);
                    if (parse["ErrorType"] != null)
                    {
                        if (Open)
                        {
                            Open = false;

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                CloseEvent(true, parse["ErrorType"].ToString());
                            });
                        }
                    }
                    else if (parse["Maze"] != null)
                    {
                        if (Open)
                        {
                            Open = false;

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                CloseEvent(false, "Got Maze");

                                Maze m = Maze.FromJSON(e.PropertyName);
                                // Open the MultiMaze
                            });


                        }
                    }
                }
            }
        }

        public void StartGameClicked(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
