﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MazeGameDesktop.NewSingleplayer.ViewModel
{
    public delegate void CloseFunc(bool error, string reason);
    public interface INewSingleViewModel : INotifyPropertyChanged
    {
        event CloseFunc CloseEvent;
        int Rows { get; set; }
        int Columns { get; set; }
        string Name { get; set; }
        bool Close { set; get; }
        void StartGameClicked(object sender, RoutedEventArgs e);
    }
}
