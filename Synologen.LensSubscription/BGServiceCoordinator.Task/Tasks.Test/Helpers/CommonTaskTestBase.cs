using FakeItEasy;
using log4net;
using NUnit.Framework;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.BGServer;
using Spinit.Wpc.Synologen.Core.Domain.Services;
using Spinit.Wpc.Synologen.Core.Domain.Services.Coordinator;
using Synologen.LensSubscription.BGServiceCoordinator.App.Logging;
using Synologen.Test.Core;

namespace Synologen.LensSubscription.BGServiceCoordinator.Task.Test.Helpers
{
	[TestFixture]
	public abstract class CommonTaskTestBase : BehaviorTestBase<ITask>
	{
		protected ILog Log;
		protected IEventLoggingService EventLoggingService;
		protected IFileSectionToSendRepository FileSectionToSendRepository;
	    protected IReceivedFileRepository ReceivedFileRepository;
        protected IBGConfigurationSettingsService BgConfigurationSettingsService;
		protected ITaskRepositoryResolver TaskRepositoryResolver;
		protected IAutogiroPayerRepository AutogiroPayerRepository;
		protected Log4NetLogger Log4NetLogger;

		protected override void SetUp()
		{
			Log = A.Fake<ILog>();
			EventLoggingService = A.Fake<IEventLoggingService>();
			Log4NetLogger = new Log4NetLogger(Log, EventLoggingService);
			FileSectionToSendRepository = A.Fake<IFileSectionToSendRepository>();
			ReceivedFileRepository = A.Fake<IReceivedFileRepository>();
			BgConfigurationSettingsService = A.Fake<IBGConfigurationSettingsService>();
			TaskRepositoryResolver = A.Fake<ITaskRepositoryResolver>();
			AutogiroPayerRepository = A.Fake<IAutogiroPayerRepository>();
			
			A.CallTo(() => TaskRepositoryResolver.GetRepository<IFileSectionToSendRepository>()).Returns(FileSectionToSendRepository);
			A.CallTo(() => TaskRepositoryResolver.GetRepository<IReceivedFileRepository>()).Returns(ReceivedFileRepository);
			A.CallTo(() => TaskRepositoryResolver.GetRepository<IAutogiroPayerRepository>()).Returns(AutogiroPayerRepository);
		}

		protected abstract ITask GetTask();

		protected ITask Task { get { return TestModel; } }
		protected override ITask GetTestModel() { return GetTask(); }
	}
}