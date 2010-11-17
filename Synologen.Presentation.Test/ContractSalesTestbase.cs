using System;
using Moq;
using NUnit.Framework;
using Spinit.Wpc.Synologen.Business.Domain.Interfaces;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.ContractSales;
using Spinit.Wpc.Synologen.Core.Domain.Persistence.LensSubscription;
using Spinit.Wpc.Synologen.Core.Domain.Services;
using Spinit.Wpc.Synologen.Presentation.Application.Services;
using Spinit.Wpc.Synologen.Presentation.Controllers;

namespace Spinit.Wpc.Synologen.Presentation.Test
{
	[TestFixture]
	public abstract class ContractSalesTestbase
	{
		protected ContractSalesTestbase()
		{
			Context = () => { };
			Because = controller =>  { throw new AssertionException("An action for Because has not been set!"); };
		}

		[SetUp]
		protected void SetUpTest()
		{
			MockedSettlementRepository = new Mock<ISettlementRepository>();
			MockedSettingsService = new Mock<IAdminSettingsService>();
			MockedContractSaleRepository = new Mock<IContractSaleRepository>();
			MockedSynologenSqlProvider = new Mock<ISqlProvider>();
			MockedTransactionRepository = new Mock<ITransactionRepository>();
			ViewService = new ContractSalesViewService(
				MockedSettlementRepository.Object,
				MockedContractSaleRepository.Object, 
				MockedSettingsService.Object,
				MockedTransactionRepository.Object,
				MockedSynologenSqlProvider.Object);
			Controller = new ContractSalesController(ViewService);
			Context();
			Because(Controller);
		}

		protected Action Context;
		protected Action<ContractSalesController> Because;
		protected Mock<ISettlementRepository> MockedSettlementRepository;
		protected Mock<IAdminSettingsService> MockedSettingsService;
		protected Mock<IContractSaleRepository> MockedContractSaleRepository;
		protected Mock<ISqlProvider> MockedSynologenSqlProvider;
		protected Mock<ITransactionRepository> MockedTransactionRepository;
		protected ContractSalesViewService ViewService;
		protected ContractSalesController Controller;
	}
}