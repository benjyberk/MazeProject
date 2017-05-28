using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MazeLib;
using System.Diagnostics;

namespace MazeGameDesktop.MultiplayerMazeWindow.Model
{
    class MultiplayerWindowModel : IMultiplayerWindowModel
    {
        public string EndPos
        {
            get; set;
        }

        public string EnemyPosition
        {
            get; set;
        }

        public string LocalPosition
        {
            get; set;
        }

        private Maze _Maze;
        public Maze Maze
        {
            get
            {
                return _Maze;
            }
            set
            {
                _Maze = value;
                UpdatePropInvoke("Maze");

                SetInitialValues();
            }
        }

        public string MazeString
        {
            get; set;
        }

        public string StartPos
        {
            get; set;
        }
        private string _MazeGameName;

        public event PropertyChangedEventHandler PropertyChanged;
        public event ServerUpdate ServerUpdateEvent;

        private Client Client;

        public MultiplayerWindowModel(string command_label)
        {
            _MazeGameName = null;
            this.Client = new Client();
            Client.PropertyChanged += SendUpdate;
            Client.start();
            RunStartCommand(command_label);
        }

        private void RunStartCommand(string command_label)
        {
            for (int i = 0; i < 10; i++)
            {
                if (Client.IsRunning())
                {
                    Client.sendData(command_label);
                    _MazeGameName = command_label.Split(' ')[1];
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(250);
                }
            }
        }

        private void SendUpdate(object sender, PropertyChangedEventArgs e)
        {
            ServerUpdateEvent?.Invoke(e.PropertyName);
        }

        private void SetInitialValues()
        {
            UpdatePropInvoke("Rows");
            UpdatePropInvoke("Columns");
            for (int i = 0; i < Maze.Rows; i++)
            {
                for (int j = 0; j < Maze.Cols; j++)
                {
                    if (Maze[i, j] == CellType.Free)
                    {
                        MazeString += "0";
                    }
                    else
                    {
                        MazeString += "1";
                    }
                }
            }
            UpdatePropInvoke("MazeString");
            StartPos = String.Format("{0}#{1}", Maze.InitialPos.Col, Maze.InitialPos.Row);
            UpdatePropInvoke("StartPos");
            EndPos = String.Format("{0}#{1}", Maze.GoalPos.Col, Maze.GoalPos.Row);
            UpdatePropInvoke("EndPos");
            LocalPosition = String.Format("{0}#{1}", Maze.InitialPos.Col, Maze.InitialPos.Row);
            UpdatePropInvoke("LocalPosition");
            EnemyPosition = String.Format("{0}#{1}", Maze.InitialPos.Col, Maze.InitialPos.Row);
            UpdatePropInvoke("EnemyPosition");
        }

        private void UpdatePropInvoke(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void Close()
        {
            Client?.sendData(String.Format("close {0}", _MazeGameName));
            Client?.stop();
        }

        public void HandleKey(object sender, KeyEventArgs e)
        {
            List<int> coords = TryGetValues(LocalPosition);
            if (coords != null)
            {
                Debug.WriteLine("Handling key in model");
                string move = "";
                if (e.Key == Key.Left) {
                    coords[0] = coords[0] - 1;
                    move = "left";
                }
                else if (e.Key == Key.Right) {
                    coords[0] = coords[0] + 1;
                    move = "right";
                }
                else if (e.Key == Key.Up) {
                    coords[1] = coords[1] - 1;
                    move = "up";
                }
                else if (e.Key == Key.Down) {
                    coords[1] = coords[1] + 1;
                    move = "down";
                }

                if (coords[0] >= 0 && coords[0] < Maze.Cols &&
                    coords[1] >= 0 && coords[1] < Maze.Rows)
                {
                    if (Maze[coords[1], coords[0]] == CellType.Free)
                    {
                        LocalPosition = String.Format("{0}#{1}", coords[0], coords[1]);
                        Client.sendData(String.Format("play {0}", move));
                        UpdatePropInvoke("LocalPosition");
                    }
                }
            }
        }

        public List<int> TryGetValues(string input)
        {
            if (input != null)
            {
                int x, y;
                string[] parse = input.Split('#');
                if (parse.Count() != 2 || !int.TryParse(parse[0], out x) || !int.TryParse(parse[1], out y))
                {
                    return null;
                }
                else
                {
                    List<int> returnList = new List<int>();
                    returnList.Add(x);
                    returnList.Add(y);
                    return returnList;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
