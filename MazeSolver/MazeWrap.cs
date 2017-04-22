using MazeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    public class MazeWrap : Maze
    {
        private string maze = "000011011011011011000011";
        public new int Rows {
            get;
            set;
        }
        public new int Cols
        {
            get;
            set;
        }

        public new Position InitialPos
        {
            get;
            set;
        }
        public new Position GoalPos
        {
            get;
            set;
        }
        public new string Name
        {
            get;
            set;
        }

        public MazeWrap()
        {
            Rows = 4;
            Cols = 6;
            Name = "A";
            InitialPos = new Position(3, 0);
            GoalPos = new Position(2, 3);
        }

        public new CellType this[int row, int col]
        {
            get
            {
                if (maze[row*Cols + col] == '0')
                {
                    return CellType.Free;
                }
                else
                {
                    return CellType.Wall;
                }
            }
        }

    }
}
