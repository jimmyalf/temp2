using Spinit.Wpc.Synologen.Core.Domain.Services.Coordinator;
using Spinit.Wpc.Synologen.LensSubscriptionServiceCoordinator.Tasks;

namespace Synologen.ServiceCoordinator.Test.TestHelpers
{
	public class SendConsentsTaskTestBase : TaskTestBase
	{
		protected override ITask GetTask() 
		{
			return new SendConsentsTask(
				MockedWebServiceClient.Object,
				MockedSubscriptionRepository.Object,
				LoggingService);
		}
	}
}