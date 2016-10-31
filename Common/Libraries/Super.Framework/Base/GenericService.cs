using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Super.Framework
{
    #region 泛型通用服务类
    /// <summary>
    /// 泛型通用服务类
    /// </summary>
    public class GenericService<T> where T : new()
    {
        #region DataTable转List
        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <param name="InputDT">输入的数据表</param>
        public static List<T> DataTable2List(DataTable InputDT)
        {
            List<T> reVal = new List<T>();
            Type type = typeof(T);
            List<PropertyInfo> PropertyArr = type.GetPropertyList();
            foreach (DataRow dw in InputDT.Rows)
            {
                T t = new T();
                foreach (PropertyInfo PI in PropertyArr)
                {
                    if (InputDT.Columns.Contains(PI.Name))
                    {
                        if (!PI.CanWrite) continue;
                        string ObjVal = dw[PI.Name].ToString();
                        if (PI.PropertyType.Name == "Boolean" && ObjVal.ToUpper() != "TRUE")
                        {
                            ObjVal = "False";
                        }
                        t.SetObjectPropertyValue(PI.Name, ObjVal);
                    }
                }
                reVal.Add(t);
            }

            return reVal;
        }
        #endregion DataTable转List

        #region List转DataTable
        /// <summary>
        /// List转DataTable
        /// </summary>
        /// <param name="InputList">输入的对象列表</param>
        public static DataTable List2DataTable(List<T> InputList, string ColumnDisplayOrder = "")
        {
            DataTable reVal = new DataTable();
            Type type = typeof(T);
            List<PropertyInfo> PropertyArr = type.GetPropertyList();
            List<string> ColumnDisplayOrderList = ColumnDisplayOrder.SplitString(",").ToList();
            List<string> PropertyList = new List<string>();
            foreach (PropertyInfo PI in PropertyArr)
            {
                if (PI.CanRead)
                {
                    PropertyList.Add(PI.Name);
                }
            }
            if (ColumnDisplayOrder.Length > 0)
            {
                foreach (string ColumnName in ColumnDisplayOrderList)
                {
                    if (ColumnName.In(PropertyList))
                    {
                        reVal.Columns.Add(ColumnName);
                    }
                }
            }

            if (reVal.Columns.Count == 0)
            {
                foreach (string PIN in PropertyList)
                {
                    reVal.Columns.Add(PIN);
                }
            }
            foreach (T InsertModel in InputList)
            {
                DataRow DW = reVal.NewRow();
                foreach (string PIN in PropertyList)
                {
                    DW[PIN] = InsertModel.GetObjectPropertyValue(PIN);
                }
                reVal.Rows.Add(DW);
            }
            return reVal;
        }
        #endregion List转DataTable

        #region 字典参数转泛型
        /// <summary>
        /// 字典参数转泛型
        /// </summary>
        /// <param name="InputDict">字典实体</param>
        public static T Dictionary2T(Dictionary<string, string> InputDict)
        {
            T reVal = new T();
            Type type = typeof(T);
            List<string> PropertyNameString = type.GetPropertyNameList();
            foreach (string Key in InputDict.Keys)
            {
                if (Key.RemoveStartChar("@").In(PropertyNameString))
                {
                    reVal.SetObjectPropertyValue(Key.RemoveStartChar("@"), InputDict[Key]);
                }
            }
            if (reVal.GetObjectPropertyValue("ID").Length != 36)
            {
                reVal.SetObjectPropertyValue("ID", Guid.NewGuid().ToString().ToUpper());
            }
            return reVal;
        }
        #endregion 字典参数转泛型

        #region 字典参数转泛型列表
        /// <summary>
        /// 字典参数转泛型列表
        /// </summary>
        /// <param name="InputDict">字典参数列表</param>
        /// <param name="Key">关键字段</param>
        public static List<T> Dictionary2ListT(Dictionary<string, string> InputDict, string Key = "ID")
        {
            List<T> reVal = new List<T>();
            T ModelT = new T();
            Type type = typeof(T);
            List<string> PropertyNameString = type.GetPropertyNameList();
            List<string> KeyList = new List<string>();

            #region 分拆关键字段
            if (InputDict.ContainsKey(Key))
            {
                foreach (string DKey in InputDict.Keys)
                {
                    if (DKey.RemoveStartChar("@") == Key)
                    {
                        KeyList = InputDict[DKey].SplitString(",").ToList();
                    }
                }
            }
            else
            {
                foreach (string DKey in InputDict.Keys)
                {
                    if (!InputDict[DKey].IsNullOrEmpty())
                    {
                        KeyList = InputDict[DKey].SplitString(",").ToList();
                    }
                }
            }
            #endregion 分拆关键字段

            #region 如果是ID则特殊处理
            if (Key == "ID")
            {
                List<string> TKeyList = new List<string>();
                foreach (string TKey in KeyList)
                {
                    if (TKey.Length != 36)
                    {
                        TKeyList.Add(BaseTool.NewID);
                    }
                    else
                    {
                        TKeyList.Add(TKey);
                    }
                }
                KeyList = TKeyList;
            }
            #endregion 如果是ID 则特殊处理

            #region 创建泛型对象
            if (KeyList.Count > 0)
            {
                Dictionary<string, List<string>> TDict = new Dictionary<string, List<string>>();
                TDict.Add(Key, KeyList);
                #region 解析出各种属性的列表
                foreach (string DKey in InputDict.Keys)
                {
                    if (DKey.RemoveStartChar("@").In(PropertyNameString) && DKey.RemoveStartChar("@") != Key)
                    {
                        List<string> DataList = InputDict[DKey].SplitString(",").ToList();
                        if (DataList.Count < KeyList.Count)
                        {
                            for (int i = DataList.Count; i < KeyList.Count; i++)
                            {
                                DataList.Add("");
                            }
                        }
                        TDict.Add(DKey.RemoveStartChar("@"), DataList);
                    }
                }
                #endregion 解析出各种属性的列表

                #region 由属性创建对象并推入列表
                for (int Count = 0; Count < KeyList.Count; Count++)
                {
                    ModelT = new T();
                    foreach (string DTKey in TDict.Keys)
                    {
                        ModelT.SetObjectPropertyValue(DTKey, TDict[DTKey][Count]);
                    }
                    reVal.Add(ModelT);
                }
                #endregion 由属性创建对象并推入列表
            }
            #endregion 创建泛型对象

            return reVal;
        }
        #endregion 字典参数转泛型列表
    }
    #endregion 泛型通用服务类
}