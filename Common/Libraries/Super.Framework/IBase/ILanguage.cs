using System.Collections.Generic;

namespace Super.Framework
{
    #region 语言接口
    /// <summary>
    /// 语言接口
    /// </summary>
    public interface ILanguage
    {
        #region 获取值
        /// <summary>
        /// 获取值
        /// </summary>
        string GetValue(string _IKey, string _IDomain, string _ILang = "ZH-CN");
        #endregion 获取值

        #region 获取列表值
        /// <summary>
        /// 获取列表值
        /// </summary>
        Dictionary<string, string> GetValueList(string _IDomain, string _ILang);
        #endregion 获取列表值

        #region 设置值
        /// <summary>
        /// 设置值
        /// </summary>
        bool SetValue(string _ILang, string _IDomain, string _IKey, string _IValue);
        #endregion 设置值
    }
    #endregion 语言接口
}