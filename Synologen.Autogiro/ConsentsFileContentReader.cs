using System.Linq;
using Spinit.Wp.Synologen.Autogiro.Helpers;
using Spinit.Wpc.Synologen.Core.Domain.Model.Autogiro.CommonTypes;
using Spinit.Wpc.Synologen.Core.Domain.Model.Autogiro.Recieve;
using Spinit.Wpc.Synologen.Core.Extensions;

namespace Spinit.Wp.Synologen.Autogiro
{
	public class ConsentsFileContentReader : BaseContentReader, IFileReader<ConsentsFile, Consent> 
	{
		public ConsentsFileContentReader(string fileContents) : base(fileContents) {  }
		public ConsentsFile Read(IItemReader<Consent> itemReader)
		{
			return new ConsentsFile
			{
				PaymentRecieverBankgiroNumber = FirstRow.ReadFrom(15).To(24).TrimStart('0'),
                WriteDate = FirstRow.ReadFrom(3).To(10).ParseDate(),
                NumberOfItemsInFile = LastRow.ReadFrom(15).To(21).ToInt(),
				Posts = AllRowsButFirstAndLast.Select(line => itemReader.Read(line)),
			};
		}
	}
}