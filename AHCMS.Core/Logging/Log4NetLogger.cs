using System;

namespace AHCMS.Core.Logging
{
    public class Log4NetLogger : Common.Logging.ILog
    {
        private log4net.Core.ILogger _logger = null;
        private static readonly Type declaringType = typeof(Log4NetLogger);

        public bool IsInfoEnabled
        {
            get
            {
                return this._logger.IsEnabledFor(log4net.Core.Level.Info);
            }
        }
        public bool IsWarnEnabled
        {
            get
            {
                return this._logger.IsEnabledFor(log4net.Core.Level.Warn);
            }
        }

        public bool IsErrorEnabled
        {
            get
            {
                return this._logger.IsEnabledFor(log4net.Core.Level.Error);
            }
        }

        public bool IsFatalEnabled
        {
            get
            {
                return this._logger.IsEnabledFor(log4net.Core.Level.Fatal);
            }
        }

        public bool IsDebugEnabled
        {
            get
            {
                return this._logger.IsEnabledFor(log4net.Core.Level.Debug);
            }
        }

        public bool IsTraceEnabled
        {
            get
            {
                return this._logger.IsEnabledFor(log4net.Core.Level.Trace);
            }
        }

        internal Log4NetLogger(log4net.ILog log)
        {
            this._logger = log.Logger;
        }

        public void Info(object message, Exception e)
        {
            this._logger.Log(Log4NetLogger.declaringType, log4net.Core.Level.Info, message, e);
        }

        public void Info(object message)
        {
            this._logger.Log(Log4NetLogger.declaringType, log4net.Core.Level.Info, message, null);
        }

        public void Debug(object message, Exception e)
        {
            this._logger.Log(Log4NetLogger.declaringType, log4net.Core.Level.Debug, message, e);
        }
        public void Debug(object message)
        {
            this._logger.Log(Log4NetLogger.declaringType, log4net.Core.Level.Debug, message, null);
        }

        public void Warn(object message, Exception e)
        {
            this._logger.Log(Log4NetLogger.declaringType, log4net.Core.Level.Warn, message, e);
        }

        public void Warn(object message)
        {
            this._logger.Log(Log4NetLogger.declaringType, log4net.Core.Level.Warn, message, null);
        }

        public void Trace(object message, Exception e)
        {
            this._logger.Log(Log4NetLogger.declaringType, log4net.Core.Level.Trace, message, e);
        }

        public void Trace(object message)
        {
            this._logger.Log(Log4NetLogger.declaringType, log4net.Core.Level.Trace, message, null);
        }

        public void Fatal(object message, Exception e)
        {
            this._logger.Log(Log4NetLogger.declaringType, log4net.Core.Level.Fatal, message, e);
        }

        public void Fatal(object message)
        {
            this._logger.Log(Log4NetLogger.declaringType, log4net.Core.Level.Fatal, message, null);
        }

        public void Error(object message, Exception e)
        {
            this._logger.Log(Log4NetLogger.declaringType, log4net.Core.Level.Error, message, e);
        }

        public void Error(object message)
        {
            this._logger.Log(Log4NetLogger.declaringType, log4net.Core.Level.Error, message, null);
        }

    }
}