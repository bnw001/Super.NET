using System;

namespace Super.Framework
{
    #region 异常类扩展
    /// <summary>
    /// 异常类扩展
    /// </summary>
    public static class ExceptionExtensions
    {
        #region 将异常输出到日志文件
        /// <summary>
        /// 将异常输出到日志文件
        /// </summary>
        public static Exception ToLog(this Exception ex)
        {
            foreach (string Key in ex.Data.Keys)
            {
                Log.Debug(Key + ":" + ex.Data[Key].ToString());
            }
            Log.Debug(ex.ToString());
            return ex;
        }
        #endregion 将异常输出到日志文件

        #region 继续抛出异常
        /// <summary>
        /// 继续抛出异常
        /// </summary>
        public static void Throw(this Exception ex)
        {
            throw ex;
        }
        #endregion 继续抛出异常
    }
    #endregion 异常类扩展
}