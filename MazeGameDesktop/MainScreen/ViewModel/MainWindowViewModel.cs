using MazeGameDesktop.NewMultiplayer.Model;
using MazeGameDesktop.NewMultiplayer.View;
using MazeGameDesktop.NewMultiplayer.ViewModel;
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
            INewMultiModel multiMod = new NewMultiMazeModel();
            INewMultiViewModel multiVM = new NewMultiViewModel(multiMod);
            NewMultiplayerWindow multi = new NewMultiplayerWindow(multiVM);
            multi.Show();
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
