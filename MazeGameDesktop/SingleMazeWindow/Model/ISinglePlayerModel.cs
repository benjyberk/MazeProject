using MazeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MazeGameDesktop.SingleMazeWindow.Model
{
    public interface ISinglePlayerModel : INotifyPropertyChanged
    {
        string PlayerPosition { get; set; }
        Maze Maze { get; set; }
        string MazeString { get; set; }
        string StartPos { get; set; }
        string EndPos { get; set; }
        string Solution { get; set; }
        void HandleKey(object sender, KeyEventArgs e);
        List<int> TryGetValues(string input);
        void GetSolution();
        void Close();
    }
}
