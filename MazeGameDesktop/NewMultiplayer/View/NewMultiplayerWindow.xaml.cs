using MazeGameDesktop.NewMultiplayer.ViewModel;
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

namespace MazeGameDesktop.NewMultiplayer.View
{
    /// <summary>
    /// Interaction logic for NewMultiplayerWindow.xaml
    /// </summary>
    public partial class NewMultiplayerWindow : Window
    {
        private INewMultiViewModel vm;

        /// <summary>
        /// The constructor sets the VM context
        /// </summary>
        /// <param name="vm"></param>
        public NewMultiplayerWindow(INewMultiViewModel vm)
        {
            this.DataContext = vm;
            this.vm = vm;
            vm.CloseEvent += CloseFunc;
            InitializeComponent();
            Form.DataContext = vm;
            Form.Start.Click += vm.StartGameClicked;
        }

        /// <summary>
        /// Closes the window with the appropriate error message
        /// in the event of server closure or successful maze end
        /// </summary>
        /// <param name="error"></param>
        /// <param name="reason"></param>
        public void CloseFunc(bool error, string reason)
        {
            if (error)
            {
                string errorMsg = String.Format("Error in connection with server!\n\nError was of Type {0}",
                    reason);
                MessageBox.Show(errorMsg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            try
            {
                Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Data);
            }
        }

        /// <summary>
        /// Joins the selected game using the VM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JoinGame(object sender, RoutedEventArgs e)
        {
            if (GameBox.SelectedItem != null)
            {
                vm.JoinGameClicked((string)GameBox.SelectedItem);
            }
        }
    }

}

