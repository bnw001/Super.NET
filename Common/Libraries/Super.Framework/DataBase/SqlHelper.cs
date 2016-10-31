using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Super.Framework
{
    #region SQL SERVER辅助类
    /// <summary>
    /// SQL SERVER辅助类
    /// </summary>
    public class SqlHelper : IDBHelper
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

        #region 缓存过期时间
        /// <summary>
        /// 缓存过期时间(单位秒)
        /// </summary>
        public int _CacheTimeOut = 300;
        #endregion 缓存过期时间

        #endregion 变量

        #region 静态缓存

        #region 参数缓存
        /// <summary>
        /// 参数缓存
        /// </summary>
        private static Dictionary<string, SqlParameter[]> _CacheProcSqlParameter = new Dictionary<string, SqlParameter[]>();

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
        public SqlHelper()
        {
            this.ConnectionString = Config.SqlServerConnectionString;
            this.CommandTimeOut = 600;
        }
        #endregion 无参初始化

        #region 带连接字符串初始化
        /// <summary>
        /// 带连接字符串初始化
        /// </summary>
        /// <param name="connString">数据库连接字符串</param>
        public SqlHelper(string connString)
        {
            this.ConnectionString = connString;
        }
        #endregion 带连接字符串初始化

        #region 带具体参数的初始化
        /// <summary>
        /// 带具体参数的初始化
        /// </summary>
        public SqlHelper(string _IHost, string _IDataBase, string _IUserID, string _IPass, int _IPort = 1433)
        {
            string _Port = (_IPort == 1433 ? "" : "," + _IPort.ToString());
            this.ConnectionString = "Server=@Host@Port;DataBase=@DataBase;User ID=@UserID;Password=@Password;"
                                .Replace("@Host", _IHost)
                                .Replace("@Port", _Port)
                                .Replace("@DataBase", _IDataBase)
                                .Replace("@UserID", _IUserID)
                                .Replace("@Password", _IPass);
        }
        #endregion 带具体参数的初始化

        #region 带执行超时时间的初始化
        /// <summary>
        /// 带执行超时时间的初始化
        /// </summary>
        /// <param name="commandTimeOut">命令执行超时时间</param>
        public SqlHelper(int commandTimeOut)
        {
            if (commandTimeOut >= 0)
            {
                this.CommandTimeOut = commandTimeOut;
            }
        }
        #endregion  带执行超时时间的初始化

        #region 带连接字符串和超时时间初始化
        /// <summary>
        /// 带连接字符串和超时时间初始化
        /// </summary>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="timeOut">命令执行超时时间</param>
        public SqlHelper(string connString, int commandTimeOut)
        {
            this.ConnectionString = connString;
            if (commandTimeOut > 0)
            {
                this.CommandTimeOut = commandTimeOut;
            }
        }
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
        public int RunProcedureNonQuery(string _IStoredProcName, params DbParameter[] _IParms)
        {
            int reVal = 0;
            using (SqlConnection SqlConn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, SqlConn, null, CommandType.StoredProcedure, _IStoredProcName, _IParms);
                        reVal = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    catch (SqlException ex)
                    {
                        string strParms = string.Empty;
                        foreach (DbParameter parm in _IParms)
                        {
                            strParms += parm.ParameterName + ":" + parm.Value + "\r\n";
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

        /// <summary>
        /// 执行带参数的存储过程，获取受影响的行数
        /// </summary>
        public int RunProcedureNonQuery(string _IStoredProcName, Dictionary<string, string> _IParms)
        {
            int reVal = 0;
            using (SqlConnection SqlConn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, SqlConn, null, CommandType.StoredProcedure, _IStoredProcName, _IParms);
                        reVal = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    catch (SqlException ex)
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
        #endregion 执行带参数的存储过程，获取受影响的行数

        #endregion 执行存储过程，获取受影响的行数

        #region 执行SQL语句，获取受影响的行数

        #region 执行不带参数的SQL语句，获取受影响的行数
        /// <summary>
        /// 执行不带参数的SQL语句，获取受影响的行数
        /// </summary>
        /// <param name="_ISQL">执行的SQL语句</param>
        public int ExecuteNonQuery(string _ISQL)
        {
            DbParameter[] parms = new DbParameter[] { };
            return ExecuteNonQuery(_ISQL, parms);
        }
        #endregion 执行不带参数的SQL语句，获取受影响的行数

        #region 执行带参数的SQL语句，获取受影响的行数
        /// <summary>
        /// 执行带参数的SQL语句，获取受影响的行数
        /// </summary>
        public int ExecuteNonQuery(string _SQL, params DbParameter[] _IParms)
        {
            int reVal = 0;
            using (SqlConnection SqlConn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, SqlConn, null, CommandType.Text, _SQL, _IParms);
                        reVal = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    catch (SqlException ex)
                    {
                        string strParms = string.Empty;
                        foreach (DbParameter parm in _IParms)
                        {
                            strParms += parm.ParameterName + ":" + parm.Value + "\r\n";
                        }
                        ex.Data.Add("Sql", _SQL);
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
        public int ExecuteNonQuery(string _ISQL, Dictionary<string, string> _IParms)
        {
            int reVal = 0;

            using (SqlConnection SqlConn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, SqlConn, null, CommandType.Text, _ISQL, _IParms);
                        reVal = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    catch (SqlException ex)
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
        /// <param name="_IStoredProcName">存储过程名称</param>
        /// <param name="_IParms">参数列表</param>
        public object RunProcedureScalar(string _IStoredProcName, DbParameter[] _IParms, bool _IsRealData = false)
        {
            object reVal = null;
            using (SqlConnection SqlConn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
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
                    catch (SqlException ex)
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
        /// <param name="_IStoredProcName">存储过程名称</param>
        /// <param name="_IParms">参数列表</param>
        public object RunProcedureScalar(string _IStoredProcName, Dictionary<string, string> _IParms, bool _IsRealData = false)
        {
            object reVal = null;
            using (SqlConnection SqlConn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
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
                    catch (SqlException ex)
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
        /// <param name="_ISQL">执行的SQL语句</param>
        /// <param name="_IParms">参数列表</param>
        public object ExecuteScalar(string _ISQL, DbParameter[] _IParms, bool _IsRealData = false)
        {
            object reVal = null;
            using (SqlConnection SqlConn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
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
                    catch (SqlException ex)
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
        /// <param name="_ISQL">执行的SQL语句</param>
        /// <param name="_IParms">参数列表</param>
        public object ExecuteScalar(string _ISQL, Dictionary<string, string> _IParms, bool _IsRealData = false)
        {
            object reVal = null;
            using (SqlConnection SqlConn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
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
                    catch (SqlException ex)
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
        /// <param name="_IStoredProcName">存储过程名称</param>
        /// <param name="_IParms">参数列表</param>
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
            }
            else
            {
                using (SqlConnection SqlConn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, SqlConn, null, CommandType.StoredProcedure, _IStoredProcName, _IParms);
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            adapter.Fill(reVal);
                            cmd.Parameters.Clear();
                            OwnCacheManage.SaveCache<DataSet>(_CacheDataSet, _CacheDataSetTime, _HashKey, reVal);
                        }
                        catch (SqlException ex)
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
                using (SqlConnection SqlConn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, SqlConn, null, CommandType.StoredProcedure, _IStoredProcName, _IParms);
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            adapter.Fill(reVal);
                            cmd.Parameters.Clear();
                            OwnCacheManage.SaveCache<DataSet>(_CacheDataSet, _CacheDataSetTime, _HashKey, reVal);
                        }
                        catch (SqlException ex)
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
                using (SqlConnection SqlConn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, SqlConn, null, CommandType.Text, _ISQL, _IParms);
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            adapter.Fill(reVal);
                            cmd.Parameters.Clear();
                            OwnCacheManage.SaveCache(_CacheDataSet, _CacheDataSetTime, _HashKey, reVal);
                        }
                        catch (SqlException ex)
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
                using (SqlConnection SqlConn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, SqlConn, null, CommandType.Text, _ISQL, _IParms);
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            adapter.Fill(reVal);
                            cmd.Parameters.Clear();
                            OwnCacheManage.SaveCache<DataSet>(_CacheDataSet, _CacheDataSetTime, _HashKey, reVal);
                        }
                        catch (SqlException ex)
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
            return RunProcedureDataTable(_IStoredProcName, parms);
        }
        #endregion 执行不带参数的存储过程，获取DataTable对象

        #region 执行带参数的存储过程，获取DataTable对象
        /// <summary>
        /// 执行带参数的存储过程，获取DataTable对象
        /// </summary>
        /// <param name="_IStoredProcName">存储过程名称</param>
        /// <param name="_IParms">参数列表</param>
        public DataTable RunProcedureDataTable(string _IStoredProcName, DbParameter[] _IParms, bool _IsRealData = false)
        {
            DataTable reVal = new DataTable();
            string _HashKey = ConnectionString + "-RunProcedureDataTable-" + _IStoredProcName;
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
                using (SqlConnection SqlConn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, SqlConn, null, CommandType.StoredProcedure, _IStoredProcName, _IParms);
                            SqlDataReader _reader = cmd.ExecuteReader();
                            reVal.Load(_reader);
                            _reader.Close();
                            _reader.Dispose();
                            cmd.Parameters.Clear();
                            OwnCacheManage.SaveCache<DataTable>(_CacheDataTable, _CacheDataTableDateTime, _HashKey, reVal);
                        }
                        catch (SqlException ex)
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
                using (SqlConnection SqlConn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, SqlConn, null, CommandType.StoredProcedure, _IStoredProcName, _IParms);
                            SqlDataReader _reader = cmd.ExecuteReader();
                            reVal.Load(_reader);
                            _reader.Close();
                            _reader.Dispose();
                            cmd.Parameters.Clear();
                            OwnCacheManage.SaveCache<DataTable>(_CacheDataTable, _CacheDataTableDateTime, _HashKey, reVal);
                        }
                        catch (SqlException ex)
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
        /// <param name="sqlString">执行的SQL语句</param>
        public DataTable ExecuteDataTable(string _ISQL, bool _IsRealData = false)
        {
            SqlParameter[] parms = new SqlParameter[] { };
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
            if (!_IsRealData && _CacheDataTable.ContainsKey(_HashKey))
            {
                reVal = _CacheDataTable[_HashKey];
            }
            else
            {
                using (SqlConnection SqlConn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, SqlConn, null, CommandType.Text, _ISQL, _IParms);
                            SqlDataReader _reader = cmd.ExecuteReader();
                            reVal.Load(_reader);
                            _reader.Close();
                            _reader.Dispose();
                            cmd.Parameters.Clear();
                            OwnCacheManage.SaveCache<DataTable>(_CacheDataTable, _CacheDataTableDateTime, _HashKey, reVal);
                        }
                        catch (SqlException ex)
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
                using (SqlConnection SqlConn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, SqlConn, null, CommandType.Text, _ISQL, _IParms);
                            SqlDataReader _reader = cmd.ExecuteReader();
                            reVal.Load(_reader);
                            _reader.Close();
                            _reader.Dispose();
                            cmd.Parameters.Clear();
                            OwnCacheManage.SaveCache<DataTable>(_CacheDataTable, _CacheDataTableDateTime, _HashKey, reVal);
                        }
                        catch (SqlException ex)
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
                using (SqlConnection SqlConn = new SqlConnection(ConnectionString))
                {
                    SqlConn.Open();
                    SqlBulkCopy _BulkCopy = new SqlBulkCopy(SqlConn);
                    _BulkCopy.DestinationTableName = _IDTName;
                    _BulkCopy.BatchSize = _IDT.Rows.Count;
                    try
                    {
                        _BulkCopy.WriteToServer(_IDT);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (SqlConn.State != ConnectionState.Closed)
                        {
                            SqlConn.Close();
                        }
                    }
                }
            }
        }
        #endregion 批量新增

        #region 为执行命令准备参数
        /// <summary>
        /// 为执行命令附加参数列表
        /// </summary>
        private void PrepareCommand(SqlCommand _ICmd, SqlConnection _IConn, SqlTransaction _ITrans, CommandType _ICmdType, string _ICmdText, DbParameter[] _ICmdParms)
        {
            if (_IConn.State != ConnectionState.Open)
            {
                _IConn.Open();
            }
            _ICmd.Connection = _IConn;
            _ICmd.CommandText = _ICmdText;
            _ICmd.CommandTimeout = this.CommandTimeOut;
            if (_ITrans != null)
            {
                _ICmd.Transaction = _ITrans;
            }
            _ICmd.CommandType = _ICmdType;

            if (_ICmdParms != null)
            {
                foreach (SqlParameter parameter in _ICmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    _ICmd.Parameters.Add(parameter);
                }
            }
        }

        /// <summary>
        /// 为执行命令附加参数列表
        /// </summary>
        private void PrepareCommand(SqlCommand _ICmd, SqlConnection _IConn, SqlTransaction _ITrans, CommandType _ICmdType, string _ICmdText, Dictionary<string, string> _ICmdParms)
        {
            if (_IConn.State != ConnectionState.Open)
            {
                _IConn.Open();
            }
            _ICmd.Connection = _IConn;
            _ICmd.CommandText = _ICmdText;
            _ICmd.CommandTimeout = this.CommandTimeOut;
            if (_ITrans != null)
            {
                _ICmd.Transaction = _ITrans;
            }
            _ICmd.CommandType = _ICmdType;
            SqlParameter SqlParm = new SqlParameter();
            if (_ICmdParms != null)
            {
                foreach (KeyValuePair<string, string> parm in _ICmdParms)
                {
                    SqlParm = new SqlParameter(parm.Key, parm.Value);
                    _ICmd.Parameters.Add(SqlParm);
                }
            }
        }

        #endregion 为执行命令准备参数
    }
    #endregion SQL SERVER辅助类
}