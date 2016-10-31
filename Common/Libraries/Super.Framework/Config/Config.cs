using System.Collections.Generic;

namespace Super.Framework
{
    #region 配置档
    /// <summary>
    /// 配置档
    /// </summary>
    public class Config
    {
        #region 是否启用读写分离
        /// <summary>
        /// 是否启用读写分离
        /// </summary>
        public static bool IsRWSeparate
        {
            get
            {
                SystemConfig SysConfiog = new SystemConfig();
                return SysConfiog.IsRWSeparate;
            }
            set
            {
                SystemConfig SysConfig = new SystemConfig();
                SysConfig.IsRWSeparate = value;
            }
        }
        #endregion 是否启用读写分离

        #region 日志输出类型列表
        /// <summary>
        /// 日志输出类型列表
        /// </summary>
        public static List<LogType> LogOutputLevel
        {
            get
            {
                SystemConfig SysConfiog = new SystemConfig();
                return SysConfiog.LogOutputLevel;
            }
            set
            {
                SystemConfig SysConfiog = new SystemConfig();
                SysConfiog.LogOutputLevel = value;
            }
        }
        #endregion 日志输出类型列表

        #region 设置日志输出类型
        /// <summary>
        /// 设置日志输出类型
        /// </summary>
        public static void SetTenantLogOutputLevel(List<LogType> _IOutputLevel)
        {
            SystemConfig SysConfiog = new SystemConfig();
            SysConfiog.SetLogOutputLevel(_IOutputLevel);
        }
        #endregion 设置日志输出类型

        #region 缓冲时间
        /// <summary>
        /// 缓冲时间
        /// </summary>
        public static int BufferTime
        {
            get
            {
                SystemConfig SysConfiog = new SystemConfig();
                return SysConfiog.BufferTime;
            }
            set
            {
                SystemConfig SysConfiog = new SystemConfig();
                SysConfiog.BufferTime = value;
            }
        }
        #endregion 缓冲时间

        #region 系统使用的语言
        /// <summary>
        /// 系统使用的语言
        /// </summary>
        public static string Language
        {
            get
            {
                SystemConfig _SysConfig = new SystemConfig();
                return _SysConfig.DBType;
            }
            set
            {
                SystemConfig _SysConfig = new SystemConfig();
                _SysConfig.Language = value;
            }
        }
        #endregion 系统使用的语言

        #region 系统名称
        /// <summary>
        /// 系统名称
        /// </summary>
        public static string SystemName
        {
            get
            {
                SystemConfig _SysConfig = new SystemConfig();
                return _SysConfig.SystemName;
            }
        }
        #endregion 系统名称

        #region 系统使用的数据库类型
        /// <summary>
        /// 系统使用的数据库类型
        /// </summary>
        public static string DBType
        {
            get
            {
                SystemConfig SysConfig = new SystemConfig();
                return SysConfig.DBType.ToStandardDbType();
            }
            set
            {
                SystemConfig SysConfig = new SystemConfig();
                SysConfig.DBType = value.ToStandardDbType();
            }
        }
        #endregion 系统使用的数据库类型

        #region SQLite数据库连接字符串
        /// <summary>
        /// SQLite数据库连接字符串
        /// </summary>
        public static string SQLiteConnectionString
        {
            get
            {
                SQLiteConfig DbConfig = new SQLiteConfig();
                return DbConfig.ConnString;
            }
        }
        #endregion SQLite数据库连接字符串

        #region SQLite只读数据库连接字符串
        /// <summary>
        /// SQLite只读数据库连接字符串
        /// </summary>
        public static string SQLiteReadConnectionString
        {
            get
            {
                string reVal = "";
                SQLiteConfig DbConfig = new SQLiteConfig();
                if (Config.IsRWSeparate)
                {
                    reVal = DbConfig.ReadDBConnString;
                }
                else
                {
                    reVal = DbConfig.ConnString;
                }

                return reVal;
            }
        }
        #endregion SQLite只读数据库连接字符串

        #region SQLite只写数据库连接字符串
        /// <summary>
        /// SQLite只写数据库连接字符串
        /// </summary>
        public static string SQLiteWriteConnectionString
        {
            get
            {
                string reVal = "";
                SQLiteConfig DbConfig = new SQLiteConfig();
                if (Config.IsRWSeparate)
                {
                    reVal = DbConfig.WriteDBConnString;
                }
                else
                {
                    reVal = DbConfig.ConnString;
                }

                return reVal;
            }
        }
        #endregion SQLite只写数据库连接字符串

        #region SqlServer数据库连接字符串
        /// <summary>
        /// SqlServer数据库连接字符串
        /// </summary>
        public static string SqlServerConnectionString
        {
            get
            {
                SqlServerConfig DbConfig = new SqlServerConfig();
                return DbConfig.ConnString;
            }
        }
        #endregion SqlServer数据库连接字符串

        #region SqlServer只读数据库连接字符串
        /// <summary>
        /// SqlServer只读数据库连接字符串
        /// </summary>
        public static string SqlServerReadDBConnectionString
        {
            get
            {
                SqlServerConfig DbConfig = new SqlServerConfig();
                return DbConfig.ReadDBConnString;
            }
        }
        #endregion SqlServer只读数据库连接字符串

        #region SqlServer只写数据库连接字符串
        /// <summary>
        /// SqlServer只写数据库连接字符串
        /// </summary>
        public static string SqlServerWriteDBConnectionString
        {
            get
            {
                SqlServerConfig DbConfig = new SqlServerConfig();
                return DbConfig.WriteDBConnString;
            }
        }
        #endregion SqlServer只写数据库连接字符串

        #region MySql数据库连接字符串
        /// <summary>
        /// MySql数据库连接字符串
        /// </summary>
        public static string MySqlConnectionString
        {
            get
            {
                MySqlConfig DbConfig = new MySqlConfig();
                return DbConfig.ConnString;
            }
        }
        #endregion MySQL数据库连接字符串

        #region MySQL只读数据库连接字符串
        /// <summary>
        /// MySQL只读数据库连接字符串
        /// </summary>
        public static string MySqlReadDBConnectionString
        {
            get
            {
                MySqlConfig DbConfig = new MySqlConfig();
                return DbConfig.ReadDBConnString;
            }
        }
        #endregion MySQL只读数据库连接字符串

        #region MySQL只写数据库连接字符串
        /// <summary>
        /// MySQL只写数据库连接字符串
        /// </summary>
        public static string MySqlWriteDBConnectionString
        {
            get
            {
                MySqlConfig DbConfig = new MySqlConfig();
                return DbConfig.WriteDBConnString;
            }
        }
        #endregion MySQL只写数据库连接字符串

        #region Oracle数据库连接字符串
        /// <summary>
        /// Oracle数据库连接字符串
        /// </summary>
        public static string OracleConnectionString
        {
            get
            {
                OracleConfig DbConfig = new OracleConfig();
                return DbConfig.ConnString;
            }
        }
        #endregion Oracle数据库连接字符串

        #region Oracle只读数据库连接字符串
        /// <summary>
        /// Oracle只读数据库连接字符串
        /// </summary>
        public static string OracleReadDBConnectionString
        {
            get
            {
                OracleConfig DbConfig = new OracleConfig();
                return DbConfig.ReadDBConnString;
            }
        }
        #endregion Oracle只读数据库连接字符串

        #region Oracle只写数据库连接字符串
        /// <summary>
        /// Oracle只写数据库连接字符串
        /// </summary>
        public static string OracleWriteDBConnectionString
        {
            get
            {
                OracleConfig DbConfig = new OracleConfig();
                return DbConfig.WriteDBConnString;
            }
        }
        #endregion Oracle只写数据库连接字符串

        #region 通过类型获取数据库连接字符串
        /// <summary>
        /// 通过类型获取数据库连接字符串
        /// </summary>
        public static string GetConnString(string _IDbType)
        {
            string reVal = string.Empty;
            if (_IDbType.IsSQLite())
            {
                reVal = SQLiteConnectionString;
            }
            else if (_IDbType.IsSqlServer())
            {
                reVal = SqlServerConnectionString;
            }
            else if (_IDbType.IsOracle())
            {
                reVal = OracleConnectionString;
            }
            else if (_IDbType.IsMySQL())
            {
                reVal = MySqlConnectionString;
            }
            return reVal;
        }
        #endregion 通过类型获取数据库连接字符串

        #region 通过类型获取只读数据库连接字符串
        /// <summary>
        /// 通过类型获取只读数据库连接字符串
        /// </summary>
        public static string GetReadConnString(string _IDbType)
        {
            string reVal = string.Empty;
            if (_IDbType.IsSQLite())
            {
                reVal = SQLiteReadConnectionString;
            }
            else if (_IDbType.IsSqlServer())
            {
                reVal = SqlServerReadDBConnectionString;
            }
            else if (_IDbType.IsOracle())
            {
                reVal = OracleReadDBConnectionString;
            }
            else if (_IDbType.IsMySQL())
            {
                reVal = MySqlReadDBConnectionString;
            }
            return reVal;
        }
        #endregion 通过类型获取只读数据库连接字符串

        #region 通过类型获取只写数据库连接字符串
        /// <summary>
        /// 通过类型获取只写数据库连接字符串
        /// </summary>
        public static string GetWriteConnString(string _IDbType)
        {
            string reVal = string.Empty;
            if (_IDbType.IsSQLite())
            {
                reVal = SQLiteReadConnectionString;
            }
            else if (_IDbType.IsSqlServer())
            {
                reVal = SqlServerReadDBConnectionString;
            }
            else if (_IDbType.IsOracle())
            {
                reVal = OracleReadDBConnectionString;
            }
            else if (_IDbType.IsMySQL())
            {
                reVal = MySqlReadDBConnectionString;
            }
            return reVal;
        }
        #endregion 通过类型获取只写数据库连接字符串

        #region 连接字符串
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnString
        {
            get
            {
                string reVal = GetConnString(DBType);
                return reVal;
            }
        }
        #endregion 连接字符串

        #region 只读数据库连接字符串
        /// <summary>
        /// 只读数据库连接字符串
        /// </summary>
        public static string ReadDBConnString
        {
            get
            {
                string reVal = GetReadConnString(DBType);
                return reVal;
            }
        }
        #endregion 只读数据库连接字符串

        #region 只写数据库连接字符串
        /// <summary>
        /// 只写数据库连接字符串
        /// </summary>
        public static string WriteDBConnString
        {
            get
            {
                string reVal = GetWriteConnString(DBType);
                return reVal;
            }
        }
        #endregion 只写数据库连接字符串
    }
    #endregion 配置档
}
