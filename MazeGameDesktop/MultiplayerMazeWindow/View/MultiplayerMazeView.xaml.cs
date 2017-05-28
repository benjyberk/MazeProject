using MazeGameDesktop.MultiplayerMazeWindow.ViewModel;
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
using System.Windows.Shapes;

namespace MazeGameDesktop.MultiplayerMazeWindow.View
{
    /// <summary>
    /// Interaction logic for MultiplayerMazeView.xaml
    /// </summary>
    public partial class MultiplayerMazeView : Window
    {
        private IMultiplayerViewModel vm;

        public MultiplayerMazeView(IMultiplayerViewModel vm)
        {
            InitializeComponent();
            this.vm = vm;
            this.DataContext = vm;
            vm.EndEvent += MazeEndReached;
            vm.ServerError += ServerError;
            vm.MazeLoaded += MazeLoaded;
        }

        private void ServerError()
        {
            string message = "Connection lost with server\n Either the other player left the game, or the server couldn't be reached";
            System.Windows.MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }

        private void MazeEndReached(bool whoWon)
        {
            string message;
            if (whoWon)
            {
                message = "You won the maze race! Good Job!";
            }
            else
            {
                message = "Your opponent won the race! Better luck next time!";
            }
            System.Windows.MessageBox.Show(message, "Maze Finished", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
            vm.EndEvent -= MazeEndReached;
        }

        private void MazeLoaded()
        {
            this.KeyDown += vm.HandleKey;
            LoadingBox.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.CloseOperation();
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            vm.CloseOperation();
        }
    }
}
