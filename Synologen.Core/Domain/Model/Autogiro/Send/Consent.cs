using Spinit.Wpc.Synologen.Core.Domain.Model.Autogiro.CommonTypes;

namespace Spinit.Wpc.Synologen.Core.Domain.Model.Autogiro.Send
{
	public class Consent
	{
		public ConsentType Type { get; set; }
		public Payer Transmitter { get; set; }
		public string RecieverBankgiroNumber{ get; set; }
		public Account Account { get; set; }
		public string PersonalIdNumber { get; set; }
		public string OrgNumber { get; set; }
	}
}