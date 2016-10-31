namespace Super.Framework
{
    #region 策略管理
    /// <summary>
    /// 策略管理
    /// </summary>
    public class StrategyManage
    {
        #region 类型名称
        /// <summary>
        /// 类型名称
        /// </summary>
        private const string TypeName = "Strategy";
        #endregion 类型名称

        #region 获取指定名称的使用策略信息
        /// <summary>
        /// 获取指定名称的使用策略信息
        /// </summary>
        /// <param name="Name">策略名称</param>
        /// <returns>返回值：获得指定名称策略的文件名和类名</returns>
        public static TargetStrategyInfo GetTargetStrategy(string InputName)
        {
            TargetStrategyInfo reVal = new TargetStrategyInfo();
            string[] StrategyInfo = ConfigHelper.ReadData(TypeName, InputName).SplitString(",");
            if (StrategyInfo.Length == 2)
            {
                reVal.StrategyFile = StrategyInfo[0];
                reVal.ClassName = StrategyInfo[1];
            }

            return reVal;
        }
        #endregion 获取指定名称的使用策略信息

        #region 设置指定名称的策略使用的策略集
        /// <summary>
        /// 设置指定名称的策略使用的策略集
        /// </summary>
        /// <param name="Name">策略名称</param>
        /// <param name="Obj">策略集对象</param>
        /// <returns>返回值：设置是否成功</returns>
        public static bool SetTargetStrategy(string InputName, TargetStrategyInfo InputObj)
        {
            bool reVal = false;
            string StrategyInfo = InputObj.StrategyFile + "," + InputObj.ClassName;
            ConfigHelper.WriteData(TypeName, InputName, StrategyInfo);
            return reVal;
        }
        #endregion 设置指定名称的策略使用的策略集
    }
    #endregion 策略管理

    #region 目标策略信息类
    /// <summary>
    /// 目标策略信息类
    /// </summary>
    public class TargetStrategyInfo
    {
        #region 目标策略文件名
        /// <summary>
        /// 目标策略文件名
        /// </summary>
        public string StrategyFile { get; set; }
        #endregion 目标策略文件名

        #region 目标策略类名
        /// <summary>
        /// 目标策略类名
        /// </summary>
        public string ClassName { get; set; }
        #endregion 目标策略类名
    }
    #endregion 目标策略信息类
}