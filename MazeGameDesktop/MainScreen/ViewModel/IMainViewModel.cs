using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGameDesktop.MainScreen
{
    public interface IMainViewModel
    {
        void OpenSettings();
        void OpenSinglePlayer();
        void OpenMultiPlayer();
    }
}
