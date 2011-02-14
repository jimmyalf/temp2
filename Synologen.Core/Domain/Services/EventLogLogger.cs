using System;
using System.Diagnostics;

namespace Spinit.Wpc.Synologen.Core.Domain.Services
{
	public class EventLogLogger : IEventLoggingService
	{
		private readonly string _applicationName;

		public EventLogLogger(string applicationName)
		{
			_applicationName = applicationName;
		}

		public void Info(string message)
		{
			EventLog.WriteEntry(_applicationName, message, EventLogEntryType.Information);
		}

		public void Info(string format, params object[] parameters)
		{
			EventLog.WriteEntry(_applicationName, string.Format(format, parameters), EventLogEntryType.Information);
		}

		public void Warning(string message)
		{
			EventLog.WriteEntry(_applicationName, message, EventLogEntryType.Warning);
		}

		public void Warning(string format, params object[] parameters)
		{
			EventLog.WriteEntry(_applicationName, string.Format(format, parameters), EventLogEntryType.Warning);
		}

		public void Error(string message)
		{
			EventLog.WriteEntry(_applicationName, message, EventLogEntryType.Error);
		}

		public void Error(string format, params object[] parameters)
		{
			EventLog.WriteEntry(_applicationName, string.Format(format, parameters), EventLogEntryType.Warning);
		}

		public void Error(string message, Exception ex)
		{
			Error(message);
			const string format = "Source: {0}, Message: {1}, Stacktrace: {2}";
			Error(format, ex.Source, ex.Message, ex.StackTrace);
		}

		public void SuccessAudit(string message)
		{
			EventLog.WriteEntry(_applicationName, message, EventLogEntryType.SuccessAudit);
		}

		public void SuccessAudit(string format, params object[] parameters)
		{
			EventLog.WriteEntry(_applicationName, string.Format(format, parameters), EventLogEntryType.SuccessAudit);
		}

		public void FailureAudit(string message)
		{
			EventLog.WriteEntry(_applicationName, message, EventLogEntryType.FailureAudit);
		}

		public void FailureAudit(string format, params object[] parameters)
		{
			EventLog.WriteEntry(_applicationName, string.Format(format, parameters), EventLogEntryType.FailureAudit);
		}
	}
}