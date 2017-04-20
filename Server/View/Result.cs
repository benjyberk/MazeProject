using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.View
{
    public class Result
    {
        public string resultString
        {
            get;
        }
        public bool keepOpen
        {
            get;
        }

        public Result(string r, bool b)
        {
            resultString = r;
            keepOpen = b;
        }
    }
}
