using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MazeGameDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// The view does not require a model (but it has a viewmodel)
    /// </summary>
    public partial class MainWindow : Window
    {
        MainScreen.IMainViewModel vm;

        /// <summary>
        /// The constructor establishes the VM as a data-context
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            vm = new MainScreen.MainWindowViewModel();
            this.DataContext = vm;
        }

        /// <summary>
        /// Forward to the VM settings click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsButtonClick(object sender, RoutedEventArgs e)
        {
            vm.OpenSettings();
        }

        /// <summary>
        /// Forward to the VM single player click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SinglePlayerClick(object sender, RoutedEventArgs e)
        {
            vm.OpenSinglePlayer();
        }

        /// <summary>
        /// Forward to the VM multiplayer click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultiPlayerClick(object sender, RoutedEventArgs e)
        {
            vm.OpenMultiPlayer();
        }
    }
}
