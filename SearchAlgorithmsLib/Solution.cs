using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    public class Solution<T>
    {
        List<State<T>> solutionPath;
        
        public Solution(List<State<T>> path)
        {
            solutionPath = path;
        } 
    }
}
