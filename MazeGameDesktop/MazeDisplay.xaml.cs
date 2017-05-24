using MazeLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MazeGameDesktop
{
    /// <summary>
    /// Interaction logic for MazeDisplay.xaml
    /// </summary>
    public partial class MazeDisplay : UserControl
    {
        public Rectangle PlayerIcon;
        double width;
        double height;


        public MazeDisplay()
        {
            InitializeComponent();
            PlayerIcon = null;
        }

        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        public string PlayerPosition
        {
            get { return (string)GetValue(PlayerPositionProperty); }
            set { SetValue(PlayerPositionProperty, value); }
        }

        public string MazeString
        {
            get { return (string)GetValue(MazeStringProperty); }
            set { SetValue(MazeStringProperty, value); }
        }

        public string StartPos
        {
            get { return (string)GetValue(StartPosProperty); }
            set { SetValue(StartPosProperty, value); }
        }

        public string EndPos
        {
            get { return (string)GetValue(EndPosProperty); }
            set { SetValue(EndPosProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Rows.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register("Rows", typeof(int), typeof(MazeDisplay), new PropertyMetadata(0));

        // Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(int), typeof(MazeDisplay), new PropertyMetadata(0));

        // Using a DependencyProperty as the backing store for PlayerPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayerPositionProperty =
            DependencyProperty.Register("PlayerPosition", typeof(string), typeof(MazeDisplay), 
                new PropertyMetadata(OnPositionChange));

        // Using a DependencyProperty as the backing store for MazeString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MazeStringProperty =
            DependencyProperty.Register("MazeString", typeof(string), typeof(MazeDisplay), new PropertyMetadata("0"));

        // Using a DependencyProperty as the backing store for StartPos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartPosProperty =
            DependencyProperty.Register("StartPos", typeof(string), typeof(MazeDisplay), new PropertyMetadata("0#0"));

        // Using a DependencyProperty as the backing store for EndPos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndPosProperty =
            DependencyProperty.Register("EndPos", typeof(string), typeof(MazeDisplay), new PropertyMetadata("0#0"));

        private static void OnPositionChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MazeDisplay me = (MazeDisplay)d;
            me.DrawPlayerIcon();
        }


        private void MazeLoaded(object sender, RoutedEventArgs e)
        {
            width = (MazeSpace.Width / Columns);
            height = (MazeSpace.Height / Rows);
            Debug.WriteLine("Started constructing maze");
            // First Construct The Maze-board
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Width = width;
                    rect.Height = height;

                    char block = MazeString[(i * Columns) + j];
                    if (block == '0')
                    {
                        rect.Fill = new SolidColorBrush(Colors.White);
                    }
                    else
                    {
                        rect.Fill = new SolidColorBrush(Colors.Black);
                    }

                    MazeSpace.Children.Add(rect);
                    Canvas.SetTop(rect, i * height);
                    Canvas.SetLeft(rect, j * width);
                    Canvas.SetZIndex(rect, 1);
                    //Window.GetWindow(this).KeyDown += MovePlayer;
                }
            }
            // Now we draw the 'Start' and 'End' Points
            DrawStartEnd();
            // Now we draw the player icon
            DrawPlayerIcon();
        }

        private void DrawStartEnd()
        {
            width = (MazeSpace.Width / Columns);
            height = (MazeSpace.Height / Rows);

            List<int> coords;

            Rectangle start = new Rectangle();
            Rectangle end = new Rectangle();

            coords = TryGetValues(StartPos);
            if (coords != null)
            {
                start.Width = width;
                start.Height = height;
                start.Fill = new ImageBrush((BitmapImage)Resources["StartPicture"]);
                MazeSpace.Children.Add(start);
                Canvas.SetLeft(start, coords[0] * width);
                Canvas.SetTop(start, coords[1] * height);
                Canvas.SetZIndex(start, 2);
            }

            coords = TryGetValues(EndPos);
            if (coords != null)
            {
                end.Width = width;
                end.Height = height;
                end.Fill = new ImageBrush((BitmapImage)Resources["EndPicture"]);
                MazeSpace.Children.Add(end);
                Canvas.SetLeft(end, coords[0] * width);
                Canvas.SetTop(end, coords[1] * height);
                Canvas.SetZIndex(end, 2);
            }


        }

        private void DrawPlayerIcon()
        {
            List<int> coords;
            width = (MazeSpace.Width / Columns);
            height = (MazeSpace.Height / Rows);

            if (PlayerIcon == null)
            {
                PlayerIcon = new Rectangle();
                PlayerIcon.Height = height;
                PlayerIcon.Width = width;

                BitmapImage image = (BitmapImage)Resources["Player"];
                PlayerIcon.Fill = new ImageBrush(image);

                Canvas.SetZIndex(PlayerIcon, 3);
                MazeSpace.Children.Add(PlayerIcon);
            }

            coords = TryGetValues(PlayerPosition);

            if (coords == null)
            {
                return;
            }
            else
            {
                Canvas.SetLeft(PlayerIcon, coords[0] * width);
                Canvas.SetTop(PlayerIcon, coords[1] * height);
            }
        }

        private static List<int> TryGetValues(string input)
        {
            int x, y;
            string[] parse = input.Split('#');
            if (parse.Count() != 2 || !int.TryParse(parse[0], out x) || !int.TryParse(parse[1], out y))
            {
                return null;
            }
            else
            {
                List<int> returnList = new List<int>();
                returnList.Add(x);
                returnList.Add(y);
                return returnList;
            }
        }
    }
}
