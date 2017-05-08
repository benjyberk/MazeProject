using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MazeGameDesktop.NewSingleplayer.ViewModel
{
    public interface INewSingleViewModel : INotifyPropertyChanged
    {
        int Rows { get; set; }
        int Columns { get; set; }
        bool Close { set; get; }
        void StartGameClicked(object sender, RoutedEventArgs e);
    }
}
