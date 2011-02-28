using System;
using Spinit.Wpc.Synologen.Core.Domain.Services;
using Synologen.LensSubscription.Autogiro.Writers;

namespace Synologen.LensSubscription.BGServiceCoordinator.App.Factories
{
	public static class TamperProtectedFileWriterFactory
	{
		public static TamperProtectedFileWriter Get(IHashService hashService)
		{
			var writeDate = DateTime.Now;
			return new TamperProtectedFileWriter(hashService, writeDate);
		}
	}
}