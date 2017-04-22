using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.View
{
    /*
     * A 'Result' object is a simple container class to hold a result message as well
     * as a boolean indicating whether a socket should be closed after the result is returned
     */
    public class Result
    {
        public string resultString
        {
            get;
        }
        public bool keepOpen
        {
            get;
            set;
        }

        public Result(string r, bool b)
        {
            resultString = r;
            keepOpen = b;
        }
    }
}
