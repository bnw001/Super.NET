using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Super.Framework
{
    #region 配置文件助理类
    /// <summary>
    /// 配置文件助理类
    /// </summary>
    public class ConfigHelper
    {
        #region Config基础目录
        /// <summary>
        /// Config基础目录
        /// </summary>
        private const string ConfigBaseDir = "~/Config/";
        #endregion Config基础目录

        #region 读取配置信息
        /// <summary>
        /// 读取配置信息
        /// </summary>
        public static string ReadData(string _InputConfigType, string _IKey)
        {
            string reVal = string.Empty;
            _InputConfigType = _InputConfigType.TBBRTrim();
            string XmlFilePath = SuperManager.FileFullPath(ConfigBaseDir + "/" + _InputConfigType + ".config");
            if (File.Exists(XmlFilePath))
            {
                XmlNode Node = XMLHelper.SingleNode(XmlFilePath, "//root/config/" + _IKey);
                if (Node != null)
                {
                    reVal = Node.InnerText;
                }
                else
                {
                    WriteData(_InputConfigType, _IKey, "");
                }
            }
            else
            {
                WriteData(_InputConfigType, _IKey, "");
            }
            return reVal;
        }
        #endregion 读取配置信息

        #region 写配置信息
        /// <summary>
        /// 写配置信息
        /// </summary>
        public static void WriteData(string _InputConfigType, string _IKey, string _IValue)
        {
            _InputConfigType = _InputConfigType.TBBRTrim();
            string XmlFilePath = SuperManager.FileFullPath(ConfigBaseDir + "/" + _InputConfigType + ".config");
            try
            {
                if (!File.Exists(XmlFilePath))
                {
                    XMLHelper.CreateXMLDocument(XmlFilePath, "root");
                    XMLHelper.CreateNode(XmlFilePath, "//root", "config", null, null, null);
                }
                XMLHelper.CreateNode(XmlFilePath, "//root/config/" + _IKey, _IValue, null, null, null);
                XMLHelper.UpdateNodeText(XmlFilePath, "//root/config/" + _IKey, _IValue);
            }
            catch (Exception ex)
            {
                ex.ToLog();
            }
        }
        #endregion 写配置信息

        #region 获取某个类型的配置项列表
        /// <summary>
        /// 获取某个类型的配置项列表
        /// </summary>
        public static Dictionary<string, string> GetItemListByTypeName(string _IConfigTypeName)
        {
            Dictionary<string, string> reVal = new Dictionary<string, string>();
            string XmlFilePath = SuperManager.FileFullPath(ConfigBaseDir + "/" + _IConfigTypeName.Trim() + ".config");
            XmlNodeList NodeList = XMLHelper.SingleNode(XmlFilePath, "//root/config").ChildNodes;
            foreach (XmlNode Node in NodeList)
            {
                ConfigItem Item = new ConfigItem()
                {
                    Key = Node.Name,
                    Value = Node.InnerText
                };
                if (!reVal.ContainsKey(Item.Key))
                {
                    reVal.Add(Item.Key, Item.Value);
                }
            }
            return reVal;
        }
        #endregion 获取某个类型的配置项列表

        #region 写入配置列表项
        /// <summary>
        /// 写入配置列表项
        /// </summary>
        public static void WriteData(string InputTypeName, List<ConfigItem> _IList)
        {
            string XmlFilePath = SuperManager.FileFullPath(ConfigBaseDir + "/" + InputTypeName.Trim() + ".config");
            try
            {
                if (!File.Exists(XmlFilePath))
                {
                    XMLHelper.CreateXMLDocument(XmlFilePath, "root");
                    XMLHelper.CreateNode(XmlFilePath, "//root", "config", null, null, null);
                }
                foreach (ConfigItem Item in _IList)
                {
                    XMLHelper.CreateNode(XmlFilePath, "//root/config/" + Item.Key, Item.Value, null, null, null);
                    XMLHelper.UpdateNodeText(XmlFilePath, "//root/config/" + Item.Key, Item.Value);
                }
            }
            catch (Exception ex)
            {
                ex.ToLog();
            }
        }
        #endregion 写入配置列表项
    }
    #endregion 配置文件助理类
}