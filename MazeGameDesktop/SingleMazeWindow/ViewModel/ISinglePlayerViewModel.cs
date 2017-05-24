using MazeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MazeGameDesktop.SingleMazeWindow.ViewModel
{
    public delegate void EndFunction();
    public interface ISinglePlayerViewModel
    {
        event EndFunction EndEvent;
        event EndFunction ServerError;
        Maze Maze { get; }
        string MazeString { get; }
        string StartPos { get; }
        string EndPos { get; }
        string PlayerPosition { get; }
        void HandleKey(object sender, KeyEventArgs e);
        void SolveClicked();
        void CloseOperation();
        void ResetOperation();
    }
}
