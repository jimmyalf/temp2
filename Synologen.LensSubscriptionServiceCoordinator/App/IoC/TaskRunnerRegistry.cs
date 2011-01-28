﻿using System.Reflection;
using log4net;
using NHibernate;
using Spinit.Data;
using Spinit.Data.NHibernate;
using Spinit.Wpc.Core.Dependencies.NHibernate;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.LensSubscription;
using Spinit.Wpc.Synologen.Core.Domain.Services;
using Spinit.Wpc.Synologen.Core.Domain.Services.Coordinator;
using Spinit.Wpc.Synologen.Data.Repositories.CriteriaConverters;
using Spinit.Wpc.Synologen.Data.Repositories.LensSubscriptionRepositories;
using Spinit.Wpc.Synologen.LensSubscriptionServiceCoordinator.App.Logging;
using StructureMap.Configuration.DSL;

namespace Spinit.Wpc.Synologen.LensSubscriptionServiceCoordinator.App.Ioc
{
	public class TaskRunnerRegistry : Registry
	{
		public TaskRunnerRegistry()
		{
			// NHibernate
			For<IUnitOfWork>().HybridHttpOrThreadLocalScoped().Use<NHibernateUnitOfWork>();
			For<ISessionFactory>().Singleton().Use(NHibernateFactory.Instance.GetSessionFactory);
			For<ISession>().Use(x => ((NHibernateUnitOfWork)x.GetInstance<IUnitOfWork>()).Session);
			For<ISubscriptionRepository>().Use<SubscriptionRepository>();

			// Logging
			For<ILog>().Use(Log4NetFactory.Create);
			For<IEventLoggingService>().Use(new EventLogLogger("TaskRunner"));
			For<ILoggingService>().Singleton().Use<Log4NetLogger>();


			For<IBGWebService>().Use<BgWebServiceClient>();
			
			// Task scan
			Scan(x =>
			{
				x.Assembly(Assembly.GetExecutingAssembly());
				x.AddAllTypesOf<ITask>();
			});

			// Register criteria converters
			Scan(x =>
			{
				x.AssemblyContainingType<PageOfFramesMatchingCriteriaConverter>();
				x.Assembly(typeof(NHibernateActionCriteriaConverter<,>).Assembly.FullName);
				x.ConnectImplementationsToTypesClosing(typeof(IActionCriteriaConverter<,>));
			});
		}
	}
}