using Business.AspNet;
using Business.Core;
using Business.Core.Result;
using LinqToDB;
using LinqToDB.Data;
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
                log.Log();
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

            await con.BeginTransactionAsync();



            var dd = await con.dd.ToListAsync();

            var data = new DataModel.dd { gid = System.Guid.NewGuid().ToString("N"), dd2 = "ssss", ddColumn = "22222" };

            var list = new List<DataModel.dd>();

            list.Add(data);

            //con.dd.InsertOrUpdate(list as ITable<DataModel.dd>);

            //con.dd.Insert(list as ITable<DataModel.dd>, c => c.dd2);
            //con.dd.InsertOrUpdate();
            //LinqExtensions.Insert(dd as IQueryable<DataModel.dd>, con.dd, null);

            //con.Insert(list);

            //foreach (var item in dd)
            //{
            //    item.dd2 = "999999";
            //    item.ddColumn = "888888";
            //}

            dd[0].dd2 = "wwwwwww";

            await con.UpdateAsync(dd[0]);
            //dd[0].ddColumn = "33333";
            //con.BulkCopy(new BulkCopyOptions() {   }, dd);
            //con.dd.Update(c => dd[0]);
            //con.BulkCopy(list);

            //con.dd.Merge(dd);

            //await con.dd.Where(c => c.dd2 == "33333").Set(c => c.dd2, "666").UpdateAsync();

            //con.dd
            //.Merge()
            //.Using(dd)
            //.OnTargetKey()
            //.UpdateWhenMatched()
            //.InsertWhenNotMatched()
            ////.DeleteWhenNotMatchedBySourceAnd(predicate)
            //.Merge();
            //LinqToDB.Expressions.ExpressionHelper.

            //var FilteredCount = Sql.Ext.Count().Over().ToValue();

            var data2 = await con.dd.GetPagingAsync(1, 300);

            //foreach (var item in data2.Data)
            //{
            //    item.dd2 = "333";
            //}

            con.CommitTransaction();

            return this.ResultCreate(dd);
        }
    }
}
