//---------------------------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by T4Model template for T4 (https://github.com/linq2db/linq2db).
//    Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------

#pragma warning disable 1591

using System;
using System.Linq;

using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Mapping;

namespace DataModel
{
	/// <summary>
	/// Database       : dd
	/// Data Source    : tcp://192.168.1.121:5920
	/// Server Version : 10.11
	/// </summary>
	public partial class Connection : LinqToDB.LinqToDBConnection
	{
		/// <summary>
		/// ddd
		/// </summary>
		public ITable<dd> dd { get { return this.GetTable<dd>(); } }

		partial void InitMappingSchema()
		{
		}

		public Connection()
		{
			InitDataContext();
			InitMappingSchema();
		}

		public Connection(string configuration)
			: base(configuration)
		{
			InitDataContext();
			InitMappingSchema();
		}

		public Connection(LinqToDbConnectionOptions options)
			: base(options)
		{
			InitDataContext();
			InitMappingSchema();
		}

		partial void InitDataContext  ();
		partial void InitMappingSchema();
	}

	/// <summary>
	/// ddd
	/// </summary>
	[Table(Schema="public", Name="dd")]
	public partial class dd
	{
		/// <summary>
		/// ddd
		/// </summary>
		[Column("dd"),    Nullable         ] public string ddColumn { get; set; } // character varying(255)
		/// <summary>
		/// ddd2
		/// </summary>
		[Column(),        Nullable         ] public string dd2      { get; set; } // character varying(255)
		/// <summary>
		/// key
		/// </summary>
		[Column(),     PrimaryKey,  NotNull] public string gid      { get; set; } // character varying(32)
	}

	public static partial class TableExtensions
	{
		public static dd Find(this ITable<dd> table, string gid)
		{
			return table.FirstOrDefault(t =>
				t.gid == gid);
		}
	}
}

#pragma warning restore 1591
