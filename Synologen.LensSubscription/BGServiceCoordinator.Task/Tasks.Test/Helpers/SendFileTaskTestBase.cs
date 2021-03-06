using FakeItEasy;
using Spinit.Wpc.Synologen.Core.Domain.Model.Autogiro.CommonTypes;
using Spinit.Wpc.Synologen.Core.Domain.Services;
using Spinit.Wpc.Synologen.Core.Domain.Services.Coordinator;

namespace Synologen.LensSubscription.BGServiceCoordinator.Task.Test.Helpers
{
	public abstract class SendFileTaskTestBase : CommonTaskTestBase
	{
		protected ITamperProtectedFileWriter TamperProtectedFileWriter;
		protected IFtpService FtpService;
		protected IFileWriterService FileWriterService;

		protected override void SetUp()
		{
			base.SetUp();

			TamperProtectedFileWriter = A.Fake<ITamperProtectedFileWriter>();
			FtpService = A.Fake<IFtpService>();
			FileWriterService = A.Fake<IFileWriterService>();
			TaskRepositoryResolver.AddRepository(FtpService);
		}
		protected override ITask GetTask()
		{
			return new SendFile.Task(LoggingService, TamperProtectedFileWriter, FileWriterService);
		}
	}
}