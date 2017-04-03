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

        public State<T> predecessor
        {
            get;
            set;
        }

        public double cost
        {
            get;
            set;
        }

        public State(T inputState)
        {
            state = inputState;
            predecessor = null;
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
