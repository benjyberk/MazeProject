using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MazeLib;
using MazeGameDesktop.MultiplayerMazeWindow.Model;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Windows;

namespace MazeGameDesktop.MultiplayerMazeWindow.ViewModel
{
    /// <summary>
    /// The VM for the multiplayer window keeps track of the game process and
    /// forwards/translates messages from the View to the Model and back
    /// </summary>
    class MultiplayerViewModel : IMultiplayerViewModel
    {
        public string EndPos
        {
            get
            {
                return Model.EndPos;
            }
        }

        public string EnemyPosition
        {
            get
            {
                return Model.EnemyPosition;
            }
        }

        public string LocalPosition
        {
            get
            {
                return Model.LocalPosition;
            }
        }

        public Maze Maze
        {
            get
            {
                return Model.Maze;
            }
        }

        public string MazeString
        {
            get
            {
                return Model.MazeString;
            }
        }

        public string StartPos
        {
            get
            {
                return Model.StartPos;
            }
        }

        // When a server error occurs, the VM updates the view
        public event NoParams ServerError;
        public event PropertyChangedEventHandler PropertyChanged;
        public event MazeEnd EndEvent;
        // When the maze is successfully loaded, the game is 'activated'
        public event NoParams MazeLoaded;

        private IMultiplayerWindowModel Model;

        /// <summary>
        /// The constructor registers the VM to the model
        /// </summary>
        /// <param name="model"></param>
        public MultiplayerViewModel(IMultiplayerWindowModel model)
        {
            this.Model = model;
            Model.PropertyChanged += UpdateProperty;
            Model.ServerUpdateEvent += HandleServerMessage;
        }

        /// <summary>
        /// The messages from the server are parsed into their types
        /// and then DeSerialized from their json forms
        /// </summary>
        /// <param name="update"></param>
        private void HandleServerMessage(string update)
        {
            if (update.Contains("ErrorType"))
            {
                Model.Close();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ServerError?.Invoke();
                });
            }
            // Updating the maze in the model automatically updates the VM and the View
            else if (update.Contains("Maze"))
            {
                Maze m = Maze.FromJSON(update);
                Model.Maze = m;
            }
            // The only other message possible is a 'direction' message
            else if (update.Contains("Direction"))
            {
                JObject parse = JObject.Parse(update);
                MoveEnemy(parse["Direction"].ToString());
            }
        }

        /// <summary>
        /// A helper function used to parse the enemy movement into the required
        /// directional change
        /// </summary>
        /// <param name="direction"></param>
        private void MoveEnemy(string direction)
        {
            List<int> coords = Model.TryGetValues(EnemyPosition);
            if (coords != null) {
                if (direction == "up")
                {
                    coords[1] = coords[1] - 1;
                } else if (direction == "down")
                {
                    coords[1] = coords[1] + 1;
                }
                else if (direction == "left")
                {
                    coords[0] = coords[0] - 1;
                }
                else if (direction == "right")
                {
                    coords[0] = coords[0] + 1;
                }
                // When the direction is properly handled, the position is updated
                Model.EnemyPosition = String.Format("{0}#{1}", coords[0], coords[1]);
                UpdateProperty(this, new PropertyChangedEventArgs("EnemyPosition"));
            }
        }

        /// <summary>
        /// Used to handle model property updates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateProperty(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);

            // If the user reaches the end, end the game
            if (e.PropertyName == "LocalPosition" && LocalPosition == EndPos)
            {
                Model.ServerUpdateEvent -= HandleServerMessage;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    EndEvent?.Invoke(true);
                });
                Model.Close();
            } 
            // If the enemy reaches the end, end the game
            else if (e.PropertyName == "EnemyPosition" && EnemyPosition == EndPos)
            {
                Model.ServerUpdateEvent -= HandleServerMessage;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    EndEvent?.Invoke(false);
                });
                Model.Close();
            }
            // If the maze is update; enable the user inputs
            else if (e.PropertyName == "Maze")
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MazeLoaded?.Invoke();
                });
            }
        }

        /// <summary>
        /// Close the model and deregister from updates
        /// </summary>
        public void CloseOperation()
        {
            Model.ServerUpdateEvent -= HandleServerMessage;
            Model.Close();
        }

        /// <summary>
        /// Pass on function to the model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleKey(object sender, KeyEventArgs e)
        {
            if (Maze != null)
            {
                if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down)
                {
                    Model.HandleKey(sender, e);
                }
            }
        }
    }
}
