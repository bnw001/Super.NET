namespace Super.Framework
{
    #region My SQL数据库配置档
    /// <summary>
    /// My SQL数据库配置档
    /// </summary>
    public class MySqlConfig
    {
        #region 类型名称
        /// <summary>
        /// 类型名称
        /// </summary>
        private const string TypeName = "MySql";
        #endregion 类型名称

        #region 主机名
        /// <summary>
        /// 主机名
        /// </summary>
        public string Host
        {
            get
            {
                string reVal = ConfigHelper.ReadData(TypeName, "Host");
                return reVal;
            }
            set
            {
                ConfigHelper.WriteData(TypeName, "Host", value);
            }
        }
        #endregion 主机名

        #region 端口(预设端口3306)
        /// <summary>
        /// 端口(预设端口3306)
        /// </summary>
        public int Port
        {
            get
            {
                int reVal = ConfigHelper.ReadData(TypeName, "Port").ToInt();
                return reVal;
            }
            set
            {
                if (value > 0 && value < 65535)
                {
                    ConfigHelper.WriteData(TypeName, "Port", value.ToString());
                }
            }
        }
        #endregion 端口(预设端口3306)

        #region 账号
        /// <summary>
        /// 账号
        /// </summary>
        public string UserID
        {
            get
            {
                string reVal = ConfigHelper.ReadData(TypeName, "UserName");
                return reVal;
            }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    ConfigHelper.WriteData(TypeName, "UserName", value);
                }
            }
        }
        #endregion 账号

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

        #region 数据库名称
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DataBase
        {
            get
            {
                string reVal = ConfigHelper.ReadData(TypeName, "DBName");
                return reVal;
            }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    ConfigHelper.WriteData(TypeName, "DBName", value);
                }
            }
        }
        #endregion 数据库名称

        #region 创建连接字符串
        /// <summary>
        /// 创建连接字符串
        /// </summary>
        public static string CreateConnectionString(string _IHost, string _IDataBase, string _IUserID, string _IPassword, int _IPort = 3306)
        {
            string reVal = "Data Source=@Host; User ID=@UserID; Password=@Password; Port=@Port; DataBase=@DataBase; Pooling=true; CharSet=utf8; Port=@Port;"
                            .Replace("@Host", _IHost.Trim())
                            .Replace("@Port", _IPort.ToString())
                            .Replace("@DataBase", _IDataBase.Trim())
                            .Replace("@UserID", _IUserID.Trim())
                            .Replace("@Password", _IPassword.Trim());
            return reVal;
        }
        #endregion 创建连接字符串

        #region 连接字符串
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnString
        {
            get
            {
                string reVal = CreateConnectionString(Host, DataBase, UserID, Password, Port);
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
                string _Host = ConfigHelper.ReadData("MySqlReadDB", "Host");
                int _Port = ConfigHelper.ReadData("MySqlReadDB", "Port").ToInt();
                string _UserID = ConfigHelper.ReadData("MySqlReadDB", "UserName");
                string _Password = ConfigHelper.ReadData("MySqlReadDB", "Password");
                string _DataBase = ConfigHelper.ReadData("MySqlReadDB", "DBName");
                string reVal = CreateConnectionString(_Host, _DataBase, _UserID, _Password, _Port);

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
                string _Host = ConfigHelper.ReadData("MySqlWriteDB", "Host");
                int _Port = ConfigHelper.ReadData("MySqlWriteDB", "Port").ToInt();
                string _UserID = ConfigHelper.ReadData("MySqlWriteDB", "UserName");
                string _Password = ConfigHelper.ReadData("MySqlWriteDB", "Password");
                string _DataBase = ConfigHelper.ReadData("MySqlWriteDB", "DBName");
                string reVal = CreateConnectionString(_Host, _DataBase, _UserID, _Password, _Port);

                return reVal;
            }
        }
        #endregion 只写数据库连接字符串
    }
    #endregion Sql Server数据库配置档
}