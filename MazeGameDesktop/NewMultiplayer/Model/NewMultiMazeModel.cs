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
    class NewMultiMazeModel : INewMultiModel
    {
        public int Rows { set; get; }
        public int Columns { set; get; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event ServerUpdated ServerMessageEvent;

        private Client client;

        public NewMultiMazeModel()
        {
            client = new Client();
            client.PropertyChanged += ServerUpdate;
            //client.ReopenFlag = true;
            client.start();
        }

        private void ServerUpdate(object sender, PropertyChangedEventArgs e)
        {
            ServerMessageEvent?.Invoke(e.PropertyName);
        }

        private void ClientUpdated(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void StartMaze(string name, int rows, int cols)
        {
            name = name.Replace(' ', '_');
            string execute = String.Format("start {0} {1} {2}", name, rows, cols);
            OpenNewWindow(execute);
        }

        public void JoinGame(string name)
        {
            name = name.Replace(' ', '_');
            string execute = String.Format("join {0}", name);
            OpenNewWindow(execute);
        }

        private void OpenNewWindow(string execute)
        {
            MultiplayerWindowModel WindowModel = new MultiplayerWindowModel(execute);
            MultiplayerViewModel WindowVM = new MultiplayerViewModel(WindowModel);
            MultiplayerMazeView WindowView = new MultiplayerMazeView(WindowVM);
            WindowView.Show();
        }

        public void Stop()
        {
            client?.stop();
        }

        public void UpdateDefaultValues()
        {
            Rows = Properties.Settings.Default.DefaultRows;
            ClientUpdated("Rows");
            Columns = Properties.Settings.Default.DefaultCols;
            ClientUpdated("Columns");
        }

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
