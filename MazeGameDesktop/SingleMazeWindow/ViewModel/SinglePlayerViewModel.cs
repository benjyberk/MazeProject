using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;
using System.Windows.Input;
using System.Diagnostics;
using System.ComponentModel;
using MazeGameDesktop.SingleMazeWindow.Model;
using Newtonsoft.Json.Linq;
using System.Windows;

namespace MazeGameDesktop.SingleMazeWindow.ViewModel
{
    public class SinglePlayerViewModel : ISinglePlayerViewModel, INotifyPropertyChanged
    {

        private bool ended;
        public Maze Maze
        {
            get
            {
                return Model.Maze;
            }
        }
        public string MazeString {
            get
            {
                return Model.MazeString;
            }
        }
        public string StartPos {
            get
            {
                return Model.StartPos;
            }
        }

        public string EndPos {
            get
            {
                return Model.EndPos;
            }
        }

        public string PlayerPosition {
            get
            {
                return Model.PlayerPosition;
            }
        }
        private ISinglePlayerModel Model;

        public SinglePlayerViewModel(ISinglePlayerModel model)
        {
            ended = false;
            this.Model = model;
            model.PropertyChanged += ModelUpdate;
        }

        private void SolutionUpdate()
        {
            List<int> coords = Model.TryGetValues(StartPos);
            if (Model.Solution != "-1")
            {
                Model.PlayerPosition = StartPos;
                foreach (char instruction in Model.Solution)
                {
                    System.Threading.Thread.Sleep(250);
                    if (instruction == '0')
                        coords[0] = coords[0] - 1;
                    else if (instruction == '1')
                        coords[0] = coords[0] + 1;
                    else if (instruction == '2')
                        coords[1] = coords[1] - 1;
                    else if (instruction == '3')
                        coords[1] = coords[1] + 1;
                    Model.PlayerPosition = String.Format("{0}#{1}", coords[0], coords[1]);
                }
            } else
            {
                ServerError?.Invoke();
            }
        }

        private void ModelUpdate(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);

            if (PlayerPosition == EndPos)
            {
                if (!ended)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        EndEvent?.Invoke();
                    });
                    Model.PropertyChanged -= ModelUpdate;
                    ended = true;
                }
            }
            if (e.PropertyName == "Solution")
            {
                SolutionUpdate();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EndFunction EndEvent;
        public event EndFunction ServerError;

        public void HandleKey(object sender, KeyEventArgs e)
        {
            Model.HandleKey(sender, e);
        }

        public void SolveClicked()
        {
            Model.GetSolution();
        }

        public void CloseOperation()
        {
            Model.PropertyChanged -= ModelUpdate;
            Model.Close();
        }

        public void ResetOperation()
        {
            Model.PlayerPosition = StartPos;
        }
    }
}
