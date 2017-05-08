using MazeGameDesktop.NewSingleplayer;
using MazeGameDesktop.NewSingleplayer.Model;
using MazeGameDesktop.NewSingleplayer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGameDesktop.MainScreen
{
    class MainWindowViewModel : IMainViewModel
    {
        public void OpenMultiPlayer()
        {
            throw new NotImplementedException();
        }

        public void OpenSettings()
        {
            SettingsWindow settings = new SettingsWindow();
            settings.Show();
        }

        public void OpenSinglePlayer()
        {
            INewSingleModel newMod = new NewSingleModel();
            INewSingleViewModel newVm = new NewSingleViewModel(newMod);
            NewSingleplayerWindow single = new NewSingleplayerWindow(newVm);
            single.Show();
        }
    }
}
