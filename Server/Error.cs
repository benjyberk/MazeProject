using Newtonsoft.Json;
using Server.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Error
    {
        public string ErrorType;
        public Error(string errorType)
        {
            ErrorType = errorType;
        }

        public static Result makeError(string errorMsg)
        {
            Error e = new Server.Error(errorMsg);
            string message = JsonConvert.SerializeObject(e);
            return new View.Result(message, false);
        }
    }
}
