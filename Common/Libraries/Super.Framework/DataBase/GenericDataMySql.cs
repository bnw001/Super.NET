using System.Collections.Generic;
using System.Data;

namespace Super.Framework
{
    #region MySql泛型数据类
    /// <summary>
    /// MySql泛型数据类
    /// </summary>
    public class GenericDataMySql<T> : IGenericData<T> where T : new()
    {
        #region 查询

        #region 查询全部数据
        /// <summary>
        /// 查询全部数据
        /// </summary>
        public DataTable Select()
        {
            DataTable reVal = new DataTable();

            return reVal;
        }
        #endregion 查询全部数据

        #region 参数查询
        /// <summary>
        /// 参数查询
        /// </summary>
        public DataTable Select(QueryInfo _IQI)
        {
            DataTable reVal = new DataTable();

            return reVal;
        }
        #endregion 参数查询

        #region 参数集查询转实体集
        /// <summary>
        /// 参数集查询转实体集
        /// </summary>
        public DataTable Parms2TList(List<string> _IPrams, string _IKey = "Code", string _IConnString = "")
        {
            DataTable reVal = new DataTable();
            string _ConnStrng = _IConnString.IsNullOrEmpty() ? Config.MySqlReadDBConnectionString : _IConnString;
            string _Where = _IKey + " IN (";
            foreach (string _FP in _IPrams)
            {
                _Where += "'" + _FP + "',";
            }
            _Where = _Where.RemoveEndChar(",") + ")";
            string _SelectSQL = "SELECT * FROM " + typeof(T).Name + _Where;
            MySqlHelper DbHelper = new MySqlHelper(_ConnStrng);
            reVal = DbHelper.ExecuteDataTable(_SelectSQL);

            return reVal;
        }
        #endregion 参数集查询转实体集

        #endregion 查询

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        public bool Insert(DataTable _IDT, string _IConnString = "", string _ITableName = "", string _INoRepeatColumn = "")
        {
            bool reVal = false;

            return reVal;
        }
        #endregion 新增

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        public bool Update(DataTable _IDT, string _IUpdateColumn = "*", string _IWhere = "", string _IConnString = "", string _ITableName = "")
        {
            bool reVal = false;

            return reVal;
        }
        #endregion 更新

        #region 删除

        #region 关联键值删除
        /// <summary>
        /// 关联键值删除
        /// </summary>
        public bool Delete(string _IDelKeyList, string _IConnString = "", string _ITableName = "")
        {
            bool reVal = false;

            return reVal;
        }
        #endregion 关联键值删除

        #region 关联条件删除
        /// <summary>
        /// 关联条件删除
        /// </summary>
        public bool DeleteByWhere(string _IWhereString, string _IConnString = "", string _ITableName = "")
        {
            bool reVal = false;

            return reVal;
        }
        #endregion 关联条件删除

        #endregion 删除
    }
    #endregion MySql泛型数据类
}