using Super.Framework;
using System.Collections.Generic;
using System.Data;

namespace Super.Framework
{
    #region 泛型数据类接口定义
    /// <summary>
    /// 泛型数据类接口定义
    /// </summary>
    public interface IGenericData<T> where T : new()
    {
        #region 查询

        #region 查询全部数据
        /// <summary>
        /// 查询全部数据
        /// </summary>
        DataTable Select();
        #endregion 查询全部数据

        #region 参数查询
        /// <summary>
        /// 参数查询
        /// </summary>
        DataTable Select(QueryInfo _IQI);
        #endregion 参数查询

        #region 参数集查询转实体集
        /// <summary>
        /// 参数集查询转实体集
        /// </summary>
        DataTable Parms2TList(List<string> _IPrams, string _IKey = "Code", string _IConnString = "");
        #endregion 参数集查询转实体集

        #endregion 查询

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        bool Insert(DataTable _IDT, string _IConnString = "", string _ITableName = "", string _INoRepeatColumn = "");
        #endregion 新增

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        bool Update(DataTable _IDT, string _IUpdateColumn = "*", string _IWhere = "", string _IConnString = "", string _ITableName = "");
        #endregion 更新

        #region 删除

        #region 关联键值删除
        /// <summary>
        /// 关联键值删除
        /// </summary>
        bool Delete(string _IDelKeyList, string _IConnString = "", string _ITableName = "");
        #endregion 关联键值删除

        #region 关联条件删除
        /// <summary>
        /// 关联条件删除
        /// </summary>
        bool DeleteByWhere(string _IWhereString, string _IConnString = "", string _ITableName = "");
        #endregion 关联条件删除

        #endregion 删除
    }
    #endregion 泛型数据类接口定义
}
