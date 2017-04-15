using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchAlgorithmsLib;
using MazeLib;

namespace MazeSolver
{
    /*
     * The class adapts the Maze class to become 'Searchable' (Adapter design pattern).
     */
    public class SearchableMaze : ISearchable<Position>
    {
        private Maze maze;

        // The maze itself is given a parameter
        public SearchableMaze(Maze maze)
        {
            this.maze = maze;
        }


        /*
         * This private helper method generates the state object based on parameters
         */
        private State<Position> makeState(int row, int col, double cost, State<Position> pred)
        {
            Position p = new Position(row, col);
            State<Position> s = new State<Position>(p, cost);
            s.predecessor = pred;
            return s;
        }

        // The goal state of the maze is translated (from position form) into a state
        // The 'cost' is not changed from the default value of 0.
        public State<Position> getGoalState()
        {
            return new State<Position>(maze.GoalPos);
        }

        // The origin state of the maze is translated (from position form) into a state
        // The 'cost' is not changed from the default value of 0.
        public State<Position> getInitialState()
        {
            return new State<Position>(maze.InitialPos);
        }

        /*
         * All possible states from a given state are calculated to make sure they are 1) inside
         * the bounds of the maze and 2) of the type 'free'.
         * The makeState helper method is used to build the required states.
         */
        public List<State<Position>> getPossibleStates(State<Position> s)
        {
            double cost = s.cost + 1;
            int row = s.state.Row;
            int col = s.state.Col;
            List<State<Position>> retList = new List<State<Position>>();

            // 'Above' position
            if ((col - 1 >= 0) && (maze[row, col - 1] == CellType.Free))
            {
                retList.Add(makeState(row, col - 1, cost, s));
            }
            // 'Right' position
            if ((row + 1 < maze.Rows) && (maze[row + 1, col] == CellType.Free))
            {
                retList.Add(makeState(row + 1, col, cost, s));
            }
            // 'Below' position
            if ((col + 1 < maze.Cols) && (maze[row, col + 1] == CellType.Free))
            {
                retList.Add(makeState(row, col + 1, cost, s));
            }
            // 'Left' position
            if ((row - 1 >= 0) && (maze[row - 1, col] == CellType.Free))
            {
                retList.Add(makeState(row - 1, col, cost, s));
            }

            return retList;
        }

        public void print()
        {
            Console.WriteLine("{0} rows and {0} cols", maze.Rows, maze.Cols);
            for (int i = 0; i < maze.Rows; i++)
            {
                string oneLine = "";
                for (int j = 0; j < maze.Cols; j++)
                {
                    if (maze.GoalPos.Row == i && maze.GoalPos.Col == j)
                    {
                        oneLine += "E";
                    }
                    else if (maze.InitialPos.Row == i && maze.InitialPos.Col == j)
                    {
                        oneLine += "S";
                    }
                    else if (maze[i,j] == CellType.Free)
                    {
                        oneLine += "0";
                    }
                    else
                    {
                        oneLine += "X";
                    }
                }
                Console.WriteLine(oneLine);
            }
            return;
        }
    }
}
