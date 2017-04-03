using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    public class State<T>
    {
        private T state;
        private double cost;
        private State<T> predecessor;

        public State(T inputState)
        {
            state = inputState;
        }

        public override bool Equals(object obj)
        {
            return state.Equals((obj as State<T>).state);
        }

        public override int GetHashCode()
        {
            return state.GetHashCode();
        }
    }
}
