namespace Super.Framework
{
    #region 日志接口
    /// <summary>
    /// 日志接口
    /// </summary>
    interface ILog
    {
        #region 写入一般信息
        /// <summary>
        /// 写入一般信息
        /// </summary>
        void Info(string _IMessage);
        #endregion 写入一般信息

        #region 写入警告信息
        /// <summary>
        /// 写入警告信息
        /// </summary>
        void Warning(string _IMessage);
        #endregion 写入警告信息

        #region 写入错误信息
        /// <summary>
        /// 写入错误信息
        /// </summary>
        void Error(string _IMessage);
        #endregion 写入错误信息

        #region 写入调试信息
        /// <summary>
        /// 写入调试信息
        /// </summary>
        void Debug(string _IMessage);
        #endregion 写入调试信息
    }
    #endregion 日志接口
}