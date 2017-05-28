using MazeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MazeGameDesktop.MultiplayerMazeWindow.ViewModel
{
    public delegate void NoParams();
    public delegate void MazeEnd(bool whoWon);
    public interface IMultiplayerViewModel : INotifyPropertyChanged
    {
        event MazeEnd EndEvent;
        event NoParams ServerError;
        event NoParams MazeLoaded;
        Maze Maze { get; }
        string MazeString { get; }
        string StartPos { get; }
        string EndPos { get; }
        string LocalPosition { get; }
        string EnemyPosition { get; }
        void HandleKey(object sender, KeyEventArgs e);
        void CloseOperation();
    }
}
