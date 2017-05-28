using MazeGameDesktop.SingleMazeWindow.ViewModel;
using MazeLib;
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

namespace MazeGameDesktop.SingleMazeWindow.View
{
    /// <summary>
    /// Interaction logic for SingleMazeView.xaml
    /// </summary>
    public partial class SingleMazeView : Window
    {
        private ISinglePlayerViewModel vm;

        /// <summary>
        /// The constructor connects to the view model via events and initializes
        /// values
        /// </summary>
        /// <param name="vm"></param>
        public SingleMazeView(ISinglePlayerViewModel vm)
        {
            InitializeComponent();
            this.vm = vm;
            this.DataContext = vm;
            this.KeyDown += vm.HandleKey;
            vm.EndEvent += MazeEndReached;
            vm.ServerError += ServerError;
        }

        /// <summary>
        /// The function to be run when the user reaches the end of the maze
        /// </summary>
        public void MazeEndReached()
        {
            string message = "Congratulations! You finished the maze!";
            System.Windows.MessageBox.Show(message, "You Win", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
            vm.EndEvent -= MazeEndReached;
        }

        /// <summary>
        /// The function run when a server error is encountered
        /// </summary>
        public void ServerError()
        {
            string message = "Error connecting to the server. Cannot load Solution. You can finish the maze though.";
            System.Windows.MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Passes the 'solve' button click on to the vm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Solve_Click(object sender, RoutedEventArgs e)
        {
            vm.SolveClicked();
        }

        /// <summary>
        /// Passes the 'restart' button click on to the vm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            string msg = "Do you really want to restart the maze?";
            string title = "Confirm";
            MessageBoxResult res = System.Windows.MessageBox.Show(msg, title, MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (res == MessageBoxResult.OK)
            {
                vm.ResetOperation();
            }
        }

        /// <summary>
        /// Passes the 'go back' button click onto the vm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            string msg = "Do you really want to exit?";
            string title = "Confirm";
            MessageBoxResult res = System.Windows.MessageBox.Show(msg, title, MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (res == MessageBoxResult.OK)
            {
                vm.CloseOperation();
                Close();
            }
        }
    }
}
