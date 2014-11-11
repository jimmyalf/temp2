#pragma warning disable 1591

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Spinit's LINQ to SQL template for T4 C#
//     Generated at 06/07/2012 10:51:22
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
	[Table(Name=@"dbo.SynologenOpqNodeSupplierConnections")]
	public partial class ENodeSupplierConnection : EntityBase
	{
		#region Spinit search extension
		
		/// <summary>
		/// Creates a lambda-expression for use with the data-load-option feature,
		/// </summary>
		/// <param name="property">The property to search-for.</param>
		/// <returns>A lambda-expression.</returns>

		public override LambdaExpression BuildSearchExpression (string property)
		{
			ParameterExpression parameter = Expression.Parameter (GetType (), "eNodeSupplierConnection");
			return Expression.Lambda<Func<ENodeSupplierConnection, object>> (
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
		public ENodeSupplierConnection()
		{
			_SynologenOpqNode = default(EntityRef<ENode>); 
			_tblBaseUser = default(EntityRef<EBaseUser>); 
			OnCreated();
		}
		#endregion

		#region Column Mappings
		partial void OnNdeIdChanging(int value);
		partial void OnNdeIdChanged();
		private int _NdeId;
		[Column(Storage=@"_NdeId", DbType=@"Int NOT NULL", IsPrimaryKey=true, UpdateCheck=UpdateCheck.Never)]
		public int NdeId
		{
			get { return _NdeId; }
			set {
				if (_NdeId != value) {
					if (_SynologenOpqNode.HasLoadedOrAssignedValue) {
						throw new ForeignKeyReferenceAlreadyHasValueException();
					}
					OnNdeIdChanging(value);
					SendPropertyChanging();
					_NdeId = value;
					SendPropertyChanged("NdeId");
					OnNdeIdChanged();
				}
			}
		}
		
		partial void OnSupIdChanging(int value);
		partial void OnSupIdChanged();
		private int _SupId;
		[Column(Storage=@"_SupId", DbType=@"Int NOT NULL", IsPrimaryKey=true, UpdateCheck=UpdateCheck.Never)]
		public int SupId
		{
			get { return _SupId; }
			set {
				if (_SupId != value) {
					if (_tblBaseUser.HasLoadedOrAssignedValue) {
						throw new ForeignKeyReferenceAlreadyHasValueException();
					}
					OnSupIdChanging(value);
					SendPropertyChanging();
					_SupId = value;
					SendPropertyChanged("SupId");
					OnSupIdChanged();
				}
			}
		}
		
		#endregion
		
		#region Associations
		private EntityRef<ENode> _SynologenOpqNode;
		[Association(Name=@"SynologenOpqNode_SynologenOpqNodeSupplierConnection", Storage=@"_SynologenOpqNode", ThisKey=@"NdeId", OtherKey=@"Id", IsForeignKey=true)]
		public ENode Node
		{
			get {
				return _SynologenOpqNode.Entity;
			}
			set {
				ENode previousValue = _SynologenOpqNode.Entity;
				if ((previousValue != value) || (!_SynologenOpqNode.HasLoadedOrAssignedValue)) {
					SendPropertyChanging();
					if (previousValue != null) {
						_SynologenOpqNode.Entity = null;
						previousValue.NodeSupplierConnections.Remove(this);
					}
					_SynologenOpqNode.Entity = value;
					if (value != null) {
						value.NodeSupplierConnections.Add(this);
						_NdeId = value.Id;
					}
					else {
						_NdeId = default(int);
					}
					SendPropertyChanged("Node");
				}
			}
		}

		private EntityRef<EBaseUser> _tblBaseUser;
		[Association(Name=@"tblBaseUser_SynologenOpqNodeSupplierConnection", Storage=@"_tblBaseUser", ThisKey=@"SupId", OtherKey=@"Id", IsForeignKey=true)]
		public EBaseUser BaseUser
		{
			get {
				return _tblBaseUser.Entity;
			}
			set {
				EBaseUser previousValue = _tblBaseUser.Entity;
				if ((previousValue != value) || (!_tblBaseUser.HasLoadedOrAssignedValue)) {
					SendPropertyChanging();
					if (previousValue != null) {
						_tblBaseUser.Entity = null;
						previousValue.NodeSupplierConnections.Remove(this);
					}
					_tblBaseUser.Entity = value;
					if (value != null) {
						value.NodeSupplierConnections.Add(this);
						_SupId = value.Id;
					}
					else {
						_SupId = default(int);
					}
					SendPropertyChanged("BaseUser");
				}
			}
		}

		#endregion

		#region Converters
		
		/// <summary>
		/// Converts from ENodeSupplierConnection to NodeSupplierConnection.
		/// </summary>
		/// <param name="eNodeSupplierConnection">The ENodeSupplierConnection.</param>
		/// <returns>The converted NodeSupplierConnection.</returns>

		public static NodeSupplierConnection Convert (ENodeSupplierConnection eNodeSupplierConnection)
		{
			NodeSupplierConnection nodeSupplierConnection = new NodeSupplierConnection
			{
				NdeId = eNodeSupplierConnection.NdeId,
				SupId = eNodeSupplierConnection.SupId,
			};
			
			return nodeSupplierConnection;
		}
		
		/// <summary>
		/// Converts from NodeSupplierConnection to ENodeSupplierConnection.
		/// </summary>
		/// <param name="nodeSupplierConnection">The NodeSupplierConnection.</param>
		/// <returns>The converted ENodeSupplierConnection.</returns>

		public static ENodeSupplierConnection Convert (NodeSupplierConnection nodeSupplierConnection)
		{		
			return new ENodeSupplierConnection
			{
				NdeId = nodeSupplierConnection.NdeId,
				SupId = nodeSupplierConnection.SupId,
			};
		}

		#endregion
	}
}
#pragma warning restore 1591
