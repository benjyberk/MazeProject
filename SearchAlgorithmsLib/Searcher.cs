using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Priority_Queue;

namespace SearchAlgorithmsLib
{
    public abstract class Searcher<T> : ISearcher<T>
    {
        // It is assumed that every search algorithm keeps a priority-queue of nodes to check
        private SimplePriorityQueue<State<T>> openList;
        private int numberOfNodesChecked;

        /*
         * The constructor initializes the priority queue
         */
        public Searcher()
        {
            openList = new SimplePriorityQueue<State<T>>();
            numberOfNodesChecked = 0;
        }

        /*
         * Returns the element at the top of the priority queue, and removes it from the queue
         */
        protected State<T> popOpenList()
        {
            numberOfNodesChecked++;
            return openList.Dequeue();
        }

        /*
         * Returns the amount of evaluations performed so-far by the searching algorithm
         */
        public int getNumberOfEvaluations()
        {
            return numberOfNodesChecked;
        }

        /*
         * Returns the number of nodes still to be checked
         */
        public int openListSize()
        {
            return openList.Count;
        }

        /*
         * Adds a node to the list of nodes to be checked
         */
        public void addToOpenList(State<T> newState)
        {
            openList.Enqueue(newState, Convert.ToSingle(newState.cost));
        }

        public abstract Solution<T> search(ISearchable<T> domain);
    }
}
