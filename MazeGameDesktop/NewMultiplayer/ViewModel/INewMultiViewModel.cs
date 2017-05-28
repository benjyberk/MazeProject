using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MazeGameDesktop.NewMultiplayer.ViewModel
{
    public delegate void CloseFunc(bool error, string reason);
    public interface INewMultiViewModel : INotifyPropertyChanged
    {
        event CloseFunc CloseEvent;
        int Rows { get; set; }
        int Columns { get; set; }
        string Name { get; set; }
        bool Close { set; get; }
        ObservableCollection<string> GameList { get; set; }
        void StartGameClicked(object sender, RoutedEventArgs e);
        void JoinGameClicked(string name);

    }
}
