using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    public abstract class StateBacktrace<T> : IBacktracer<T>
    {
        /*
         * The StateBacktracer is used as a shared abstract class to return
         * a Solution from a given state
         */
        public Solution<T> Backtrace(State<T> end, int numberOfEvaluations)
        {
            List<State<T>> backlist = new List<State<T>>();
            State<T> next = end;
            while (next != null)
            {
                backlist.Add(next);
                next = next.predecessor;
            }
            backlist.Reverse();

            Solution<T> result = new Solution<T>(backlist, numberOfEvaluations);
            return result;
        }
    }
}
