using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Spinit.Extensions;
using Spinit.Wpc.Synologen.Business.Domain.Enumerations;
using Spinit.Wpc.Synologen.Business.Domain.Interfaces;
using Spinit.Wpc.Synologen.Core.Domain.Model.ContractSales;
using Spinit.Wpc.Utility.Business;
using Shop=Spinit.Wpc.Synologen.Business.Domain.Entities.Shop;

namespace Spinit.Wpc.Synologen.Data {
	public partial class SqlProvider {

		public bool AddUpdateDeleteShop(Enumerations.Action action, ref Shop shop) {
			try {
				int numAffected;
				SqlParameter[] parameters = {
            		new SqlParameter("@type", SqlDbType.Int, 4),
					new SqlParameter("@categoryId", SqlDbType.Int, 4),
            		new SqlParameter("@shopName", SqlDbType.NVarChar, 50),
            		new SqlParameter("@shopNumber", SqlDbType.NVarChar, 50),
					new SqlParameter("@organizationNumber", SqlDbType.NVarChar, 50),
            		new SqlParameter("@shopDescription", SqlDbType.NVarChar, 255),
            		new SqlParameter("@contactFirstName", SqlDbType.NVarChar, 50),
            		new SqlParameter("@contactLastName", SqlDbType.NVarChar, 50),
            		new SqlParameter("@email", SqlDbType.NVarChar, 50),
            		new SqlParameter("@phone", SqlDbType.NVarChar, 50),
            		new SqlParameter("@phone2", SqlDbType.NVarChar, 50),
            		new SqlParameter("@fax", SqlDbType.NVarChar, 50),
            		new SqlParameter("@url", SqlDbType.NVarChar, 50),
            		new SqlParameter("@mapUrl", SqlDbType.NVarChar, 255),
            		new SqlParameter("@address", SqlDbType.NVarChar, 50),
            		new SqlParameter("@address2", SqlDbType.NVarChar, 50),
            		new SqlParameter("@zip", SqlDbType.NVarChar, 50),
            		new SqlParameter("@city", SqlDbType.NVarChar, 50),
                    new SqlParameter("@latitude", SqlDbType.Decimal), 
                    new SqlParameter("@longitude", SqlDbType.Decimal), 
            		new SqlParameter("@active", SqlDbType.Bit),
            		new SqlParameter("@giroId", SqlDbType.Int,4),
            		new SqlParameter("@giroNumber", SqlDbType.NVarChar, 50),
            		new SqlParameter("@giroSupplier", SqlDbType.NVarChar, 100),
					new SqlParameter("@shopAccess", SqlDbType.Int, 4), 
					new SqlParameter("@externalAccessUsername", SqlDbType.NVarChar, 50), 
					new SqlParameter("@externalAccessHashedPassword", SqlDbType.NVarChar, 50),
					new SqlParameter("@shopGroupId", SqlDbType.Int),
            		new SqlParameter("@status", SqlDbType.Int, 4),
            		new SqlParameter("@id", SqlDbType.Int, 4),
				};

				var counter = 0;
				parameters[counter++].Value = (int)action;
				if (action == Enumerations.Action.Create || action == Enumerations.Action.Update) {
					parameters[counter++].Value = shop.CategoryId;
					parameters[counter++].Value = shop.Name ?? SqlString.Null;
					parameters[counter++].Value = shop.Number ?? SqlString.Null;
					parameters[counter++].Value = shop.OrganizationNumber ?? SqlString.Null;
					parameters[counter++].Value = shop.Description ?? SqlString.Null;
					parameters[counter++].Value = shop.ContactFirstName ?? SqlString.Null;
					parameters[counter++].Value = shop.ContactLastName ?? SqlString.Null;
					parameters[counter++].Value = shop.Email ?? SqlString.Null;
					parameters[counter++].Value = shop.Phone ?? SqlString.Null;
					parameters[counter++].Value = shop.Phone2 ?? SqlString.Null;
					parameters[counter++].Value = shop.Fax ?? SqlString.Null;
					parameters[counter++].Value = shop.Url ?? SqlString.Null;
					parameters[counter++].Value = shop.MapUrl ?? SqlString.Null;
					parameters[counter++].Value = shop.Address ?? SqlString.Null;
					parameters[counter++].Value = shop.Address2 ?? SqlString.Null;
					parameters[counter++].Value = shop.Zip ?? SqlString.Null;
					parameters[counter++].Value = shop.City ?? SqlString.Null;
				    parameters[counter++].Value = shop.Latitude > 0 ? shop.Latitude : SqlDecimal.Null;
				    parameters[counter++].Value = shop.Longitude > 0 ? shop.Longitude : SqlDecimal.Null;
					parameters[counter++].Value = shop.Active;
					parameters[counter++].Value = shop.GiroId > 0 ? shop.GiroId : SqlInt32.Null;
					parameters[counter++].Value = shop.GiroNumber ?? SqlString.Null;
					parameters[counter++].Value = shop.GiroSupplier ?? SqlString.Null;
					parameters[counter++].Value = shop.Access;
					parameters[counter++].Value = shop.ExternalAccessUsername ?? SqlString.Null;
					parameters[counter++].Value = shop.ExternalAccessHashedPassword ?? SqlString.Null;
					parameters[counter].Value = shop.ShopGroupId ?? SqlInt32.Null;
				}
				parameters[parameters.Length - 2].Direction = ParameterDirection.Output;
				if (action == Enumerations.Action.Create) {
					parameters[parameters.Length - 1].Direction = ParameterDirection.Output;
				}
				else {
					parameters[parameters.Length - 1].Value = shop.ShopId;
				}

				RunProcedure("spSynologenAddUpdateDeleteShop", parameters, out numAffected);

				if (((int)parameters[parameters.Length - 2].Value) == 0) {
					shop.ShopId = (int)parameters[parameters.Length - 1].Value;
					return true;
				}
				throw new GeneralData.DatabaseInterface.DataException("Insert failed. Error: " + (int)parameters[parameters.Length - 2].Value, (int)parameters[parameters.Length - 2].Value);
			}
			catch (SqlException e) {
				throw new GeneralData.DatabaseInterface.DataException("SqlException: " + e);
			}
		}

		public DataSet GetShopsByPage(int categoryId, int contractCustomerId, int equipmentId, string searchString, string orderBy, int currentPage, int pageSize, ref int totalSize) {

			try {
				var counter = 0;

				SqlParameter[] parameters = {
					new SqlParameter ("@categoryId", SqlDbType.Int, 4),
					new SqlParameter ("@contractCustomerId", SqlDbType.Int, 4),
					new SqlParameter ("@equipmentId", SqlDbType.Int, 4),
					new SqlParameter ("@SearchString", SqlDbType.NVarChar, 255),
					new SqlParameter ("@OrderBy", SqlDbType.NVarChar, 255),
					new SqlParameter ("@CurrentPage", SqlDbType.Int, 4),
					new SqlParameter ("@PageSize", SqlDbType.Int, 4)
				};
				parameters[counter++].Value = categoryId;
				parameters[counter++].Value = contractCustomerId;
				parameters[counter++].Value = equipmentId;
				parameters[counter++].Value = searchString ?? SqlString.Null;
				parameters[counter++].Value = orderBy ?? SqlString.Null;
				parameters[counter++].Value = currentPage;
				parameters[counter].Value = pageSize;
				var retSet = RunProcedure("spSynologenGetShopsByPage", parameters, "tblSynologenShop");
				if (retSet.Tables.Count > 1) totalSize = Convert.ToInt32(retSet.Tables[1].Rows[0][0]);
				return retSet;

			}
			catch (SqlException e) {
				throw new GeneralData.DatabaseInterface.DataException("SqlException: "+ e);
			}

		}

		public Shop GetShop(int shopId) {
			try {
				var shopDataSet = GetShops(shopId, null, null, null, null, null, null, "cId");
				var shopDataRow = shopDataSet.Tables[0].Rows[0];
				return ParseShopRow(shopDataRow);
			}
			catch (Exception ex) {
				throw new Exception("Exception found while parsing a Shop object.", ex);
			}
		}

		public Shop ParseShopRow(DataRow shopDataRow)
		{
		    var accessValue = Util.CheckNullInt(shopDataRow, "cShopAccess");
		    var access = accessValue.ToEnum<ShopAccess>();
			var shopRow = new Shop
			{
				ShopId = Util.CheckNullInt(shopDataRow, "cId"),
				Active = (bool) shopDataRow["cActive"],
				Address = Util.CheckNullString(shopDataRow, "cAddress"),
				Address2 = Util.CheckNullString(shopDataRow, "cAddress2"),
				City = Util.CheckNullString(shopDataRow, "cCity"),
                Latitude = Util.CheckNullDecimal(shopDataRow, "cLatitude"),
                Longitude = Util.CheckNullDecimal(shopDataRow, "cLongitude"),
				ContactFirstName = Util.CheckNullString(shopDataRow, "cContactFirstName"),
				ContactLastName = Util.CheckNullString(shopDataRow, "cContactLastName"),
				Description = Util.CheckNullString(shopDataRow, "cShopDescription"),
				Email = Util.CheckNullString(shopDataRow, "cEmail"),
				Fax = Util.CheckNullString(shopDataRow, "cFax"),
				MapUrl = Util.CheckNullString(shopDataRow, "cMapUrl"),
				Name = Util.CheckNullString(shopDataRow, "cShopName"),
				Number = Util.CheckNullString(shopDataRow, "cShopNumber"),
				Phone = Util.CheckNullString(shopDataRow, "cPhone"),
				Phone2 = Util.CheckNullString(shopDataRow, "cPhone2"),
				Url = Util.CheckNullString(shopDataRow, "cUrl"),
				Zip = Util.CheckNullString(shopDataRow, "cZip"),
				CategoryId = Util.CheckNullInt(shopDataRow, "cCategoryId"),
				GiroId = Util.CheckNullInt(shopDataRow, "cGiroId"),
				GiroNumber = Util.CheckNullString(shopDataRow, "cGiroNumber"),
				GiroSupplier = Util.CheckNullString(shopDataRow, "cGiroSupplier"),
				Equipment = GetAllEquipmentRowsPerShop(Util.CheckNullInt(shopDataRow, "cId")),
				Access = access,
                OrganizationNumber = Util.CheckNullString(shopDataRow, "cOrganizationNumber"),
				ExternalAccessUsername = Util.CheckNullString(shopDataRow, "cExternalAccessUsername"),
				ExternalAccessHashedPassword = Util.CheckNullString(shopDataRow, "cExternalAccessHashedPassword")
			};
			var concernId = Util.CheckNullInt(shopDataRow, "cConcernId");
			var shopGroupId = Util.CheckNullInt(shopDataRow, "cShopGroupId");
			shopRow.Concern = (concernId>0) ? GetConcern(concernId) : null;
			shopRow.ShopGroupId = (shopGroupId > 0) ? shopGroupId : (int?) null;
			return shopRow;
		}

		public DataSet GetShops(int? shopId, int? shopCategoryId, int? contractCustomer, int? memberId, int? equipmentId, bool? includeInactive, int? concernId, string orderBy) {
			try {
				var counter = 0;
				SqlParameter[] parameters = {
						new SqlParameter ("@shopId", SqlDbType.Int, 4),
						new SqlParameter ("@shopCategoryId", SqlDbType.Int, 4),
						new SqlParameter ("@contractCustomerId", SqlDbType.Int, 4),
						new SqlParameter ("@memberId", SqlDbType.Int, 4),
						new SqlParameter ("@equipmentId", SqlDbType.Int, 4),
						new SqlParameter ("@includeInactive", SqlDbType.Bit),
						new SqlParameter ("@concernId", SqlDbType.Int, 4),
						new SqlParameter ("@orderBy", SqlDbType.NVarChar, 255),
						new SqlParameter ("@status", SqlDbType.Int, 4)
					};
				parameters[counter++].Value = GetNullableSqlType(shopId);
				parameters[counter++].Value = GetNullableSqlType(shopCategoryId);
				parameters[counter++].Value = GetNullableSqlType(contractCustomer);
				parameters[counter++].Value = GetNullableSqlType(memberId);
				parameters[counter++].Value = GetNullableSqlType(equipmentId);
				parameters[counter++].Value = GetNullableSqlType(includeInactive);
				parameters[counter++].Value = GetNullableSqlType(concernId);
				parameters[counter++].Value = GetNullableSqlType(orderBy);
				parameters[counter].Direction = ParameterDirection.Output;
				var retSet = RunProcedure("spSynologenGetShops", parameters, "tblSynologenShop");
				return retSet;
			}
			catch (SqlException e) {
				throw new GeneralData.DatabaseInterface.DataException("SqlException: " + e);
			}
		}

		public IEnumerable<IShop> GetShopRows(int? shopId, int? shopCategoryId, int? contractCustomer,int? memberId, int? equipmentId, bool? includeInactive, int? concernId, string orderBy) {
			var shopRows = new Collection<IShop>();
			var dataSet = GetShops(shopId, shopCategoryId, contractCustomer, memberId, equipmentId, includeInactive,concernId, orderBy);
			if(dataSet.Tables[0]==null) return shopRows;
			foreach (DataRow dataRow in dataSet.Tables[0].Rows) {
				shopRows.Add(ParseShopRow(dataRow));
			}
			return shopRows;

		}

		public List<int> GetAllShopIdsPerCategory(int categoryId) {
			var returnList = new List<int>();
			var shops = GetShops(null, categoryId, null, null, null, null, null, null);
			if(shops == null || shops.Tables[0] == null) return returnList;
			foreach(DataRow row in shops.Tables[0].Rows) {
				returnList.Add(Util.CheckNullInt(row, "cId"));
			}
			return returnList;
		}

		public List<int> GetAllShopIdsPerMember(int memberId) {
			var returnList = new List<int>();
			var shops = GetShops(null, null, null, memberId, null, null, null, null);
			if (shops == null || shops.Tables[0] == null) return returnList;
			foreach (DataRow row in shops.Tables[0].Rows) {
				returnList.Add(Util.CheckNullInt(row, "cId"));
			}
			return returnList;
		}

		public bool ShopHasConnectedOrders(int shopId) {
			var orderDataSet = GetOrders(0, shopId, 0, 0, 0, 0, 0, null);
			return DataSetHasRows(orderDataSet);
		}

		public bool ShopHasConnectedMembers(int shopId) {
			var memberDataSet = GetSynologenMembers(0, shopId, 0, null);
			return DataSetHasRows(memberDataSet);
		}

		public bool ShopHasConnectedContracts(int shopId) {
			var contractDataSet = GetContracts(FetchCustomerContract.AllPerShop, 0,shopId, null);
			return DataSetHasRows(contractDataSet);
		}

		#region Shop-Contract connection

		private bool UpdateShopContractConnection(ConnectionAction action, int shopId, int contractCustomerId) {
			try {
				int numAffected;
				SqlParameter[] parameters = {
            		new SqlParameter("@type", SqlDbType.Int, 4),
					new SqlParameter("@shopId", SqlDbType.Int, 4),
            		new SqlParameter("@contractCustomerId", SqlDbType.Int, 4),
            		new SqlParameter("@status", SqlDbType.Int, 4)
				};

				int counter = 0;
				parameters[counter++].Value = (int)action;
				parameters[counter++].Value = shopId;
				parameters[counter++].Value = contractCustomerId;
				parameters[counter].Direction = ParameterDirection.Output;
				RunProcedure("spSynologenUpdateShopContractConnection", parameters, out numAffected);

				if (((int)parameters[parameters.Length - 1].Value) == 0) {
					return true;
				}
				throw new GeneralData.DatabaseInterface.DataException("UpdateShopContractConnection failed. Error: " + (int)parameters[parameters.Length - 1].Value, (int)parameters[parameters.Length - 1].Value);
			}
			catch (SqlException e) {
				throw new GeneralData.DatabaseInterface.DataException("SqlException: " + e);
			}
		}

		public bool DisconnectShopFromAllContracts(int shopId) {
			const ConnectionAction action = ConnectionAction.Delete;
			return UpdateShopContractConnection(action, shopId, 0);
		}


		public bool ConnectShopToContract(int shopId, int contractCustomerId) {
			const ConnectionAction action = ConnectionAction.Connect;
			return UpdateShopContractConnection(action, shopId, contractCustomerId);
		}

		#endregion

		#region Shop-Member connection

		private bool UpdateShopMemberConnection(ShopMemberConnectionAction action, int shopId, int memberId) {
			try {
				int numAffected;
				SqlParameter[] parameters = {
            		new SqlParameter("@type", SqlDbType.Int, 4),
					new SqlParameter("@shopId", SqlDbType.Int, 4),
            		new SqlParameter("@memberId", SqlDbType.Int, 4),
            		new SqlParameter("@status", SqlDbType.Int, 4)
				};

				int counter = 0;
				parameters[counter++].Value = (int)action;
				parameters[counter++].Value = shopId;
				parameters[counter++].Value = memberId;
				parameters[counter].Direction = ParameterDirection.Output;
				RunProcedure("spSynologenUpdateShopMemberConnection", parameters, out numAffected);

				if (((int)parameters[parameters.Length - 1].Value) == 0) {
					return true;
				}
				throw new GeneralData.DatabaseInterface.DataException("UpdateShopMemberConnection failed. Error: " + (int)parameters[parameters.Length - 1].Value, (int)parameters[parameters.Length - 1].Value);
			}
			catch (SqlException e) {
				throw new GeneralData.DatabaseInterface.DataException("SqlException: " + e);
			}
		}

		public bool DisconnectAllShopsFromMember(int memberId) {
			const ShopMemberConnectionAction action = ShopMemberConnectionAction.DeleteAllPerMember;
			return UpdateShopMemberConnection(action,0, memberId);
		}

		public bool ConnectShopToMember(int shopId, int memberId) {
			const ShopMemberConnectionAction action = ShopMemberConnectionAction.ConnectShopAndMember;
			return UpdateShopMemberConnection(action, shopId, memberId);
		}

		#endregion

		#region Giro
		public DataSet GetGiros(int giroId, string orderBy) {
			try {
				var counter = 0;
				SqlParameter[] parameters = {
					new SqlParameter ("@giroId", SqlDbType.Int, 4),
					new SqlParameter ("@orderBy", SqlDbType.NVarChar, 255),
					new SqlParameter ("@status", SqlDbType.Int, 4)
				};
				parameters[counter++].Value = giroId;
				parameters[counter++].Value = orderBy ?? SqlString.Null;
				parameters[counter].Direction = ParameterDirection.Output;
				var retSet = RunProcedure("spSynologenGetGiros", parameters, "tblSynologenGiro");
				return retSet;
			}
			catch (Exception ex) {
				throw CreateDataException("Got exception while fetching giros",ex);
			}
		}
		#endregion

	}
}
