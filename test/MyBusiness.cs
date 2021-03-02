using Business.AspNet;
using Business.Core;
using Business.Core.Result;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test
{
    //Maybe you need a custom base class? To unify the processing of logs and token
    public class MyBusiness : Business.AspNet.BusinessBase
    {
        public MyBusiness()
        {
            this.Logger = new Logger(async (Logger.LoggerData log) =>
            {
                //Output log
                Console.WriteLine(log.ToString());
            });
        }

        /// <summary>
        /// MyLogicArg!
        /// </summary>
        public struct MyLogicArg
        {
            /// <summary>
            /// AAA
            /// </summary>
            public string A { get; set; }
            /// <summary>
            /// BBB
            /// </summary>
            public string B { get; set; }
        }

        //My first business logic
        //Logical method must be public virtual!
        //If you customize the base class, you just need to concentrate on writing logical methods!
        public virtual async Task<dynamic> MyLogic(Token token, MyLogicArg arg)
        {
            using var con = DataBase.DB.GetConnection();
            var dd = await con.dd.ToListAsync();

            var data = new DataModel.dd { dd2 = "ssss" };

            var list = new List<DataModel.dd>();

            list.Add(data);

            con.BulkCopy(list);

            return this.ResultCreate(dd);
        }
    }
}
