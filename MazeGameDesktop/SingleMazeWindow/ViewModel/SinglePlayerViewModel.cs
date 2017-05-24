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
                return model.Maze;
            }
        }
        public string MazeString {
            get
            {
                return model.MazeString;
            }
        }
        public string StartPos {
            get
            {
                return model.StartPos;
            }
        }

        public string EndPos {
            get
            {
                return model.EndPos;
            }
        }

        public string PlayerPosition {
            get
            {
                return model.PlayerPosition;
            }
        }
        private ISinglePlayerModel model;

        public SinglePlayerViewModel(ISinglePlayerModel model)
        {
            ended = false;
            this.model = model;
            model.PropertyChanged += ModelUpdate;
        }

        private void SolutionUpdate()
        {
            List<int> coords = model.TryGetValues(StartPos);
            if (model.Solution != "-1")
            {
                model.PlayerPosition = StartPos;
                foreach (char instruction in model.Solution)
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
                    model.PlayerPosition = String.Format("{0}#{1}", coords[0], coords[1]);
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
            model.HandleKey(sender, e);
        }

        public void SolveClicked()
        {
            model.GetSolution();
        }

        public void CloseOperation()
        {
            model.Close();
        }

        public void ResetOperation()
        {
            model.PlayerPosition = StartPos;
        }
    }
}
