#pragma warning disable 1591

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Spinit's LINQ to SQL template for T4 C#
//     Generated at 11/16/2009 09:19:49
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Spinit.Wpc.Synologen.OPQ.Core.Entities
{	
	public class ShopCategory
	{
		#region Column Mappings
		
		public int Id { get; set; }
		
		public string Name { get; set; }
		
		#endregion
		
		#region Associations
		
		public List<ShopCategoryMemberCategoryConnection> ShopCategoryMemberCategoryConnections { get; set; }
		
		public List<Shop> Shops { get; set; }
		
		#endregion
		
		#region Overrides
		
		/// <summary>
		/// Returnes the string representation of this class.
		/// </summary>
		/// <returns>The string.</returns>

		public override string ToString ()
		{
			string str = string.Empty;
				 				  
			str += string.Concat (Name, " ") ?? string.Empty;

			return str.Length > 1 ? str.Substring(0, str.Length - 1) : str;
		}		
		
		/// <summary>
		/// Overrides the equals base method.
		/// </summary>
		/// <param name="o">The compare object.</param>
		/// <returns>If match, true.</returns>

		public override bool Equals (object o)
		{
			if (!(o is ShopCategory )) {
				return false;
			}

			ShopCategory eq = o as ShopCategory;

			return Id == eq.Id;
		}

		/// <summary>
		/// Overrides the GetHashCode base method.
		/// </summary>
		/// <returns>The hash-code for this object.</returns>

		public override int GetHashCode ()
		{
			return Id.GetHashCode ();
		}

		#endregion
	}
}
#pragma warning restore 1591
