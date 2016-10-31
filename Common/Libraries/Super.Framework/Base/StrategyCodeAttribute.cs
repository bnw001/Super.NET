using System;

namespace Super.Framework
{
    #region 策略识别码
    /// <summary>
    /// 策略识别码
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class StrategyCodeAttribute : Attribute
    {
        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="InputCode">策略识别码</param>
        public StrategyCodeAttribute(string InputCode)
        {
            Code = InputCode;
        }
        #endregion 初始化

        #region 识别码
        /// <summary>
        /// 识别码
        /// </summary>
        public string Code { get; set; }
        #endregion 识别码
    }
    #endregion 策略识别码

    #region 策略标题
    /// <summary>
    /// 策略标题
    /// </summary>
    public class StrategyTitleAttribute : Attribute
    {
        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="InputTitle">策略标题</param>
        public StrategyTitleAttribute(string InputTitle)
        {
            Title = InputTitle;
        }
        #endregion 初始化

        #region 策略标题
        /// <summary>
        /// 策略标题
        /// </summary>
        public string Title { get; set; }
        #endregion 策略标题
    }
    #endregion 策略标题

    #region 策略版本号
    /// <summary>
    /// 策略版本号
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class StrategyVersionAttribute : Attribute
    {
        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="InputVersion">版本号</param>
        public StrategyVersionAttribute(string InputVersion)
        {
            Version = InputVersion;
        }
        #endregion 初始化

        #region 版本号
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
        #endregion 版本号
    }
    #endregion 策略版本号
}