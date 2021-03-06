using System;
using System.Collections.Generic;
using System.Linq;

namespace Spinit.Wpc.Synologen.Core.Domain.Model.LensSubscription
{
	public class SubscriptionTransaction : Entity
	{
		public virtual Subscription Subscription { get; set; }
		public virtual decimal Amount { get; set; }
		public virtual TransactionType Type { get; set; }
		public virtual TransactionReason Reason { get; set; }
		public virtual DateTime CreatedDate { get; set; }
		public virtual Settlement Settlement { get; set; }
		public virtual TransactionArticle Article { get; set;}

		public static decimal GetCurrentAccountBalance(IEnumerable<SubscriptionTransaction> transactions)
		{
			if (transactions == null || !transactions.Any()) return 0;
			Func<SubscriptionTransaction, bool> isWithdrawal = transaction => (transaction.Reason == TransactionReason.Withdrawal || transaction.Reason == TransactionReason.Correction) && transaction.Type == TransactionType.Withdrawal;
			Func<SubscriptionTransaction, bool> isDeposit = transaction => (transaction.Reason == TransactionReason.Payment || transaction.Reason == TransactionReason.Correction) && transaction.Type == TransactionType.Deposit;
			var withdrawals = transactions.Where(isWithdrawal).Sum(x => x.Amount);
			var deposits = transactions.Where(isDeposit).Sum(x => x.Amount);
			return deposits - withdrawals;
		}
	}
}