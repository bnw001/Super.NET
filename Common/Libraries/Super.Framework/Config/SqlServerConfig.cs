namespace Super.Framework
{
    #region Sql Server数据库配置档
    /// <summary>
    /// Sql Server数据库配置档
    /// </summary>
    public class SqlServerConfig
    {
        #region 类型名称
        /// <summary>
        /// 类型名称
        /// </summary>
        private const string TypeName = "SqlServer";
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

        #region 端口
        /// <summary>
        /// 端口
        /// </summary>
        public int Port
        {
            get
            {
                int reVal = 1433;
                if (ConfigHelper.ReadData(TypeName, "Port").IsNotNullOrEmpty())
                {
                    reVal = ConfigHelper.ReadData(TypeName, "Port").ToInt();
                    if (reVal <= 0 || reVal >= 65535)
                    {
                        reVal = 1433;
                    }
                }
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
        #endregion 端口

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
                if (value.IsNotNullOrEmpty())
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
        public static string CreateConnectionString(string _IHost, string _IDataBase, string _IUserID, string _IPassword, int _IPort = 1433)
        {
            string _Port = (_IPort == 1433 ? "" : "," + _IPort.ToString());
            string reVal = "Data Source=@Host@Port;DataBase=@DataBase;User ID=@UserID;Password=@Password;Pooling=true;Connection LifeTime=0;Min Pool Size = 1;Max Pool Size=40000;"
                            .Replace("@Host", _IHost.Trim())
                            .Replace("@Port", _Port)
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
                string _Host = ConfigHelper.ReadData("SqlServerReadDB", "Host");
                int _Port = ConfigHelper.ReadData("SqlServerReadDB", "Port").ToInt();
                string _UserID = ConfigHelper.ReadData("SqlServerReadDB","UserName");
                string _Password = ConfigHelper.ReadData("SqlServerReadDB", "Password");
                string _DataBase = ConfigHelper.ReadData("SqlServerReadDB", "DBName");
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
                string _Host = ConfigHelper.ReadData("SqlServerWriteDB", "Host");
                int _Port = ConfigHelper.ReadData("SqlServerWriteDB", "Port").ToInt();
                string _UserID = ConfigHelper.ReadData("SqlServerWriteDB", "UserName");
                string _Password = ConfigHelper.ReadData("SqlServerWriteDB", "Password");
                string _DataBase = ConfigHelper.ReadData("SqlServerWriteDB", "DBName");
                string reVal = CreateConnectionString(_Host, _DataBase, _UserID, _Password, _Port);

                return reVal;
            }
        }
        #endregion 只写数据库连接字符串
    }
    #endregion Sql Server数据库配置档
}