using System;

namespace AHCMS.Core.Logging
{
    public class Logger
    {        
        private Common.Logging.ILog log;

        public  Logger()
        {
            
        }

        public static void ConfigureConsoleOut()
        {
            Common.Logging.LogManager.Adapter =
                new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter();
        }


        public static void ConfigureTrace()
        {
            Common.Logging.LogManager.Adapter =
                new Common.Logging.Simple.TraceLoggerFactoryAdapter();
        }


        public static void ConfigureLog4Net(string filelogPath,
            LogLevel displayLogLevel, LogLevel saveLogLevel)
        {
            log4net.Repository.Hierarchy.Logger root =
                 ((log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository()).Root;

            root.AddAppender(GetConsoleAppender(GetLog4NetLevel(displayLogLevel)));
            root.AddAppender(GetTraceAppender(GetLog4NetLevel(displayLogLevel)));
            root.AddAppender(GetSiteFileAppender(filelogPath, GetLog4NetLevel(saveLogLevel)));
            root.Repository.Configured = true;

            log4net.Repository.Hierarchy.Logger nhibernate =
               (log4net.Repository.Hierarchy.Logger)log4net.LogManager.GetLogger("NHibernate").Logger;
            nhibernate.AddAppender(GetNHibernateFileAppender(filelogPath, GetLog4NetLevel(saveLogLevel)));
            nhibernate.Additivity = false;
            nhibernate.Repository.Configured = true;

            log4net.Repository.Hierarchy.Logger autofac =
              (log4net.Repository.Hierarchy.Logger)log4net.LogManager.GetLogger("Autofac").Logger;
            autofac.AddAppender(GetSpringFileAppender(filelogPath, GetLog4NetLevel(saveLogLevel)));
            autofac.Additivity = false;
            autofac.Repository.Configured = true;


            Common.Logging.LogManager.Adapter = new Log4NetLoggerFactoryAdapter();
        }      

      

        public static Logger GetLogger(string name)
        {
            Logger logger = new Logger();
            logger.log = Common.Logging.LogManager.GetLogger(name);

            return logger;
        }

        public static Logger GetLogger(Type type)
        {
            Logger logger = new Logger();
            logger.log = Common.Logging.LogManager.GetLogger(type);

            return logger;
        }
   
        public void Debug(object message)
        {         
            log.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            log.Debug(message, exception);
        }

        

        public void Error(object message)
        {
          log.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            log.Error(message, exception);
        }

        public void Fatal(object message)
        {
            log.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            log.Fatal(message, exception);
        }


        public void Info(object message)
        {
            log.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            log.Info(message, exception);
        }

        public void Warn(object message)
        {
            log.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            log.Warn(message, exception);
        }

        #region log4net

        private static log4net.Appender.ConsoleAppender GetConsoleAppender(
            log4net.Core.Level consoleLogLevel)
        {
            log4net.Appender.ConsoleAppender appender = new log4net.Appender.ConsoleAppender();
            appender.Name = "Console";
            appender.Layout = new log4net.Layout.PatternLayout(
                "%d{yyyy-MM-dd HH:mm:ss} [%t] %-5p %c - %m%n");
            appender.Threshold = consoleLogLevel; 
            appender.ActivateOptions();

            return appender;
        }

        private static log4net.Appender.TraceAppender GetTraceAppender(
            log4net.Core.Level traceLogLevel)
        {
            log4net.Appender.TraceAppender appender = new log4net.Appender.TraceAppender();
            appender.Name = "Trace";
            appender.Layout = new log4net.Layout.PatternLayout(
                "%d{yyyy-MM-dd HH:mm:ss} [%t] %-5p %c - %m%n");
            appender.Threshold = traceLogLevel;
            appender.ActivateOptions();

            return appender;
        }



        private static log4net.Appender.FileAppender GetSiteFileAppender(
            string filelogPath, log4net.Core.Level fileLogLevel)
        {
            log4net.Appender.RollingFileAppender appender = new log4net.Appender.RollingFileAppender();
            appender.Name = "SiteLog";
            appender.AppendToFile = true;
            appender.File = filelogPath + "\\Site.log";
            appender.RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Size;
            appender.MaxSizeRollBackups = 10;
            appender.MaximumFileSize = "10MB";
            appender.StaticLogFileName = true;
            appender.Layout = new log4net.Layout.PatternLayout(
                "%d{yyyy-MM-dd HH:mm:ss} [%t] %-5p %c - %m%n");
            appender.Threshold = fileLogLevel;
            appender.ActivateOptions();

            return appender;
        }

        private static log4net.Appender.FileAppender GetNHibernateFileAppender(
            string filelogPath, log4net.Core.Level fileLogLevel)
        {
            log4net.Appender.RollingFileAppender appender = new log4net.Appender.RollingFileAppender();
            appender.Name = "NHibernateLog";
            appender.AppendToFile = true;
            appender.File = filelogPath + "\\NHibernate.log";
            appender.RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Size;
            appender.MaxSizeRollBackups = 10;
            appender.MaximumFileSize = "10MB";
            appender.StaticLogFileName = true;
            appender.Layout = new log4net.Layout.PatternLayout(
                "%d{yyyy-MM-dd HH:mm:ss} [%t] %-5p %c - %m%n");
            appender.Threshold = fileLogLevel;
            appender.ActivateOptions();

            return appender;
        }

        private static log4net.Appender.FileAppender GetSpringFileAppender(
            string filelogPath, log4net.Core.Level fileLogLevel)
        {
            log4net.Appender.RollingFileAppender appender = new log4net.Appender.RollingFileAppender();
            appender.Name = "SpringLog";
            appender.AppendToFile = true;
            appender.File = filelogPath + "\\Spring.log";
            appender.RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Size;
            appender.MaxSizeRollBackups = 10;
            appender.MaximumFileSize = "10MB";
            appender.StaticLogFileName = true;
            appender.Layout = new log4net.Layout.PatternLayout(
                "%d{yyyy-MM-dd HH:mm:ss} [%t] %-5p %c - %m%n");
            appender.Threshold = fileLogLevel;
            appender.ActivateOptions();

            return appender;
        }

        private static log4net.Core.Level GetLog4NetLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.All:
                    return log4net.Core.Level.All;
                case LogLevel.Trace:
                    return log4net.Core.Level.Trace;
                case LogLevel.Debug:
                    return log4net.Core.Level.Debug;
                case LogLevel.Info:
                    return log4net.Core.Level.Info;
                case LogLevel.Warn:
                    return log4net.Core.Level.Warn;
                case LogLevel.Error:
                    return log4net.Core.Level.Error;
                case LogLevel.Fatal:
                    return log4net.Core.Level.Fatal;
                default:
                    throw new ArgumentOutOfRangeException("logLevel", logLevel, "unknown log level");
            }

            #endregion log4net
        }
    }
}