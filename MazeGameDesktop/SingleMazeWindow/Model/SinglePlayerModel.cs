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
                    if (Maze[coords[1], coords[0]] == CellType.Free)
                    {
                        PlayerPosition = String.Format("{0}#{1}", coords[0], coords[1]);
                        UpdatePropInvoke("PlayerPosition");
                    }
                }
            }
        }

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

        private void ServerUpdate(object sender, PropertyChangedEventArgs e)
        {
            if (!e.PropertyName.Contains("ErrorType"))
            {
                JObject parse = JObject.Parse(e.PropertyName);
                Solution = parse["Solution"].ToString();
                UpdatePropInvoke("Solution");
            }
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

        private void UpdatePropInvoke(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void Close()
        {
            client?.stop();
        }
    }
}
