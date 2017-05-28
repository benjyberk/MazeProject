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

        public event NoParams ServerError;
        public event PropertyChangedEventHandler PropertyChanged;
        public event MazeEnd EndEvent;
        public event NoParams MazeLoaded;

        private IMultiplayerWindowModel Model;

        public MultiplayerViewModel(IMultiplayerWindowModel model)
        {
            this.Model = model;
            Model.PropertyChanged += UpdateProperty;
            Model.ServerUpdateEvent += HandleServerMessage;
        }


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
            else if (update.Contains("Maze"))
            {
                Maze m = Maze.FromJSON(update);
                Model.Maze = m;
            }
            else
            {

                JObject parse = JObject.Parse(update);
                MoveEnemy(parse["Direction"].ToString());

            }
        }

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
                Model.EnemyPosition = String.Format("{0}#{1}", coords[0], coords[1]);
                UpdateProperty(this, new PropertyChangedEventArgs("EnemyPosition"));
            }
        }

        private void UpdateProperty(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);

            if (e.PropertyName == "LocalPosition" && LocalPosition == EndPos)
            {
                Model.ServerUpdateEvent -= HandleServerMessage;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    EndEvent?.Invoke(true);
                });
                Model.Close();
            } 
            else if (e.PropertyName == "EnemyPosition" && EnemyPosition == EndPos)
            {
                Model.ServerUpdateEvent -= HandleServerMessage;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    EndEvent?.Invoke(false);
                });
                Model.Close();
            }
            else if (e.PropertyName == "Maze")
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MazeLoaded?.Invoke();
                });
            }
        }

        public void CloseOperation()
        {
            Model.ServerUpdateEvent -= HandleServerMessage;
            Model.Close();
        }

        public void HandleKey(object sender, KeyEventArgs e)
        {
            if (Maze != null)
            {
                Model.HandleKey(sender, e);
            }
        }
    }
}
