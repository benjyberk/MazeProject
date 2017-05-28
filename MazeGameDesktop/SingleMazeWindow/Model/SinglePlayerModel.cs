using MazeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace MazeGameDesktop.SingleMazeWindow.Model
{
    /// <summary>
    /// The Singleplayer Model handles passing on messages from
    /// the client to the VM for display
    /// </summary>
    class SinglePlayerModel : ISinglePlayerModel
    {

        private string playerPosition;
        public string PlayerPosition
        {
            get
            {
                return playerPosition;
            }
            set
            {
                playerPosition = value;
                UpdatePropInvoke("PlayerPosition");
            }
        }
        public Maze Maze { get; set; }
        public string MazeString { get; set; }
        public string StartPos { get; set; }
        public string EndPos { get; set; }
        public string Solution { get; set; }
        private Client client;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The constructor initializes and connects to the client
        /// as well as initializing properties
        /// </summary>
        /// <param name="m"></param>
        public SinglePlayerModel(Maze m)
        {
            Solution = null;
            client = new Client();
            client.PropertyChanged += ServerUpdate;
            client.start();

            this.Maze = m;
            Solution = null;
            SetMazeProperties();
        }

        /// <summary>
        /// A helper function that parses the provided maze to pass on to the
        /// VM and eventually View
        /// </summary>
        private void SetMazeProperties()
        {
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
            StartPos = String.Format("{0}#{1}", Maze.InitialPos.Col, Maze.InitialPos.Row);
            UpdatePropInvoke("StartPos");
            EndPos = String.Format("{0}#{1}", Maze.GoalPos.Col, Maze.GoalPos.Row);
            UpdatePropInvoke("EndPos");
            PlayerPosition = String.Format("{0}#{1}", Maze.InitialPos.Col, Maze.InitialPos.Row);
            UpdatePropInvoke("PlayerPosition");
        }

        /// <summary>
        /// Handles user input and parses it into practical movement updates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleKey(object sender, KeyEventArgs e)
        {
            List<int> coords = TryGetValues(PlayerPosition);
            if (coords != null)
            {
                Debug.WriteLine("Handling key in model");
                if (e.Key == Key.Left)
                    coords[0] = coords[0] - 1;
                else if (e.Key == Key.Right)
                    coords[0] = coords[0] + 1;
                else if (e.Key == Key.Up)
                    coords[1] = coords[1] - 1;
                else if (e.Key == Key.Down)
                    coords[1] = coords[1] + 1;

                if (coords[0] >= 0 && coords[0] < Maze.Cols &&
                    coords[1] >= 0 && coords[1] < Maze.Rows)
                {
                    // If the move is valid, the player position is updated
                    if (Maze[coords[1], coords[0]] == CellType.Free)
                    {
                        PlayerPosition = String.Format("{0}#{1}", coords[0], coords[1]);
                        UpdatePropInvoke("PlayerPosition");
                    }
                }
            }
        }

        /// <summary>
        /// A helper function used to translate strings of the form D#D into two coordinates
        /// (returned in array of ints form)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<int> TryGetValues(string input)
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

        /// <summary>
        /// Sends the solution request to the server
        /// </summary>
        public void GetSolution()
        {
            if (Solution == null)
            {
                if (client.IsRunning())
                {
                    string request = String.Format("solve {0} {1}", Maze.Name, Properties.Settings.Default.Algorithm);
                    client.sendData(request);
                }
            }
        }

        /// <summary>
        /// Update the VM on server update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerUpdate(object sender, PropertyChangedEventArgs e)
        {
            // If the client has received a non-error, it must be the solution
            if (!e.PropertyName.Contains("ErrorType"))
            {
                JObject parse = JObject.Parse(e.PropertyName);
                Solution = parse["Solution"].ToString();
                UpdatePropInvoke("Solution");
            }
            // Otherwise, we indicate that a server error has occured
            else
            {
                if (Solution == null)
                {
                    Solution = "-1";
                    UpdatePropInvoke("Solution");
                }
            }
            Debug.WriteLine(String.Format("Server Response: {0}", e.PropertyName));
        }

        /// <summary>
        /// A helper function to update property changes
        /// </summary>
        /// <param name="prop"></param>
        private void UpdatePropInvoke(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        /// <summary>
        /// Closes the client
        /// </summary>
        public void Close()
        {
            client?.stop();
        }
    }
}
