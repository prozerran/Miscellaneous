using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using log4net;

namespace TestLibrary
{
    public sealed class Logger
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());
        public static Logger Instance { get { return _instance.Value; } }
        private Logger() {}

        public void WriteInfoLog(string message)
        {
            _log.Info(message);
        }

        public void WriteErrorLog(string message)
        {
            _log.Error(message);
        }

        public void WriteInfoLog(string message, params object[] args)
        {
            try
            {
                _log.Info((args.Length == 0) ? message : string.Format(message, args));
            }
            finally
            {
                //_log.Error("Message parameter errors.");
            }
        }

        public void WriteErrorLog(string message, params object[] args)
        {
            try
            {
                _log.Error((args.Length == 0) ? message : string.Format(message, args));
            }
            finally
            {
                //_log.Error("Message parameter errors.");
            }
        }
    }
}
