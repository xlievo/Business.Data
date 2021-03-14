/*==================================
             ########
            ##########

             ########
            ##########
          ##############
         #######  #######
        ######      ######
        #####        #####
        ####          ####
        ####   ####   ####
        #####  ####  #####
         ################
          ##############
==================================*/

namespace LinqToDB
{
    using LinqToDB.Data;
    using LinqToDB.DataProvider;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// the a LinqToDBConnection
    /// </summary>
    public class LinqToDBConnection : DataConnection
    {
        static int ForEach<T>(IEnumerable<T> obj, System.Func<T, int> func)
        {
            int count = 0;//, ex = 0;

            foreach (var item in obj)
            {
                var result = func(item);

                if (-1 == result)
                {
                    count = result;
                    break;
                }
                else
                {
                    count += result;
                }
            }

            return count;

            //return ex == 0 ? count : ex;
        }

        static async ValueTask<int> ForEach<T>(IEnumerable<T> obj, System.Func<T, Task<int>> func)
        {
            int count = 0;//, ex = 0;

            foreach (var item in obj)
            {
                var result = await func(item);

                if (-1 == result)
                {
                    count = result;
                    break;
                }

                count += result;
            }

            return count;

            //return ex == 0 ? count : ex;
        }

        /// <summary>
        /// LinqToDBConnection
        /// </summary>
        public LinqToDBConnection() : base() { }

        /// <summary>
        /// LinqToDBConnection
        /// </summary>
        /// <param name="configuration"></param>
        public LinqToDBConnection(string configuration) : base(configuration) { }

        /// <summary>
        /// LinqToDBConnection
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="connectionString"></param>
        public LinqToDBConnection(string providerName, string connectionString) : base(providerName, connectionString) { }

        /// <summary>
        /// LinqToDBConnection
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="conString"></param>
        public LinqToDBConnection(IDataProvider provider, string conString) : base(provider, conString) { }

        /// <summary>
        /// LinqToDBConnection
        /// </summary>
        /// <param name="options"></param>
        public LinqToDBConnection(Configuration.LinqToDbConnectionOptions options) : base(options) { }

        /// <summary>
        /// TraceMethod
        /// </summary>
        public string TraceMethod { get; set; }

        /// <summary>
        /// TraceId
        /// </summary>
        public string TraceId { get; } = System.Guid.NewGuid().ToString("N");

        //public abstract IEntity Entity { get; }

        //Business.Data.IEntity IEntitys.Entity { get => Entity; }

        //public new void BeginTransaction() => base.BeginTransaction();

        //public new void BeginTransaction(System.Data.IsolationLevel isolationLevel) => base.BeginTransaction(isolationLevel);

        //public void Commit() => base.CommitTransaction();

        //public void Rollback() => base.RollbackTransaction();

        /// <summary>
        /// Performs bulk insert operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public long Insert<T>(IEnumerable<T> source)
            where T : class => DataConnectionExtensions.BulkCopy(this, source).RowsCopied;

        /// <summary>
        /// Asynchronously performs bulk insert operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public async ValueTask<long> InsertAsync<T>(IEnumerable<T> source)
            where T : class => (await DataConnectionExtensions.BulkCopyAsync(this, source)).RowsCopied;

        /// <summary>
        /// Updates record in table, identified by T mapping class, using values from obj parameter. Record to update identified by match on primary key value from obj value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update<T>(IEnumerable<T> obj) => ForEach(obj, item => DataExtensions.Update(this, item));

        /// <summary>
        /// Asynchronously updates record in table, identified by T mapping class, using values from obj parameter. Record to update identified by match on primary key value from obj value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async ValueTask<int> UpdateAsync<T>(IEnumerable<T> obj) => await ForEach(obj, item => DataExtensions.UpdateAsync(this, item));

        /// <summary>
        /// Inserts new record into table, identified by T mapping class, using values from obj parameter or update existing record, identified by match on primary key value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="tableName"></param>
        /// <param name="databaseName"></param>
        /// <param name="schemaName"></param>
        /// <param name="serverName"></param>
        /// <param name="tableOptions"></param>
        /// <returns></returns>
        public int InsertOrReplace<T>(IEnumerable<T> obj, string tableName = null, string databaseName = null, string schemaName = null, string serverName = null, TableOptions tableOptions = TableOptions.NotSet) => ForEach(obj, item => DataExtensions.InsertOrReplace(this, item, tableName, databaseName, schemaName, serverName, tableOptions));

        /// <summary>
        /// Asynchronously inserts new record into table, identified by T mapping class, using values from obj parameter or update existing record, identified by match on primary key value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="tableName"></param>
        /// <param name="databaseName"></param>
        /// <param name="schemaName"></param>
        /// <param name="serverName"></param>
        /// <param name="tableOptions"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async ValueTask<int> InsertOrReplaceAsync<T>(IEnumerable<T> obj, string tableName = null, string databaseName = null, string schemaName = null, string serverName = null, TableOptions tableOptions = TableOptions.NotSet, CancellationToken token = default) => await ForEach(obj, item => DataExtensions.InsertOrReplaceAsync(this, item, tableName, databaseName, schemaName, serverName, tableOptions, token));

        /*
        public int Save<T>(System.Collections.Generic.IEnumerable<T> obj) => this.ExecutePack(() => ForEach(obj, item => DataExtensions.Insert(this, item)));

        public int Save<T>(T obj)
        {
            return this.ExecutePack(() => DataExtensions.Insert(this, obj));
        }

        public int SaveWithInt32Identity<T>(T obj)
        {
            return this.ExecutePack(() => DataExtensions.InsertWithInt32Identity(this, obj));
        }

        public long SaveWithInt64Identity<T>(T obj)
        {
            return this.ExecutePack(() => DataExtensions.InsertWithInt64Identity(this, obj));
        }

        public int SaveOrUpdate<T>(System.Collections.Generic.IEnumerable<T> obj)
        {
            return this.ExecutePack(() => ForEach(obj, item => DataExtensions.InsertOrReplace(this, item)));
        }

        public int SaveOrUpdate<T>(T obj)
        {
            return this.ExecutePack(() => DataExtensions.InsertOrReplace(this, obj));
        }

        public int Update<T>(System.Collections.Generic.IEnumerable<T> obj)
        {
            return this.ExecutePack(() => ForEach(obj, item => DataExtensions.Update(this, item)));
        }

        public int Update<T>(T obj)
        {
            return this.ExecutePack(() => DataExtensions.Update(this, obj));
        }

        public int Delete<T>(System.Collections.Generic.IEnumerable<T> obj)
        {
            return this.ExecutePack(() => ForEach(obj, item => DataExtensions.Delete(this, item)));
        }

        public int Delete<T>(T obj)
        {
            return this.ExecutePack(() => DataExtensions.Delete(this, obj));
        }

        public void BulkCopy<T>(System.Collections.Generic.IEnumerable<T> source) where T : class
        {
            Data.DataConnectionExtensions.BulkCopy(this, source);
        }
        */
        //public new void Dispose()
        //{
        //    DisposeCommand();
        //    base.Dispose();
        //    Transaction?.Dispose();
        //    Connection?.Dispose();
        //}
    }

    #region Paging Object

    /// <summary>
    /// IPaging
    /// </summary>
    public interface IPaging<T>
    {
        /// <summary>
        /// Get paging data for
        /// </summary>
        List<T> Data { get; set; }

        /// <summary>
        /// The length of the obtained paging data
        /// </summary>
        int Length { get; set; }

        /// <summary>
        /// The current paging index is determined by paging calculation
        /// </summary>
        int CurrentPage { get; set; }

        /// <summary>
        /// Total records
        /// </summary>
        int Count { get; set; }

        /// <summary>
        /// Total pages
        /// </summary>
        int CountPage { get; set; }
    }

    /// <summary>
    /// Paging
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Paging<T> : IPaging<T>
    {
        /// <summary>
        /// Get paging data for
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// The length of the obtained paging data
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// The current paging index is determined by paging calculation
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Total records
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Total pages
        /// </summary>
        public int CountPage { get; set; }
    }

    /// <summary>
    /// PagingInfo
    /// </summary>
    public readonly struct PagingInfo
    {
        /// <summary>
        /// PagingInfo
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="currentPage"></param>
        /// <param name="countPage"></param>
        public PagingInfo(int skip, int take, int currentPage, int countPage)
        {
            Skip = skip;
            Take = take;
            CurrentPage = currentPage;
            CountPage = countPage;
        }

        /// <summary>
        /// Skip
        /// </summary>
        public int Skip { get; }

        /// <summary>
        /// Take
        /// </summary>
        public int Take { get; }

        /// <summary>
        /// CurrentPage
        /// </summary>
        public int CurrentPage { get; }

        /// <summary>
        /// CountPage
        /// </summary>
        public int CountPage { get; }
    }

    /// <summary>
    /// Order
    /// </summary>
    public enum Order
    {
        /// <summary>
        /// Ascending
        /// </summary>
        Ascending = 1,

        /// <summary>
        /// Descending
        /// </summary>
        Descending = 2
    }

    #endregion

    /// <summary>
    /// DataConnectionEx
    /// </summary>
    public static class DataConnectionEx
    {
        /*
        public static System.Data.IDbCommand GetCommand(this IConnection connection, string commandText, System.Data.IDbTransaction t = null, System.Data.CommandType commandType = System.Data.CommandType.Text, params DataParameter[] parameter)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandType = commandType;
            cmd.CommandText = commandText;

            if (null != parameter)
            {
                foreach (var item in parameter)
                {
                    var p = cmd.CreateParameter();
                    p.ParameterName = item.Name;
                    //p.Value = System.DateTime.MinValue.Equals(item.Value) ? System.DBNull.Value : item.Value ?? System.DBNull.Value;
                    p.Value = item.Value ?? System.DBNull.Value;
                    cmd.Parameters.Add(p);
                }
            }

            cmd.Transaction = t;
            return cmd;
        }

        public static int ExecuteNonQuery(this IConnection connection, string commandText, System.Data.CommandType commandType = System.Data.CommandType.Text, params DataParameter[] parameter)
        {
            return connection.ExecutePack(() =>
            {
                using (var cmd = connection.GetCommand(commandText, connection.Transaction, commandType, parameter))
                {
                    return cmd.ExecuteNonQuery();
                }
            });
        }

        public static Result ExecuteScalar<Result>(this IConnection connection, string commandText, System.Data.CommandType commandType = System.Data.CommandType.Text, params DataParameter[] parameter)
        {
            return connection.ExecutePack(() =>
            {
                using (var cmd = connection.GetCommand(commandText, connection.Transaction, commandType, parameter))
                {
                    return (Result)cmd.ExecuteScalar();
                }
            }, minusOneExcep: false);
        }

        public static System.Collections.Generic.IList<TEntity> Execute<TEntity>(this IConnection connection, string commandText, System.Data.CommandType commandType = System.Data.CommandType.Text, params DataParameter[] parameter)
        {
            return connection.ExecutePack(() =>
            {
                using (var cmd = connection.GetCommand(commandText, connection.Transaction, commandType, parameter))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.ToEntity<TEntity>();
                    }
                }
            }, minusOneExcep: false);
        }

        public static System.Collections.Generic.IList<TEntity> ToEntity<TEntity>(this System.Data.IDataReader reader) => new AutoMapper.MapperConfiguration(cfg =>
        {
            AutoMapper.Data.ConfigurationExtensions.AddDataReaderMapping(cfg);
            cfg.CreateMap<System.Data.IDataReader, TEntity>();
        }).CreateMapper().Map<System.Collections.Generic.IList<TEntity>>(reader);

        internal static Result ExecutePack<Result>(this IConnection connection, System.Func<Result> func, bool minusOneExcep = true)
        {
            bool isCreateTransaction = false;
            if (null == connection.Transaction) { connection.BeginTransaction(); isCreateTransaction = !isCreateTransaction; }

            try
            {
                var result = func();

                if (minusOneExcep && (typeof(Result).Equals(typeof(int)) || typeof(Result).Equals(typeof(long))))
                {
                    //var count = System.Convert.ToInt32(result);
                    if (Equals(-1, result))
                    {
                        connection.Rollback();
                        throw new System.Exception("Affected the number of records -1");
                    }
                }

                if (isCreateTransaction) { connection.Commit(); }
                return result;
            }
            catch (System.Exception ex) { if (null != connection.Transaction) { connection.Rollback(); } throw ex; }
            finally { if (isCreateTransaction && null != connection.Transaction) { connection.Transaction.Dispose(); } }
        }
        */

        static int Random(int maxValue)
        {
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                var bytes = new byte[4];
                rng.GetBytes(bytes);
                return new System.Random(System.BitConverter.ToInt32(bytes, 0)).Next(maxValue);
            }
        }

        /// <summary>
        /// SkipRandom
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public static IQueryable<T> SkipRandom<T>(this IQueryable<T> query, int take = 0) => 0 < take ? query.Skip(Random(query.Count() - take)).Take(take) : query.Skip(Random(query.Count()));

        /// <summary>
        /// SkipRandom
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public static async ValueTask<IQueryable<T>> SkipRandomAsync<T>(this IQueryable<T> query, int take = 0) => 0 < take ? query.Skip(Random(await query.CountAsync() - take)).Take(take) : query.Skip(Random(await query.CountAsync()));


        //public static int InsertOrUpdate<T>(this IQueryable<T> target, System.Linq.Expressions.Expression<System.Func<T>> insertSetter, System.Linq.Expressions.Expression<System.Func<T, T>> onDuplicateKeyUpdateSetter, System.Linq.Expressions.Expression<System.Func<T>> keySelector) where T : class => LinqExtensions.InsertOrUpdate(target as ITable<T>, insertSetter, onDuplicateKeyUpdateSetter, keySelector);

        //public static int InsertOrUpdate<T>(this IQueryable<T> target, System.Linq.Expressions.Expression<System.Func<T>> insertSetter, System.Linq.Expressions.Expression<System.Func<T>> keySelector) where T : class, new() => InsertOrUpdate(target as ITable<T>, insertSetter, c => new T { }, keySelector);

        /// <summary>
        /// AsTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <returns></returns>
        public static ITable<T> AsTable<T>(this IQueryable<T> queryable) => queryable as ITable<T>;

        #region Paging

        /// <summary>
        /// GetPaging
        /// </summary>
        /// <param name="count"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageSizeMax"></param>
        /// <returns></returns>
        public static PagingInfo GetPagingInfo(int count, int currentPage, int pageSize, int pageSizeMax = 50)
        {
            if (0 == count) { return default; }

            var _pageSize = System.Math.Min(pageSize, pageSizeMax);
            var _countPage = System.Convert.ToDouble(count) / System.Convert.ToDouble(_pageSize);
            var countPage = (double.IsNaN(_countPage) || double.IsPositiveInfinity(_countPage) || double.IsNegativeInfinity(_countPage)) ? 1 : System.Convert.ToInt32(System.Math.Ceiling(_countPage));

            currentPage = currentPage < 0 ? 0 : currentPage > countPage ? countPage : currentPage;
            if (currentPage <= 0 && countPage > 0) { currentPage = 1; }

            return new PagingInfo(_pageSize * (currentPage - 1), _pageSize, currentPage, countPage);
        }

        /// <summary>
        /// GetPaging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageSizeMax"></param>
        /// <returns></returns>
        public static Paging<T> GetPaging<T>(this IQueryable<T> query, int currentPage, int pageSize, int pageSizeMax = 50) => GetPaging<T, Paging<T>>(query, currentPage, pageSize, pageSizeMax);

        /// <summary>
        /// GetPaging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Paging"></typeparam>
        /// <param name="query"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageSizeMax"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        public static Paging GetPaging<T, Paging>(this IQueryable<T> query, int currentPage, int pageSize, int pageSizeMax = 50, Paging paging = default)
            where Paging : IPaging<T>, new()
        {
            if (null == query) { throw new System.ArgumentNullException(nameof(query)); }

            var count = query.Count();

            if (Equals(null, paging))
            {
                paging = new Paging();
            }

            if (0 == count) { paging.Data = new List<T>(); paging.Length = paging.Data.Count; return paging; }

            var p = GetPagingInfo(count, currentPage, pageSize, pageSizeMax);

            paging.Data = query.Skip(p.Skip).Take(p.Take).ToList();
            paging.Length = paging.Data.Count;
            paging.CurrentPage = p.CurrentPage;
            paging.Count = count;
            paging.CountPage = p.CountPage;
            return paging;
        }

        /// <summary>
        /// GetPagingAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageSizeMax"></param>
        /// <returns></returns>
        public static ValueTask<Paging<T>> GetPagingAsync<T>(this IQueryable<T> query, int currentPage, int pageSize, int pageSizeMax = 50) => GetPagingAsync<T, Paging<T>>(query, currentPage, pageSize, pageSizeMax);

        /// <summary>
        /// GetPagingAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Paging"></typeparam>
        /// <param name="query"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageSizeMax"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        public static async ValueTask<Paging> GetPagingAsync<T, Paging>(this IQueryable<T> query, int currentPage, int pageSize, int pageSizeMax = 50, Paging paging = default)
            where Paging : IPaging<T>, new()
        {
            if (null == query) { throw new System.ArgumentNullException(nameof(query)); }

            var count = await query.CountAsync();

            if (Equals(null, paging))
            {
                paging = new Paging();
            }

            if (0 == count) { paging.Data = new List<T>(); paging.Length = paging.Data.Count; return paging; }

            var p = GetPagingInfo(count, currentPage, pageSize, pageSizeMax);

            paging.Data = await query.Skip(p.Skip).Take(p.Take).ToListAsync();
            paging.Length = paging.Data.Count;
            paging.CurrentPage = p.CurrentPage;
            paging.Count = count;
            paging.CountPage = p.CountPage;
            return paging;
        }

        /// <summary>
        /// GetPagingOrderBy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="query"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="order"></param>
        /// <param name="pageSizeMax"></param>
        /// <returns></returns>
        public static Paging<T> GetPagingOrderBy<T, TKey>(this IQueryable<T> query, int currentPage, int pageSize, System.Linq.Expressions.Expression<System.Func<T, TKey>> keySelector, Order order = Order.Ascending, int pageSizeMax = 50) => GetPagingOrderBy<T, TKey, Paging<T>>(query, currentPage, pageSize, keySelector, order, pageSizeMax);

        /// <summary>
        /// GetPagingOrderBy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="Paging"></typeparam>
        /// <param name="query"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="order"></param>
        /// <param name="pageSizeMax"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        public static Paging GetPagingOrderBy<T, TKey, Paging>(this IQueryable<T> query, int currentPage, int pageSize, System.Linq.Expressions.Expression<System.Func<T, TKey>> keySelector, Order order = Order.Ascending, int pageSizeMax = 50, Paging paging = default)
            where Paging : IPaging<T>, new()
        {
            if (null == query) { throw new System.ArgumentNullException(nameof(query)); }

            var count = query.Count();

            if (Equals(null, paging))
            {
                paging = new Paging();
            }

            if (0 == count) { paging.Data = new List<T>(); paging.Length = paging.Data.Count; return paging; }

            var p = GetPagingInfo(count, currentPage, pageSize, pageSizeMax);

            List<T> data = null;

            switch (order)
            {
                case Order.Ascending:
                    data = query.Skip(p.Skip).Take(p.Take).OrderBy(keySelector).ToList();
                    break;
                case Order.Descending:
                    data = query.Skip(p.Skip).Take(p.Take).OrderByDescending(keySelector).ToList();
                    break;
            }

            paging.Data = data;
            paging.Length = data.Count;
            paging.CurrentPage = p.CurrentPage;
            paging.Count = count;
            paging.CountPage = p.CountPage;
            return paging;
        }

        /// <summary>
        /// GetPagingOrderByAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="query"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="order"></param>
        /// <param name="pageSizeMax"></param>
        /// <returns></returns>
        public static ValueTask<Paging<T>> GetPagingOrderByAsync<T, TKey>(this IQueryable<T> query, int currentPage, int pageSize, System.Linq.Expressions.Expression<System.Func<T, TKey>> keySelector, Order order = Order.Ascending, int pageSizeMax = 50) => GetPagingOrderByAsync<T, TKey, Paging<T>>(query, currentPage, pageSize, keySelector, order, pageSizeMax);

        /// <summary>
        /// GetPagingOrderByAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="Paging"></typeparam>
        /// <param name="query"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="order"></param>
        /// <param name="pageSizeMax"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        public static async ValueTask<Paging> GetPagingOrderByAsync<T, TKey, Paging>(this IQueryable<T> query, int currentPage, int pageSize, System.Linq.Expressions.Expression<System.Func<T, TKey>> keySelector, Order order = Order.Ascending, int pageSizeMax = 50, Paging paging = default)
            where Paging : IPaging<T>, new()
        {
            if (null == query) { throw new System.ArgumentNullException(nameof(query)); }
            if (null == keySelector) { throw new System.ArgumentNullException(nameof(keySelector)); }

            var count = await query.CountAsync();

            if (Equals(null, paging))
            {
                paging = new Paging();
            }

            if (0 == count) { paging.Data = new List<T>(); paging.Length = paging.Data.Count; return paging; }

            var p = GetPagingInfo(count, currentPage, pageSize, pageSizeMax);

            List<T> data = null;

            switch (order)
            {
                case Order.Ascending:
                    data = await query.Skip(p.Skip).Take(p.Take).OrderBy(keySelector).ToListAsync();
                    break;
                case Order.Descending:
                    data = await query.Skip(p.Skip).Take(p.Take).OrderByDescending(keySelector).ToListAsync();
                    break;
            }

            paging.Data = data;
            paging.Length = data.Count;
            paging.CurrentPage = p.CurrentPage;
            paging.Count = count;
            paging.CountPage = p.CountPage;
            return paging;
        }

        /// <summary>
        /// ToPaging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Paging"></typeparam>
        /// <param name="paging"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Paging ToPaging<T, Paging>(this IPaging<T> paging, IEnumerable<T> data)
            where Paging : IPaging<T>, new()
        {
            if (Equals(null, paging)) { throw new System.ArgumentNullException(nameof(paging)); }
            if (null == data) { throw new System.ArgumentNullException(nameof(data)); }

            var data2 = data.ToList();
            return new Paging { Data = data2, Length = data2.Count, CurrentPage = paging.CurrentPage, Count = paging.Count, CountPage = paging.CountPage };
        }

        /// <summary>
        /// ToPaging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Paging"></typeparam>
        /// <param name="paging"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Paging ToPaging<T, Paging>(this Paging paging, IEnumerable<T> data)
            where Paging : IPaging<T>, new()
        {
            if (Equals(null, paging)) { throw new System.ArgumentNullException(nameof(paging)); }
            if (null == data) { throw new System.ArgumentNullException(nameof(data)); }

            var data2 = data.ToList();
            return new Paging { Data = data2, Length = data2.Count, CurrentPage = paging.CurrentPage, Count = paging.Count, CountPage = paging.CountPage };
        }

        #endregion

        /// <summary>
        /// Contains for PostgreSQL {1} = ANY({0})
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        [Sql.Expression(ProviderName.PostgreSQL, "{1} = ANY({0})")]
        public static bool Contains<T>(this T[] col, T val) where T : System.IComparable => col.Contains(val);
    }

    #region Settings

    /// <summary>
    /// ConnectionStringSettings
    /// </summary>
    public class ConnectionStringSettings : Configuration.IConnectionStringSettings
    {
        /// <summary>
        /// ConnectionString
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// LinqToDB.ProviderName
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// IsGlobal
        /// </summary>
        public bool IsGlobal => false;
    }

    /// <summary>
    /// T4: NamespaceName = "DataModels"; DataContextName = "Model"; BaseDataContextClass = "LinqToDB.Entitys"; PluralizeDataContextPropertyNames = false;
    /// </summary>
    public class LinqToDBSection : Configuration.ILinqToDBSettings
    {
        readonly IEnumerable<ConnectionStringSettings> connectionStringSettings;

        /// <summary>
        /// NamespaceName = "DataModels"; DataContextName = "Model"; BaseDataContextClass = "LinqToDB.Entitys"; PluralizeDataContextPropertyNames = false;
        /// </summary>
        /// <param name="connectionStringSettings"></param>
        /// <param name="defaultConfiguration"></param>
        public LinqToDBSection(IEnumerable<ConnectionStringSettings> connectionStringSettings, string defaultConfiguration = null)
        {
            this.connectionStringSettings = connectionStringSettings;
            var first = connectionStringSettings?.FirstOrDefault();
            DefaultConfiguration = defaultConfiguration ?? first?.Name;
            DefaultDataProvider = first?.ProviderName;
        }

        /// <summary>
        /// DataProviders
        /// </summary>
        public IEnumerable<Configuration.IDataProviderSettings> DataProviders => Enumerable.Empty<Configuration.IDataProviderSettings>();

        /// <summary>
        /// Key
        /// </summary>
        public string DefaultConfiguration { get; private set; }

        /// <summary>
        /// LinqToDB.ProviderName
        /// </summary>
        public string DefaultDataProvider { get; private set; }

        /// <summary>
        /// ConnectionStrings
        /// </summary>
        public IEnumerable<Configuration.IConnectionStringSettings> ConnectionStrings
        {
            get { foreach (var item in connectionStringSettings) { yield return item; } }
        }
    }

    #endregion
}