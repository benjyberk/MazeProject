using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    /*
     * A solution holds the collection of states that leads to the final result.
     */
    public class Solution<T>
    {

        public List<State<T>> solutionPath
        {
            get;
        }

        public int nodesEvaluated
        {
            get;
        }
        
        /// <summary>
        /// The constructor; seeing as the class is a container, no processing is performed
        /// </summary>
        /// <param name="path">The path</param>
        /// <param name="nodesEvaluated">Number of nodes evaluated</param>
        public Solution(List<State<T>> path, int nodesEvaluated)
        {
            solutionPath = path;
            this.nodesEvaluated = nodesEvaluated; 
        } 
    }
}
