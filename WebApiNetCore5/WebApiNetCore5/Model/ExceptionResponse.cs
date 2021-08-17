using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiNetCore5.Model
{
    public class HttpResponseException : SystemException
    {
        public int Status { get; set; } = 500;

        public object Value { get; set; }

        public string ErrorMessage { get; set; }

        public HttpResponseException(string message)
        {
            ErrorMessage = message;
        }

    }
}
