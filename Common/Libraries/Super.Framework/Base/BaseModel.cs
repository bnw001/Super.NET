using System;
using System.Collections.Generic;
using System.Web;

namespace Super.Framework
{
    #region 基底模型类
    /// <summary>
    /// 基底模型类
    /// </summary>
    public class BaseModel<T> where T : new()
    {
        #region 查询

        #region 通过ID查询
        /// <summary>
        /// 通过ID查询
        /// </summary>
        public static T IDSelect(dynamic _ID)
        {
            T reVal = default(T);
            IGenericData<T> IGD = DataSuperManage<T>.GenericData();
            QueryInfo _QUI = new QueryInfo()
            {
                TableName = typeof(T).Name,
                Filter = "ID='" + _ID + "'",
            };
            List<T> _DT = GenericService<T>.DataTable2List(IGD.Select(_QUI));
            if (_DT.Count > 0)
            {
                reVal = _DT[0];
            }
            return reVal;
        }
        #endregion 通过ID查询

        #region 普通查询
        /// <summary>
        /// 普通查询
        /// </summary>
        public static List<T> Select(int _IPageIndex = 0, int _IPageSize = 100000)
        {
            List<T> reVal = new List<T>();
            IGenericData<T> IGD = DataSuperManage<T>.GenericData();
            QueryInfo _QI = new QueryInfo()
            {
                TableName = typeof(T).Name,
                PageIndex = _IPageIndex,
                PageSize = _IPageSize
            };
            reVal = GenericService<T>.DataTable2List(IGD.Select(_QI));
            return reVal;
        }
        #endregion 普通查询

        #region Where查询
        /// <summary>
        /// Where查询
        public static List<T> WhereSelect(string _IWhere, int _IPageIndex = 0, int _IPageSize = 100000)
        {
            List<T> reVal = new List<T>();
            IGenericData<T> IGD = DataSuperManage<T>.GenericData();
            QueryInfo _QI = new QueryInfo()
            {
                TableName = typeof(T).Name,
                Filter = _IWhere,
                PageIndex = _IPageIndex,
                PageSize = _IPageSize
            };
            reVal = GenericService<T>.DataTable2List(IGD.Select(_QI));
            return reVal;
        }
        #endregion Where查询

        #region 参数集查询转实体集
        /// <summary>
        /// 参数集查询转实体集
        /// </summary>
        public static List<T> Parms2TList(List<string> _IPrams, string _IKey = "Code")
        {
            List<T> reVal = new List<T>();
            IGenericData<T> IGD = DataSuperManage<T>.GenericData();
            reVal = GenericService<T>.DataTable2List(IGD.Parms2TList(_IPrams, _IKey));

            return reVal;
        }

        /// <summary>
        /// 参数集查询转实体集
        /// </summary>
        public static List<T> Parms2TList(string _IParms, string _IKey = "Code")
        {
            List<T> reVal = new List<T>();
            IGenericData<T> IGD = DataSuperManage<T>.GenericData();
            reVal = GenericService<T>.DataTable2List(IGD.Parms2TList(_IParms.SplitString(",").ToList(), _IKey));

            return reVal;
        }
        #endregion 参数集查询转实体集

        #endregion 查询

        #region 新增

        #region 实体新增
        /// <summary>
        /// 实体新增
        /// </summary>
        public static bool Insert(T _IT, string _INoRepeatColumn = "")
        {
            bool reVal = false;
            IGenericData<T> IGD = DataSuperManage<T>.GenericData();
            List<T> _DT = new List<T>();
            _DT.Add(_IT);
            reVal = IGD.Insert(GenericService<T>.List2DataTable(_DT), "", "", _INoRepeatColumn);
            return reVal;
        }
        #endregion 实体新增

        #endregion 新增

        #region 更新

        #region 实体更新
        /// <summary>
        /// 实体更新
        /// </summary>
        public static bool Update(T _IT, string _UpdateColumn = "*", string _IWhere = "")
        {
            bool reVal = false;
            IGenericData<T> IGD = DataSuperManage<T>.GenericData();
            List<T> _DT = new List<T>();
            _DT.Add(_IT);
            reVal = IGD.Update(GenericService<T>.List2DataTable(_DT));
            return reVal;
        }
        #endregion 实体更新

        #endregion 更新

        #region 删除

        #region 键值删除
        /// <summary>
        /// 键值删除
        /// </summary>
        public static bool Delete(string _IKeyList, string _IKeyName = "ID")
        {
            bool reVal = false;

            return reVal;
        }
        #endregion 键值删除

        #region Where条件删除
        /// <summary>
        /// Where条件删除
        /// </summary>
        public static bool WhereDelete(string _IWhere)
        {
            bool reVal = false;

            return reVal;
        }
        #endregion Where条件删除

        #endregion 删除

        #region 页面请求转模型变量
        /// <summary>
        /// 页面请求转模型变量
        /// </summary>
        public static T FromRequest()
        {
            T reVal = new T();
            Type type = typeof(T);
            List<string> PropertyNameString = type.GetPropertyNameList();
            var _From = HttpContext.Current.Request.Form;
            var _Query = HttpContext.Current.Request.QueryString;
            foreach (string _Pram in PropertyNameString)
            {
                bool _IsDo = false;

                foreach (string _FK in _From.AllKeys)
                {
                    if (_FK.ToLower() == _Pram.ToLower())
                    {
                        reVal.SetObjectPropertyValue(_Pram, _From[_FK].ToString());
                        _IsDo = true;
                        break;
                    }
                }

                if (!_IsDo)
                {
                    foreach (string _QK in _Query.AllKeys)
                    {
                        if (_QK.ToLower() == _Pram.ToLower())
                        {
                            reVal.SetObjectPropertyValue(_QK, _Query[_QK].ToString());
                            break;
                        }
                    }
                }
            }
            return reVal;
        }
        #endregion 页面请求转模型变量
    }
    #endregion 基底模型类
}