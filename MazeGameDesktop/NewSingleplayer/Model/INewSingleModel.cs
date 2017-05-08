using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGameDesktop.NewSingleplayer.Model
{
    public interface INewSingleModel : INotifyPropertyChanged
    {
        int Rows { set; get; }
        int Columns { set; get; }
        void UpdateDefaultValues();
    }
}
