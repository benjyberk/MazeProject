﻿using MazeGameDesktop.MainScreen;
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
        private bool open;
        public NewSingleplayerWindow(INewSingleViewModel vm)
        {
            this.DataContext = vm;
            open = true; 
            vm.CloseEvent += CloseFunc;
            InitializeComponent();
            Form.DataContext = vm;
            Form.Start.Click += vm.StartGameClicked;
        }

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