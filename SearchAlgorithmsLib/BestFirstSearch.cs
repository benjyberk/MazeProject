using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    /*
     * The 'Best First Search' algorithm iterates through the list of possible nodes led
     * from by each state, always choosing to follow the 'best' path first.  The 'best' path
     * is established by the cost of each state.
     */
    public class BestFirstSearch<T> : Searcher<T>
    {
        public override Solution<T> search(ISearchable<T> domain)
        {
            HashSet<State<T>> closed = new HashSet<State<T>>();
            // We add the inital state to the list of states to check
            addToOpenList(domain.getInitialState());

            while (openListSize() > 0)
            {
                State<T> nextNode = popOpenList();
                closed.Add(nextNode);

                if (nextNode.Equals(domain.getGoalState()))
                {
                    return backTrace(nextNode);
                }


            }

        }

        private Solution<T> backTrace(State<T> end)
        {
            List<State<T>> backlist = new List<State<T>>();
            State<T> next = end;
            while (next != null)
            {
                backlist.Add(next);
                next = next.predecessor;
            }
            Solution<T> result = new Solution<T>(backlist);
            return result;
        }
    }
}
