using MazeGameDesktop.Settings;
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

namespace MazeGameDesktop
{
    /// <summary>
    /// Interaction logic for SettingsWindows.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private ISettingsViewModel vm;

        /// <summary>
        /// The constructor initializes the ViewModel (and Model for the Viewmodel)
        /// as well as setting the data context
        /// </summary>
        public SettingsWindow()
        {
            InitializeComponent();
            ISettingsModel model = new SettingsModel();
            vm = new SettingsViewModel(model);
            this.DataContext = vm;
        }

        /// <summary>
        /// The function is passed on to the View Model
        /// </summary>
        /// <param name="sender">The object sending</param>
        /// <param name="e">The event arguments</param>
        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            vm.SaveSettings(this);
        }

        /// <summary>
        /// The function is passed on to the View Model
        /// </summary>
        /// <param name="sender">The object sending</param>
        /// <param name="e">The event arguments</param>
        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            vm.Cancel(this);
        }
    }
}
