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
    using Business.Data;
    using System.Linq;
    using LinqToDB.DataProvider;

    /// <summary>
    /// the a LinqToDBConnection
    /// </summary>
    public class LinqToDBConnection : Data.DataConnection, IConnection
    {
        static int ForEach<T>(System.Collections.Generic.IEnumerable<T> obj, System.Func<T, int> func)
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

        public LinqToDBConnection() : base() { }

        public LinqToDBConnection(string configuration) : base(configuration) { }

        public LinqToDBConnection(string providerName, string connectionString) : base(providerName, connectionString) { }

        public LinqToDBConnection(IDataProvider provider, string conString) : base(provider, conString) { }

        public string TraceMethod { get; set; }

        public string TraceId { get; } = System.Guid.NewGuid().ToString("N");

        //public abstract IEntity Entity { get; }

        //Business.Data.IEntity IEntitys.Entity { get => Entity; }

        public new void BeginTransaction() => base.BeginTransaction();

        public new void BeginTransaction(System.Data.IsolationLevel isolationLevel) => base.BeginTransaction(isolationLevel);

        public void Commit() => base.CommitTransaction();

        public void Rollback() => base.RollbackTransaction();

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

        //public new void Dispose()
        //{
        //    DisposeCommand();
        //    base.Dispose();
        //    Transaction?.Dispose();
        //    Connection?.Dispose();
        //}
    }

    #region Settings

    public class ConnectionStringSettings : Configuration.IConnectionStringSettings
    {
        public string ConnectionString { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// LinqToDB.ProviderName
        /// </summary>
        public string ProviderName { get; set; }

        public bool IsGlobal => false;
    }

    /// <summary>
    /// T4: NamespaceName = "DataModels"; DataContextName = "Model"; BaseDataContextClass = "LinqToDB.Entitys"; PluralizeDataContextPropertyNames = false;
    /// </summary>
    public class LinqToDBSection : Configuration.ILinqToDBSettings
    {
        readonly System.Collections.Generic.IEnumerable<ConnectionStringSettings> connectionStringSettings;

        /// <summary>
        /// NamespaceName = "DataModels"; DataContextName = "Model"; BaseDataContextClass = "LinqToDB.Entitys"; PluralizeDataContextPropertyNames = false;
        /// </summary>
        /// <param name="connectionStringSettings"></param>
        /// <param name="defaultConfiguration"></param>
        public LinqToDBSection(System.Collections.Generic.IEnumerable<ConnectionStringSettings> connectionStringSettings, string defaultConfiguration = null)
        {
            this.connectionStringSettings = connectionStringSettings;
            var first = connectionStringSettings?.FirstOrDefault();
            DefaultConfiguration = defaultConfiguration ?? first?.Name;
            DefaultDataProvider = first?.ProviderName;
        }

        public System.Collections.Generic.IEnumerable<Configuration.IDataProviderSettings> DataProviders => Enumerable.Empty<Configuration.IDataProviderSettings>();
        /// <summary>
        /// Key
        /// </summary>
        public string DefaultConfiguration { get; private set; }
        /// <summary>
        /// LinqToDB.ProviderName
        /// </summary>
        public string DefaultDataProvider { get; private set; }

        public System.Collections.Generic.IEnumerable<Configuration.IConnectionStringSettings> ConnectionStrings
        {
            get { foreach (var item in connectionStringSettings) { yield return item; } }
        }
    }

    #endregion
}