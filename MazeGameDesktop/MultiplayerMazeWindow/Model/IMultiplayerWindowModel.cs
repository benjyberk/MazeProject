using MazeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MazeGameDesktop.MultiplayerMazeWindow.Model
{
    public delegate void ServerUpdate(string update);
    interface IMultiplayerWindowModel : INotifyPropertyChanged
    {
        event ServerUpdate ServerUpdateEvent;
        string LocalPosition { get; set; }
        string EnemyPosition { get; set; }
        Maze Maze { get; set; }
        string MazeString { get; set; }
        string StartPos { get; set; }
        string EndPos { get; set; }
        void HandleKey(object sender, KeyEventArgs e);
        List<int> TryGetValues(string input);
        void Close();
    }
}
