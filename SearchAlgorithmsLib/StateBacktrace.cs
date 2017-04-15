using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    public abstract class StateBacktrace<T> : IBacktracer<T>
    {
        public Solution<T> backtrace(State<T> end)
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
