using System;

namespace Spinit.Wpc.Synologen.Core.Domain.Model.BGWebService
{
	public class RecievedConsent
	{
		public int PayerId { get; set; }
		public int ConsentId { get; set; }
		public DateTime ActionDate { get; set; }
		public DateTime? ConsentValidForDate { get; set; }
		public ConsentInformationCode? InformationCode { get; set; }
		public ConsentCommentCode CommentCode { get; set; }
	}
}