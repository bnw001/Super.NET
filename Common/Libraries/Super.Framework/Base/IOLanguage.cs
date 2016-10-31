using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Super.Framework
{
    #region 语言文件访问类
    /// <summary>
    /// 语言文件访问类
    /// </summary>
    public class IOLanguage : ILanguage
    {
        #region 获取值
        /// <summary>
        /// 获取值
        /// </summary>
        public string GetValue(string _ILanguage, string _IDomain, string _IKey)
        {
            string reVal = string.Empty;
            _IDomain = _IDomain.IsNullOrEmpty() ? _IKey : _IDomain;
            string FilePath = SuperManager.FileFullPath("~/Lang/" + _ILanguage + "/" + _IDomain + ".lang");
            if (File.Exists(FilePath))
            {
                if (_IDomain.IsNullOrEmpty())
                {
                    reVal = File.ReadAllText(FilePath);
                }
                else
                {
                    try
                    {
                        string NodePath = "//root/Item[@Name='" + _IKey + "']";
                        XmlNodeList NodeList = XMLHelper.NodeList(FilePath, NodePath);
                        if (NodeList.Count > 0)
                        {
                            reVal = NodeList[0].InnerText;
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToLog();
                    }
                }
            }

            return reVal;
        }
        #endregion 获取值

        #region 获取列表值
        /// <summary>
        /// 获取列表值
        /// </summary>
        public Dictionary<string, string> GetValueList(string _ILanguage, string _IDomain)
        {
            Dictionary<string, string> reVal = new Dictionary<string, string>();
            string FilePath = SuperManager.FileFullPath("~/Lang/" + _IDomain + ".dll");
            string NodePath = "//root/Item";
            XmlNodeList NodeList = XMLHelper.NodeList(FilePath, NodePath);
            foreach (XmlNode Node in NodeList)
            {
                if (!reVal.ContainsKey(Node.AttriteValue("Name")))
                {
                    reVal.Add(Node.AttriteValue("Name"), Node.InnerText);
                }
            }

            return reVal;
        }
        #endregion 获取列表值

        #region 设置值
        /// <summary>
        /// 设置值
        /// </summary>
        public bool SetValue(string _ILanguage, string _IDomain, string _IKey, string _IValue)
        {
            bool reVal = true;
            _IDomain = _IDomain.IsNullOrEmpty() ? _IKey : _IDomain;
            string FilePath = SuperManager.FileFullPath("~/Lang/" + _ILanguage + "/" + _IDomain + ".dll");

            try
            {
                if (_IDomain.IsNullOrEmpty())
                {
                    File.WriteAllText(FilePath, _IValue, Encoding.UTF8);
                }
                else
                {
                    string NodePath = "//root/Item[Name='" + _IKey + "']";
                    XMLHelper.UpdateNodeText(FilePath, NodePath, _IValue);
                }
            }
            catch (Exception ex)
            {
                ex.ToLog();
                reVal = false;
            }
            return reVal;
        }
        #endregion 设置值
    }
    #endregion 语言文件访问类
}