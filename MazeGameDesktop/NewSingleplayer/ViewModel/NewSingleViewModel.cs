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
using System.Windows.Threading;
using MazeGameDesktop.SingleMazeWindow.View;
using MazeLib;
using MazeGameDesktop.SingleMazeWindow.ViewModel;
using MazeGameDesktop.SingleMazeWindow.Model;

namespace MazeGameDesktop.NewSingleplayer.ViewModel
{
    /// <summary>
    /// The ViewModel handles communication between the view and the model
    /// </summary>
    class NewSingleViewModel : INewSingleViewModel
    {
        private INewSingleModel model;

        bool Open;

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

        public bool Close
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event CloseFunc CloseEvent;

        /// <summary>
        /// The constructor connects to the model and initializes needed values
        /// </summary>
        /// <param name="model"></param>
        public NewSingleViewModel(INewSingleModel model)
        {
            Name = "Default Name";
            Open = true;
            Close = false;
            this.model = model;
            model.PropertyChanged += ModelUpdated;
            model.UpdateDefaultValues();
        }

        /// <summary>
        /// A helper function used to update on property changes
        /// </summary>
        /// <param name="e"></param>
        private void UpdateListeners(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Handles model updates, parsing the message and adjusting the view as needed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                // If Rows or Columns wasn't updated, it means a server update was received
                else
                {
                    model.PropertyChanged -= ModelUpdated;
                    JObject parse = JObject.Parse(e.PropertyName);
                    // If an error is found, we close the model and the view
                    if (parse["ErrorType"] != null)
                    {
                        if (Open)
                        {
                            Open = false;

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                CloseEvent(true, parse["ErrorType"].ToString());
                            });
                            model.Stop();
                        }
                        // If the maze is given, we open the Maze window with the provided maze
                    } else if (parse["Maze"] != null)
                    {
                        if (Open)
                        {
                            Open = false;
                            // We close the 'options' menu and open the maze window
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                CloseEvent(false, "Got Maze");

                                Maze m = Maze.FromJSON(e.PropertyName);
                                SingleMazeView OpenMaze = new SingleMazeView(new SinglePlayerViewModel(new SinglePlayerModel(m)));
                                OpenMaze.Show();
                            });
                            model.Stop();
                        }
                    }
                }
            }
        }


        /// <summary>
        /// When start game is clicked the message is passed on to the model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void StartGameClicked(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Start game clicked!");
            model.GenerateMaze(Name.Replace(' ', '_'), Rows, Columns);
        }
    }
}
