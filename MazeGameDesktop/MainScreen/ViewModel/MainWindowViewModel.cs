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
    /// <summary>
    /// The ViewModel for the main window handles initialization of the screens
    /// </summary>
    class MainWindowViewModel : IMainViewModel
    {
        /// <summary>
        /// The Multiplayer MVVM architecture is intialized and run
        /// </summary>
        public void OpenMultiPlayer()
        {
            INewMultiModel multiMod = new NewMultiMazeModel();
            INewMultiViewModel multiVM = new NewMultiViewModel(multiMod);
            NewMultiplayerWindow multi = new NewMultiplayerWindow(multiVM);
            multi.Show();
        }

        /// <summary>
        /// The Settings window is initialized and run (still in MVVM form,
        /// it simply generates its MVVM internally)
        /// </summary>
        public void OpenSettings()
        {
            SettingsWindow settings = new SettingsWindow();
            settings.Show();
        }

        /// <summary>
        /// The SinglePlayer selection window is initialized and run
        /// </summary>
        public void OpenSinglePlayer()
        {
            INewSingleModel newMod = new NewSingleModel();
            INewSingleViewModel newVm = new NewSingleViewModel(newMod);
            NewSingleplayerWindow single = new NewSingleplayerWindow(newVm);
            single.Show();
        }
    }
}
