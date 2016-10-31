namespace Super.Framework
{
    #region 缓冲配置档
    /// <summary>
    /// 缓冲配置档
    /// </summary>
    public class CacheConfig
    {
        #region 类型
        /// <summary>
        /// 类型
        /// </summary>
        private static string _TypeName = "Cache";
        #endregion 类型

        #region 获取指定值
        /// <summary>
        /// 获取指定值
        /// </summary>
        private static string Get(string _IKey)
        {
            string reVal = string.Empty;
            ConfigHelper _Helper = new ConfigHelper();
            reVal = ConfigHelper.ReadData(_TypeName, _IKey);

            return reVal;
        }
        #endregion 获取指定值

        #region 查询缓冲时间
        /// <summary>
        /// 查询缓冲时间
        /// </summary>
        public static int DBSelectTimeOut
        {
            get
            {
                int reVal = 1;
                if (ConfigHelper.ReadData(_TypeName, "DBSelectTimeOut").ToInt() > 0)
                {
                    reVal = ConfigHelper.ReadData(_TypeName, "DBSelectTimeOut").ToInt();
                }
                return reVal;
            }
            set
            {
                if (value >= 0)
                {
                    ConfigHelper.WriteData(_TypeName, "DBSelectTimeOut", value.ToString());
                }
            }
        }
        #endregion 查询缓冲时间

        #region 存储过程参数缓存时间
        /// <summary>
        /// 存储过程参数缓存时间
        /// </summary>
        public static int ProcParmsTimeOut
        {
            get
            {
                int reVal = 300;
                if (ConfigHelper.ReadData(_TypeName, "ProcParmsTimeOut").ToInt() > 0)
                {
                    reVal = ConfigHelper.ReadData(_TypeName, "ProcParmsTimeOut").ToInt();
                }
                return reVal;
            }
            set
            {
                if (value >= 0)
                {
                    ConfigHelper.WriteData(_TypeName, "ProcParmsTimeOut", value.ToString());
                }
            }
        }
        #endregion 存储过程参数缓存时间
    }
    #endregion 缓冲配置档
}