using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Super.Framework
{
    #region Oracle辅助类
    /// <summary>
    /// Oracle辅助类
    /// </summary>
    public sealed class OracleHelper : IDBHelper
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

        #region 参数缓存
        /// <summary>
        /// 参数缓存
        /// </summary>
        private static Dictionary<string, OracleParameter[]> _CacheProcSqlParameter = new Dictionary<string, OracleParameter[]>();

        /// <summary>
        /// 参数缓存时间
        /// </summary>
        private static Dictionary<string, DateTime> _CacheProcSqlParameterTime = new Dictionary<string, DateTime>();
        #endregion 参数缓存

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
        public OracleHelper()
        {
            this.ConnectionString = Config.OracleConnectionString;
            this.CommandTimeOut = 600;
        }
        #endregion 无参初始化

        #region 带连接字符串初始化
        /// <summary>
        /// 带连接字符串初始化
        /// </summary>
        /// <param name="_IConnString">数据库连接字符串</param>
        public OracleHelper(string _IConnString)
        {
            this.ConnectionString = _IConnString;
        }
        #endregion 带连接字符串初始化

        #region 带执行超时时间的初始化
        /// <summary>
        /// 带执行超时时间的初始化
        /// </summary>
        /// <param name="_ICommandTimeOut">命令执行超时时间</param>
        public OracleHelper(int _ICommandTimeOut)
        {
            if (_ICommandTimeOut >= 0)
            {
                this.CommandTimeOut = _ICommandTimeOut;
            }
        }
        #endregion  带执行超时时间的初始化

        #region 带连接字符串和超时时间初始化
        /// <summary>
        /// 带连接字符串和超时时间初始化
        /// </summary>
        /// <param name="_IConnString">数据库连接字符串</param>
        /// <param name="timeOut">命令执行超时时间</param>
        public OracleHelper(string _IConnString, int _ICommandTimeOut)
        {
            this.ConnectionString = _IConnString;
            if (_ICommandTimeOut > 0)
            {
                this.CommandTimeOut = _ICommandTimeOut;
            }
        }
        #endregion 带连接字符串和超时时间初始化

        #endregion 初始化

        #region 执行存储过程，获取受影响的行数

        #region 执行不带参数的存储过程，获取受影响的行数
        /// <summary>
        /// 执行不带参数的存储过程，获取受影响的行数
        /// </summary>
        /// <param name="_IStoredProcName">存储过程名称</param>
        public int RunProcedureNonQuery(string _IStoredProcName)
        {
            DbParameter[] parms = new DbParameter[] { };
            return RunProcedureNonQuery(_IStoredProcName, parms);
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
            using (OracleConnection SqlConn = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, SqlConn, null, CommandType.StoredProcedure, StoredProcName, cmdParms);
                        reVal = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    catch (OracleException ex)
                    {
                        string strParms = string.Empty;
                        foreach (DbParameter parm in cmdParms)
                        {
                            strParms += parm.ParameterName + ":" + parm.Value + "\r\n";
                        }
                        ex.Data.Add("StoredProcedureName", StoredProcName);
                        ex.Data.Add("SqlParms", strParms);
                        throw ex;
                    }
                    finally
                    {
                        cmd.Dispose();
                        if (SqlConn.State != ConnectionState.Closed)
                        {
                            SqlConn.Close();
                        }
                    }
                }
            }
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
            using (OracleConnection SqlConn = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, SqlConn, null, CommandType.StoredProcedure, StoredProcName, cmdParms);
                        reVal = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    catch (OracleException ex)
                    {
                        string strParms = string.Empty;
                        foreach (string Key in cmdParms.Keys)
                        {
                            strParms += Key + ":" + cmdParms[Key] + "\r\n";
                        }
                        ex.Data.Add("StoredProcedureName", StoredProcName);
                        ex.Data.Add("SqlParms", strParms);
                        throw ex;
                    }
                    finally
                    {
                        cmd.Dispose();
                        if (SqlConn.State != ConnectionState.Closed)
                        {
                            SqlConn.Close();
                        }
                    }
                }
            }
            return reVal;
        }
        #endregion 执行带参数的存储过程，获取受影响的行数

        #endregion 执行存储过程，获取受影响的行数

        #region 执行SQL语句，获取受影响的行数

        #region 执行不带参数的SQL语句，获取受影响的行数
        /// <summary>
        /// 执行不带参数的SQL语句，获取受影响的行数
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
            using (OracleConnection SqlConn = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, SqlConn, null, CommandType.Text, sqlString, cmdParms);
                        reVal = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    catch (OracleException ex)
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
                        if (SqlConn.State != ConnectionState.Closed)
                        {
                            SqlConn.Close();
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
            using (OracleConnection SqlConn = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, SqlConn, null, CommandType.Text, sqlString, cmdParms);
                        reVal = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    catch (OracleException ex)
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
                        if (SqlConn.State != ConnectionState.Closed)
                        {
                            SqlConn.Close();
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
        public object RunProcedureScalar(string _IStoredProcName, bool _IsRealData = false)
        {
            DbParameter[] cmdParms = new DbParameter[] { };
            return RunProcedureScalar(_IStoredProcName, cmdParms, _IsRealData);
        }
        #endregion 执行不带参数的存储过程，获取首行首列

        #region 执行带参数的存储过程，获取首行首列
        /// <summary>
        /// 执行带参数的存储过程，获取首行首列
        /// </summary>
        public object RunProcedureScalar(string _IStoredProcName, DbParameter[] _IParms, bool _IsRealData = false)
        {
            object reVal = null;
            using (OracleConnection SqlConn = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        string _HashKey = ConnectionString + "-RunProcedureScalar-" + _IStoredProcName;
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
                            PrepareCommand(cmd, SqlConn, null, CommandType.StoredProcedure, _IStoredProcName, _IParms);
                            object obj = cmd.ExecuteScalar();
                            cmd.Parameters.Clear();
                            if (!((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value))))
                            {
                                reVal = obj;
                                OwnCacheManage.SaveCache<object>(_CacheOBJ, _CacheOBJTime, _HashKey, reVal);
                            }
                        }
                    }
                    catch (OracleException ex)
                    {
                        string strParms = string.Empty;
                        foreach (DbParameter parm in _IParms)
                        {
                            strParms += parm.ParameterName + ":" + parm.Value + "\r\n";
                        }
                        ex.Data.Add("StoredProcedureName", _IStoredProcName);
                        ex.Data.Add("SqlParms", _IParms);
                        throw ex;
                    }
                    finally
                    {
                        cmd.Dispose();
                        if (SqlConn.State != ConnectionState.Closed)
                        {
                            SqlConn.Close();
                        }
                    }
                }
            }

            return reVal;
        }

        /// <summary>
        /// 执行带参数的存储过程，获取首行首列
        /// </summary>
        public object RunProcedureScalar(string _IStoredProcName, Dictionary<string, string> _IParms, bool _IsRealData = false)
        {
            object reVal = null;
            using (OracleConnection SqlConn = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        string _HashKey = ConnectionString + "-RunProcedureScalar-" + _IStoredProcName;
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
                            PrepareCommand(cmd, SqlConn, null, CommandType.StoredProcedure, _IStoredProcName, _IParms);
                            object obj = cmd.ExecuteScalar();
                            cmd.Parameters.Clear();
                            if (!((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value))))
                            {
                                reVal = obj;
                                OwnCacheManage.SaveCache<object>(_CacheOBJ, _CacheOBJTime, _HashKey, reVal);
                            }
                        }
                    }
                    catch (OracleException ex)
                    {
                        string strParms = string.Empty;
                        foreach (string Key in _IParms.Keys)
                        {
                            strParms += Key + ":" + _IParms[Key] + "\r\n";
                        }
                        ex.Data.Add("StoredProcedureName", _IStoredProcName);
                        ex.Data.Add("SqlParms", strParms);
                        throw ex;
                    }
                    finally
                    {
                        cmd.Dispose();
                        if (SqlConn.State != ConnectionState.Closed)
                        {
                            SqlConn.Close();
                        }
                    }
                }
            }

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
            DbParameter[] cmdParms = new DbParameter[] { };
            return ExecuteScalar(_ISQL, cmdParms, _IsRealData);
        }
        #endregion 执行带参数的SQL语句，获取首行首列

        #region 执行带参数的SQL语句，获取首行首列
        /// <summary>
        /// 执行带参数的SQL语句，获取首行首列
        /// </summary>
        public object ExecuteScalar(string _ISQL, DbParameter[] _IParms, bool _IsRealData = false)
        {
            object reVal = null;
            using (OracleConnection SqlConn = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
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
                            PrepareCommand(cmd, SqlConn, null, CommandType.Text, _ISQL, _IParms);
                            object obj = cmd.ExecuteScalar();
                            cmd.Parameters.Clear();
                            if (!((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value))))
                            {
                                reVal = obj;
                                OwnCacheManage.SaveCache<object>(_CacheOBJ, _CacheOBJTime, _HashKey, reVal);
                            }
                        }
                    }
                    catch (OracleException ex)
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
                        if (SqlConn.State != ConnectionState.Closed)
                        {
                            SqlConn.Close();
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
            using (OracleConnection SqlConn = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
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
                            PrepareCommand(cmd, SqlConn, null, CommandType.Text, _ISQL, _IParms);
                            object obj = cmd.ExecuteScalar();
                            cmd.Parameters.Clear();
                            if (!((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value))))
                            {
                                reVal = obj;
                                OwnCacheManage.SaveCache<object>(_CacheOBJ, _CacheOBJTime, _HashKey, reVal);
                            }
                        }
                    }
                    catch (OracleException ex)
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
                        if (SqlConn.State != ConnectionState.Closed)
                        {
                            SqlConn.Close();
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
        public DataSet RunProcedureDataSet(string _IStoredProcName, bool _IsRealData = false)
        {
            DbParameter[] cmdParms = new DbParameter[] { };
            return RunProcedureDataSet(_IStoredProcName, cmdParms, _IsRealData);
        }
        #endregion 执行部带参数的存储过程，获取DataSet对象

        #region 执行带参数的存储过程，获取DataSet对象
        /// <summary>
        /// 执行带参数的存储过程，获取DataSet对象
        /// </summary>
        public DataSet RunProcedureDataSet(string _IStoredProcName, DbParameter[] _IParms, bool _IsRealData = false)
        {
            DataSet reVal = new DataSet();
            string _HashKey = ConnectionString + "-RunProcedureDataSet-" + _IStoredProcName;
            foreach (DbParameter _FParm in _IParms)
            {
                _HashKey += "-" + _FParm.ParameterName + "-" + _FParm.Value;
            }
            OwnCacheManage.ClearCache<DataSet>(_CacheDataSet, _CacheDataSetTime, CacheConfig.DBSelectTimeOut, _HashKey);
            if (!_IsRealData && _CacheDataSet.ContainsKey(_HashKey))
            {
                reVal = _CacheDataSet[_HashKey];
                OwnCacheManage.SaveCache<DataSet>(_CacheDataSet, _CacheDataSetTime, _HashKey, reVal);
            }
            else
            {
                using (OracleConnection SqlConn = new OracleConnection(ConnectionString))
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, SqlConn, null, CommandType.StoredProcedure, _IStoredProcName, _IParms);
                            OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                            adapter.Fill(reVal);
                            cmd.Parameters.Clear();
                        }
                        catch (OracleException ex)
                        {
                            string strParms = string.Empty;
                            foreach (DbParameter parm in _IParms)
                            {
                                strParms += parm.ParameterName + ":" + parm.Value + "\r\n";
                            }
                            ex.Data.Add("StoredProcedureName", _IStoredProcName);
                            ex.Data.Add("SqlParms", _IParms);
                            throw ex;
                        }
                        finally
                        {
                            cmd.Dispose();
                            if (SqlConn.State != ConnectionState.Closed)
                            {
                                SqlConn.Close();
                            }
                        }
                    }
                }
            }

            return reVal;
        }

        public DataSet RunProcedureDataSet(string _IStoredProcName, Dictionary<string, string> _IParms, bool _IsRealData = false)
        {
            DataSet reVal = new DataSet();
            string _HashKey = ConnectionString + "-RunProcedureDataSet-" + _IStoredProcName;
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
                using (OracleConnection SqlConn = new OracleConnection(ConnectionString))
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        try
                        {

                            PrepareCommand(cmd, SqlConn, null, CommandType.StoredProcedure, _IStoredProcName, _IParms);
                            OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                            adapter.Fill(reVal);
                            cmd.Parameters.Clear();
                            OwnCacheManage.SaveCache<DataSet>(_CacheDataSet, _CacheDataSetTime, _HashKey, reVal);
                        }
                        catch (OracleException ex)
                        {
                            string strParms = string.Empty;
                            foreach (string Key in _IParms.Keys)
                            {
                                strParms += Key + ":" + _IParms[Key] + "\r\n";
                            }
                            ex.Data.Add("StoredProcedureName", _IStoredProcName);
                            ex.Data.Add("SqlParms", _IParms);
                            throw ex;
                        }
                        finally
                        {
                            cmd.Dispose();
                            if (SqlConn.State != ConnectionState.Closed)
                            {
                                SqlConn.Close();
                            }
                        }
                    }
                }
            }

            return reVal;
        }
        #endregion 执行带参数的存储过程，获取DataSet对象

        #endregion 执行存储过程，获取DataSet对象

        #region 执行SQL语句，获取DataSet对象

        #region 执行不带参数的SQL语句，获取DataSet对象
        /// <summary>
        /// 执行不带参数的SQL语句，获取DataSet对象
        /// </summary>
        /// <param name="_ISQL">执行的SQL语句</param>
        public DataSet ExecuteDataSet(string _ISQL, bool _IsRealData = false)
        {
            DbParameter[] cmdParms = new DbParameter[] { };
            return ExecuteDataSet(_ISQL, cmdParms, _IsRealData);
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
                using (OracleConnection SqlConn = new OracleConnection(ConnectionString))
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, SqlConn, null, CommandType.Text, _ISQL, _IParms);
                            OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                            adapter.Fill(reVal);
                            cmd.Parameters.Clear();
                            OwnCacheManage.SaveCache(_CacheDataSet, _CacheDataSetTime, _HashKey, reVal);
                        }
                        catch (OracleException ex)
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
                            if (SqlConn.State != ConnectionState.Closed)
                            {
                                SqlConn.Close();
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
                using (OracleConnection SqlConn = new OracleConnection(ConnectionString))
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, SqlConn, null, CommandType.Text, _ISQL, _IParms);
                            OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                            adapter.Fill(reVal);
                            cmd.Parameters.Clear();
                            OwnCacheManage.SaveCache(_CacheDataSet, _CacheDataSetTime, _HashKey, reVal);
                        }
                        catch (OracleException ex)
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
                            if (SqlConn.State != ConnectionState.Closed)
                            {
                                SqlConn.Close();
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
        public DataTable RunProcedureDataTable(string _IStoredProcName, bool _IsRealData = false)
        {
            DbParameter[] parms = new DbParameter[] { };
            return RunProcedureDataTable(_IStoredProcName, parms, _IsRealData);
        }
        #endregion 执行不带参数的存储过程，获取DataTable对象

        #region 执行带参数的存储过程，获取DataTable对象
        /// <summary>
        /// 执行带参数的存储过程，获取DataTable对象
        /// </summary>
        public DataTable RunProcedureDataTable(string _IStoredProcName, DbParameter[] _IParms, bool _IsRealData = false)
        {
            DataTable reVal = new DataTable();
            string _HashKey = ConnectionString + "-RunProcedureDataTable-" + _IStoredProcName;
            foreach (DbParameter _FParm in _IParms)
            {
                _HashKey += "-" + _FParm.ParameterName + "-" + _FParm.Value;
            }
            OwnCacheManage.ClearCache<DataTable>(_CacheDataTable, _CacheDataTableDateTime, CacheConfig.DBSelectTimeOut, _HashKey);
            if (!_IsRealData && _CacheDataTable.ContainsKey(_HashKey))
            {
                reVal = _CacheDataTable[_HashKey];
            }
            else
            {
                using (OracleConnection SqlConn = new OracleConnection(ConnectionString))
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, SqlConn, null, CommandType.StoredProcedure, _IStoredProcName, _IParms);
                            OracleDataReader _reader = cmd.ExecuteReader();
                            reVal.Load(_reader);
                            _reader.Close();
                            _reader.Dispose();
                            cmd.Parameters.Clear();
                        }
                        catch (OracleException ex)
                        {
                            string strParms = string.Empty;
                            foreach (DbParameter parm in _IParms)
                            {
                                strParms += parm.ParameterName + ":" + parm.Value + "\r\n";
                            }
                            ex.Data.Add("StoredProcedureName", _IStoredProcName);
                            ex.Data.Add("SqlParms", _IParms);
                            throw ex;
                        }
                        finally
                        {
                            cmd.Dispose();
                            if (SqlConn.State != ConnectionState.Closed)
                            {
                                SqlConn.Close();
                            }
                        }
                    }
                }
            }

            return reVal;
        }

        /// <summary>
        /// 执行带参数的存储过程，获取DataTable对象
        /// </summary>
        public DataTable RunProcedureDataTable(string _IStoredProcName, Dictionary<string, string> _IParms, bool _IsRealData = false)
        {
            DataTable reVal = new DataTable();
            string _HashKey = ConnectionString + "-RunProcedureDataTable-" + _IStoredProcName;
            foreach (string _FParm in _IParms.Keys)
            {
                _HashKey += "-" + _FParm + "-" + _IParms[_FParm];
            }
            OwnCacheManage.ClearCache<DataTable>(_CacheDataTable, _CacheDataTableDateTime, CacheConfig.DBSelectTimeOut, _HashKey);
            if (!_IsRealData && _CacheDataTable.ContainsKey(_HashKey))
            {
                reVal = _CacheDataTable[_HashKey];
            }
            else
            {
                using (OracleConnection SqlConn = new OracleConnection(ConnectionString))
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, SqlConn, null, CommandType.StoredProcedure, _IStoredProcName, _IParms);
                            OracleDataReader _reader = cmd.ExecuteReader();
                            reVal.Load(_reader);
                            _reader.Close();
                            _reader.Dispose();
                            cmd.Parameters.Clear();
                            OwnCacheManage.SaveCache<DataTable>(_CacheDataTable, _CacheDataTableDateTime, _HashKey, reVal);
                        }
                        catch (OracleException ex)
                        {
                            string strParms = string.Empty;
                            foreach (string Key in _IParms.Keys)
                            {
                                strParms += Key + ":" + _IParms[Key] + "\r\n";
                            }
                            ex.Data.Add("StoredProcedureName", _IStoredProcName);
                            ex.Data.Add("SqlParms", strParms);
                            throw ex;
                        }
                        finally
                        {
                            cmd.Dispose();
                            if (SqlConn.State != ConnectionState.Closed)
                            {
                                SqlConn.Close();
                            }
                        }
                    }
                }
            }

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
            return ExecuteDataTable(_ISQL, parms, _IsRealData);
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
                using (OracleConnection SqlConn = new OracleConnection(ConnectionString))
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, SqlConn, null, CommandType.Text, _ISQL, _IParms);
                            OracleDataReader _reader = cmd.ExecuteReader();
                            reVal.Load(_reader);
                            _reader.Close();
                            _reader.Dispose();
                            cmd.Parameters.Clear();
                            OwnCacheManage.SaveCache<DataTable>(_CacheDataTable, _CacheDataTableDateTime, _HashKey, reVal);
                        }
                        catch (OracleException ex)
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
                            if (SqlConn.State != ConnectionState.Closed)
                            {
                                SqlConn.Close();
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
            if (!_IsRealData && _CacheDataTable.ContainsKey(_HashKey))
            {
                reVal = _CacheDataTable[_HashKey];
            }
            else
            {
                using (OracleConnection SqlConn = new OracleConnection(ConnectionString))
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, SqlConn, null, CommandType.Text, _ISQL, _IParms);
                            OracleDataReader _reader = cmd.ExecuteReader();
                            reVal.Load(_reader);
                            _reader.Close();
                            _reader.Dispose();
                            cmd.Parameters.Clear();
                            OwnCacheManage.SaveCache<DataTable>(_CacheDataTable, _CacheDataTableDateTime, _HashKey, reVal);
                        }
                        catch (OracleException ex)
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
                            if (SqlConn.State != ConnectionState.Closed)
                            {
                                SqlConn.Close();
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
        /// <param name="cmd">SqlCommand  命令</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">数据库事物处理</param>
        /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句))</param>
        /// <param name="cmdText">Command text，T-SQL语句</param>
        /// <param name="cmdParms">参数列表</param>
        private void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
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
            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (OracleParameter parameter in cmdParms)
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
        /// <param name="cmd">SqlCommand  命令</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">数据库事物处理</param>
        /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句))</param>
        /// <param name="cmdText">Command text，T-SQL语句</param>
        /// <param name="cmdParms">参数列表</param>
        private void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, Dictionary<string, string> cmdParms)
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
            cmd.CommandType = cmdType;
            OracleParameter SqlParm = new OracleParameter();
            if (cmdParms != null)
            {
                foreach (KeyValuePair<string, string> parm in cmdParms)
                {
                    SqlParm = new OracleParameter(parm.Key, parm.Value);
                    cmd.Parameters.Add(SqlParm);
                }
            }
        }

        #endregion 为执行命令准备参数
    }
    #endregion Oracle辅助类
}
