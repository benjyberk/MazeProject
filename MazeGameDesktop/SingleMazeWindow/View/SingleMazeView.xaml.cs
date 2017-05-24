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

        public SingleMazeView(ISinglePlayerViewModel vm)
        {
            InitializeComponent();
            this.vm = vm;
            this.DataContext = vm;
            this.KeyDown += vm.HandleKey;
            vm.EndEvent += MazeEndReached;
            vm.ServerError += ServerError;
        }

        public void MazeEndReached()
        {
            string message = "Congratulations! You finished the maze!";
            System.Windows.MessageBox.Show(message, "You Win", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
            vm.EndEvent -= MazeEndReached;
        }

        public void ServerError()
        {
            string message = "Error connecting to server. Cannot load Solution";
            System.Windows.MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Solve_Click(object sender, RoutedEventArgs e)
        {
            vm.SolveClicked();
        }

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
