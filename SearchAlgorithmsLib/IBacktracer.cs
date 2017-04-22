using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    public interface IBacktracer<T>
    {
        Solution<T> Backtrace(State<T> root, int nodesEvaluated);
    }
}
