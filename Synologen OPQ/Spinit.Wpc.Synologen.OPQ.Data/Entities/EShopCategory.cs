#pragma warning disable 1591

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Spinit's LINQ to SQL template for T4 C#
//     Generated at 11/03/2009 15:07:31
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq.Expressions;

using Spinit.Data.Linq;

using Spinit.Wpc.Synologen.OPQ.Core.Entities;

namespace Spinit.Wpc.Synologen.OPQ.Data.Entities
{	
	[Table(Name=@"dbo.tblSynologenShopCategory")]
	public partial class EShopCategory : EntityBase
	{
		#region Spinit search extension
		
		/// <summary>
		/// Creates a lambda-expression for use with the data-load-option feature,
		/// </summary>
		/// <param name="property">The property to search-for.</param>
		/// <returns>A lambda-expression.</returns>

		public override LambdaExpression BuildSearchExpression (string property)
		{
			ParameterExpression parameter = Expression.Parameter (GetType (), "eShopCategory");
			return Expression.Lambda<Func<EShopCategory, object>> (
						Expression.Property (parameter, property),
						parameter);
		}
		
		#endregion
		
		#region Extensibility Method Definitions
		partial void OnLoaded();
		partial void OnValidate(ChangeAction action);
		partial void OnCreated();
		#endregion

		#region Construction
		public EShopCategory()
		{
			_tblSynologenShopCategoryMemberCategoryConnections = new EntitySet<EShopCategoryMemberCategoryConnection>(AttachShopCategoryMemberCategoryConnections, DetachShopCategoryMemberCategoryConnections);
			_tblSynologenShops = new EntitySet<EShop>(AttachShops, DetachShops);
			OnCreated();
		}
		#endregion

		#region Column Mappings
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		private int _Id;
		[Column(Storage=@"_Id", Name=@"cId", AutoSync=AutoSync.OnInsert, DbType=@"Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true, UpdateCheck=UpdateCheck.Never)]
		public int Id
		{
			get { return _Id; }
			set {
				if (_Id != value) {
					OnIdChanging(value);
					SendPropertyChanging();
					_Id = value;
					SendPropertyChanged("Id");
					OnIdChanged();
				}
			}
		}
		
		partial void OnNameChanging(string value);
		partial void OnNameChanged();
		private string _Name;
		[Column(Storage=@"_Name", Name=@"cName", DbType=@"NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get { return _Name; }
			set {
				if (_Name != value) {
					OnNameChanging(value);
					SendPropertyChanging();
					_Name = value;
					SendPropertyChanged("Name");
					OnNameChanged();
				}
			}
		}
		
		#endregion
		
		#region Associations
		private EntitySet<EShopCategoryMemberCategoryConnection> _tblSynologenShopCategoryMemberCategoryConnections;
		[Association(Name=@"tblSynologenShopCategory_tblSynologenShopCategoryMemberCategoryConnection", Storage=@"_tblSynologenShopCategoryMemberCategoryConnections", ThisKey=@"Id", OtherKey=@"ShopCategoryId")]
		public EntitySet<EShopCategoryMemberCategoryConnection> ShopCategoryMemberCategoryConnections
		{
			get {
				return _tblSynologenShopCategoryMemberCategoryConnections;
			}
			set {
				_tblSynologenShopCategoryMemberCategoryConnections.Assign(value);
			}
		}

		private void AttachShopCategoryMemberCategoryConnections(EShopCategoryMemberCategoryConnection entity)
		{
			SendPropertyChanging();
			entity.ShopCategory = this;
		}
		
		private void DetachShopCategoryMemberCategoryConnections(EShopCategoryMemberCategoryConnection entity)
		{
			SendPropertyChanging();
			entity.ShopCategory = null;
		}
		private EntitySet<EShop> _tblSynologenShops;
		[Association(Name=@"tblSynologenShopCategory_tblSynologenShop", Storage=@"_tblSynologenShops", ThisKey=@"Id", OtherKey=@"CategoryId")]
		public EntitySet<EShop> Shops
		{
			get {
				return _tblSynologenShops;
			}
			set {
				_tblSynologenShops.Assign(value);
			}
		}

		private void AttachShops(EShop entity)
		{
			SendPropertyChanging();
			entity.ShopCategory = this;
		}
		
		private void DetachShops(EShop entity)
		{
			SendPropertyChanging();
			entity.ShopCategory = null;
		}
		#endregion

		#region Converters
		
		/// <summary>
		/// Converts from EShopCategory to ShopCategory.
		/// </summary>
		/// <param name="eShopCategory">The EShopCategory.</param>
		/// <returns>The converted ShopCategory.</returns>

		public static ShopCategory Convert (EShopCategory eShopCategory)
		{
			ShopCategory shopCategory = new ShopCategory
			{
				Id = eShopCategory.Id,
				Name = eShopCategory.Name,
			};
			
			return shopCategory;
		}
		
		/// <summary>
		/// Converts from ShopCategory to EShopCategory.
		/// </summary>
		/// <param name="shopCategory">The ShopCategory.</param>
		/// <returns>The converted EShopCategory.</returns>

		public static EShopCategory Convert (ShopCategory shopCategory)
		{		
			return new EShopCategory
			{
				Id = shopCategory.Id,
				Name = shopCategory.Name,
			};
		}

		#endregion
	}
}
#pragma warning restore 1591
