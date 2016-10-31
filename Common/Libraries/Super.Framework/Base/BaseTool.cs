using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Super.Framework
{
    /// <summary>
    /// 基础工具
    /// </summary>
    public static class BaseTool
    {
        #region 空的36位ID
        /// <summary>
        /// 空的36位ID
        /// </summary>
        public static string EmptyID = "000000000000000000000000000000000000";
        #endregion 空的36位ID

        #region 获取一个新的随机GUID
        /// <summary>
        /// 获取一个新的随机GUID
        /// </summary>
        public static string NewID
        {
            get
            {
                return Guid.NewGuid().ToString().ToUpper();
            }
        }
        #endregion 获取一个新的随机GUID

        #region 获取一个随机MD5
        /// <summary>
        /// 获取一个随机MD5
        /// </summary>
        public static string NewMD5
        {
            get
            {
                return Secret.MD5(Guid.NewGuid().ToString().ToUpper() + DateTime.Now.ToString("yyyyMMddHHmmssffff"));
            }
        }
        #endregion 获取一个随机MD5

        #region 获取一个128位的随机字符串
        /// <summary>
        /// 获取一个128位的随机字符串
        /// </summary>
        public static string New128Safety
        {
            get
            {
                return NewCountString(128);
            }
        }
        #endregion 获取一个128位的随机字符串

        #region 获取一个指定位数的随机字符串
        /// <summary>
        /// 获取一个指定位数的随机字符串
        /// </summary>
        public static string NewCountString(int _ICount)
        {
            string reVal = "";

            if (_ICount > 0)
            {
                int CreateArrCount = (_ICount + 31) / 32;
                string _MD5S = "";
                for (int i = 0; i < CreateArrCount; i++)
                {
                    _MD5S += NewMD5;
                }
                char[] _CAS = _MD5S.ToCharArray();
                for (int i = 0; i < _ICount && i < _CAS.Length; i++)
                {
                    int _LS = 0;
                    if (_CAS[i].ToString().IsNumeric() && i % 3 != 0)
                    {
                        char[] _CSA = "abcrefghyjklmnopqdstuvwxiz".ToCharArray();
                        Random rand = new Random(BaseTool.NewID.GetHashCode());
                        int _R = rand.Next(26);
                        _CAS[i] = _CSA[_R];
                    }
                    if (i > 0)
                    {
                        _LS = _CAS[i - 1].GetHashCode() + _CAS[i].GetHashCode();
                    }
                    else
                    {
                        _LS = _CAS[i].GetHashCode() + _MD5S.GetHashCode();
                    }
                    if (_LS % 2 == 0)
                    {
                        reVal += _CAS[i].ToString().ToLower();
                    }
                    else
                    {
                        reVal += _CAS[i].ToString().ToUpper();
                    }
                }
            }

            return reVal;
        }
        #endregion 获取一个指定位数的随机字符串

        #region 返回现在时间
        /// <summary>
        /// 返回现在时间
        /// </summary>
        public static DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }
        #endregion 返回现在时间
    }
}
