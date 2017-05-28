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
    /// <summary>
    /// The Multiplayer window model handles connection with the server, and forwards
    /// on messages.  It also parses user input when necessary
    /// </summary>
    class MultiplayerWindowModel : IMultiplayerWindowModel
    {
        /// <summary>
        /// The End Position of the maze
        /// </summary>
        public string EndPos
        {
            get; set;
        }

        /// <summary>
        /// The position of the enmy player
        /// </summary>
        public string EnemyPosition
        {
            get; set;
        }
        
        /// <summary>
        /// The position of the player at the host computer
        /// </summary>
        public string LocalPosition
        {
            get; set;
        }

        /// <summary>
        /// The Maze is held as private to avoid C# infinite get/set loops
        /// </summary>
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
                // Because this property is set multiple times, it is easier to Update here
                UpdatePropInvoke("Maze");

                SetInitialValues();
            }
        }

        /// <summary>
        /// The MazeString that represents the build of the maze in string form
        /// </summary>
        public string MazeString
        {
            get; set;
        }

        /// <summary>
        /// The starting position of the players
        /// </summary>
        public string StartPos
        {
            get; set;
        }

        /// <summary>
        /// A local representation of the maze game name (which is null when unset)
        /// </summary>
        private string _MazeGameName;

        /// <summary>
        /// The event that is triggered when a property change occurs
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// The event that is triggered when a server update occurs
        /// </summary>
        public event ServerUpdate ServerUpdateEvent;

        private Client Client;

        /// <summary>
        /// The constructor generates a client and runs the 'join' or 'start' command provided
        /// </summary>
        /// <param name="command_label"></param>
        public MultiplayerWindowModel(string command_label)
        {
            _MazeGameName = null;
            this.Client = new Client();
            // Register to updates from the client
            Client.PropertyChanged += SendUpdate;
            Client.start();
            RunStartCommand(command_label);
        }

        /// <summary>
        /// The 'start' or 'join' command is sent to the server as soon as the client connects
        /// 10 attempts are made, after which it is assumed the client will not connect
        /// and automatically a timeout message will be sent (from the client)
        /// </summary>
        /// <param name="command_label"></param>
        private void RunStartCommand(string command_label)
        {
            for (int i = 0; i < 10; i++)
            {
                if (Client.IsRunning())
                {
                    Client.sendData(command_label);
                    // Here the server holds a reference to the maze game in the
                    // event we must cancel before receiving a maze string from the server
                    _MazeGameName = command_label.Split(' ')[1];
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(250);
                }
            }
        }

        /// <summary>
        /// A shortcut method to send updates to listeners (the VM)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendUpdate(object sender, PropertyChangedEventArgs e)
        {
            ServerUpdateEvent?.Invoke(e.PropertyName);
        }

        /// <summary>
        /// The values are extracted from the received maze and placed into
        /// their necessary properties used for Data-Binding
        /// </summary>
        private void SetInitialValues()
        {
            UpdatePropInvoke("Rows");
            UpdatePropInvoke("Columns");
            // The MazeString is updated
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
            // All necessary properties are updated
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

        /// <summary>
        /// A shortcut method for sending updates
        /// </summary>
        /// <param name="prop"></param>
        private void UpdatePropInvoke(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        /// <summary>
        /// Used to close the client when the game ends
        /// If the client is already closed, nothing will happen
        /// </summary>
        public void Close()
        {
            Client?.sendData(String.Format("close {0}", _MazeGameName));
            Client?.stop();
        }

        /// <summary>
        /// Handles the logic for a passed on keypress from the VM
        /// Determines if the move is valid, and if so, updates the user and
        /// the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleKey(object sender, KeyEventArgs e)
        {
            List<int> coords = TryGetValues(LocalPosition);
            if (coords != null)
            {
                string move = "";
                // Basically a switch case to determine direction
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

                // If the key-handling turned out valid
                if (coords[0] >= 0 && coords[0] < Maze.Cols &&
                    coords[1] >= 0 && coords[1] < Maze.Rows)
                {
                    // If the maze location exists
                    if (Maze[coords[1], coords[0]] == CellType.Free)
                    {
                        // Update the position locally
                        LocalPosition = String.Format("{0}#{1}", coords[0], coords[1]);
                        // Send the information to the server
                        Client.sendData(String.Format("play {0}", move));
                        UpdatePropInvoke("LocalPosition");
                    }
                }
            }
        }

        /// <summary>
        /// A helper function used to turn a string of form X#X (where X are numbers)
        /// into an array of those two numbers.  If not possible, null is returned.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<int> TryGetValues(string input)
        {
            if (input != null)
            {
                int x, y;
                string[] parse = input.Split('#');
                // TryParse is used to check if the splits are valid.
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
