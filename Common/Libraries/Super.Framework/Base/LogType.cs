using System.ComponentModel;

namespace Super.Framework
{
    #region 日志类型
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        #region 日常记录
        /// <summary>
        /// 日常记录
        /// </summary>
        [Description("日常记录")]
        Info = 1,
        #endregion 日常记录

        #region 警告记录
        /// <summary>
        /// 警告记录
        /// </summary>
        [Description("警告记录")]
        Warning = 2,
        #endregion 警告记录

        #region 调试记录
        /// <summary>
        /// 调试记录
        /// </summary>
        [Description("调试记录")]
        Debug = 4,
        #endregion 调试记录

        #region 错误记录
        /// <summary>
        /// 错误记录
        /// </summary>
        [Description("错误记录")]
        Error = 8,
        #endregion 错误记录
    }
    #endregion 日志类型
}