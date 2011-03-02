using System;
using Spinit.Wpc.Synologen.Core.Domain.Services;

namespace Synologen.LensSubscription.BGServiceCoordinator.App.Services
{
	public class BGSentFileWriterService : IFileWriterService
	{
		private readonly IFileIOService _fileIOService;
		private readonly IBGConfigurationSettingsService _bgConfigurationSettingsService;
		private readonly DateTime _writeDate;

		public BGSentFileWriterService(IFileIOService fileIOService, IBGConfigurationSettingsService bgConfigurationSettingsService, DateTime writeDate)
		{
			_fileIOService = fileIOService;
			_bgConfigurationSettingsService = bgConfigurationSettingsService;
			_writeDate = writeDate;
		}

		public void WriteFileToDisk(string fileContents, string fileName)
		{
			if(fileName.Contains("\\") || fileName.Contains(":"))
			{
				throw new ArgumentException("Filename contains illegal characters", "fileName");
			}
			var newFileName = GetUniqueFileName(fileName);
			var filePath = GetSaveFilePath(_bgConfigurationSettingsService, newFileName);
			if(_fileIOService.FileExists(filePath))
			{
				throw new ArgumentException(String.Format("A file ({0}) already exists.", filePath), "fileName");
			}

			_fileIOService.WriteFile(filePath, fileContents);
		}

		private static string GetSaveFilePath(IBGConfigurationSettingsService bgConfigurationSettingsService, string fileName)
		{
			var sentFilesFolderPath = bgConfigurationSettingsService.GetSentFilesFolderPath();
			var directory = new System.IO.DirectoryInfo(sentFilesFolderPath);
			if(!directory.Exists) directory.Create();
			return System.IO.Path.Combine(sentFilesFolderPath, fileName);
		}

		private string GetUniqueFileName(string originalFileName)
		{
			return string.Format("{0}.D{1}.T{2}",
				originalFileName,
				_writeDate.ToString("yyMMdd"),
				_writeDate.ToString("HHmmss"));
		}
	}
}