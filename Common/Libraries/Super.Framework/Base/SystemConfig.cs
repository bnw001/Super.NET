using System.Collections.Generic;

namespace Super.Framework
{
    #region 系统配置档实体类
    /// <summary>
    /// 系统配置档实体类
    /// </summary>
    public class SystemConfig
    {
        #region 类型名称
        /// <summary>
        /// 类型名称
        /// </summary>
        private const string TypeName = "System";
        #endregion 类型名称

        #region 系统使用的语言
        /// <summary>
        /// 系统使用的语言
        /// </summary>
        public string Language
        {
            get
            {
                string reVal = ConfigHelper.ReadData(TypeName, "Language");
                return reVal;
            }
            set
            {
                ConfigHelper.WriteData(TypeName, "Language", value);
            }
        }
        #endregion 系统使用的语言

        #region 系统名称
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName
        {
            get
            {
                string reVal = string.Empty;
                reVal = LanguageService.GetValue(this.Language, "System", "SYSTEM_NAME");

                return reVal;
            }
        }
        #endregion 系统名称

        #region 日志输出级别
        /// <summary>
        /// 日志输出级别
        /// </summary>
        public List<LogType> LogOutputLevel
        {
            get
            {
                List<LogType> reVal = new List<LogType>();
                string _configS = ConfigHelper.ReadData(TypeName, "LogType").ToUpper();
                List<string> _configList = _configS.SplitString("|").ToList();
                if (_configList.Contains("NO"))
                {
                    return reVal;
                }
                else if (_configList.Contains("ALL"))
                {
                    reVal.Add(LogType.Info);
                    reVal.Add(LogType.Warning);
                    reVal.Add(LogType.Error);
                    reVal.Add(LogType.Debug);
                }
                if (_configList.Contains("INFO") && !reVal.Contains(LogType.Info))
                {
                    reVal.Add(LogType.Info);
                }
                if (_configList.Contains("WARNING") && !reVal.Contains(LogType.Warning))
                {
                    reVal.Add(LogType.Warning);
                }
                if (_configList.Contains("ERROR") && !reVal.Contains(LogType.Error))
                {
                    reVal.Add(LogType.Error);
                }
                if (_configList.Contains("DEBUG") && !reVal.Contains(LogType.Debug))
                {
                    reVal.Add(LogType.Debug);
                }
                return reVal;
            }
            set
            {
                List<LogType> _SetLogTypeList = new List<LogType>();
                string _SetConfigS = string.Empty;
                if (value.Count > 0)
                {
                    if (value.Contains(LogType.Info))
                    {
                        _SetLogTypeList.Add(LogType.Info);
                        _SetConfigS += "Info,";
                    }
                    if (value.Contains(LogType.Warning))
                    {
                        _SetLogTypeList.Add(LogType.Warning);
                        _SetConfigS += "Warning,";
                    }
                    if (value.Contains(LogType.Error))
                    {
                        _SetLogTypeList.Add(LogType.Error);
                        _SetConfigS += "Error,";
                    }
                    if (value.Contains(LogType.Debug))
                    {
                        _SetLogTypeList.Add(LogType.Debug);
                        _SetConfigS += "Debug,";
                    }
                }
                if (_SetLogTypeList.Count == 4)
                {
                    ConfigHelper.WriteData(TypeName, "LogDirectory", "All");
                }
                else if (_SetConfigS.IsNotNullOrEmpty())
                {
                    _SetConfigS = _SetConfigS.RemoveEndChar(",");
                    ConfigHelper.WriteData(TypeName, "LogDirectory", _SetConfigS);
                }
            }
        }
        #endregion 日志输出级别

        #region 日志输出级别
        /// <summary>
        /// 日志输出级别
        /// </summary>
        public List<LogType> GetLogOutputLevel()
        {
            List<LogType> reVal = new List<LogType>();
            string _configS = ConfigHelper.ReadData(TypeName, "LogType").ToUpper();
            List<string> _configList = _configS.SplitString("|").ToList();
            if (_configList.Contains("NO"))
            {
                return reVal;
            }
            else if (_configList.Contains("ALL"))
            {
                reVal.Add(LogType.Info);
                reVal.Add(LogType.Warning);
                reVal.Add(LogType.Error);
                reVal.Add(LogType.Debug);
            }
            if (_configList.Contains("INFO") && !reVal.Contains(LogType.Info))
            {
                reVal.Add(LogType.Info);
            }
            if (_configList.Contains("WARNING") && !reVal.Contains(LogType.Warning))
            {
                reVal.Add(LogType.Warning);
            }
            if (_configList.Contains("ERROR") && !reVal.Contains(LogType.Error))
            {
                reVal.Add(LogType.Error);
            }
            if (_configList.Contains("DEBUG") && !reVal.Contains(LogType.Debug))
            {
                reVal.Add(LogType.Debug);
            }
            return reVal;
        }
        #endregion 日志输出级别

        #region 设置日志输出级别
        /// <summary>
        /// 设置日志输出级别
        /// </summary>
        public void SetLogOutputLevel(List<LogType> logOutputLevel)
        {
            List<LogType> _SetLogTypeList = new List<LogType>();
            string _SetConfigS = string.Empty;
            if (logOutputLevel.Count > 0)
            {
                if (logOutputLevel.Contains(LogType.Info))
                {
                    _SetLogTypeList.Add(LogType.Info);
                    _SetConfigS += "Info,";
                }
                if (logOutputLevel.Contains(LogType.Warning))
                {
                    _SetLogTypeList.Add(LogType.Warning);
                    _SetConfigS += "Warning,";
                }
                if (logOutputLevel.Contains(LogType.Error))
                {
                    _SetLogTypeList.Add(LogType.Error);
                    _SetConfigS += "Error,";
                }
                if (logOutputLevel.Contains(LogType.Debug))
                {
                    _SetLogTypeList.Add(LogType.Debug);
                    _SetConfigS += "Debug,";
                }
            }
            if (_SetLogTypeList.Count == 4)
            {
                ConfigHelper.WriteData(TypeName, "LogDirectory", "All");
            }
            else if (_SetConfigS.IsNotNullOrEmpty())
            {
                _SetConfigS = _SetConfigS.RemoveEndChar(",");
                ConfigHelper.WriteData(TypeName, "LogDirectory", _SetConfigS);
            }
        }
        #endregion 设置日志输出级别

        #region 用户目录
        /// <summary>
        /// 用户目录
        /// </summary>
        public string UserDirectory
        {
            get
            {
                string reVal = SuperManager.FileFullPath(ConfigHelper.ReadData(TypeName, "UserDirectory"));

                return reVal;
            }
            set
            {
                ConfigHelper.WriteData(TypeName, "UserDirectory", value);
            }
        }
        #endregion 用户目录

        #region 是否启用读写分离
        /// <summary>
        /// 是否启用读写分离
        /// </summary>
        public bool IsRWSeparate
        {
            get
            {
                bool reVal = false;
                bool.TryParse(ConfigHelper.ReadData(TypeName, "IsRWSeparate"), out reVal);

                return reVal;
            }
            set
            {
                ConfigHelper.WriteData(TypeName, "IsRWSeparate", value.ToString());
            }
        }
        #endregion 是否启用读写分离

        #region 获取租户是否启用读写分离
        /// <summary>
        /// 获取租户是否启用读写分离
        /// </summary>
        public bool GetIsRWSeparate()
        {
            bool reVal = false;
            bool.TryParse(ConfigHelper.ReadData(TypeName, "IsRWSeparate"), out reVal);

            return reVal;
        }
        #endregion 获取租户是否启用读写分离

        #region 写入租户是否启用读写分离
        /// <summary>
        /// 写入租户是否启用读写分离
        /// </summary>
        public void SetIsRWSeparate(bool isRWSeparate)
        {
            ConfigHelper.WriteData(TypeName, "IsRWSeparate", isRWSeparate.ToString());
        }
        #endregion 写入租户是否启用读写分离

        #region 是否启用租户模式(Saas)
        /// <summary>
        /// 是否启用租户模式(Saas)
        /// </summary>
        public bool IsTenantModel
        {
            get
            {
                bool reVal = false;
                bool.TryParse(ConfigHelper.ReadData(TypeName, "IsTenantModel"), out reVal);

                return reVal;
            }
            set
            {
                ConfigHelper.WriteData(TypeName, "IsTenantModel", value.ToString());
            }
        }
        #endregion 是否启用租户模式(Saas)

        #region 数据库类型
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DBType
        {
            get
            {
                string reVal = string.Empty;
                string _DbType = ConfigHelper.ReadData(TypeName, "DBType");
                if (_DbType.IsDBType())
                {
                    reVal = _DbType;
                }
                return reVal;
            }
            set
            {
                if (value.IsDBType())
                {
                    ConfigHelper.WriteData(TypeName, "DBType", value.ToStandardDbType());
                }
            }
        }
        #endregion 数据库类型

        #region 设置租户的数据库类型
        /// <summary>
        /// 设置租户的数据库类型
        /// </summary>
        public void SetDbType(string _IDbType)
        {
            ConfigHelper.WriteData(TypeName, "DBType", _IDbType.ToString());
        }
        #endregion 设置租户的数据库类型

        #region 系统缓冲时间
        /// <summary>
        /// 系统缓冲时间
        /// </summary>
        public int BufferTime
        {
            get
            {
                int reVal = ConfigHelper.ReadData(TypeName, "BufferTime").ToInt();

                return reVal;
            }
            set
            {
                ConfigHelper.WriteData(TypeName, "BufferTime", value.ToString());
            }
        }
        #endregion 系统缓冲时间
    }
    #endregion 系统配置档实体类
}