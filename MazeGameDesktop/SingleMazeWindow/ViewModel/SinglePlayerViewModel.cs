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
    /// <summary>
    /// The ViewModel passes on GUI user input from the view to the model, and returns
    /// Model server updates to the view.
    /// </summary>
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

        public event PropertyChangedEventHandler PropertyChanged;
        public event EndFunction EndEvent;
        public event EndFunction ServerError;

        /// <summary>
        /// The constructor sets to model and connects via events
        /// </summary>
        /// <param name="model"></param>
        public SinglePlayerViewModel(ISinglePlayerModel model)
        {
            ended = false;
            this.Model = model;
            model.PropertyChanged += ModelUpdate;
        }

        /// <summary>
        /// A helper function used to 'walk' the player from start to end using
        /// model updates and thread sleeps
        /// </summary>
        private void SolutionUpdate()
        {
            List<int> coords = Model.TryGetValues(StartPos);
            // If the solution is -1, there was an error receiving it from the server
            if (Model.Solution != "-1")
            {
                Model.PlayerPosition = StartPos;
                // We translate the solution into movement and update the position
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
                // If the position is -1 we return a server error
                ServerError?.Invoke();
            }
        }

        /// <summary>
        /// Pass on the property change notifications from the model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModelUpdate(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);

            // If the player reaches the end of the maze, we display a message and close
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
            // If the server loaded a solution, we begin the animation
            if (e.PropertyName == "Solution")
            {
                SolutionUpdate();
            }
        }


        /// <summary>
        /// We pass on key handling events to the model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down)
            {
                Model.HandleKey(sender, e);
            }
        }

        /// <summary>
        /// We pass on the solve click event to the model
        /// </summary>
        public void SolveClicked()
        {
            Model.GetSolution();
        }

        /// <summary>
        /// We pass on the close click to the model
        /// </summary>
        public void CloseOperation()
        {
            Model.PropertyChanged -= ModelUpdate;
            Model.Close();
        }

        /// <summary>
        /// We reset the location of the player
        /// </summary>
        public void ResetOperation()
        {
            Model.PlayerPosition = StartPos;
        }
    }
}
