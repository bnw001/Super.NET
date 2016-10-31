using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace Super.Framework
{
    #region XML辅助类
    /// <summary>
    /// XML辅助类
    /// </summary>
    public static class XMLHelper
    {
        #region 创建XML文档
        /// <summary>
        /// 创建XML文档
        /// </summary>
        /// <param name="_IXmlFilePath">XML文件路径</param>
        /// <param name="_IRootNodeName"> 根节点名称</param>
        /// <returns>返回值：创建XML文档是否成功</returns>
        public static bool CreateXMLDocument(string _IXmlFilePath, string _IRootNodeName)
        {
            bool reVal = true;
            try
            {
                _IXmlFilePath = SuperManager.FileFullPath(_IXmlFilePath);
                XmlDocument XmlDoc = new XmlDocument();
                XmlDeclaration XmlDeclaration = XmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                XmlNode Root = XmlDoc.CreateElement(_IRootNodeName);
                XmlDoc.AppendChild(XmlDeclaration);
                XmlDoc.AppendChild(Root);
                XmlDoc.Save(_IXmlFilePath);
                reVal = true;
            }
            catch (Exception ex)
            {
                ex.Data.Add("XmlFilePath", _IXmlFilePath);
                ex.Data.Add("RootNodeName", _IRootNodeName);
                throw ex;
            }
            return reVal;
        }
        #endregion 创建XML文档

        #region 创建XML节点
        /// <summary>
        /// 创建XML节点
        /// </summary>
        /// <param name="_IXmlFilePath">XML文件路径</param>
        /// <param name="_INodePath">节点路径</param>
        /// <param name="_INodeInnerText">节点文本</param>
        /// <param name="AttriteName">属性</param>
        /// <param name="AttriteValue">属性值</param>
        /// <returns>返回值：创建是否成功</returns>
        public static bool CreateNode(string _IXmlFilePath, string _INodePath, string _INodeName, string _INodeInnerText, string[] AttriteName, string[] AttriteValue)
        {
            bool reVal = false;
            try
            {
                _IXmlFilePath = SuperManager.FileFullPath(_IXmlFilePath);
                XmlDocument Doc = new XmlDocument();
                Doc.Load(_IXmlFilePath);
                XmlNode Node = Doc.SelectSingleNode(_INodePath);
                if (Node != null)
                {
                    XmlElement SubElement = Doc.CreateElement(_INodeName);
                    SubElement.InnerText = _INodeInnerText;
                    if (AttriteName.Length == AttriteValue.Length)
                    {
                        for (int i = 0; i < AttriteName.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(AttriteName[i]) && !string.IsNullOrEmpty(AttriteValue[i]))
                            {
                                if (!SubElement.HasAttribute(AttriteName[i]))
                                {
                                    XmlAttribute NodeAttribute = Doc.CreateAttribute(AttriteName[i]);
                                    NodeAttribute.Value = AttriteValue[i];
                                    SubElement.Attributes.Append(NodeAttribute);
                                }
                            }
                        }
                    }
                    Node.AppendChild(SubElement);
                    Doc.Save(_IXmlFilePath);
                    reVal = true;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("XmlFilePath", _IXmlFilePath);
                ex.Data.Add("NodePath", _INodePath);
                throw ex;
            }
            return reVal;
        }
        #endregion 创建XML节点

        #region 修改节点文本值
        /// <summary>
        /// 修改节点文本值
        /// </summary>
        /// <param name="XmlFilePath">XML文件路径</param>
        /// <param name="NodePath">节点路径</param>
        /// <param name="NodeInnerText">节点文本</param>
        /// <returns>返回值：修改是否成功</returns>
        public static bool UpdateNodeText(string XmlFilePath, string NodePath, string NodeInnerText)
        {
            bool reVal = false;
            try
            {
                XmlFilePath = SuperManager.FileFullPath(XmlFilePath);
                XmlDocument Doc = new XmlDocument();
                Doc.Load(XmlFilePath);
                XmlNode Node = Doc.SelectSingleNode(NodePath);
                Node.InnerText = NodeInnerText;
                Doc.Save(XmlFilePath);
                reVal = true;
            }
            catch (Exception ex)
            {
                ex.Data.Add("XmlFilePath", XmlFilePath);
                ex.Data.Add("NodePath", NodePath);
                throw ex;
            }
            return reVal;
        }
        #endregion 修改节点文本值

        #region 删除节点
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="XmlFilePath">XML文件路径</param>
        /// <param name="NodePath">节点路径</param>
        /// <returns>返回值：删除是否成功</returns>
        public static bool DeleteNode(string XmlFilePath, string NodePath)
        {
            bool reVal = false;
            try
            {
                XmlFilePath = SuperManager.FileFullPath(XmlFilePath);
                XmlDocument Doc = new XmlDocument();
                Doc.Load(XmlFilePath);
                XmlNode Node = Doc.SelectSingleNode(NodePath);
                Node.ParentNode.RemoveChild(Node);
                Doc.Save(XmlFilePath);
                reVal = true;
            }
            catch (Exception ex)
            {
                ex.Data.Add("XmlFileFullPath", XmlFilePath);
                ex.Data.Add("NodePath", NodePath);
                throw ex;
            }
            return reVal;
        }
        #endregion 删除节点

        #region 更新或插入XML节点属性
        /// <summary>
        /// 更新或插入XML节点属性
        /// </summary>
        /// <param name="XmlFilePath">XML文件路径</param>
        /// <param name="NodePath">节点路径</param>
        /// <param name="WhereCondition">查询条件</param>
        /// <returns>返回值：更新XML节点属性</returns>
        public static bool UpdateOrCreateNodeAttrite(string XmlFilePath, string NodePath, string AttriteName, string AttriteValue, string WhereNodeText = null, Dictionary<string, string> WhereConditionAttrite = null)
        {
            bool reVal = false;
            try
            {
                XmlDocument Doc = new XmlDocument();
                XmlFilePath = SuperManager.FileFullPath(XmlFilePath);
                Doc.Load(XmlFilePath);
                XmlNodeList SelectNodeList = Doc.SelectNodes(NodePath);
                foreach (XmlNode node in SelectNodeList)
                {
                    bool isExistAttrite = false;
                    if (string.IsNullOrEmpty(WhereNodeText) || (WhereConditionAttrite != null && WhereConditionAttrite.Count > 0))
                    {
                        if (!string.IsNullOrEmpty(WhereNodeText) && node.InnerText.ToLower() == WhereNodeText.ToLower())
                        {
                            foreach (XmlAttribute nodeAttrite in node.Attributes)
                            {
                                if (nodeAttrite.Name.ToLower() == AttriteName.ToLower())
                                {
                                    nodeAttrite.Value = AttriteValue.Trim();
                                    isExistAttrite = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            foreach (string key in WhereConditionAttrite.Keys)
                            {
                                foreach (XmlAttribute nodeAttrite in node.Attributes)
                                {
                                    if (nodeAttrite.Name == key && nodeAttrite.Value == WhereConditionAttrite[key])
                                    {
                                        foreach (XmlAttribute EveryAttrite in node.Attributes)
                                        {
                                            if (EveryAttrite.Name.ToLower() == AttriteName.ToLower())
                                            {
                                                EveryAttrite.Value = AttriteValue.Trim();
                                                isExistAttrite = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (!isExistAttrite)
                        {
                            XmlAttribute NodeAttribute = Doc.CreateAttribute(AttriteName);
                            NodeAttribute.Value = AttriteValue;
                            node.Attributes.Append(NodeAttribute);
                        }
                    }
                }
                return reVal;
            }
            catch (Exception ex)
            {
                ex.Data.Add("XmlFilePath", XmlFilePath);
                ex.Data.Add("NodePath", NodePath);
                ex.Data.Add("UpdateAttriteName", AttriteName);
                ex.Data.Add("UpdateAttriteValue", AttriteValue);
                throw ex;
            }
        }
        #endregion 更新或插入XML节点属性

        #region 删除节点指定属性
        /// <summary>
        ///  删除节点指定属性
        /// </summary>
        /// <param name="XmlFilePath">XML文件路径</param>
        /// <param name="NodePath">节点路径</param>
        /// <param name="AttriteName">属性名称</param>
        /// <returns>返回值：节点属性删除是否成功</returns>
        public static bool DeleteNodeAttrite(string XmlFilePath, string NodePath, string AttriteName)
        {
            bool reVal = false;
            try
            {
                XmlFilePath = SuperManager.FileFullPath(XmlFilePath);
                XmlDocument Doc = new XmlDocument();
                Doc.Load(XmlFilePath);
                XmlNode Node = Doc.SelectSingleNode(NodePath);
                if (Node != null)
                {
                    Node.ParentNode.RemoveChild(Node);
                }
                Doc.Save(XmlFilePath);
                reVal = true;
            }
            catch (Exception ex)
            {
                ex.Data.Add("XmlFilePath", XmlFilePath);
                ex.Data.Add("NodePath", NodePath);
                throw ex;
            }
            return reVal;
        }
        #endregion 删除节点指定属性

        #region 删除节点全部属性
        /// <summary>
        /// 删除节点全部属性
        /// </summary>
        /// <param name="XmlFilePath">XML文件路径</param>
        /// <param name="NodePath">节点路径</param>
        /// <returns>返回值：删除是否成功</returns>
        public static bool DeleteAllAttrite(string XmlFilePath, string NodePath)
        {
            bool reVal = false;
            try
            {
                XmlFilePath = SuperManager.FileFullPath(XmlFilePath);
                XmlDocument Doc = new XmlDocument();
                Doc.Load(XmlFilePath);
                XmlNode Node = Doc.SelectSingleNode(NodePath);
                if (Node != null)
                {
                    Node.Attributes.RemoveAll();
                }
                Doc.Save(XmlFilePath);
            }
            catch (Exception ex)
            {
                ex.Data.Add("XmlFilePath", XmlFilePath);
                ex.Data.Add("NodePath", NodePath);
                throw ex;
            }
            return reVal;
        }
        #endregion 删除节点全部属性

        #region 单节点
        /// <summary>
        /// 单节点
        /// </summary>
        /// <param name="XmlFileFullPath">XML文件路径</param>
        /// <param name="NodePath">节点路径</param>
        /// <returns>返回值：单节点</returns>
        public static XmlNode SingleNode(string XmlFileFullPath, string NodePath)
        {
            XmlDocument Doc = new XmlDocument();
            try
            {
                Doc.Load(XmlFileFullPath);
                XmlNode reVal = Doc.SelectSingleNode(NodePath);
                return reVal;
            }
            catch (Exception ex)
            {
                ex.Data.Add("XmlFilePath", XmlFileFullPath);
                ex.Data.Add("NodePath", NodePath);
                throw ex;
            }
        }
        #endregion 单节点

        #region 节点列表
        /// <summary>
        /// 节点列表
        /// </summary>
        /// <param name="XmlFilePath">XML文件路径</param>
        /// <param name="NodePath">节点路径</param>
        /// <returns>返回值：节点列表</returns>
        public static XmlNodeList NodeList(string XmlFilePath, string NodePath)
        {
            XmlDocument Doc = new XmlDocument();
            try
            {
                XmlFilePath = SuperManager.FileFullPath(XmlFilePath);
                Doc.Load(XmlFilePath);
                XmlNodeList reVal = Doc.SelectNodes(NodePath);

                return reVal;
            }
            catch (Exception ex)
            {
                ex.Data.Add("XmlFilePath", XmlFilePath);
                ex.Data.Add("NodePath", NodePath);
                throw ex;
            }
        }
        #endregion 节点列表

        #region 节点属性值
        /// <summary>
        /// 节点属性值
        /// </summary>
        /// <param name="XmlFilePath"></param>
        /// <param name="NodePath"></param>
        /// <param name="xmlAttributeName"></param>
        /// <returns></returns>
        public static XmlAttribute NodeAttribute(string XmlFilePath, string NodePath, string XmlAttributeName)
        {
            XmlAttribute reVal = null;
            try
            {
                string Content = string.Empty;
                XmlDocument Doc = new XmlDocument();
                XmlFilePath = SuperManager.FileFullPath(XmlFilePath);
                Doc.Load(XmlFilePath);
                XmlNode Node = Doc.SelectSingleNode(NodePath);
                if (Node != null)
                {
                    if (Node.Attributes.Count > 0)
                    {
                        reVal = Node.Attributes[XmlAttributeName];
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("XmlFilePath", XmlFilePath);
                ex.Data.Add("NodePath", NodePath);
                ex.Data.Add("XmlAttributeName", XmlAttributeName);
                throw ex;
            }
            return reVal;
        }

        /// <summary>
        /// 节点属性值
        /// </summary>
        /// <param name="Node">节点</param>
        /// <returns>返回值：节点的属性值</returns>
        public static string AttriteValue(this XmlNode Node, string AttriteName)
        {
            string reVal = string.Empty;
            if (Node.Attributes[AttriteName] != null)
            {
                reVal = Node.Attributes[AttriteName].Value;
            }
            return reVal;
        }
        #endregion 节点属性值

        #region XML转换为DataTable
        public static DataTable XmlToDataTable(this string InputXmlString)
        {
            try
            {
                XmlDocument Doc = new XmlDocument();
                Doc.LoadXml(InputXmlString);
                XmlNode RootNode = Doc.DocumentElement;
                DataTable reVal = new DataTable(RootNode.Name);
                if (RootNode.ChildNodes.Count > 0)
                {
                    string ColName;

                    foreach (XmlNode node in RootNode.ChildNodes[0].ChildNodes)
                    {
                        ColName = node.Name;
                        reVal.Columns.Add(ColName);
                    }
                    foreach (XmlNode node in RootNode.ChildNodes)
                    {
                        DataRow dw = reVal.NewRow();
                        foreach (XmlNode SubNode in node.ChildNodes)
                        {
                            dw[SubNode.Name] = SubNode.InnerText;
                        }
                        reVal.Rows.Add(dw);
                    }
                }
                return reVal;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Running Process", "Super.Framework.XMLHelper.XmlToDataTable");
                ex.Data.Add("Working Parameter InputXmlString", InputXmlString);
                throw ex;
            }
        }
        #endregion XML转换为DataTable

        #region 字符串是否为XML字符串
        /// <summary>
        /// 字符串是否为XML字符串
        /// </summary>
        /// <param name="InputString">输入测试的字符串</param>
        public static bool IsXML(this string InputString)
        {
            bool reVal = true;
            XmlDocument Doc = new XmlDocument();
            try
            {
                Doc.LoadXml(InputString);
                reVal = true;
            }
            catch
            {
                reVal = false;
            }

            return reVal;
        }
        #endregion 字符串是否为XML字符串
    }
    #endregion XML辅助类
}