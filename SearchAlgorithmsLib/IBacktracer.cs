using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    /// <summary>
    /// The interface defining the necessary attributes of a backtracer
    /// </summary>
    /// <typeparam name="T">The states type</typeparam>
    public interface IBacktracer<T>
    {
        Solution<T> Backtrace(State<T> root, int nodesEvaluated);
    }
}
