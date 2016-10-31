using System.Collections.Generic;
using System.IO;

namespace Super.Framework
{
    #region 超级IO类
    /// <summary>
    /// 超级IO类
    /// </summary>
    public static class SuperIO
    {
        #region 获取文件夹大小
        /// <summary>
        /// 获取文件夹大小
        /// </summary>
        public static long GetDirectoryLength(string _IDirPath)
        {
            long reVal = 0;
            if (!Directory.Exists(_IDirPath))
                return 0;
            DirectoryInfo DI = new DirectoryInfo(_IDirPath);
            foreach (FileInfo FI in DI.GetFiles())
            {
                reVal += FI.Length;
            }
            DirectoryInfo[] DIS = DI.GetDirectories();
            if (DIS.Length > 0)
            {
                for (int i = 0; i < DIS.Length; i++)
                {
                    reVal += GetDirectoryLength(DIS[i].FullName);
                }
            }
            return reVal;
        }
        #endregion 获取文件夹大小

        #region 获取目录（不包含子目录）的子目录
        /// <summary>
        /// 获取目录（不包含子目录）的子目录
        /// </summary>
        public static List<DirectoryInfo> SubDirInfo(string _IPath)
        {
            List<DirectoryInfo> reVal = new List<DirectoryInfo>();
            DirectoryInfo _DI = new DirectoryInfo(_IPath);
            DirectoryInfo[] _DIA = _DI.GetDirectories();
            foreach (DirectoryInfo _FDI in _DIA)
            {
                reVal.Add(_FDI);
            }
            return reVal;
        }
        #endregion 获取目录（不包含子目录）的子目录

        #region 获取目录（不包含子目录）的子目录名称列表
        /// <summary>
        /// 获取目录（不包含子目录）的子目录名称列表
        /// </summary>
        public static List<string> SubDirName(string _IPath)
        {
            List<string> reVal = new List<string>();
            DirectoryInfo _DI = new DirectoryInfo(_IPath);
            DirectoryInfo[] _DIA = _DI.GetDirectories();
            foreach (DirectoryInfo _FDI in _DIA)
            {
                reVal.Add(_FDI.Name);
            }
            return reVal;
        }
        #endregion 获取目录（不包含子目录）的子目录名称列表

        #region 获取目录（不包含子目录）的文件列表
        /// <summary>
        /// 获取目录（不包含子目录）的文件列表
        /// </summary>
        public static List<FileInfo> SubFileInfo(string _IPath, string _ISuffix)
        {
            List<FileInfo> reVal = new List<FileInfo>();
            _ISuffix = "." + _ISuffix.Trim().ToLower().RemoveStartChar(".");
            DirectoryInfo _DI = new DirectoryInfo(_IPath);
            FileInfo[] _FIA = _DI.GetFiles();
            foreach (FileInfo _FI in _FIA)
            {
                if (_ISuffix.Length > 1)
                {
                    if (_FI.Name.ToLower().EndsWith(_ISuffix))
                    {
                        reVal.Add(_FI);
                    }
                }
                else
                {
                    reVal.Add(_FI);
                }
            }

            return reVal;
        }
        #endregion 获取目录（不包含子目录）的文件列表

        #region 获取目录（不包含子目录）的文件名称列表
        /// <summary>
        /// 获取目录（不包含子目录）的文件名称列表
        /// </summary>
        public static List<string> SubFileName(string _IPath, string _ISuffix)
        {
            List<string> reVal = new List<string>();
            _ISuffix = "." + _ISuffix.Trim().ToLower().RemoveStartChar(".");
            DirectoryInfo _DI = new DirectoryInfo(_IPath);
            FileInfo[] _FIA = _DI.GetFiles();
            foreach (FileInfo _FI in _FIA)
            {
                if (_ISuffix.Length > 1)
                {
                    if (_FI.Name.ToLower().EndsWith(_ISuffix))
                    {
                        reVal.Add(_FI.Name);
                    }
                }
                else
                {
                    reVal.Add(_FI.Name);
                }
            }
            return reVal;
        }
        #endregion 获取目录（不包含子目录）的文件名称列表
    }
    #endregion 超级IO类
}