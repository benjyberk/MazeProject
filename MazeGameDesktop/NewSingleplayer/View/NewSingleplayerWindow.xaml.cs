using MazeGameDesktop.MainScreen;
using MazeGameDesktop.NewSingleplayer.ViewModel;
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

namespace MazeGameDesktop.NewSingleplayer
{
    /// <summary>
    /// Interaction logic for NewSingleplayerWindow.xaml
    /// </summary>
    public partial class NewSingleplayerWindow : Window
    {
        /// <summary>
        /// The constructor sets the vm and attaches required events
        /// </summary>
        /// <param name="vm"></param>
        public NewSingleplayerWindow(INewSingleViewModel vm)
        {
            this.DataContext = vm;
            vm.CloseEvent += CloseFunc;
            InitializeComponent();
            Form.DataContext = vm;
            Form.Start.Click += vm.StartGameClicked;
        }

        /// <summary>
        /// A function that gets attatched to events to close the view
        /// </summary>
        /// <param name="error">True or false if there was an error</param>
        /// <param name="reason">The reason for closure</param>
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
    }
}
