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
            DependencyProperty.Register("Rows", typeof(int), typeof(MazeDisplay), new PropertyMetadata(1));

        // Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(int), typeof(MazeDisplay), new PropertyMetadata(1));

        // Using a DependencyProperty as the backing store for PlayerPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayerPositionProperty =
            DependencyProperty.Register("PlayerPosition", typeof(string), typeof(MazeDisplay), 
                new PropertyMetadata("-1#-1",OnPositionChange));

        // Using a DependencyProperty as the backing store for MazeString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MazeStringProperty =
            DependencyProperty.Register("MazeString", typeof(string), typeof(MazeDisplay), new PropertyMetadata("0", MazeUpdated));

        // Using a DependencyProperty as the backing store for StartPos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartPosProperty =
            DependencyProperty.Register("StartPos", typeof(string), typeof(MazeDisplay), new PropertyMetadata("0#0", StartPosUpdate));



        // Using a DependencyProperty as the backing store for EndPos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndPosProperty =
            DependencyProperty.Register("EndPos", typeof(string), typeof(MazeDisplay), new PropertyMetadata("0#0", EndPosUpdate));

        private static void StartPosUpdate(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MazeDisplay me = (MazeDisplay)d;
            me.DrawStartEnd(e.Property);
        }

        private static void EndPosUpdate(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MazeDisplay me = (MazeDisplay)d;
            me.DrawStartEnd(e.Property);
        }

        private static void OnPositionChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MazeDisplay me = (MazeDisplay)d;
            me.DrawPlayerIcon();
        }

        private static void MazeUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MazeDisplay me = (MazeDisplay)d;
            me.MazeLoaded(me, new RoutedEventArgs());
        }

        public void MazeLoaded(object sender, RoutedEventArgs e)
        {
            if (MazeString != null)
            {
                double ColorCount = 0;
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
                            rect.Fill = new SolidColorBrush(Colors.Black);
                        }
                        else
                        {
                            SolidColorBrush brush = new SolidColorBrush(HSL2RGB(ColorCount, 0.5, 0.5));
                            rect.Fill = brush;
                            rect.Stroke = brush;
                            rect.StrokeThickness = 1;
                            ColorCount += 0.005;
                            ColorCount = ColorCount % 1;
                        }

                        MazeSpace.Children.Add(rect);
                        Canvas.SetTop(rect, i * height);
                        Canvas.SetLeft(rect, j * width);
                        Canvas.SetZIndex(rect, 1);
                        int a = 5;
                        //Window.GetWindow(this).KeyDown += MovePlayer;
                    }
                }
            }
        }

        private void DrawStartEnd(DependencyProperty propName)
        {
            List<int> coords;
            ImageBrush image;
            if (propName.Name == "StartPos")
            {
                if (StartPos != null)
                {
                    coords = TryGetValues(StartPos);
                    image = new ImageBrush((BitmapImage)Resources["StartPicture"]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (EndPos != null)
                {
                    coords = TryGetValues(EndPos);
                    image = new ImageBrush((BitmapImage)Resources["EndPicture"]);
                }
                else
                {
                    return;
                }
            }
            width = (MazeSpace.Width / Columns);
            height = (MazeSpace.Height / Rows);

            Rectangle position = new Rectangle();

            if (coords != null)
            {
                position.Width = width;
                position.Height = height;
                position.Fill = image;
                MazeSpace.Children.Add(position);
                Canvas.SetLeft(position, coords[0] * width);
                Canvas.SetTop(position, coords[1] * height);
                Canvas.SetZIndex(position, 2);
            }
        }

        private void DrawPlayerIcon()
        {
            if (PlayerPosition != null)
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

        // Given H,S,L in range of 0-1
        // Returns a Color (RGB struct) in range of 0-255
        public static Color HSL2RGB(double h, double sl, double l)
        {
            double v;
            double r, g, b;

            r = l;   // default to gray
            g = l;
            b = l;
            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            byte R = Convert.ToByte(r * 255.0f);
            byte G = Convert.ToByte(g * 255.0f);
            byte B = Convert.ToByte(b * 255.0f);

            return Color.FromRgb(R,G,B);
        }

    }
}
