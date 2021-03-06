using log4net;
using Spinit.Wpc.Synologen.Core.Domain.Services;

namespace Synologen.Service.Client.SubscriptionTaskRunner.App.Logging
{
	public static class LogFactory
	{
		public static ILog CreateLogger()
		{
			return LogManager.GetLogger("TaskRunner");
		}

		//public static IEventLoggingService CreateEventLogger()
		//{
		//    return new EventLogLogger("TaskRunner");
		//}

		public static ILoggingService CreateLoggingService()
		{
			return new Log4NetLogger(CreateLogger()/*, CreateEventLogger()*/);
		}
	}
}