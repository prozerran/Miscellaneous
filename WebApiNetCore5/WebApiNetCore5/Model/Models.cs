
using System;

namespace WebApiNetCore5.Models
{
    public class HomePage
    {
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string IndexPage { get; set; }
        public string Version { get; set; }
    }

    public class ReqMessage
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class ExceptionMessage
    {
        public string Message { get; set; }
    }
}
