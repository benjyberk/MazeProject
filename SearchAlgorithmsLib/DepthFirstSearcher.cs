using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    /*
     * The 'Depth First Search' algorithm starts at the initial position, and uses a basic stack
     * to explore the 'depth' of each search branch, until all possible search branches are covered
     * We use a StateBacktracer to share backtracing code.
     */
    public class DepthFirstSearcher<T> : StateBacktrace<T>, ISearcher<T>
    {
        private int numberOfEvaluations = 0;
        Stack<State<T>> openList;
        HashSet<State<T>> closedList;

        /// <summary>
        /// Returns the number of evaluations that occured during the search
        /// </summary>
        /// <returns>The number of evaluations</returns>
        public int getNumberOfEvaluations()
        {
            return numberOfEvaluations;
        }
        
        // The counter for the number of evaluations is incremented every time a node is popped
        private State<T> getTop()
        {
            numberOfEvaluations++;
            return openList.Pop();
        }

        // The search method simply iterates through the stack, exploring all possible options
        // with no heuristic to decide which path to take
        public Solution<T> search(ISearchable<T> domain)
        {
            openList = new Stack<State<T>>();
            closedList = new HashSet<State<T>>();

            // The initial state is loaded into the list of nodes to be evaluated
            State<T> node = domain.getInitialState();
            openList.Push(node);
            while (openList.Count > 0)
            {
                node = getTop();
                // If we reach the goal, we return our path using the backtracer provided
                if (node.Equals(domain.getGoalState()))
                {
                    return Backtrace(node, numberOfEvaluations);
                }

                // If we haven't reached the goal, we add all possible successors to the stack
                if (!closedList.Contains(node))
                {
                    closedList.Add(node);
                    List<State<T>> successors = domain.getPossibleStates(node);
                    foreach (State<T> next in successors)
                    {
                        openList.Push(next);
                    }
                }
            }
            // If we reach here, there is no path to the destination
            return null;
        }
    }
}
