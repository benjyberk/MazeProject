using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGameDesktop
{
    /*
     * An 'Error' result returns its result value, and indicates the socket should
     * be closed after it is received.
     */
    public class Error
    {
        public string ErrorType;
        public Error(string errorType)
        {
            ErrorType = errorType;
        }

        public static string makeError(string errorMsg)
        {
            Error e = new Error(errorMsg);
            // The Error Message is converted to json form
            string message = JsonConvert.SerializeObject(e);
            return message;
        }
    }
}
