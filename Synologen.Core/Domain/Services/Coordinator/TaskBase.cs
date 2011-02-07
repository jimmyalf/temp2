using System;

namespace Spinit.Wpc.Synologen.Core.Domain.Services.Coordinator
{
	public abstract class TaskBase : ITask
	{
		private readonly ILoggingService _loggingService;
		private const string DefaultTaskName = "Untitled task";

		protected TaskBase(string taskName,  ILoggingService loggingService)
		{
			_loggingService = loggingService;
			TaskName = taskName ?? DefaultTaskName;
		}

		public virtual void RunLoggedTask(Action action)
		{
			try
			{
				LogInfo("Started Task Execution");
				action.Invoke();
				LogInfo("Finished Task Execution");
			}
			catch(Exception ex)
			{
				_loggingService.LogError(String.Format("{0}: Caught exception while executing task", TaskName),ex);
			}
		}


		public virtual void LogDebug(string message)
		{
			_loggingService.LogDebug("{0}: {1}", TaskName, message);
		}

		public virtual void LogDebug(string format, params object[] parameters)
		{
			_loggingService.LogDebug("{0}: {1}", TaskName, String.Format(format, parameters));
		}

		public virtual void LogInfo(string message)
		{
			_loggingService.LogInfo("{0}: {1}", TaskName, message);
		}

		public virtual void LogInfo(string format, params object[] parameters)
		{
			_loggingService.LogInfo("{0}: {1}", TaskName, String.Format(format, parameters));
		}

		public abstract void Execute();

		public virtual string TaskName { get; private set; }
	}
}