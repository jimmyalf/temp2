
#pragma warning disable 1591

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Spinit's LINQ to SQL template for T4 C#
//     Generated at 10/26/2009 13:57:19
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Spinit.Wpc.Synologen.OPQ.Core
{
	
	/// <summary>
	/// The configuration data-container.
	/// </summary>

	public class Configuration
	{
		/// <summary>
		/// The minimum constructor.
		/// </summary>
		/// <param name="connectionString">The connection-string.</param>
		/// <param name="defaultCulture">The default-culture.</param>

		public Configuration (string connectionString, string defaultCulture)
		{
			ConnectionString = connectionString;
			DefaultCulture = defaultCulture;
		}
		
		/// <summary>
		/// Gets the connection-string.
		/// </summary>

		public string ConnectionString { get; private set; }
		
		/// <summary>
		/// Gets the default-culture.
		/// </summary>

		public string DefaultCulture { get; private set; }
	}
}

#pragma warning restore 1591
 
