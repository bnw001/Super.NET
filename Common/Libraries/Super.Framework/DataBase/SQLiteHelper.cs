using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace Super.Framework
{
    #region SQLite辅助类
    /// <summary>
    /// SQLite辅助类
    /// </summary>
    public class SQLiteHelper : IDBHelper
    {
        #region 变量

        #region 数据库连接字符串
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
        #endregion 数据库连接字符串

        #region 命令执行超时时间
        /// <summary>
        /// 命令执行超时时间
        /// </summary>
        public int CommandTimeOut { get; set; }
        #endregion 命令执行超时时间

        #endregion 变量

        #region 静态缓存

        #region DataTable缓存
        /// <summary>
        /// DataTable缓存
        /// </summary>
        private static Dictionary<string, DataTable> _CacheDataTable = new Dictionary<string, DataTable>();

        /// <summary>
        /// DataTable缓存时间
        /// </summary>
        private static Dictionary<string, DateTime> _CacheDataTableDateTime = new Dictionary<string, DateTime>();
        #endregion DataTable缓存

        #region DataSet缓存
        /// <summary>
        /// DataSet缓存
        /// </summary>
        private static Dictionary<string, DataSet> _CacheDataSet = new Dictionary<string, DataSet>();

        /// <summary>
        /// DataSet缓存时间
        /// </summary>
        private static Dictionary<string, DateTime> _CacheDataSetTime = new Dictionary<string, DateTime>();

        #endregion DataSet缓存

        #region Object缓存
        /// <summary>
        /// Object缓存
        /// </summary>
        private static Dictionary<string, object> _CacheOBJ = new Dictionary<string, object>();

        /// <summary>
        /// Object缓存时间
        /// </summary>
        private static Dictionary<string, DateTime> _CacheOBJTime = new Dictionary<string, DateTime>();
        #endregion Object缓存

        #endregion 静态缓存

        #region 初始化

        #region 无参初始化
        /// <summary>
        /// 无参初始化
        /// </summary>
        public SQLiteHelper()
        {
        }
        #endregion 无参初始化

        #region 带连接字符串初始化
        /// <summary>
        /// 带连接字符串初始化
        /// </summary>
        /// <param name="connString">数据库连接字符串</param>
        public SQLiteHelper(string connString)
        {
            ConnectionString = connString;
        }
        #endregion 带连接字符串初始化

        #region 带执行超时时间的初始化
        /// <summary>
        /// 带执行超时时间的初始化
        /// </summary>
        /// <param name="commandTimeOut">命令执行超时时间</param>
        public SQLiteHelper(int commandTimeOut) { }
        #endregion  带执行超时时间的初始化

        #region 带连接字符串和超时时间初始化
        /// <summary>
        /// 带连接字符串和超时时间初始化
        /// </summary>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="timeOut">命令执行超时时间</param>
        public SQLiteHelper(string connString, int commandTimeOut) { }
        #endregion 带连接字符串和超时时间初始化

        #endregion  初始化

        #region 执行存储过程，获取受影响的行数

        #region 执行不带参数的存储过程，获取受影响的行数
        /// <summary>
        /// 执行不带参数的存储过程，获取受影响的行数
        /// </summary>
        /// <param name="StoredProcName">存储过程名称</param>
        public int RunProcedureNonQuery(string StoredProcName)
        {
            DbParameter[] parms = new DbParameter[] { };
            return RunProcedureNonQuery(StoredProcName, parms);
        }
        #endregion 执行不带参数的存储过程，获取受影响的行数

        #region 执行带参数的存储过程，获取受影响的行数
        /// <summary>
        /// 执行带参数的存储过程，获取受影响的行数
        /// </summary>
        /// <param name="StoredProcName">存储过程名称</param>
        /// <param name="cmdParms">参数列表</param>
        public int RunProcedureNonQuery(string StoredProcName, params DbParameter[] cmdParms)
        {
            int reVal = 0;

            return reVal;
        }

        /// <summary>
        /// 执行带参数的存储过程，获取受影响的行数
        /// </summary>
        /// <param name="StoredProcName">存储过程名称</param>
        /// <param name="cmdParms">参数列表</param>
        public int RunProcedureNonQuery(string StoredProcName, Dictionary<string, string> cmdParms)
        {
            int reVal = 0;

            return reVal;
        }
        #endregion 执行带参数的存储过程，获取受影响的行数

        #endregion 执行存储过程，获取受影响的行数

        #region 执行SQL语句，获取受影响的行数

        #region 执行不带参数的SQL语句，获取受影响的行数
        /// <summary>
        /// 执行SQL语句，获取受影响的行数
        /// </summary>
        /// <param name="sqlString">执行的SQL语句</param>
        public int ExecuteNonQuery(string sqlString)
        {
            DbParameter[] parms = new DbParameter[] { };
            return ExecuteNonQuery(sqlString, parms);
        }
        #endregion 执行不带参数的SQL语句，获取受影响的行数

        #region 执行带参数的SQL语句，获取受影响的行数
        /// <summary>
        /// 执行带参数的SQL语句，获取受影响的行数
        /// </summary>
        /// <param name="sqlString">执行的SQL语句</param>
        /// <param name="cmdParms">参数列表</param>
        public int ExecuteNonQuery(string sqlString, params DbParameter[] cmdParms)
        {
            int reVal = 0;
            using (SQLiteConnection sqliteConn = new SQLiteConnection(ConnectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, sqliteConn, null, CommandType.Text, sqlString, cmdParms);
                        reVal = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    catch (SQLiteException ex)
                    {
                        string strParms = string.Empty;
                        foreach (DbParameter parm in cmdParms)
                        {
                            strParms += parm.ParameterName + ":" + parm.Value + "\r\n";
                        }
                        ex.Data.Add("Sql", sqlString);
                        ex.Data.Add("SqlParms", strParms);
                        throw ex;
                    }
                    finally
                    {
                        cmd.Dispose();
                        if (sqliteConn.State != ConnectionState.Closed)
                        {
                            sqliteConn.Close();
                        }
                    }
                }
            }

            return reVal;
        }

        /// <summary>
        /// 执行带参数的SQL语句，获取受影响的行数
        /// </summary>
        /// <param name="sqlString">执行的SQL语句</param>
        /// <param name="cmdParms">参数列表</param>
        public int ExecuteNonQuery(string sqlString, Dictionary<string, string> cmdParms)
        {
            int reVal = 0;
            using (SQLiteConnection sqliteConn = new SQLiteConnection(ConnectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, sqliteConn, null, CommandType.Text, sqlString, cmdParms);
                        reVal = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    catch (SQLiteException ex)
                    {
                        string strParms = string.Empty;
                        foreach (string Key in cmdParms.Keys)
                        {
                            strParms += Key + ":" + cmdParms[Key] + "\r\n";
                        }
                        ex.Data.Add("Sql", sqlString);
                        ex.Data.Add("SqlParms", strParms);
                        throw ex;
                    }
                    finally
                    {
                        cmd.Dispose();
                        if (sqliteConn.State != ConnectionState.Closed)
                        {
                            sqliteConn.Close();
                        }
                    }
                }
            }

            return reVal;
        }
        #endregion 执行带参数的SQL语句，获取受影响的行数

        #endregion 执行SQL语句，获取受影响的行数

        #region 执行存储过程，获取首行首列

        #region 执行不带参数的存储过程，获取首行首列
        /// <summary>
        /// 执行不带参数的存储过程，获取首行首列
        /// </summary>
        /// <param name="StoredProcName">存储过程名称</param>
        public object RunProcedureScalar(string StoredProcName, bool _IsRealData = false)
        {
            DbParameter[] cmdParms = new DbParameter[] { };
            return RunProcedureScalar(StoredProcName, cmdParms);
        }
        #endregion 执行不带参数的存储过程，获取首行首列

        #region 执行带参数的存储过程，获取首行首列
        /// <summary>
        /// 执行带参数的存储过程，获取首行首列
        /// </summary>
        public object RunProcedureScalar(string _IStoredProcName, DbParameter[] _IParms, bool _IsRealData = false)
        {
            object reVal = null;

            return reVal;
        }

        /// <summary>
        /// 执行带参数的存储过程，获取首行首列
        /// </summary>
        public object RunProcedureScalar(string _IStoredProcName, Dictionary<string, string> _IParms, bool _IsRealData = false)
        {
            object reVal = null;

            return reVal;
        }
        #endregion 执行带参数的存储过程，获取首行首列

        #endregion 执行存储过程，获取首行首列

        #region 执行SQL语句，获取首行首列

        #region 执行不带参数的SQL语句，获取首行首列
        /// <summary>
        /// 执行不带参数的SQL语句，获取首行首列
        /// </summary>
        public object ExecuteScalar(string _ISQL, bool _IsRealData = false)
        {
            DbParameter[] parms = new DbParameter[] { };
            return ExecuteScalar(_ISQL, parms, _IsRealData);
        }
        #endregion 执行带参数的SQL语句，获取首行首列

        #region 执行带参数的SQL语句，获取首行首列
        /// <summary>
        /// 执行带参数的SQL语句，获取首行首列
        /// </summary>
        public object ExecuteScalar(string _ISQL, DbParameter[] _IParms, bool _IsRealData = false)
        {
            object reVal = null;
            using (SQLiteConnection sqliteConn = new SQLiteConnection(ConnectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    try
                    {
                        string _HashKey = ConnectionString + "-ExecuteScalar-" + _ISQL;
                        foreach (DbParameter _FParm in _IParms)
                        {
                            _HashKey += "-" + _FParm.ParameterName + "-" + _FParm.Value;
                        }
                        OwnCacheManage.ClearCache<object>(_CacheOBJ, _CacheOBJTime, CacheConfig.DBSelectTimeOut, _HashKey);
                        if (!_IsRealData && _CacheOBJ.ContainsKey(_HashKey))
                        {
                            reVal = _CacheOBJ[_HashKey];
                        }
                        else
                        {
                            PrepareCommand(cmd, sqliteConn, null, CommandType.Text, _ISQL, _IParms);
                            object obj = cmd.ExecuteScalar();
                            cmd.Parameters.Clear();
                            if (!((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value))))
                            {
                                reVal = obj;
                                OwnCacheManage.SaveCache<object>(_CacheOBJ, _CacheOBJTime, _HashKey, reVal);
                            }
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        string strParms = string.Empty;
                        foreach (DbParameter parm in _IParms)
                        {
                            strParms += parm.ParameterName + ":" + parm.Value + "\r\n";
                        }
                        ex.Data.Add("Sql", _ISQL);
                        ex.Data.Add("SqlParms", strParms);
                        throw ex;
                    }
                    finally
                    {
                        cmd.Dispose();
                        if (sqliteConn.State != ConnectionState.Closed)
                        {
                            sqliteConn.Close();
                        }
                    }
                }
            }

            return reVal;
        }

        /// <summary>
        /// 执行带参数的SQL语句，获取首行首列
        /// </summary>
        public object ExecuteScalar(string _ISQL, Dictionary<string, string> _IParms, bool _IsRealData = false)
        {
            object reVal = null;
            using (SQLiteConnection sqliteConn = new SQLiteConnection(ConnectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    try
                    {
                        string _HashKey = ConnectionString + "-ExecuteScalar-" + _ISQL;
                        foreach (string _FParm in _IParms.Keys)
                        {
                            _HashKey += "-" + _FParm + "-" + _IParms[_FParm];
                        }
                        OwnCacheManage.ClearCache<object>(_CacheOBJ, _CacheOBJTime, CacheConfig.DBSelectTimeOut, _HashKey);
                        if (!_IsRealData && _CacheOBJ.ContainsKey(_HashKey))
                        {
                            reVal = _CacheOBJ[_HashKey];
                        }
                        else
                        {
                            PrepareCommand(cmd, sqliteConn, null, CommandType.Text, _ISQL, _IParms);
                            object obj = cmd.ExecuteScalar();
                            cmd.Parameters.Clear();
                            if (!((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value))))
                            {
                                reVal = obj;
                                OwnCacheManage.SaveCache<object>(_CacheOBJ, _CacheOBJTime, _HashKey, reVal);
                            }
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        string strParms = string.Empty;
                        foreach (string Key in _IParms.Keys)
                        {
                            strParms += Key + ":" + _IParms[Key] + "\r\n";
                        }
                        ex.Data.Add("Sql", _ISQL);
                        ex.Data.Add("SqlParms", strParms);
                        throw ex;
                    }
                    finally
                    {
                        cmd.Dispose();
                        if (sqliteConn.State != ConnectionState.Closed)
                        {
                            sqliteConn.Close();
                        }
                    }
                }
            }

            return reVal;
        }
        #endregion 执行带参数的SQL语句，获取首行首列

        #endregion 执行SQL语句，获取首行首列

        #region 执行存储过程，获取DataSet对象

        #region 执行部带参数的存储过程，获取DataSet对象
        /// <summary>
        /// 执行部带参数的存储过程，获取DataSet对象
        /// </summary>
        /// <param name="StoredProcName">存储过程名称</param>
        public DataSet RunProcedureDataSet(string StoredProcName, bool _IsRealData = false)
        {
            DbParameter[] cmdParms = new DbParameter[] { };
            return RunProcedureDataSet(StoredProcName, cmdParms);
        }
        #endregion 执行部带参数的存储过程，获取DataSet对象

        #region 执行带参数的存储过程，获取DataSet对象
        /// <summary>
        /// 执行带参数的存储过程，获取DataSet对象
        /// </summary>
        public DataSet RunProcedureDataSet(string StoredProcName, DbParameter[] cmdParms, bool _IsRealData = false)
        {
            DataSet reVal = null;

            return reVal;
        }

        /// <summary>
        /// 执行带参数的存储过程，获取DataSet对象
        /// </summary>
        public DataSet RunProcedureDataSet(string StoredProcName, Dictionary<string, string> cmdParms, bool _IsRealData = false)
        {
            DataSet reVal = null;

            return reVal;
        }
        #endregion 执行带参数的存储过程，获取DataSet对象

        #endregion 执行存储过程，获取DataSet对象

        #region 执行SQL语句，获取DataSet对象

        #region 执行不带参数的SQL语句，获取DataSet对象
        /// <summary>
        /// 执行不带参数的SQL语句，获取DataSet对象
        /// </summary>
        /// <param name="sqlString">执行的SQL语句</param>
        public DataSet ExecuteDataSet(string sqlString, bool _IsRealData = false)
        {
            DbParameter[] parms = new DbParameter[] { };
            return ExecuteDataSet(sqlString, parms, _IsRealData);
        }
        #endregion  执行不带参数的SQL语句，获取DataSet对象

        #region 执行带参数的SQL语句，获取DataSet对象
        /// <summary>
        /// 执行带参数的SQL语句，获取DataSet对象
        /// </summary>
        public DataSet ExecuteDataSet(string _ISQL, DbParameter[] _IParms, bool _IsRealData = false)
        {
            DataSet reVal = new DataSet();
            string _HashKey = ConnectionString + "-ExecuteDataSet-" + _ISQL;
            foreach (DbParameter _FParm in _IParms)
            {
                _HashKey += "-" + _FParm.ParameterName + "-" + _FParm.Value;
            }
            OwnCacheManage.ClearCache<DataSet>(_CacheDataSet, _CacheDataSetTime, CacheConfig.DBSelectTimeOut, _HashKey);
            if (!_IsRealData && _CacheDataSet.ContainsKey(_HashKey))
            {
                reVal = _CacheDataSet[_HashKey];
            }
            else
            {
                using (SQLiteConnection sqliteConn = new SQLiteConnection(ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, sqliteConn, null, CommandType.Text, _ISQL, _IParms);
                            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                            adapter.Fill(reVal);
                            cmd.Parameters.Clear();
                            OwnCacheManage.SaveCache(_CacheDataSet, _CacheDataSetTime, _HashKey, reVal);
                        }
                        catch (SQLiteException ex)
                        {
                            string strParms = string.Empty;
                            foreach (DbParameter parm in _IParms)
                            {
                                strParms += parm.ParameterName + ":" + parm.Value + "\r\n";
                            }
                            ex.Data.Add("Sql", _ISQL);
                            ex.Data.Add("SqlParms", strParms);
                            throw ex;
                        }
                        finally
                        {
                            cmd.Dispose();
                            if (sqliteConn.State != ConnectionState.Closed)
                            {
                                sqliteConn.Close();
                            }
                        }
                    }
                }
            }

            return reVal;
        }

        /// <summary>
        /// 执行带参数的SQL语句，获取DataSet对象
        /// </summary>
        public DataSet ExecuteDataSet(string _ISQL, Dictionary<string, string> _IParms, bool _IsRealData = false)
        {
            DataSet reVal = new DataSet();
            string _HashKey = ConnectionString + "-ExecuteDataSet-" + _ISQL;
            foreach (string _FParm in _IParms.Keys)
            {
                _HashKey += "-" + _FParm + "-" + _IParms[_FParm];
            }
            OwnCacheManage.ClearCache<DataSet>(_CacheDataSet, _CacheDataSetTime, CacheConfig.DBSelectTimeOut, _HashKey);
            if (!_IsRealData && _CacheDataSet.ContainsKey(_HashKey))
            {
                reVal = _CacheDataSet[_HashKey];
            }
            else
            {
                using (SQLiteConnection _Conn = new SQLiteConnection(ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, _Conn, null, CommandType.Text, _ISQL, _IParms);
                            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                            adapter.Fill(reVal);
                            cmd.Parameters.Clear();
                            OwnCacheManage.SaveCache(_CacheDataSet, _CacheDataSetTime, _HashKey, reVal);
                        }
                        catch (SQLiteException ex)
                        {
                            string strParms = string.Empty;
                            foreach (string Key in _IParms.Keys)
                            {
                                strParms += Key + ":" + _IParms[Key] + "\r\n";
                            }
                            ex.Data.Add("Sql", _ISQL);
                            ex.Data.Add("SqlParms", strParms);
                            throw ex;
                        }
                        finally
                        {
                            cmd.Dispose();
                            if (_Conn.State != ConnectionState.Closed)
                            {
                                _Conn.Close();
                            }
                        }
                    }
                }
            }

            return reVal;
        }
        #endregion  执行带参数的SQL语句，获取DataSet对象

        #endregion 执行SQL语句，获取DataSet对象

        #region 执行存储过程，获取DataTable 对象

        #region 执行不带参数的存储过程，获取DataTable对象
        /// <summary>
        /// 执行不带参数的存储过程，获取DataTable对象
        /// </summary>
        public DataTable RunProcedureDataTable(string StoredProcName, bool _IsRealData = false)
        {
            DbParameter[] parms = new DbParameter[] { };
            return RunProcedureDataTable(StoredProcName, parms);
        }
        #endregion 执行不带参数的存储过程，获取DataTable对象

        #region 执行带参数的存储过程，获取DataTable对象
        /// <summary>
        /// 执行带参数的存储过程，获取DataTable对象
        /// </summary>
        public DataTable RunProcedureDataTable(string StoredProcName, DbParameter[] cmdParms, bool _IsRealData = false)
        {
            DataTable reVal = null;

            return reVal;
        }

        /// <summary>
        /// 执行带参数的存储过程，获取DataTable对象
        /// </summary>
        /// <param name="StoredProcName">存储过程名称</param>
        /// <param name="cmdParms">参数列表</param>
        public DataTable RunProcedureDataTable(string StoredProcName, Dictionary<string, string> cmdParms, bool _IsRealData = false)
        {
            DataTable reVal = null;

            return reVal;
        }
        #endregion 执行带参数的存储过程，获取DataTable对象

        #endregion 执行存储过程，获取DataTable对象

        #region 执行SQL语句，获取DataTable对象

        #region 执行不带参数的SQL语句，获取DataTable对象
        /// <summary>
        /// 执行不带参数的SQL语句，获取DataTable对象
        /// </summary>
        public DataTable ExecuteDataTable(string _ISQL, bool _IsRealData = false)
        {
            DbParameter[] parms = new DbParameter[] { };
            return ExecuteDataTable(_ISQL, parms);
        }
        #endregion 执行不带参数的SQL语句，获取DataTable对象

        #region 执行带参数的SQL语句，获取DataTable对象
        /// <summary>
        /// 执行带参数的SQL语句，获取DataTable对象
        /// </summary>
        public DataTable ExecuteDataTable(string _ISQL, DbParameter[] _IParms, bool _IsRealData = false)
        {
            DataTable reVal = new DataTable();
            string _HashKey = ConnectionString + "-ExecuteDataTable-" + _ISQL;
            foreach (DbParameter _FParm in _IParms)
            {
                _HashKey += "-" + _FParm.ParameterName + "-" + _FParm.Value;
            }
            OwnCacheManage.ClearCache<DataTable>(_CacheDataTable, _CacheDataTableDateTime, CacheConfig.DBSelectTimeOut, _HashKey);
            if (_IsRealData && _CacheDataTable.ContainsKey(_HashKey))
            {
                reVal = _CacheDataTable[_HashKey];
            }
            else
            {
                using (SQLiteConnection sqliteConn = new SQLiteConnection(ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, sqliteConn, null, CommandType.Text, _ISQL, _IParms);
                            SQLiteDataReader _reader = cmd.ExecuteReader();
                            reVal.Load(_reader);
                            _reader.Close();
                            _reader.Dispose();
                            cmd.Parameters.Clear();
                            OwnCacheManage.SaveCache<DataTable>(_CacheDataTable, _CacheDataTableDateTime, _HashKey, reVal);
                        }
                        catch (SQLiteException ex)
                        {
                            string strParms = string.Empty;
                            foreach (DbParameter parm in _IParms)
                            {
                                strParms += parm.ParameterName + ":" + parm.Value + "\r\n";
                            }
                            ex.Data.Add("Sql", _ISQL);
                            ex.Data.Add("SqlParms", strParms);
                            throw ex;
                        }
                        finally
                        {
                            cmd.Dispose();
                            if (sqliteConn.State != ConnectionState.Closed)
                            {
                                sqliteConn.Close();
                            }
                        }
                    }
                }
            }

            return reVal;
        }

        /// <summary>
        /// 执行带参数的SQL语句，获取DataTable对象
        /// </summary>
        public DataTable ExecuteDataTable(string _ISQL, Dictionary<string, string> _IParms, bool _IsRealData = false)
        {
            DataTable reVal = new DataTable();
            string _HashKey = ConnectionString + "-ExecuteDataTable-" + _ISQL;
            foreach (string _FParm in _IParms.Keys)
            {
                _HashKey += "-" + _FParm + "-" + _IParms[_FParm];
            }
            OwnCacheManage.ClearCache<DataTable>(_CacheDataTable, _CacheDataTableDateTime, CacheConfig.DBSelectTimeOut, _HashKey);
            if (_IsRealData && _CacheDataTable.ContainsKey(_HashKey))
            {
                reVal = _CacheDataTable[_HashKey];
            }
            else
            {
                using (SQLiteConnection sqliteConn = new SQLiteConnection(ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, sqliteConn, null, CommandType.Text, _ISQL, _IParms);
                            SQLiteDataReader _reader = cmd.ExecuteReader();
                            reVal.Load(_reader);
                            _reader.Close();
                            _reader.Dispose();
                            cmd.Parameters.Clear();
                            OwnCacheManage.SaveCache<DataTable>(_CacheDataTable, _CacheDataTableDateTime, _HashKey, reVal);
                        }
                        catch (SQLiteException ex)
                        {
                            string strParms = string.Empty;
                            foreach (string Key in _IParms.Keys)
                            {
                                strParms += Key + ":" + _IParms[Key] + "\r\n";
                            }
                            ex.Data.Add("Sql", _ISQL);
                            ex.Data.Add("SqlParms", strParms);
                            throw ex;
                        }
                        finally
                        {
                            cmd.Dispose();
                            if (sqliteConn.State != ConnectionState.Closed)
                            {
                                sqliteConn.Close();
                            }
                        }
                    }
                }
            }

            return reVal;
        }
        #endregion 执行带参数的SQL语句，获取DataTable对象

        #endregion 执行SQL语句，获取DataTable对象

        #region 批量新增
        /// <summary>
        /// 批量新增
        /// </summary>
        public void BatchInsert(string _IDTName, DataTable _IDT)
        {
            if (_IDTName.IsNotNullOrEmpty() && _IDT.Rows.Count > 0)
            {

            }
        }
        #endregion 批量新增

        #region 为执行命令准备参数
        /// <summary>
        /// 为执行命令附加参数列表
        /// </summary>
        /// <param name="cmd">SQLiteCommand  命令</param>
        /// <param name="trans">数据库事物处理</param>
        /// <param name="cmdType">SQLiteCommand命令类型 (存储过程， T-SQL语句))</param>
        /// <param name="cmdText">Command text，T-SQL语句</param>
        /// <param name="cmdParms">参数列表</param>
        private void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, SQLiteTransaction trans, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = this.CommandTimeOut;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = CommandType.Text;
            if (cmdParms != null)
            {
                foreach (SQLiteParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        /// <summary>
        /// 为执行命令附加参数列表
        /// </summary>
        private void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, SQLiteTransaction trans, CommandType cmdType, string cmdText, Dictionary<string, string> cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = this.CommandTimeOut;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = CommandType.Text;
            SQLiteParameter sqliteParm = new SQLiteParameter();
            if (cmdParms != null)
            {
                foreach (KeyValuePair<string, string> parm in cmdParms)
                {
                    sqliteParm = new SQLiteParameter(parm.Key, parm.Value);
                    cmd.Parameters.Add(sqliteParm);
                }
            }
        }
        #endregion 为执行命令准备参数
    }
    #endregion SQLite辅助类
}