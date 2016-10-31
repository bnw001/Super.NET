using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Super.Framework
{
    #region 数据库模型
    /// <summary>
    /// 数据库模型
    /// </summary>
    public class DataBaseModel
    {
        #region 数据表模型列表
        /// <summary>
        /// 数据表模型列表
        /// </summary>
        public List<DataTableModel> DTMList = new List<DataTableModel>();
        #endregion 数据表模型列表

        #region 索引模型列表
        /// <summary>
        /// 约束模型列表
        /// </summary>
        public List<IndexModel> IMList = new List<IndexModel>();
        #endregion 索引模型列表

        #region 视图模型列表
        /// <summary>
        /// 视图模型列表
        /// </summary>
        public List<DataViewModel> DVMList = new List<DataViewModel>();
        #endregion 视图模型列表

        #region 存储过程模型列表
        /// <summary>
        /// 存储过程模型列表
        /// </summary>
        public List<StoredProcedureModel> DPMList = new List<StoredProcedureModel>();
        #endregion 存储过程模型列表

        #region 方法列表
        /// <summary>
        /// 方法列表
        /// </summary>
        List<SQLFunctionModel> SFList = new List<SQLFunctionModel>();
        #endregion 方法列表

        #region 加载数据文件
        /// <summary>
        /// 加载数据文件
        /// </summary>
        /// <param name="XmlFilePath">数据文件路径</param>
        /// <returns>返回值：加载是否成功</returns>
        public bool LoadData(string _DirPath = "")
        {
            bool reVal = true;
            if (_DirPath.IsNullOrEmpty())
            {
                _DirPath = SuperManager.FileFullPath("~/DataModel/");
            }
            if (!_DirPath.EndsWith("\\"))
            {
                _DirPath = _DirPath + "\\";
            }
            if (Directory.Exists(_DirPath))
            {
                #region 加载表数据
                List<string> _DTMF = SuperIO.SubFileName(_DirPath, ".dtml");
                List<string> _DTList = new List<string>();
                foreach (string _FMF in _DTMF)
                {
                    string _FilePath = _DirPath + _FMF; ;
                    List<DataColumnModel> _DCMList = new List<DataColumnModel>();
                    DataTableModel _DTM = new DataTableModel();
                    XmlNode _Node = XMLHelper.SingleNode(_FilePath, "//root/Model");
                    _DTM.Name = _Node.AttriteValue("DataTable");
                    _DTM.DisplayName = _Node.AttriteValue("DName");
                    XmlNodeList _NodeList = XMLHelper.NodeList(_FilePath, "//root/Model/Property");
                    foreach (XmlNode _FXN in _NodeList)
                    {
                        if (_FXN.Attributes["DBColumn"] != null && _FXN.Attributes["Type"] != null)
                        {
                            DataColumnModel _DCM = new DataColumnModel()
                            {
                                Name = _FXN.AttriteValue("DBColumn"),
                                Length = _FXN.AttriteValue("Length").ToInt(),
                                IsPrimaryKey = _FXN.AttriteValue("IsPrimaryKey").ToBool(),
                                DisplayName = _FXN.AttriteValue("DName"),
                                DataType = _FXN.AttriteValue("Type"),
                                DecimalDigits = _FXN.AttriteValue("DecimalDigits").ToInt(),
                                DefaultValue = _FXN.AttriteValue("DefaultValue"),
                                IsAllowNull = _FXN.AttriteValue("IsAllowNull").ToBool(),
                                IsIdentity = _FXN.AttriteValue("IsIdentity").ToBool()
                            };
                            _DCMList.Add(_DCM);
                        }
                    }
                    _DTM.ColumnList = _DCMList;
                    DTMList.Add(_DTM);
                }
                #endregion 加载表数据
            }

            return reVal;
        }
        #endregion 加载数据文件

        #region 输出架构到数据文件
        /// <summary>
        /// 输出架构到数据文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>返回值：输出是否成功</returns>
        public bool OutPutData(string XmlFilePath, string DbType = null)
        {
            bool reVal = true;

            return reVal;
        }
        #endregion 输出架构到数据文件
    }
    #endregion 数据库模型

    #region 表模型
    /// <summary>
    /// 表模型
    /// </summary>
    public class DataTableModel
    {
        #region 表名
        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; set; }
        #endregion 表名

        #region 表显示名称
        /// <summary>
        /// 表描述
        /// </summary>
        public string DisplayName { get; set; }
        #endregion 表显示名称

        #region 字段列表
        /// <summary>
        /// 字段列表
        /// </summary>
        public List<DataColumnModel> ColumnList { get; set; }
        #endregion 字段列表
    }
    #endregion 表模型

    #region 列模型
    /// <summary>
    /// 列模型
    /// </summary>
    public class DataColumnModel
    {
        #region 名称
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        #endregion 名称

        #region 数据类型
        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }
        #endregion 数据类型

        #region 长度
        /// <summary>
        /// 长度
        /// </summary>
        public int Length { get; set; }
        #endregion 长度

        #region 小数位数
        /// <summary>
        /// 小数位数
        /// </summary>
        public int DecimalDigits { get; set; }
        #endregion 小数位数

        #region 预设值
        /// <summary>
        /// 预设值
        /// </summary>
        public string DefaultValue { get; set; }
        #endregion 预设值

        #region 显示名称
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        #endregion 显示名称

        #region 是否允许为空
        /// <summary>
        /// 是否允许为空
        /// </summary>
        public bool IsAllowNull { get; set; }
        #endregion 是否允许为空

        #region 是否主键
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }
        #endregion 是否主键

        #region 是否标识
        /// <summary>
        /// 是否标识
        /// </summary>
        public bool IsIdentity { get; set; }
        #endregion 是否标识
    }
    #endregion 列模型

    #region 索引模型
    /// <summary>
    /// 索引模型
    /// </summary>
    public class IndexModel
    {
        #region 索引类型
        /// <summary>
        /// 索引类型
        /// </summary>
        public string IndexType { get; set; }
        #endregion 索引类型

        #region 名称
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        #endregion 名称

        #region 索引键
        /// <summary>
        /// 索引键
        /// </summary>
        public string Key { get; set; }
        #endregion 索引键
    }
    #endregion 索引模型

    #region 视图模型
    /// <summary>
    /// 视图模型
    /// </summary>
    public class DataViewModel
    {
        public string Name { get; set; }

        public string Context { get; set; }
    }
    #endregion 视图模型

    #region 存储过程模型
    /// <summary>
    /// 存储过程模型
    /// </summary>
    public class StoredProcedureModel
    {
        #region 名称
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        #endregion 名称

        #region 内容
        /// <summary>
        /// 内容
        /// </summary>
        public string Context { get; set; }
        #endregion 内容
    }
    #endregion 存储过程模型

    #region 自定义方法模型
    /// <summary>
    /// 自定义方法模型
    /// </summary>
    public class SQLFunctionModel
    {
        #region 名称
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        #endregion 名称

        #region 内容
        /// <summary>
        /// 内容
        /// </summary>
        public string Context { get; set; }
        #endregion 内容
    }
    #endregion 自定义方法模型
}