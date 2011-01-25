﻿using System;
using log4net;
using Spinit.Wpc.Synologen.Core.Domain.Services;

namespace Synologen.ServiceCoordinator.Logging
{
	public class Log4NetLogger : ILoggingService
	{
		private static ILog _logger;
		private static IEventLoggingService _eventLogger;

		public Log4NetLogger(string applicationName)
		{
			_logger = _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        
			log4net.Config.XmlConfigurator.Configure();
			_eventLogger = new EventLogLogger(applicationName);
		}

		public Log4NetLogger(ILog logger, string applicationName)
		{
			_logger = logger;
			log4net.Config.XmlConfigurator.Configure();
			_eventLogger = new EventLogLogger(applicationName);
		}

		// For unit testing set runConfigure to false
		public Log4NetLogger(ILog logger, bool runConfigure, string applicationName)
		{
			_logger = logger;
			_eventLogger = new EventLogLogger(applicationName);
			if (runConfigure)
				log4net.Config.XmlConfigurator.Configure();
		}

		public Log4NetLogger(IEventLoggingService eventLogger)
		{
			_eventLogger = eventLogger;
		}

		public void LogDebug(string message)
		{
			_logger.Debug(message);
		}

		public void LogDebug(string format, params string[] parameters)
		{
			_logger.Debug(string.Format(format, parameters));
		}

		public void LogInfo(string message)
		{
			_logger.Info(message);
		}

		public void LogInfo(string format, params string[] parameters)
		{
			_logger.Info(string.Format(format, parameters));
		}

		public void LogWarning(string message)
		{
			_logger.Warn(message);
		}

		public void LogWarning(string format, params string[] parameters)
		{
			_logger.Warn(string.Format(format, parameters));
		}

		public void LogError(string message)
		{
			_logger.Error(message);
		}

		public void LogError(string message, Exception ex)
		{
			LogError(message);
			const string format = "Source: {0}, Message: {1}, Stacktrace: {2}";
			LogError(format, ex.Source, ex.Message, ex.StackTrace);
		}

		public void LogError(string format, params string[] parameters)
		{
			_logger.Error(string.Format(format, parameters));
		}

		public void LogFatal(string message)
		{
			_logger.Fatal(message);
		}

		public void LogFatal(string format, params string[] parameters)
		{
			_logger.Fatal(string.Format(format, parameters));
		}

		public void LogFatal(string message, Exception ex)
		{
			LogFatal(message);
			const string format = "Source: {0}, Message: {1}, Stacktrace: {2}";
			LogFatal(format, ex.Source, ex.Message, ex.StackTrace);
		}

		public void LogInfoEventLog(string message)
		{
			_eventLogger.Info(message);
		}

		public void LogInfoEventLog(string format, params string[] parameters)
		{
			_eventLogger.Info(format, parameters);
		}

		public void LogWarningEventLog(string message)
		{
			_eventLogger.Warning(message);
		}

		public void LogWarningEventLog(string format, params string[] parameters)
		{
			_eventLogger.Warning(format, parameters);
		}

		public void LogErrorEventLog(string message)
		{
			_eventLogger.Error(message);
		}

		public void LogErrorEventLog(string format, params string[] parameters)
		{
			_eventLogger.Error(format, parameters);
		}

		public void LogErrorEventLog(string message, Exception ex)
		{
			_eventLogger.Error(message, ex);
		}

		public void LogSuccessAudit(string message)
		{
			_eventLogger.SuccessAudit(message);
		}

		public void LogSuccessAudit(string format, params string[] parameters)
		{
			_eventLogger.SuccessAudit(format, parameters);
		}

		public void LogFailureAudit(string message)
		{
			_eventLogger.FailureAudit(message);
		}

		public void LogFailureAudit(string format, params string[] parameters)
		{
			_eventLogger.FailureAudit(format, parameters);
		}
	}
}