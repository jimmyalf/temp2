using System.Collections;
using System.ServiceModel;
using StructureMap.Pipeline;

namespace Synologen.Service.Web.External.App.IoC
{
	public class WcfPerOperationLifecycle : ILifecycle
	{
		public static readonly string ITEM_NAME = "STRUCTUREMAP-INSTANCES";

		public void EjectAll() 
		{ 
			FindCache().DisposeAndClear();
		}
		public IObjectCache FindCache()
		{
			var items = FindWcfOperationDictionary();
			if (!items.Contains(ITEM_NAME))
			{
				lock (items.SyncRoot)
				{
					if (!items.Contains(ITEM_NAME))
					{
						var cache = new MainObjectCache();
						items.Add(ITEM_NAME, cache);
						return cache;
					}
				}
			}
			return (IObjectCache) items[ITEM_NAME];
		}

		public string Scope
		{
			get
			{
				return GetType().Name;
			} 
		}

		protected virtual IDictionary FindWcfOperationDictionary()
		{
			var contextExtension =  OperationContext.Current.Extensions.Find<WcfContextExtension>();
			if(contextExtension == null)
			{
				contextExtension = new WcfContextExtension();
				OperationContext.Current.Extensions.Add(contextExtension);
			}
			return contextExtension.Items;
		}
	}
}