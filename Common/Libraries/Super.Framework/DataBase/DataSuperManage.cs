using System;
using System.Reflection;

namespace Super.Framework
{
    #region 泛型数据类工厂
    /// <summary>
    /// 泛型数据类工厂
    /// </summary>
    public class DataSuperManage<T> where T : new()
    {
        #region 泛型数据类
        /// <summary>
        /// 泛型数据类
        /// </summary>
        public static IGenericData<T> GenericData(string _IDBType = "")
        {
            IGenericData<T> reVal = null;
            _IDBType = _IDBType.IsDBType() ? _IDBType : Config.DBType;
            switch (_IDBType)
            {
                case "SQLite":
                    reVal = new GenericDataSqlite<T>();
                    break;
                case "SqlServer":
                    reVal = new GenericDataSqlServer<T>();
                    break;
                case "MySql":
                    reVal = new GenericDataMySql<T>();
                    break;
                case "Oracle":
                    reVal = new GenericDataOracle<T>();
                    break;
                default:
                    break;
            }
            return reVal;
        }
        #endregion 泛型数据类
    }
    #endregion 泛型数据类工厂
}