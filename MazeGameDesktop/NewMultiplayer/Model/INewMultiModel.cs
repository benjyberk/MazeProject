using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGameDesktop.NewMultiplayer.Model
{
    public delegate void ServerUpdated(string update);
    interface INewMultiModel : INotifyPropertyChanged
    {
        event ServerUpdated ServerMessageEvent;
        int Rows { set; get; }
        int Columns { set; get; }
        void UpdateDefaultValues();
        void Stop();
        void StartMaze(string name, int rows, int cols);
        void JoinGame(string name);
        void GetList();
    }
}
