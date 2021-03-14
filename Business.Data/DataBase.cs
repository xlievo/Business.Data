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

using LinqToDB;
using System.Runtime.CompilerServices;

namespace Business.Data
{
    /*
    public enum DataType
    {
        Undefined = 0,
        Char = 1,
        VarChar = 2,
        Text = 3,
        NChar = 4,
        NVarChar = 5,
        NText = 6,
        Binary = 7,
        VarBinary = 8,
        Blob = 9,
        Image = 10,
        Boolean = 11,
        Guid = 12,
        SByte = 13,
        Int16 = 14,
        Int32 = 15,
        Int64 = 16,
        Byte = 17,
        UInt16 = 18,
        UInt32 = 19,
        UInt64 = 20,
        Single = 21,
        Double = 22,
        Decimal = 23,
        Money = 24,
        SmallMoney = 25,
        Date = 26,
        Time = 27,
        DateTime = 28,
        DateTime2 = 29,
        SmallDateTime = 30,
        DateTimeOffset = 31,
        Timestamp = 32,
        Xml = 33,
        Variant = 34,
        VarNumeric = 35,
        Udt = 36,
    }
    */


    /*
    public struct DataParameter
    {
        public DataParameter(string name, object value) { this.Name = name; this.Value = value; }

        public string Name { get; private set; }

        public object Value { get; private set; }
    }
    */
    //[ProtoBuf.ProtoContract(SkipConstructor = true)]
    //public struct Paging<T>
    //{
    //    public static implicit operator Paging<T>(string value)
    //    {
    //        return Help2.JsonDeserialize<Paging<T>>(value);
    //    }
    //    public static implicit operator Paging<T>(byte[] value)
    //    {
    //        return Help2.ProtoBufDeserialize<Paging<T>>(value);
    //    }

    //    [ProtoBuf.ProtoMember(1, Name = "D")]
    //    [Newtonsoft.Json.JsonProperty(PropertyName = "D")]
    //    public System.Collections.Generic.IList<T> Data { get; set; }

    //    [ProtoBuf.ProtoMember(2, Name = "P")]
    //    [Newtonsoft.Json.JsonProperty(PropertyName = "P")]
    //    public int CurrentPage { get; set; }

    //    [ProtoBuf.ProtoMember(3, Name = "C")]
    //    [Newtonsoft.Json.JsonProperty(PropertyName = "C")]
    //    public int Count { get; set; }

    //    public override string ToString()
    //    {
    //        return Newtonsoft.Json.JsonConvert.SerializeObject(this);
    //    }

    //    public byte[] ToBytes()
    //    {
    //        return Help2.ProtoBufSerialize(this);
    //    }
    //}

    /// <summary>
    /// DataBase
    /// </summary>
    /// <typeparam name="Connection"></typeparam>
    public abstract class DataBase<Connection> : IData
        where Connection : LinqToDBConnection
    {
        static DataBase() => LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;

        /// <summary>
        /// GetConnection
        /// </summary>
        /// <param name="callMethod"></param>
        /// <returns></returns>
        public abstract Connection GetConnection([CallerMemberName] string callMethod = null);

        /// <summary>
        /// GetConnection
        /// </summary>
        /// <param name="callMethod"></param>
        /// <returns></returns>
        LinqToDBConnection IData.GetConnection([CallerMemberName] string callMethod = null) => GetConnection(callMethod);

        /*
        static Result UseConnection<Result>(System.Func<string, Data.IConnection> getConnection, System.Func<Data.IConnection, Result> func, string callMethod)
        {
            using (var con = getConnection(callMethod)) { return func(con); }
        }

        public int Save<T>(System.Collections.Generic.IEnumerable<T> obj, [CallerMemberName] string callMethod = null)
        {
            if (obj == null)
            {
                throw new System.ArgumentNullException(nameof(obj));
            }

            return UseConnection(GetConnection, con => con.Save(obj), callMethod);
        }

        public int Save<T>(T obj, [CallerMemberName] string callMethod = null)
        {
            return UseConnection(GetConnection, con => con.Save(obj), callMethod);
        }

        public int SaveWithInt32Identity<T>(T obj, [CallerMemberName] string callMethod = null)
        {
            return UseConnection(GetConnection, con => con.SaveWithInt32Identity(obj), callMethod);
        }

        public long SaveWithInt64Identity<T>(T obj, [CallerMemberName] string callMethod = null)
        {
            return UseConnection(GetConnection, con => con.SaveWithInt64Identity(obj), callMethod);
        }

        public int SaveOrUpdate<T>(System.Collections.Generic.IEnumerable<T> obj, [CallerMemberName] string callMethod = null)
        {
            if (obj == null)
            {
                throw new System.ArgumentNullException(nameof(obj));
            }

            return UseConnection(GetConnection, con => { return con.SaveOrUpdate(obj); }, callMethod);
        }

        public int SaveOrUpdate<T>(T obj, [CallerMemberName] string callMethod = null)
        {
            return UseConnection(GetConnection, con => { return con.SaveOrUpdate(obj); }, callMethod);
        }

        public int Update<T>(System.Collections.Generic.IEnumerable<T> obj, [CallerMemberName] string callMethod = null)
        {
            if (obj == null)
            {
                throw new System.ArgumentNullException(nameof(obj));
            }

            return UseConnection(GetConnection, con => con.Update(obj), callMethod);
        }

        public int Update<T>(T obj, [CallerMemberName] string callMethod = null)
        {
            return UseConnection(GetConnection, con => con.Update(obj), callMethod);
        }

        public int Delete<T>(System.Collections.Generic.IEnumerable<T> obj, [CallerMemberName] string callMethod = null)
        {
            if (obj == null)
            {
                throw new System.ArgumentNullException(nameof(obj));
            }

            return UseConnection(GetConnection, con => con.Delete(obj), callMethod);
        }

        public int Delete<T>(T obj, [CallerMemberName] string callMethod = null)
        {
            return UseConnection(GetConnection, con => con.Delete(obj), callMethod);
        }

        public int ExecuteNonQuery(string commandText, System.Data.CommandType commandType = System.Data.CommandType.Text, DataParameter[] parameter = null, [CallerMemberName] string callMethod = null)
        {
            return UseConnection(GetConnection, con => con.ExecuteNonQuery(commandText, commandType, parameter), callMethod);
        }

        public Result ExecuteScalar<Result>(string commandText, System.Data.CommandType commandType = System.Data.CommandType.Text, DataParameter[] parameter = null, [CallerMemberName] string callMethod = null)
        {
            return UseConnection(GetConnection, con => con.ExecuteScalar<Result>(commandText, commandType, parameter), callMethod);
        }
        */
    }

    //public class DB<IConnection> : DataBase<IConnection>
    //    where IConnection : class, Data.IConnection
    //{
    //    readonly System.Func<IConnection> creat;

    //    public DB(System.Func<IConnection> creat) => this.creat = creat;

    //    public override IConnection GetConnection() => creat();

    //    public static DB<IConnection> Creat(System.Func<IConnection> creat) => new DB<IConnection>(creat);
    //}

    //public abstract class EntitysBase : System.MarshalByRefObject, IEntity
    //{
    //    public abstract IQueryable<T> Get<T>() where T : class, new();
    //}
}