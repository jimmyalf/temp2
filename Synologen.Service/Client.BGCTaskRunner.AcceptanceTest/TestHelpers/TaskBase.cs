using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EnterpriseDT.Net.Ftp;
using FakeItEasy;
using NHibernate;
using Spinit.Test;
using Spinit.Wpc.Synologen.Core.Domain.Model.BGServer;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.BGServer;
using Spinit.Wpc.Synologen.Core.Domain.Services;
using Spinit.Wpc.Synologen.Core.Domain.Services.BgWebService;
using Spinit.Wpc.Synologen.Core.Domain.Services.Coordinator;
using Spinit.Wpc.Synologen.Core.Extensions;
using StructureMap;
using Synologen.LensSubscription.BGData;
using Synologen.LensSubscription.ServiceCoordinator.Core.TaskRunner;

namespace Synologen.Service.Client.BGCTaskRunner.AcceptanceTest.TestHelpers
{
	public abstract class TaskTestBase : BehaviorActionTestbase
	{
		protected IBGFtpPasswordService bGFtpPasswordService;
		protected IBGServiceCoordinatorSettingsService bgServiceCoordinatorSettingsService;
		protected IReceivedFileRepository receivedFileRepository;
		protected IBGReceivedConsentRepository bgReceivedConsentRepository;
		protected IAutogiroPayerRepository autogiroPayerRepository;
		protected IBGReceivedPaymentRepository bgReceivedPaymentRepository;
		protected IFileSectionToSendRepository fileSectionToSendRepository;
		protected IBGConsentToSendRepository bgConsentToSendRepository;
		protected IBGPaymentToSendRepository bgPaymentToSendRepository;
	    protected IBGReceivedErrorRepository bgReceivedErrorRepository;
		protected FTPClient FTPClient;
		protected string remoteFtpFolder;
		protected readonly Encoding FtpTextEncoding = Encoding.GetEncoding(858);

		protected override void SetUp()
		{
			RebuildDatabase();
			base.SetUp();
			autogiroPayerRepository = ResolveRepository<IAutogiroPayerRepository>();
			bGFtpPasswordService = ResolveRepository<IBGFtpPasswordService>();
			bgServiceCoordinatorSettingsService = ResolveEntity<IBGServiceCoordinatorSettingsService>();
			receivedFileRepository = ResolveRepository<IReceivedFileRepository>();
			fileSectionToSendRepository = ResolveRepository<IFileSectionToSendRepository>();
			bgReceivedConsentRepository = ResolveRepository<IBGReceivedConsentRepository>();
			bgReceivedPaymentRepository = ResolveRepository<IBGReceivedPaymentRepository>();
			bgConsentToSendRepository = ResolveRepository<IBGConsentToSendRepository>();
			bgPaymentToSendRepository = ResolveRepository<IBGPaymentToSendRepository>();
		    bgReceivedErrorRepository = ResolveRepository<IBGReceivedErrorRepository>();
			FTPClient = A.Fake<FTPClient>();
			A.CallTo(() => FTPClient.Password(A<string>.Ignored)).Invokes(call =>
            {
            	FTPClient.ReplyReceived += Raise.With(new FTPMessageEventArgs("230-Password was changed.")).Now;
            });
			remoteFtpFolder = ConfigurationManager.AppSettings["RemoteFtpFolder"];
		}

		private static void RebuildDatabase()
		{
			NHibernateFactory.Instance.GetConfiguration().Export();
		}

		protected TTask ResolveTask<TTask>() where TTask : ITask
		{
			return ObjectFactory
				.With(typeof(FTPClient), FTPClient)
				.GetInstance<TTask>();
		}

		protected TTask ResolveTaskWithRealFtpClient<TTask>() where TTask : ITask
		{
			var url = bgServiceCoordinatorSettingsService.GetFtpUploadFolderUrl();
			var client = new FTPClient {RemoteHost = url};
			return ObjectFactory
				.With(typeof(FTPClient), client)
				.GetInstance<TTask>();
		}

		protected ITaskRunnerService GetTaskRunnerService(ITask task)
		{
			return ObjectFactory
				.With(typeof(IEnumerable<ITask>),new []{task})
				.With(typeof(ITaskRepositoryResolver), new TestTaskRepositoryResolver())
				.GetInstance<TaskRunnerService>();
		}


		private class TestTaskRepositoryResolver : ITaskRepositoryResolver
		{
			public TRepository GetRepository<TRepository>()
			{
				return ObjectFactory.GetInstance<TRepository>();
			}
		}

		protected TRepository ResolveRepository<TRepository>()
		{
			return ObjectFactory
				.With(typeof(ISession), GetBGSession())
				.GetInstance<TRepository>();
		}

		protected TEntity ResolveEntity<TEntity>()
		{
			return ObjectFactory.GetInstance<TEntity>();
		}

		protected static ISession GetBGSession()
		{
			return NHibernateFactory.Instance.GetSessionFactory().OpenSession();
		}

		protected string StoreNewBGCFileOnDisk(string contents) 
		{ 
			var folderPath = bgServiceCoordinatorSettingsService.GetReceivedFilesFolderPath();
			var filePath = folderPath.TrimEnd('\\') + "\\BFEP.UAGZZ.K0123456.D110329.T124300";
			if(System.IO.File.Exists(filePath))
			{
				System.IO.File.Delete(filePath);
			}
			System.IO.File.WriteAllText(filePath, contents, FtpTextEncoding);
			return filePath;
		}

		protected string GetReadFilePath()
		{
			var folderPath = bgServiceCoordinatorSettingsService.GetBackupFilesFolderPath();
			return folderPath.TrimEnd('\\') + "\\BFEP.UAGZZ.K0123456.D110329.T124300";
		}

		protected void ClearFoldersOnDisk() 
		{ 
			var backupFolderPath = bgServiceCoordinatorSettingsService.GetBackupFilesFolderPath();
			var receivedFolderPath = bgServiceCoordinatorSettingsService.GetReceivedFilesFolderPath();
			var sentFolderPath = bgServiceCoordinatorSettingsService.GetSentFilesFolderPath();
			var filesToDelete = System.IO.Directory.GetFiles(backupFolderPath)
				.Append(System.IO.Directory.GetFiles(receivedFolderPath))
				.Append(System.IO.Directory.GetFiles(sentFolderPath));
			foreach (var file in filesToDelete)
			{
				System.IO.File.Delete(file);
			}
			ClearBGCFTPFiles();
		}

		private void ClearBGCFTPFiles()
		{
			var bgcFilesInRemoteFTP = System.IO.Directory
				.GetFiles(remoteFtpFolder)
				.Where(file => Regex.IsMatch(file, @"BFEP\.IAG(AG|ZZ)\.K0\d{0,6}$"));
			foreach (var file in bgcFilesInRemoteFTP) 
			{
				System.IO.File.Delete(file);
			}
		}

		protected string CreateExpectedConsentFileData(BGConsentToSend consent)
		{
			return Factory.GetConsentData(
				consent,
				bgServiceCoordinatorSettingsService.GetPaymentRevieverCustomerNumber(),
				bgServiceCoordinatorSettingsService.GetPaymentRecieverBankGiroNumber()
			);
		}

		protected string CreateExpectedPaymentFileData(BGPaymentToSend payment) 
		{
			return Factory.GetPaymentData(
				payment, 
				bgServiceCoordinatorSettingsService.GetPaymentRevieverCustomerNumber(),   
				bgServiceCoordinatorSettingsService.GetPaymentRecieverBankGiroNumber()
			);
		}
	}
}