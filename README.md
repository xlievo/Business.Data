# Business.Data
A C # wrapper class library, contains a generic ORM simple packaging. No excessive packaging as possible, restore the essence

Install-Package Business.AspNet //Optional, but you must read the configuration file yourself  
Install-Package linq2db.PostgreSQL //Select the database package you need to install

	a:=====================lin2db.T4.tt=====================
	
	NamespaceName = "DataModel";
	DataContextName = "Connection";
	BaseDataContextClass = "LinqToDB.LinqToDBConnection";
	PluralizeDataContextPropertyNames = false;
	NormalizeNames = false;
	
	b:=====================appsettings.json=====================
	
	"AppSettings": {
		"ConnectionStrings": {
			"Master": {
				"ConnectionString": "Server=MyServer;Database=MyDatabase;User Id=postgres;Password=TestPassword;port=5432;",
				"providerName": "PostgreSQL"
				},
            "Slave": { }
		}
	}
	
	c:=====================Definition=====================

    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using Business.Core.Utils;
    using Business.AspNet;

	public class DataBase : Business.Data.DataBase<DataModel.Connection>
    {
        public static readonly DataBase DB = new DataBase();//master

        //public static readonly DataBase DB2 = new DataBase("Slave");//slave

        static DataBase()
        {
            //Initialize the database
            LinqToDB.Data.DataConnection.DefaultSettings = new LinqToDB.LinqToDBSection(Utils.Hosting.Config.GetSection("AppSettings").GetSection("ConnectionStrings").GetChildren().Select(c => new LinqToDB.ConnectionStringSettings { Name = c.Key, ConnectionString = c.GetValue<string>("ConnectionString"), ProviderName = c.GetValue<string>("ProviderName") }));

            LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;
            LinqToDB.Data.DataConnection.TurnTraceSwitchOn();
            LinqToDB.Data.DataConnection.OnTrace = c =>
            {
                if (c.TraceInfoStep != LinqToDB.Data.TraceInfoStep.Completed)
                {
                    if (c.TraceInfoStep == LinqToDB.Data.TraceInfoStep.Error)
                    {
                        c.Exception?.Console();
                    }
                    return;
                }

                var con = c.DataConnection as LinqToDB.LinqToDBConnection;

                System.Console.WriteLine($"{c.StartTime}{con?.TraceMethod}:{con?.TraceId}{System.Environment.NewLine}{c.SqlText}{System.Environment.NewLine}{c.ExecutionTime}");
            };
        }

        readonly string configuration;

        public DataBase(string configuration = null) => this.configuration = configuration;

        public override DataModel.Connection GetConnection([CallerMemberName] string callMethod = null) => new DataModel.Connection(this.configuration ?? LinqToDB.Data.DataConnection.DefaultSettings.DefaultConfiguration) { TraceMethod = callMethod };
    }
	