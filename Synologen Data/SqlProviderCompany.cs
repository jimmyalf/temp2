// ==========================================================================
//
//  PROGRAM
//	
//
//  FILENAME
//	$Workfile: SqlProviderCompany.cs $
//
//  ARCHIVE
//  $Archive: /Develop/WPC/Version 4.1/CustomerSpecific/Synologen/Synologen Data/SqlProviderCompany.cs $
//
//  VERSION
//	$Revision: 3 $
//
//  DATES
//	Last check in: $Date: 09-03-04 14:18 $
//	Last modified: $Modtime: 09-03-02 12:32 $
//
//  AUTHOR(S)
//	$Author: Cber $
// 	
//
//  COPYRIGHT
// 	Copyright (c) 2009 Spinit AB --- ALL RIGHTS RESERVED
// 	Spinit AB, Datav�gen 2, 436 32 Askim, SWEDEN
//
// ==========================================================================
// 
//  DESCRIPTION
//  
//
// ==========================================================================
//
//	History
//
//	$Log: /Develop/WPC/Version 4.1/CustomerSpecific/Synologen/Synologen Data/SqlProviderCompany.cs $
//
//3     09-03-04 14:18 Cber
//
//2     09-02-25 18:00 Cber
//Changes to allow for SPCS Account property on ArticleConnection,
//OrderItemRow and IOrderItem
//
//1     09-02-05 18:01 Cber
//
//3     09-01-27 10:58 Cber
//
//2     09-01-09 17:44 Cber
//
//1     09-01-08 18:08 Cber
// 
// ==========================================================================
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Spinit.Wpc.Synologen.Business.Enumeration;
using Spinit.Wpc.Synologen.Data.Types;
using Spinit.Wpc.Utility.Business;
namespace Spinit.Wpc.Synologen.Data {
	public partial class SqlProvider {

		public bool AddUpdateDeleteCompany(Enumerations.Action action, ref CompanyRow company) {
			try {
				int numAffected = 0;
				SqlParameter[] parameters = {
		            new SqlParameter("@type", SqlDbType.Int, 4),
					new SqlParameter("@contractId", SqlDbType.Int, 4),
		            new SqlParameter("@name", SqlDbType.NVarChar, 50),
		            new SqlParameter("@address1", SqlDbType.NVarChar, 50),
		            new SqlParameter("@address2", SqlDbType.NVarChar, 50),
		            new SqlParameter("@zip", SqlDbType.NVarChar, 50),
		            new SqlParameter("@city", SqlDbType.NVarChar, 50),
					new SqlParameter("@companyCode", SqlDbType.NVarChar, 16),
					new SqlParameter("@bankCode", SqlDbType.NVarChar, 50),
					new SqlParameter("@active", SqlDbType.Bit),
					new SqlParameter("@organizationNumber", SqlDbType.NVarChar, 50),
					new SqlParameter("@invoiceCompanyName", SqlDbType.NVarChar, 50),
					new SqlParameter("@taxAccountingCode", SqlDbType.NVarChar, 50),
					new SqlParameter("@paymentDuePeriod", SqlDbType.Int, 4),
					new SqlParameter("@ediRecipientId", SqlDbType.NVarChar, 50),
					new SqlParameter("@invoicingMethodId", SqlDbType.Int, 4 ),
		            new SqlParameter("@status", SqlDbType.Int, 4),
		            new SqlParameter("@id", SqlDbType.Int, 4)
		        };

				int counter = 0;
				parameters[counter++].Value = (int)action;
				if (action == Enumerations.Action.Create || action == Enumerations.Action.Update) {
					parameters[counter++].Value = company.ContractId;
					parameters[counter++].Value = GetNullableSqlType(company.Name);
					parameters[counter++].Value = GetNullableSqlType(company.Address1);
					parameters[counter++].Value = GetNullableSqlType(company.Address2);
					parameters[counter++].Value = GetNullableSqlType(company.Zip);
					parameters[counter++].Value = GetNullableSqlType(company.City);
					parameters[counter++].Value = GetNullableSqlType(company.CompanyCode);
					parameters[counter++].Value = GetNullableSqlType(company.BankCode);
					parameters[counter++].Value = company.Active;
					parameters[counter++].Value = GetNullableSqlType(company.OrganizationNumber);
					parameters[counter++].Value = GetNullableSqlType(company.InvoiceCompanyName);
					parameters[counter++].Value = GetNullableSqlType(company.TaxAccountingCode);
					parameters[counter++].Value = company.PaymentDuePeriod;
					parameters[counter++].Value = GetNullableSqlType(company.EDIRecipientId);
					parameters[counter++].Value = company.InvoicingMethodId;

				}
				parameters[parameters.Length - 2].Direction = ParameterDirection.Output;
				if (action == Enumerations.Action.Create) {
					parameters[parameters.Length - 1].Direction = ParameterDirection.Output;
				}
				else {
					parameters[parameters.Length - 1].Value = company.Id;
				}
				RunProcedure("spSynologenAddUpdateDeleteCompany", parameters, out numAffected);

				if (((int)parameters[parameters.Length - 2].Value) == 0) {
					company.Id = (int)parameters[parameters.Length - 1].Value;
					return true;
				}
				throw new GeneralData.DatabaseInterface.DataException("Insert failed. Error: " + (int)parameters[parameters.Length - 2].Value, (int)parameters[parameters.Length - 2].Value);
			}
			catch (SqlException e) {
				throw new GeneralData.DatabaseInterface.DataException("SqlException: " + e);
			}
		}

		public CompanyRow GetCompanyRow(int companyId) {
			var contractCompanyDataSet = GetCompanies(companyId, 0, null, ActiveFilter.Both);
			var contractCompanyDataRow = contractCompanyDataSet.Tables[0].Rows[0];
			return ParseCompanyRow(contractCompanyDataRow);
		}

		private CompanyRow ParseCompanyRow(DataRow dataRow) {
			try {
				var companyRow = new CompanyRow {
					Address1 = Util.CheckNullString(dataRow, "cAddress1"), 
					Address2 = Util.CheckNullString(dataRow, "cAddress2"), 
					City = Util.CheckNullString(dataRow, "cCity"), 
					Id = Util.CheckNullInt(dataRow, "cId"), 
					ContractId = Util.CheckNullInt(dataRow, "cContractCustomerId"), 
					Name = Util.CheckNullString(dataRow, "cName"), 
					Zip = Util.CheckNullString(dataRow, "cZip"), 
					CompanyCode = Util.CheckNullString(dataRow, "cCompanyCode"), 
					BankCode = Util.CheckNullString(dataRow, "cBankCode"), 
					Active = (bool) dataRow["cActive"], 
					OrganizationNumber = Util.CheckNullString(dataRow, "cOrganizationNumber"), 
					InvoiceCompanyName = Util.CheckNullString(dataRow, "cInvoiceCompanyName"), 
					TaxAccountingCode = Util.CheckNullString(dataRow, "cTaxAccountingCode"), 
					PaymentDuePeriod = Util.CheckNullInt(dataRow, "cPaymentDuePeriod"), 
					EDIRecipientId = Util.CheckNullString(dataRow, "cEDIRecipientId"), 
					InvoicingMethodId = Util.CheckNullInt(dataRow, "cInvoicingMethodId"),
				};
				companyRow.CompanyValidationRules = new List<CompanyValidationRule>(GetCompanyValidationRules(null, companyRow.Id));
				return companyRow;
			}
			catch (Exception ex) {
				throw new Exception("Exception found while parsing a CompanyRow object: " + ex.Message);
			}
		}

		public DataSet GetCompanies(int companyId, int contractId, string orderBy, ActiveFilter activeFilter) {
			try {
				int counter = 0;
				SqlParameter[] parameters = {
					new SqlParameter ("@activeType", SqlDbType.Int, 4),
					new SqlParameter ("@companyId", SqlDbType.Int, 4),
					new SqlParameter ("@contractId", SqlDbType.Int, 4),
					new SqlParameter ("@orderBy", SqlDbType.NVarChar, 255),
					new SqlParameter ("@status", SqlDbType.Int, 4)
				};
				parameters[counter++].Value = (int)activeFilter;
				parameters[counter++].Value = companyId;
				parameters[counter++].Value = contractId;
				parameters[counter++].Value = orderBy ?? SqlString.Null;
				parameters[counter].Direction = ParameterDirection.Output;
				DataSet retSet = RunProcedure("spSynologenGetContractCompanies", parameters, "tblSynologenContractCompany");
				return retSet;
			}
			catch (SqlException e) {
				throw new GeneralData.DatabaseInterface.DataException("SqlException: " + e);
			}
		}

		public DataSet GetCompanyValidationRulesDataSet(int? validationRuleId, int? companyId) {
			try {
				var counter = 0;
				SqlParameter[] parameters = {
					new SqlParameter ("@validationRuleId", SqlDbType.Int, 4),
					new SqlParameter ("@companyId", SqlDbType.Int, 4),
					new SqlParameter ("@status", SqlDbType.Int, 4)
				};
				parameters[counter++].Value = GetNullableSqlType(validationRuleId);
				parameters[counter++].Value = GetNullableSqlType(companyId);
				parameters[counter].Direction = ParameterDirection.Output;
				var retSet = RunProcedure("spSynologenGetCompanyValidationRules", parameters, "tblSynologenCompanyValidationRules");
				return retSet;
			}
			catch (SqlException e) {
				throw new GeneralData.DatabaseInterface.DataException("GetCompanyValidationRules failed", e);
			}
		}

		private IList<CompanyValidationRule> GetCompanyValidationRules(int? validationRuleId, int? companyId) {
			var validationRuleList = new List<CompanyValidationRule>();
			var validationRulesDataSet = GetCompanyValidationRulesDataSet(validationRuleId, companyId);
			if(validationRulesDataSet == null) return new List<CompanyValidationRule>();
			if(validationRulesDataSet.Tables.Count <= 0) return new List<CompanyValidationRule>();
			if(validationRulesDataSet.Tables[0].Rows.Count <= 0) return new List<CompanyValidationRule>();
			foreach (DataRow dataRow in validationRulesDataSet.Tables[0].Rows){
				var validationRule = ParseCompanyValidationRule(dataRow);
				validationRuleList.Add(validationRule);
			}
			return validationRuleList;
		}

		private static CompanyValidationRule ParseCompanyValidationRule(DataRow row) {
			return new CompanyValidationRule {
				Id = Util.CheckNullInt(row, "cId"), 
				ValidationName = Util.CheckNullString(row, "cValidationName"), 
				ValidationDescription = Util.CheckNullString(row, "cValidationDescription"), 
				ControlToValidate = Util.CheckNullString(row, "cControlToValidate"), 
				ValidationRegex = Util.CheckNullString(row, "cValidationRegex"), 
				ErrorMessage = Util.CheckNullString(row, "cErrorMessage"), 
				ValidationType = (ValidationType) Util.CheckNullInt(row, "cValidationType")
			};
		}

		public bool CompanyHasConnectedOrders(int companyId) {
			var companyDataSet = GetOrders(0, 0, 0, 0, companyId, 0, 0, null);
			return DataSetHasRows(companyDataSet);
		}

		public int ConnectCompanyToValidationRule(int companyId, int validationRuleId) { 
			try {
				var counter = 0;
				int rowsAffected;
				SqlParameter[] parameters = {
					new SqlParameter ("@validationRuleId", SqlDbType.Int, 4),
					new SqlParameter ("@companyId", SqlDbType.Int, 4),
					new SqlParameter ("@status", SqlDbType.Int, 4)
				};
				parameters[counter++].Value = GetNullableSqlType(validationRuleId);
				parameters[counter++].Value = GetNullableSqlType(companyId);
				parameters[counter].Direction = ParameterDirection.Output;
				RunProcedure("spSynologenConnectCompanyToValidationRule", parameters, out rowsAffected);
				if (((int)parameters[parameters.Length - 1].Value) == 0) { return rowsAffected; }
				throw new GeneralData.DatabaseInterface.DataException("Insert failed. Error: " + (int)parameters[parameters.Length - 1].Value, (int)parameters[parameters.Length - 1].Value);
			}
			catch (SqlException e) {
				throw new GeneralData.DatabaseInterface.DataException("ConnectCompanyToValidationRule failed", e);
			}
		}

		public int DisconnectCompanyFromValidationRule(int companyId, int validationRuleId) { 
			try {
				var counter = 0;
				int rowsAffected;
				SqlParameter[] parameters = {
					new SqlParameter ("@validationRuleId", SqlDbType.Int, 4),
					new SqlParameter ("@companyId", SqlDbType.Int, 4),
					new SqlParameter ("@status", SqlDbType.Int, 4)
				};
				parameters[counter++].Value = GetNullableSqlType(validationRuleId);
				parameters[counter++].Value = GetNullableSqlType(companyId);
				parameters[counter].Direction = ParameterDirection.Output;
				RunProcedure("spSynologenDisconnectCompanyFromValidationRule", parameters, out rowsAffected);
				if (((int)parameters[parameters.Length - 1].Value) == 0) { return rowsAffected; }
				throw new GeneralData.DatabaseInterface.DataException("Delete failed. Error: " + (int)parameters[parameters.Length - 1].Value, (int)parameters[parameters.Length - 1].Value);
			}
			catch (SqlException e) {
				throw new GeneralData.DatabaseInterface.DataException("DisconnectCompanyFromValidationRule failed", e);
			}		
		}
	}
}