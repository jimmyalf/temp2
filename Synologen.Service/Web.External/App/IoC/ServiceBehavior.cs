using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using StructureMap;

namespace Synologen.Service.Web.External.App.IoC
{
	public class ServiceBehavior : IServiceBehavior
	{
		public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			foreach (var cdb in serviceHostBase.ChannelDispatchers)
			{
				var cd = cdb as ChannelDispatcher;
				if (cd == null) continue;
				cd.ErrorHandlers.Add(GetErrorHandler());
				foreach (var ed in cd.Endpoints)
				{
					ed.DispatchRuntime.InstanceProvider = new InstanceProvider(serviceDescription.ServiceType);
				}
			}
		}

		protected virtual IErrorHandler GetErrorHandler()
		{
			return ObjectFactory.GetInstance<IErrorHandler>();
		}

		public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
		{
		}

		public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
		}
	}
}