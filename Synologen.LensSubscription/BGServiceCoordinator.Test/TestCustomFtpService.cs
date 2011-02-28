using System;
using NUnit.Framework;
using Spinit.Wpc.Synologen.Core.Domain.Services;

namespace Synologen.LensSubscription.BGService.Test
{
	[TestFixture, Explicit("Test is for proof of concept only")]
	public class TestCustomFtpService
	{
		protected FtpCommandService FTPCommandService;

		public TestCustomFtpService()
		{
			FTPCommandService = new FtpCommandService("ftp.spinit.se");
			FTPCommandService.OnCommandExecuted += (sender, eventArgs) => Console.WriteLine("> " +eventArgs.Command);
			FTPCommandService.OnResponseReceived += (sender, eventArgs) => Console.WriteLine(eventArgs.Response);
		}

		[Test]
		public void Test_login()
		{
			FTPCommandService.Execute("USER spinitupload");
			FTPCommandService.Execute("PASS bradag");
			FTPCommandService.Execute("HELP");
		}

		[TearDown]
		public void TearDown()
		{
			FTPCommandService.Dispose();
		}
	}
}