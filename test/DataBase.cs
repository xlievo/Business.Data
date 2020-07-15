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

    public override DataModel.Connection GetConnection([System.Runtime.CompilerServices.CallerMemberName] string callMethod = null) => new DataModel.Connection(this.configuration ?? LinqToDB.Data.DataConnection.DefaultSettings.DefaultConfiguration) { TraceMethod = callMethod };
}