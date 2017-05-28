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
    /// <summary>
    /// The VM connects between the Model and the View relaying server commands
    /// and user selections
    /// </summary>
    class NewMultiViewModel : INewMultiViewModel
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public string Name { get; set; }
        public bool Close { set; get; }
        public ObservableCollection<string> GameList { get; set; }

        public event CloseFunc CloseEvent;
        public event PropertyChangedEventHandler PropertyChanged;

        private INewMultiModel model;

        /// <summary>
        /// The constructor initializes the necessary variables and hooks up
        /// events to functions in the model
        /// </summary>
        /// <param name="model"></param>
        public NewMultiViewModel(INewMultiModel model)
        {
            Name = "Default Name";
            GameList = new ObservableCollection<string>();
            this.model = model;
            model.PropertyChanged += ModelUpdated;
            model.ServerMessageEvent += ServerEventHandle;
            model.UpdateDefaultValues();
            // Gives the client time to connect before getting the list of games from the server
            System.Threading.Thread.Sleep(250);
            model.GetList();
        }

        /// <summary>
        /// Handles the update string from the server
        /// </summary>
        /// <param name="update"></param>
        private void ServerEventHandle(string update)
        {
            update = update.Trim('\n');
            JObject parse = null;
            JArray array = null;

            // Special handling to check if the returned value is a Json OBJECT or Json ARRAY
            if (update.StartsWith("["))
            {
                array = JArray.Parse(update);
            }
            else
            {
                parse = JObject.Parse(update);
            }

            // On error, we exit the game
            if (parse != null && parse["ErrorType"] != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CloseEvent(true, parse["ErrorType"].ToString());
                });
                model.Stop();
            }
            // If an array was read in, we place the array in the Observable Collection
            else if (array != null)
            {
                List<string> ParseList = null;

                ParseList = JsonConvert.DeserializeObject<List<string>>(update);

                foreach (string unit in ParseList)
                {
                    // Added by dispatcher to avoid thread conflicts
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        GameList.Add(unit);
                    });
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GameList"));
                model.ServerMessageEvent -= ServerEventHandle;
            }
        }

        /// <summary>
        /// We update the View that a property changed
        /// </summary>
        /// <param name="e"></param>
        private void UpdateListeners(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// When an update is received from the model we parse it and pass it on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            }
        }

        /// <summary>
        /// When the user clicks start game, the model is instructed to generate the
        /// new maze window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void StartGameClicked(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CloseEvent(false, "");
            });
            model.StartMaze(Name, Rows, Columns);
        }

        /// <summary>
        /// When the user clicks join game, the model is instructed to generate the
        /// new maze window
        /// </summary>
        /// <param name="inputName"></param>
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
