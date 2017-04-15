using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    /*
     * An interface which defines the basic requirements of any 'searchable' object.
     * The search process is broken down into different states
     */
    public interface ISearchable<T>
    {
        State<T> getInitialState();
        State<T> getGoalState();
        List<State<T>> getPossibleStates(State<T> s);
    }
}
