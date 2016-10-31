using System.Collections.Generic;

namespace Super.Framework
{
    #region 语言服务类
    /// <summary>
    /// 语言服务类
    /// </summary>
    public class LanguageService
    {
        #region 获取值
        /// <summary>
        /// 获取值
        /// </summary>
        public static string GetValue(string _ILanguage, string _IDomain, string _IKey)
        {
            ILanguage Lang = new IOLanguage();
            return Lang.GetValue(_ILanguage, _IDomain, _IKey);
        }
        #endregion 获取值

        #region 获取值
        /// <summary>
        /// 获取值
        /// </summary>
        public static string GetValue(string _ILanage, string _IKeyPath)
        {
            string reVal = string.Empty;
            string[] KeyPath = _IKeyPath.Split('/');
            if (KeyPath.Length == 2)
            {
                ILanguage Lang = new IOLanguage();
                reVal = Lang.GetValue(_ILanage, KeyPath[0], KeyPath[1]);
            }

            return reVal;
        }
        #endregion 获取值

        #region 获取值列表
        /// <summary>
        /// 获取值列表
        /// </summary>
        public static Dictionary<string, string> GetValueList(string _ILanguage, string _IDomain)
        {
            ILanguage Lang = new IOLanguage();
            return Lang.GetValueList(_ILanguage, _IDomain);
        }
        #endregion 获取值列表

        #region 设置值
        /// <summary>
        /// 设置值
        /// </summary>
        public static bool SetValue(string _ILanguage, string _IDomain, string _IKey, string _IValue)
        {
            bool reVal = true;
            ILanguage Lang = new IOLanguage();
            reVal = Lang.SetValue(_ILanguage, _IDomain, _IKey, _IValue);

            return reVal;
        }
        #endregion 设置值

        #region 设置值
        /// <summary>
        /// 设置值
        /// </summary>
        public static bool SetValue(string _ILanguage, string _IKeyPath, string _IValue)
        {
            bool reVal = false;
            string[] KeyPath = _IKeyPath.Split('/');
            if (KeyPath.Length == 2)
            {
                ILanguage LangData = new IOLanguage();
                reVal = LangData.SetValue(_ILanguage, KeyPath[0], KeyPath[1], _IValue);
            }

            return reVal;
        }
        #endregion 设置值
    }
    #endregion 语言服务类
}