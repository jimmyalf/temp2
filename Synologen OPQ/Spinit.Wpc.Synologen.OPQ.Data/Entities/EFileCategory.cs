#pragma warning disable 1591

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Spinit's LINQ to SQL template for T4 C#
//     Generated at 06/04/2012 12:03:10
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
using Spinit.Wpc.Synologen.OPQ.Core;
using Spinit.Wpc.Synologen.OPQ.Core.Entities;

namespace Spinit.Wpc.Synologen.OPQ.Data.Entities
{	
	[Table(Name=@"dbo.SynologenOpqFileCategories")]
	public partial class EFileCategory : EntityBase
	{
		#region Spinit search extension
		
		/// <summary>
		/// Creates a lambda-expression for use with the data-load-option feature,
		/// </summary>
		/// <param name="property">The property to search-for.</param>
		/// <returns>A lambda-expression.</returns>

		public override LambdaExpression BuildSearchExpression (string property)
		{
			ParameterExpression parameter = Expression.Parameter (GetType (), "eFileCategory");
			return Expression.Lambda<Func<EFileCategory, object>> (
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
		public EFileCategory()
		{
			_SynologenOpqFiles = new EntitySet<EFile>(AttachFiles, DetachFiles);
			_tblBaseUser = default(EntityRef<EBaseUser>); 
			_tblBaseUser1 = default(EntityRef<EBaseUser>); 
			OnCreated();
		}
		#endregion

		#region Column Mappings
		partial void OnIdChanging(int value);
		partial void OnIdChanged();
		private int _Id;
		[Column(Storage=@"_Id", AutoSync=AutoSync.OnInsert, DbType=@"Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true, UpdateCheck=UpdateCheck.Never)]
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
		[Column(Storage=@"_Name", DbType=@"NVarChar(512) NOT NULL", CanBeNull=false)]
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
		
		partial void OnIsActiveChanging(bool value);
		partial void OnIsActiveChanged();
		private bool _IsActive;
		[Column(Storage=@"_IsActive", DbType=@"Bit NOT NULL", CanBeNull=false)]
		public bool IsActive
		{
			get { return _IsActive; }
			set {
				if (_IsActive != value) {
					OnIsActiveChanging(value);
					SendPropertyChanging();
					_IsActive = value;
					SendPropertyChanged("IsActive");
					OnIsActiveChanged();
				}
			}
		}
		
		partial void OnCreatedByIdChanging(int value);
		partial void OnCreatedByIdChanged();
		private int _CreatedById;
		[Column(Storage=@"_CreatedById", DbType=@"Int NOT NULL", CanBeNull=false)]
		public int CreatedById
		{
			get { return _CreatedById; }
			set {
				if (_CreatedById != value) {
					if (_tblBaseUser.HasLoadedOrAssignedValue) {
						throw new ForeignKeyReferenceAlreadyHasValueException();
					}
					OnCreatedByIdChanging(value);
					SendPropertyChanging();
					_CreatedById = value;
					SendPropertyChanged("CreatedById");
					OnCreatedByIdChanged();
				}
			}
		}
		
		partial void OnCreatedByNameChanging(string value);
		partial void OnCreatedByNameChanged();
		private string _CreatedByName;
		[Column(Storage=@"_CreatedByName", DbType=@"NVarChar(100) NOT NULL", CanBeNull=false)]
		public string CreatedByName
		{
			get { return _CreatedByName; }
			set {
				if (_CreatedByName != value) {
					OnCreatedByNameChanging(value);
					SendPropertyChanging();
					_CreatedByName = value;
					SendPropertyChanged("CreatedByName");
					OnCreatedByNameChanged();
				}
			}
		}
		
		partial void OnCreatedDateChanging(DateTime value);
		partial void OnCreatedDateChanged();
		private DateTime _CreatedDate;
		[Column(Storage=@"_CreatedDate", DbType=@"DateTime NOT NULL", CanBeNull=false)]
		public DateTime CreatedDate
		{
			get { return _CreatedDate; }
			set {
				if (_CreatedDate != value) {
					OnCreatedDateChanging(value);
					SendPropertyChanging();
					_CreatedDate = value;
					SendPropertyChanged("CreatedDate");
					OnCreatedDateChanged();
				}
			}
		}
		
		partial void OnChangedByIdChanging(int? value);
		partial void OnChangedByIdChanged();
		private int? _ChangedById;
		[Column(Storage=@"_ChangedById", DbType=@"Int")]
		public int? ChangedById
		{
			get { return _ChangedById; }
			set {
				if (_ChangedById != value) {
					if (_tblBaseUser1.HasLoadedOrAssignedValue) {
						throw new ForeignKeyReferenceAlreadyHasValueException();
					}
					OnChangedByIdChanging(value);
					SendPropertyChanging();
					_ChangedById = value;
					SendPropertyChanged("ChangedById");
					OnChangedByIdChanged();
				}
			}
		}
		
		partial void OnChangedByNameChanging(string value);
		partial void OnChangedByNameChanged();
		private string _ChangedByName;
		[Column(Storage=@"_ChangedByName", DbType=@"NVarChar(100)")]
		public string ChangedByName
		{
			get { return _ChangedByName; }
			set {
				if (_ChangedByName != value) {
					OnChangedByNameChanging(value);
					SendPropertyChanging();
					_ChangedByName = value;
					SendPropertyChanged("ChangedByName");
					OnChangedByNameChanged();
				}
			}
		}
		
		partial void OnChangedDateChanging(DateTime? value);
		partial void OnChangedDateChanged();
		private DateTime? _ChangedDate;
		[Column(Storage=@"_ChangedDate", DbType=@"DateTime")]
		public DateTime? ChangedDate
		{
			get { return _ChangedDate; }
			set {
				if (_ChangedDate != value) {
					OnChangedDateChanging(value);
					SendPropertyChanging();
					_ChangedDate = value;
					SendPropertyChanged("ChangedDate");
					OnChangedDateChanged();
				}
			}
		}
		
		#endregion
		
		#region Associations
		private EntitySet<EFile> _SynologenOpqFiles;
		[Association(Name=@"SynologenOpqFileCategory_SynologenOpqFile", Storage=@"_SynologenOpqFiles", ThisKey=@"Id", OtherKey=@"FleCatId")]
		public EntitySet<EFile> Files
		{
			get {
				return _SynologenOpqFiles;
			}
			set {
				_SynologenOpqFiles.Assign(value);
			}
		}

		private void AttachFiles(EFile entity)
		{
			SendPropertyChanging();
			entity.FileCategory = this;
		}
		
		private void DetachFiles(EFile entity)
		{
			SendPropertyChanging();
			entity.FileCategory = null;
		}
		private EntityRef<EBaseUser> _tblBaseUser;
		[Association(Name=@"tblBaseUser_SynologenOpqFileCategory", Storage=@"_tblBaseUser", ThisKey=@"CreatedById", OtherKey=@"Id", IsForeignKey=true)]
		public EBaseUser CreatedBy
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
						previousValue.FileCategories.Remove(this);
					}
					_tblBaseUser.Entity = value;
					if (value != null) {
						value.FileCategories.Add(this);
						_CreatedById = value.Id;
					}
					else {
						_CreatedById = default(int);
					}
					SendPropertyChanged("BaseUser");
				}
			}
		}

		private EntityRef<EBaseUser> _tblBaseUser1;
		[Association(Name=@"tblBaseUser_SynologenOpqFileCategory1", Storage=@"_tblBaseUser1", ThisKey=@"ChangedById", OtherKey=@"Id", IsForeignKey=true)]
		public EBaseUser ChangedBy
		{
			get {
				return _tblBaseUser1.Entity;
			}
			set {
				EBaseUser previousValue = _tblBaseUser1.Entity;
				if ((previousValue != value) || (!_tblBaseUser1.HasLoadedOrAssignedValue)) {
					SendPropertyChanging();
					if (previousValue != null) {
						_tblBaseUser1.Entity = null;
						previousValue.FileCategories1.Remove(this);
					}
					_tblBaseUser1.Entity = value;
					if (value != null) {
						value.FileCategories1.Add(this);
						_ChangedById = value.Id;
					}
					else {
						_ChangedById = default(int?);
					}
					SendPropertyChanged("BaseUser1");
				}
			}
		}

		#endregion

		#region Converters
		
		/// <summary>
		/// Converts from EFileCategory to FileCategory.
		/// </summary>
		/// <param name="eFileCategory">The EFileCategory.</param>
		/// <returns>The converted FileCategory.</returns>

		public static FileCategory Convert (EFileCategory eFileCategory)
		{
			FileCategory fileCategory = new FileCategory
			{
				Id = (FileCategories) eFileCategory.Id,
				Name = eFileCategory.Name,
				IsActive = eFileCategory.IsActive,
				CreatedById = eFileCategory.CreatedById,
				CreatedByName = eFileCategory.CreatedByName,
				CreatedDate = eFileCategory.CreatedDate,
				ChangedById = eFileCategory.ChangedById,
				ChangedByName = eFileCategory.ChangedByName,
				ChangedDate = eFileCategory.ChangedDate,
			};
			
			return fileCategory;
		}
		
		/// <summary>
		/// Converts from FileCategory to EFileCategory.
		/// </summary>
		/// <param name="fileCategory">The FileCategory.</param>
		/// <returns>The converted EFileCategory.</returns>

		public static EFileCategory Convert (FileCategory fileCategory)
		{		
			return new EFileCategory
			{
				Id = (int) fileCategory.Id,
				Name = fileCategory.Name,
				IsActive = fileCategory.IsActive,
				CreatedById = fileCategory.CreatedById,
				CreatedByName = fileCategory.CreatedByName,
				CreatedDate = fileCategory.CreatedDate,
				ChangedById = fileCategory.ChangedById,
				ChangedByName = fileCategory.ChangedByName,
				ChangedDate = fileCategory.ChangedDate,
			};
		}

		#endregion
	}
}
#pragma warning restore 1591
