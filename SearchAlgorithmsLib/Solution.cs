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
        
        public Solution(List<State<T>> path, int nodesEvaluated)
        {
            solutionPath = path;
            this.nodesEvaluated = nodesEvaluated; 
        } 
    }
}
