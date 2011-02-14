using Spinit.Wpc.Synologen.Core.Domain.Services.Coordinator;
using Spinit.Wpc.Synologen.LensSubscription.ServiceCoordinator.Tasks;

namespace Synologen.LensSubscription.ServiceCoordinator.Test.TestHelpers
{
	public abstract class SendPaymentsTaskTestBase : TaskTestBase
	{
		protected override ITask GetTask()
		{
			return new SendPaymentsTask(
				MockedWebServiceClient.Object,
				MockedSubscriptionRepository.Object,
				LoggingService);
		}
	}
}