﻿using MazeGameDesktop.NewSingleplayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Windows.Threading;
using MazeGameDesktop.SingleMazeWindow.View;
using MazeLib;
using MazeGameDesktop.SingleMazeWindow.ViewModel;
using MazeGameDesktop.SingleMazeWindow.Model;

namespace MazeGameDesktop.NewSingleplayer.ViewModel
{
    class NewSingleViewModel : INewSingleViewModel
    {
        private INewSingleModel model;

        bool Open;

        public event PropertyChangedEventHandler PropertyChanged;
        public event CloseFunc CloseEvent;

        public NewSingleViewModel(INewSingleModel model)
        {
            Name = "Default Name";
            Open = true;
            Close = false;
            this.model = model;
            model.PropertyChanged += ModelUpdated;
            model.UpdateDefaultValues();
        }

        private void UpdateListeners(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        private void ModelUpdated(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine("Received Update: {0}", e.PropertyName, "");
            if (sender == model)
            {
                if (e.PropertyName == "Rows")
                {
                    Rows = model.Rows;
                    UpdateListeners(e);
                }

                else if (e.PropertyName == "Columns")
                {
                    Columns = model.Columns;
                    UpdateListeners(e);
                }
                else
                {
                    model.PropertyChanged -= ModelUpdated;
                    JObject parse = JObject.Parse(e.PropertyName);
                    if (parse["ErrorType"] != null)
                    {
                        if (Open)
                        {
                            Open = false;

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                CloseEvent(true, parse["ErrorType"].ToString());
                            });
                            model.Stop();
                        }
                    } else if (parse["Maze"] != null)
                    {
                        if (Open)
                        {
                            Open = false;

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                CloseEvent(false, "Got Maze");

                                Maze m = Maze.FromJSON(e.PropertyName);
                                SingleMazeView OpenMaze = new SingleMazeView(new SinglePlayerViewModel(new SinglePlayerModel(m)));
                                OpenMaze.Show();
                            });
                            model.Stop();
                        }
                    }
                }
            }
        }

        private int _Columns;
        public int Columns
        {
            get
            {
                return _Columns;
            }
            set
            {
                _Columns = value;
            }
        }

        private int _Rows;
        public int Rows
        {
            get
            {
                return _Rows;
            }
            set
            {
                _Rows = value;
            }
        }

        public bool Close
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public void StartGameClicked(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Start game clicked!");
            model.GenerateMaze(Name.Replace(' ', '_'), Rows, Columns);
        }
    }
}
