using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGameDesktop.MultiplayerMazeWindow.Model;
using MazeGameDesktop.MultiplayerMazeWindow.ViewModel;
using MazeGameDesktop.MultiplayerMazeWindow.View;

namespace MazeGameDesktop.NewMultiplayer.Model
{
    /// <summary>
    /// The VM handles passing messages from the View to the Model and eventually
    /// generates the multiplayer maze window
    /// </summary>
    class NewMultiMazeModel : INewMultiModel
    {
        public int Rows { set; get; }
        public int Columns { set; get; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event ServerUpdated ServerMessageEvent;

        private Client client;

        /// <summary>
        /// The constructor initializes the client
        /// </summary>
        public NewMultiMazeModel()
        {
            client = new Client();
            client.PropertyChanged += ServerUpdate;
            client.start();
        }

        /// <summary>
        /// Passes on a server update message to the view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerUpdate(object sender, PropertyChangedEventArgs e)
        {
            ServerMessageEvent?.Invoke(e.PropertyName);
        }

        /// <summary>
        /// Passes on a client update message to the view
        /// </summary>
        /// <param name="prop"></param>
        private void ClientUpdated(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        /// <summary>
        /// When the start maze button is clicked, the multi-maze window is started
        /// and given the parameters to send to the server
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public void StartMaze(string name, int rows, int cols)
        {
            name = name.Replace(' ', '_');
            string execute = String.Format("start {0} {1} {2}", name, rows, cols);
            OpenNewWindow(execute);
        }

        /// <summary>
        /// When the join game button is clicked the new window is created with
        /// the message to be sent to the server
        /// </summary>
        /// <param name="name"></param>
        public void JoinGame(string name)
        {
            name = name.Replace(' ', '_');
            string execute = String.Format("join {0}", name);
            OpenNewWindow(execute);
        }

        /// <summary>
        /// The function used to create the multiplayer maze window
        /// </summary>
        /// <param name="execute"></param>
        private void OpenNewWindow(string execute)
        {
            MultiplayerWindowModel WindowModel = new MultiplayerWindowModel(execute);
            MultiplayerViewModel WindowVM = new MultiplayerViewModel(WindowModel);
            MultiplayerMazeView WindowView = new MultiplayerMazeView(WindowVM);
            WindowView.Show();
        }

        /// <summary>
        /// Stops the client from running
        /// </summary>
        public void Stop()
        {
            client?.stop();
        }

        /// <summary>
        /// Gets the default values for the rows and columns
        /// </summary>
        public void UpdateDefaultValues()
        {
            Rows = Properties.Settings.Default.DefaultRows;
            ClientUpdated("Rows");
            Columns = Properties.Settings.Default.DefaultCols;
            ClientUpdated("Columns");
        }

        /// <summary>
        /// Attempts to get the list of open multiplayer games
        /// Runs in a loop to give the client time to connect to the server
        /// </summary>
        public void GetList()
        {
            for (int i = 0; i < 5; i++)
            {
                if (client.IsRunning())
                {
                    client.sendData("list");
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(250);
                }
            }
        }
    }
}
