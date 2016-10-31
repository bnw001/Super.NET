using System;

namespace Super.Framework
{
    #region 日志信息
    /// <summary>
    /// 日志信息
    /// </summary>
    public class LogMessage
    {
        #region ID
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        #endregion ID

        #region 日志类型
        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType Type { get; set; }
        #endregion 日志类型

        #region 信息
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
        #endregion 信息

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        public LogMessage()
        {
            this.ID = DateTime.Now.ToString("yyMMddHHmmssfff") + (Math.Abs(BaseTool.NewID.GetHashCode().ToString().ToLong() * BaseTool.NewID.GetHashCode().ToString().ToLong()) % 1000000000000000000).ToString().Supplement("0", 18);
            this.Type = LogType.Info;
            this.Message = "";
        }
        #endregion 初始化
    }
    #endregion 日志信息
}