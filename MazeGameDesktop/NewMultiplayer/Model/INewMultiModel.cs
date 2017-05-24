using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGameDesktop.NewMultiplayer.Model
{
    interface INewMultiModel : INotifyPropertyChanged
    {
        int Rows { set; get; }
        int Columns { set; get; }
        void UpdateDefaultValues();
        void Stop();
        void GenerateMaze(string name, int rows, int cols);
        void JoinGame(string name);
    }
}
