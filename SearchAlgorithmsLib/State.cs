using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    /*
     * The state class holds the details related to a single stage in a search.
     * The state itself is determined by the provided type T
     */
    public class State<T>
    {
        public T state
        {
            get;
        }

        // The state is able to be traced back to the origin state that led to it
        public State<T> predecessor
        {
            get;
            set;
        }

        // The cost to reach this state
        public double cost
        {
            get;
            set;
        }

        // The constructor receives the internal state value, assigning default of null to the
        // predecessor and 0 to the cost (to signify generally that a state is an endpoint)
        public State(T inputState, double val = 0)
        {
            state = inputState;
            predecessor = null;
            cost = val;
        }

        // States can be compared to other states - in order to tell when a 'final' state is reached
        public override bool Equals(object obj)
        {
            return state.Equals(((State<T>)obj).state);
        }

        // We assume that the internal state has a HashCode
        public override int GetHashCode()
        {
            return state.GetHashCode();
        }
    }
}
