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

        /// <summary>
        /// The constructor initalizes the VM datacontext and sets up events
        /// for return messages from the server
        /// </summary>
        /// <param name="vm"></param>
        public MultiplayerMazeView(IMultiplayerViewModel vm)
        {
            InitializeComponent();
            this.vm = vm;
            this.DataContext = vm;
            vm.EndEvent += MazeEndReached;
            vm.ServerError += ServerError;
            vm.MazeLoaded += MazeLoaded;
        }

        /// <summary>
        /// The ServerError function runs when the server has closed,
        /// it presents a MessageBox error and closes the window
        /// </summary>
        private void ServerError()
        {
            string message = "Connection lost with server\n Either the other player left the game, or the server couldn't be reached";
            System.Windows.MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }

        /// <summary>
        /// When the maze end is reached by one player (specified by the bool) a message
        /// is shown and the window is closed
        /// </summary>
        /// <param name="whoWon"></param>
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

        /// <summary>
        /// When the maze is loaded, we start routing keypress events to the vm
        /// and hide the waiting screen
        /// </summary>
        private void MazeLoaded()
        {
            this.KeyDown += vm.HandleKey;
            LoadingBox.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// When the 'exit' button is clicked we close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.CloseOperation();
            Close();
        }

        /// <summary>
        /// When the user presses X, we close the VM (and model)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            vm.CloseOperation();
        }
    }
}
