using System.ComponentModel;

namespace Super.Framework
{
    #region 运行模式
    /// <summary>
    /// 运行模式
    /// </summary>
    public enum RunMode
    {
        #region 未知
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        Unknown = 0,
        #endregion 未知

        #region 网站
        /// <summary>
        /// 网站
        /// </summary>
        [Description("网站")]
        Web = 1,
        #endregion 网站

        #region Windows程序
        /// <summary>
        /// Windows程序
        /// </summary>
        [Description("应用程序")]
        App = 2
        #endregion Windows程序
    }
    #endregion 运行模式
}