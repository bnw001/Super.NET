using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Super.Framework
{
    #region DBHelp接口定义
    /// <summary>
    /// DBHelp接口定义
    /// </summary>
    public interface IDBHelper
    {
        #region 执行存储过程，获取受影响的行数

        #region 执行不带参数的存储过程，获取受影响的行数
        /// <summary>
        /// 执行不带参数的存储过程，获取受影响的行数
        /// </summary>
        int RunProcedureNonQuery(string _IStoredProcName);
        #endregion 执行不带参数的存储过程，获取受影响的行数

        #region 执行带参数的存储过程，获取受影响的行数
        /// <summary>
        /// 执行带参数的存储过程，获取受影响的行数
        /// </summary>
        int RunProcedureNonQuery(string _IStoredProcName, params DbParameter[] _ICmdParms);

        /// <summary>
        /// 执行带参数的存储过程，获取受影响的行数
        /// </summary>
        int RunProcedureNonQuery(string StoredProcName, Dictionary<string, string> cmdParms);
        #endregion 执行带参数的存储过程，获取受影响的行数

        #endregion 执行存储过程，获取受影响的行数

        #region 执行不带参数的SQL语句，获取受影响的行数
        /// <summary>
        /// 执行不带参数的SQL语句，获取受影响的行数
        /// </summary>
        int ExecuteNonQuery(string _ISqlString);
        #endregion 执行不带参数的SQL语句，获取受影响的行数

        #region 执行带参数的SQL语句，获取受影响的行数
        /// <summary>
        /// 执行带参数的SQL语句，获取受影响的行数
        /// </summary>
        int ExecuteNonQuery(string _ISqlString, params DbParameter[] _ICmdParms);

        /// <summary>
        /// 执行带参数的SQL语句，获取受影响的行数
        /// </summary>
        int ExecuteNonQuery(string _ISqlString, Dictionary<string, string> _ICmdParms);
        #endregion 执行带参数的SQL语句，获取受影响的行数

        #region 执行存储过程，获取首行首列

        #region 执行不带参数的存储过程，获取首行首列
        /// <summary>
        /// 执行不带参数的存储过程，获取首行首列
        /// </summary>
        object RunProcedureScalar(string _IStoredProcName, bool _IsRealData = false);
        #endregion 执行不带参数的存储过程，获取首行首列

        #region 执行带参数的存储过程，获取首行首列
        /// <summary>
        /// 执行带参数的存储过程，获取首行首列
        /// </summary>
        object RunProcedureScalar(string _IStoredProcName, DbParameter[] _ICmdParms, bool _IsRealData = false);

        /// <summary>
        /// 执行带参数的存储过程，获取首行首列
        /// </summary>
        object RunProcedureScalar(string _IStoredProcName, Dictionary<string, string> _ICmdParms, bool _IsRealData = false);
        #endregion 执行带参数的存储过程，获取首行首列

        #endregion 执行存储过程，获取首行首列

        #region 执行SQL语句，获取首行首列

        #region 执行不带参数的SQL语句，获取首行首列
        /// <summary>
        /// 执行不带参数的SQL语句，获取首行首列
        /// </summary>
        object ExecuteScalar(string _ISqlString, bool _IsRealData = false);
        #endregion 执行不带参数的SQL语句，获取首行首列

        #region 执行带参数的SQL语句，获取首行首列
        /// <summary>
        /// 执行带参数的SQL语句，获取首行首列
        /// </summary>
        object ExecuteScalar(string _ISqlString, DbParameter[] _ICmdParms, bool _IsRealData = false);

        /// <summary>
        /// 执行带参数的SQL语句，获取首行首列
        /// </summary>
        object ExecuteScalar(string sqlString, Dictionary<string, string> cmdParms, bool _IsRealData = false);
        #endregion 执行带参数的SQL语句，获取首行首列

        #endregion 执行SQL语句，获取首行首列

        #region 执行存储过程，获取DataSet对象

        #region 执行部带参数的存储过程，获取DataSet对象
        /// <summary>
        /// 执行部带参数的存储过程，获取DataSet对象
        /// </summary>
        DataSet RunProcedureDataSet(string _IStoredProcName, bool _IsRealData = false);
        #endregion 执行部带参数的存储过程，获取DataSet对象

        #region 执行带参数的存储过程，获取DataSet对象
        /// <summary>
        /// 执行带参数的存储过程，获取DataSet对象
        /// </summary>
        DataSet RunProcedureDataSet(string _IStoredProcName, DbParameter[] _ICmdParms, bool _IsRealData = false);

        /// <summary>
        /// 执行带参数的存储过程，获取DataSet对象
        /// </summary>
        DataSet RunProcedureDataSet(string _IStoredProcName, Dictionary<string, string> _ICmdParms, bool _IsRealData = false);
        #endregion 执行带参数的存储过程，获取DataSet对象

        #endregion 执行存储过程，获取DataSet对象

        #region 执行SQL语句，获取DataSet对象

        #region 执行不带参数的SQL语句，获取DataSet对象
        /// <summary>
        /// 执行不带参数的SQL语句，获取DataSet对象
        /// </summary>
        DataSet ExecuteDataSet(string _ISqlString, bool _IsRealData = false);
        #endregion 执行不带参数的SQL语句，获取DataSet对象

        #region 执行带参数的SQL语句，获取DataSet对象
        /// <summary>
        /// 执行带参数的SQL语句，获取DataSet对象
        /// </summary>
        DataSet ExecuteDataSet(string _ISqlString, DbParameter[] _ICmdParms, bool _IsRealData = false);

        /// <summary>
        /// 执行带参数的SQL语句，获取DataSet对象
        /// </summary>
        DataSet ExecuteDataSet(string _ISqlString, Dictionary<string, string> _ICmdParms, bool _IsRealData = false);
        #endregion 执行带参数的SQL语句，获取DataSet对象

        #endregion 执行SQL语句，获取DataSet对象

        #region 执行存储过程，获取DataTable 对象

        #region 执行不带参数的存储过程，获取DataTable对象
        /// <summary>
        /// 执行不带参数的存储过程，获取DataTable对象
        /// </summary>
        DataTable RunProcedureDataTable(string _IStoredProcName, bool _IsRealData = false);
        #endregion 执行不带参数的存储过程，获取DataTable对象

        #region 执行带参数的存储过程，获取DataTable对象
        /// <summary>
        /// 执行带参数的存储过程，获取DataTable对象
        /// </summary>
        DataTable RunProcedureDataTable(string _IStoredProcName, DbParameter[] _ICmdParms, bool _IsRealData = false);


        /// <summary>
        /// 执行带参数的存储过程，获取DataTable对象
        /// </summary>
        DataTable RunProcedureDataTable(string _IStoredProcName, Dictionary<string, string> _ICmdParms, bool _IsRealData = false);
        #endregion 执行带参数的存储过程，获取DataTable对象

        #endregion 执行存储过程，获取DataTable 对象

        #region 执行SQL语句，获取DataTable对象

        #region 执行不带参数的SQL语句，获取DataTable对象
        /// <summary>
        /// 执行不带参数的SQL语句，获取DataTable对象
        /// </summary>
        DataTable ExecuteDataTable(string _ISqlString, bool _IsRealData = false);
        #endregion 执行不带参数的SQL语句，获取DataTable对象

        #region 执行带参数的SQL语句，获取DataTable对象
        /// <summary>
        /// 执行带参数的SQL语句，获取DataTable对象
        /// </summary>
        DataTable ExecuteDataTable(string _ISqlString, DbParameter[] _ICmdParms, bool _IsRealData = false);

        /// <summary>
        /// 执行带参数的SQL语句，获取DataTable对象
        /// </summary>
        DataTable ExecuteDataTable(string _ISqlString, Dictionary<string, string> _ICmdParms, bool _IsRealData = false);
        #endregion 执行带参数的SQL语句，获取DataTable对象

        #endregion 执行SQL语句，获取DataTable对象

        #region 批量新增
        /// <summary>
        /// 批量新增
        /// </summary>
        void BatchInsert(string _IDTName, DataTable _IDT);
        #endregion 批量新增
    }
    #endregion DBHelp接口定义
}