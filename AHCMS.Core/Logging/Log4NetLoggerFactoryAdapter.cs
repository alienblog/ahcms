using System;

namespace AHCMS.Core.Logging
{
    public class Log4NetLoggerFactoryAdapter : Common.Logging.ILoggerFactoryAdapter
    {
        public Log4NetLoggerFactoryAdapter()            
        {
           
        }

        public Common.Logging.ILog GetLogger(string name)
        {
            return new Log4NetLogger(log4net.LogManager.GetLogger(name));
        }

        public Common.Logging.ILog GetLogger(Type type)
        {
            return new Log4NetLogger(log4net.LogManager.GetLogger(type));
        }
    }
}