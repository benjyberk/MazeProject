using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Priority_Queue;

namespace SearchAlgorithmsLib
{
    /*
     * The 'Best First Search' algorithm iterates through the list of possible nodes led
     * from by each state, always choosing to follow the 'best' path first.  The 'best' path
     * is established by the cost of each state, and organized by the Priority Queue 'open'.
     */
    public class BestFirstSearcher<T> : StateBacktrace<T>, ISearcher<T>
    {
        private int numberOfEvaluations = 0;
        private SimplePriorityQueue<State<T>> openList;
        private HashSet<State<T>> closed;
        private Dictionary<State<T>, double> openListContains;

        /*
         * The number of evaluations is determined by the amount of nodes moved from the open
         * list to the closed list. 
         */
        public int getNumberOfEvaluations()
        {
            return numberOfEvaluations;
        }

        /*
         * A node gets added to the priority queue, and the 'contains' list for the priority
         * queue
         */
        private void addNode(State<T> node)
        {
            openList.Enqueue(node, Convert.ToSingle(node.cost));
            if (!openListContains.ContainsKey(node))
            {
                openListContains.Add(node, node.cost);
            }
        }

        /*
         * Remove the node from the top of the priority queue and return it.
         */
        private State<T> RemoveTopNode()
        {
            // Every node dealt with is counted
            numberOfEvaluations++;

            State<T> nextNode = openList.Dequeue();
            // The node gets moved into 'closed'
            closed.Add(nextNode);
            openListContains.Remove(nextNode);

            return nextNode;
        }

        public Solution<T> search(ISearchable<T> domain)
        {
            openList = new SimplePriorityQueue<State<T>>();
            closed = new HashSet<State<T>>();

            // We use a dictionary to hold the states in addition to the PriorityQueue to allow
            // random 'get' in O(1), a function not supported by Priority Queues (Space complexity is
            // now 2n as opposed to n).
            openListContains = new Dictionary<State<T>, double>();

            // We add the inital state to the list of states to check
            addNode(domain.getInitialState());

            while (openList.Count > 0)
            {
                State<T> nextNode = RemoveTopNode();
                // If we have reached the goal state, we generate the traceback and return it as a
                // solution
                if (nextNode.Equals(domain.getGoalState()))
                {
                    return Backtrace(nextNode, numberOfEvaluations);
                }

                // getPossibleStates intializes our states, and assigns the predecessors as needed
                List<State<T>> followingNodes = domain.getPossibleStates(nextNode);
                foreach (State<T> node in followingNodes)
                {
                    // If the node isn't in the open list we add it - as long as its not in the closed
                    // list
                    if (!openListContains.ContainsKey(node))
                    {
                        if (!closed.Contains(node))
                        {
                            addNode(node);
                        }
                    }
                    // If the node is in the open list already, if we have found a cheaper path to it
                    // Then we update the predecessor node and its cost.
                    else {
                        if (openListContains[node] > node.cost)
                        {
                            openList.Remove(node);
                            openListContains[node] = node.cost;
                            addNode(node);
                        }
                    }
                }
            }
            // If we reach here, there is no path to the destination
            return null;
        }

        
    }
}
