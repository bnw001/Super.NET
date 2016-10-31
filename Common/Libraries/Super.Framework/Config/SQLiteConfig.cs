using System;
using System.IO;
using System.Web;

namespace Super.Framework
{
    #region SQLite数据库配置档
    /// <summary>
    /// SQLite数据库配置档
    /// </summary>
    public class SQLiteConfig
    {
        #region 类型名称
        /// <summary>
        /// 类型名称
        /// </summary>
        private const string TypeName = "SQLite";
        #endregion 类型名称

        #region 文件路径
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get
            {
                string reVal = ConfigHelper.ReadData(TypeName, "FilePath");
                return reVal;
            }
            set
            {
                ConfigHelper.WriteData(TypeName, "FilePath", value);
            }
        }
        #endregion 文件路径

        #region 数据库版本
        /// <summary>
        /// 数据库版本
        /// </summary>
        public string Version
        {
            get
            {
                string reVal = ConfigHelper.ReadData(TypeName, "Version");
                return reVal;
            }
            set
            {
                ConfigHelper.WriteData(TypeName, "Version", value);
            }
        }
        #endregion 数据库版本

        #region 密码
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get
            {
                string reVal = Secret.DecryptDES(ConfigHelper.ReadData(TypeName, "Password"));
                return reVal;
            }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    ConfigHelper.WriteData(TypeName, "Password", Secret.EncryptDES(value));
                }
            }
        }
        #endregion 密码

        #region 连接字符串
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnString
        {
            get
            {
                string _Password = Password.IsNullOrEmpty() ? "" : "Password=" + Password + ";";
                string _Version = Version.IsNullOrEmpty() ? "" : "Version=" + Version + ";";
                string _DataSource = HttpContext.Current.Server.MapPath(FilePath);
                string reVal = "Data Source=@DataSource;@Version@Password"
                               .Replace("@DataSource", _DataSource)
                               .Replace("@Version", _Version)
                               .Replace("@Password", _Password);
                return reVal;
            }
        }
        #endregion 连接字符串

        #region 只读数据库连接字符串
        /// <summary>
        /// 只读数据库连接字符串
        /// </summary>
        public string ReadDBConnString
        {
            get
            {
                string _Password = Password.IsNullOrEmpty() ? "" : "Password=" + Password + ";";
                string _Version = Version.IsNullOrEmpty() ? "" : "Version=" + Version + ";";
                string _DataSource = HttpContext.Current.Server.MapPath(FilePath.Replace(".", "Read."));
                if (!File.Exists(_DataSource))
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath(FilePath.Replace(".", "Write."))))
                    {
                        File.Copy(HttpContext.Current.Server.MapPath(FilePath.Replace(".", "Write.")), HttpContext.Current.Server.MapPath(FilePath.Replace(".", "Read.")));
                    }
                    else if (File.Exists(HttpContext.Current.Server.MapPath(FilePath)))
                    {
                        File.Copy(HttpContext.Current.Server.MapPath(FilePath), HttpContext.Current.Server.MapPath(FilePath.Replace(".", "Read.")));
                    }
                }
                string reVal = "Data Source=@DataSource;@Version@Password"
                               .Replace("@DataSource", _DataSource)
                               .Replace("@Version", _Version)
                               .Replace("@Password", _Password);
                return reVal;
            }
        }
        #endregion 只读数据库连接字符串

        #region 只写数据库连接字符串
        /// <summary>
        /// 只写数据库连接字符串
        /// </summary>
        public string WriteDBConnString
        {
            get
            {
                string _Password = Password.IsNullOrEmpty() ? "" : "Password=" + Password + ";";
                string _Version = Version.IsNullOrEmpty() ? "" : "Version=" + Version + ";";
                string _DataSource = HttpContext.Current.Server.MapPath(FilePath.Replace(".", "Write."));
                if (!File.Exists(_DataSource))
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath(FilePath.Replace(".", "Read."))))
                    {
                        File.Copy(HttpContext.Current.Server.MapPath(FilePath.Replace(".", "Read.")), HttpContext.Current.Server.MapPath(FilePath.Replace(".", "Write.")));
                    }
                    else if (File.Exists(HttpContext.Current.Server.MapPath(FilePath)))
                    {
                        File.Copy(HttpContext.Current.Server.MapPath(FilePath), HttpContext.Current.Server.MapPath(FilePath.Replace(".", "Write.")));
                    }
                }
                string reVal = "Data Source=@DataSource;@Version@Password"
                               .Replace("@DataSource", _DataSource)
                               .Replace("@Version", _Version)
                               .Replace("@Password", _Password);
                return reVal;
            }
        }
        #endregion 只写数据库连接字符串
    }
    #endregion SQLite数据库配置档
}