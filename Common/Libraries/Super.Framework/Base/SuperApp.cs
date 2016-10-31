using System;

namespace Super.Framework
{
    #region 超级App类
    /// <summary>
    /// 超级App类
    /// </summary>
    public static class SuperApp
    {
        #region 系统目录
        /// <summary>
        /// 系统目录
        /// </summary>
        public static string SystemDirectory
        {
            get
            {
                string reVal = string.Empty;
                reVal = Environment.SystemDirectory;
                return reVal;
            }
        }
        #endregion 系统目录

        #region 当前程序的工作目录
        /// <summary>
        /// 当前程序的工作目录
        /// </summary>
        public static string WorkDir
        {
            get
            {
                string reVal = string.Empty;
                reVal = Environment.CurrentDirectory;
                return reVal;
            }
        }
        #endregion 当前程序的工作目录

        #region 获取GUID值
        /// <summary>
        /// 获取GUID值
        /// </summary>
        public static string NewGUID
        {
            get
            {
                return SuperManager.NewGUID;
            }
        }
        #endregion 获取GUID值

        #region 文件全路径
        /// <summary>
        /// 文件全路径
        /// </summary>
        public static string FileFullPath(string _IFilePath)
        {
            return SuperManager.FileFullPath(_IFilePath);
        }
        #endregion 文件全路径
    }
    #endregion 超级App类
}