﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    /*
     * An interface which defines the necessary elements for a 'Searching' algorithm
     */
    public interface ISearcher<T>
    {   
        // The searcher has the ability to search through a given domain
        Solution<T> search(ISearchable<T> domain);

        // Returns the amount of elements examined by the search method
        int getNumberOfEvaluations();
    }
}
