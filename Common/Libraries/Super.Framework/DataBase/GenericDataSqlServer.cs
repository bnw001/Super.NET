using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Super.Framework
{
    #region SqlServer泛型数据类
    /// <summary>
    /// SqlServer泛型数据类
    /// </summary>
    public class GenericDataSqlServer<T> : IGenericData<T> where T : new()
    {
        #region 数据表列表缓存
        /// <summary>
        /// 数据表列表缓存
        /// </summary>
        private static Dictionary<string, List<string>> _DTDict = new Dictionary<string, List<string>>();
        #endregion 数据表列表缓存

        #region 数据表字段列表缓存
        /// <summary>
        /// 数据表字段列表缓存
        /// </summary>
        private static Dictionary<string, List<string>> _DTCDict = new Dictionary<string, List<string>>();
        #endregion 数据表字段列表缓存

        #region 获取数据表列表
        /// <summary>
        /// 获取数据表列表
        /// </summary>
        private List<string> GetList(string _IConnString, string _ITableName)
        {
            List<string> reVal = new List<string>();
            if (_DTDict.ContainsKey(Secret.EncryptHash(_IConnString)))
            {
                reVal = _DTDict[Secret.EncryptHash(_IConnString)];
                if (!reVal.Contains(_ITableName))
                {
                    SuperSqlServer _SuperDB = new SuperSqlServer();
                    reVal = _SuperDB.GetSystemDT(_IConnString);
                    if (reVal.Count > 0)
                    {
                        _DTDict.Add(Secret.EncryptHash(_IConnString), reVal);
                    }
                }
            }
            else
            {
                SuperSqlServer _SuperDB = new SuperSqlServer();
                reVal = _SuperDB.GetSystemDT(_IConnString);
                if (reVal.Count > 0)
                {
                    _DTDict.Add(Secret.EncryptHash(_IConnString), reVal);
                }
            }

            return reVal;
        }
        #endregion 获取数据表列表

        #region 获取字段列表
        /// <summary>
        /// 获取字段列表
        /// </summary>
        private List<string> GetColumnList(string _IConnString, string _ITableName)
        {
            List<string> reVal = new List<string>();
            string _EncryptString = Secret.EncryptHash(_IConnString + "-" + _ITableName);
            if (_DTDict.ContainsKey(_EncryptString))
            {
                reVal = _DTCDict[_EncryptString];
            }
            else
            {
                SuperSqlServer _SuperDB = new SuperSqlServer();
                reVal = _SuperDB.ColumnNameList(_ITableName, _IConnString);
            }

            return reVal;
        }
        #endregion 获取字段列表

        #region 查询

        #region 查询全部数据
        /// <summary>
        /// 查询全部数据
        /// </summary>
        public DataTable Select()
        {
            string _TableName = typeof(T).Name;
            QueryInfo _QI = new QueryInfo()
            {
                SelectColumn = "*",
                ConnString = Config.SqlServerReadDBConnectionString,
                Filter = "",
                OrderColumn = "",
                OrderDirection = "",
                PageIndex = 0,
                PageSize = 1000,
                TableName = _TableName
            };
            return Select(_QI);
        }
        #endregion 查询全部数据

        #region 参数查询
        /// <summary>
        /// 参数查询
        /// </summary>
        public DataTable Select(QueryInfo _IQI)
        {
            DataTable reVal = new DataTable();
            string _TableName = typeof(T).Name;
            string _ConnStrng = _IQI.ConnString.IsNullOrEmpty() ? Config.SqlServerReadDBConnectionString : _IQI.ConnString;
            List<string> _DTList = GetList(_ConnStrng, _IQI.TableName);
            if (_IQI.TableName.In(_DTList))
            {
                _TableName = _IQI.TableName;
            }
            SimpleQuery _SimQ = new SimpleQuery();
            reVal = _SimQ.Table(_TableName).ConnS(_ConnStrng).DBType("SqlServer").Column(_IQI.SelectColumn).PageI(_IQI.PageIndex).PageS(_IQI.PageSize).Where(_IQI.Filter).OrderBy(_IQI.OrderColumn + " " + _IQI.OrderDirection).Select();

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
            string _ConnStrng = _IConnString.IsNullOrEmpty() ? Config.SqlServerReadDBConnectionString : _IConnString;
            string _Where = _IKey + " IN (";
            foreach (string _FP in _IPrams)
            {
                _Where += "'" + _FP + "',";
            }
            _Where = _Where.RemoveEndChar(",") + ")";
            string _SelectSQL = "SELECT * FROM " + typeof(T).Name + _Where;
            SqlHelper DbHelper = new SqlHelper(_ConnStrng);
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
            if (_IDT != null)
            {
                string _ConnStrng = _IConnString.IsNullOrEmpty() ? Config.SqlServerReadDBConnectionString : _IConnString;
                _ITableName = _ITableName.IsNullOrEmpty() ? typeof(T).Name : _ITableName;
                SuperSqlServer _SuperDB = new SuperSqlServer();
                List<string> StoredProcedureDT = _SuperDB.GetStoredProcedureDT();
                string _ThisProcName = _ITableName.ToUpper() + "_CREATE";
                if (_ThisProcName.In(StoredProcedureDT))
                {
                    try
                    {
                        SqlParameter[] Params = new SqlParameter[] { new SqlParameter() { ParameterName = "@INSERTDT", SqlDbType = SqlDbType.Structured, Value = _IDT } };
                        SqlHelper _DBHelper = new SqlHelper(_ConnStrng);
                        DataTable ResultDT = _DBHelper.RunProcedureDataTable(_ThisProcName, Params);
                        int RecordCount = ResultDT.Rows[0][0].ToString().ToInt();
                        if (RecordCount >= 1)
                        {
                            reVal = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("Soruce", "GenericDataSqlServer.Insert");
                        ex.Data.Add("ConnString", _ConnStrng);
                        ex.Data.Add("SQL", _ThisProcName);
                        throw ex;
                    }
                }
                else
                {
                    List<string> _ColumnList = GetColumnList(_ConnStrng, _ITableName);
                    StringBuilder _SBSQL = new StringBuilder();
                    for (int i = 0; i < _IDT.Rows.Count; i++)
                    {
                        string _SQL = "INSERT INTO [" + _ITableName + "] (@COLUMN) VALUES(@VALUES);";
                        string _COL = string.Empty;
                        string _VAL = string.Empty;
                        for (int j = 0; j < _IDT.Columns.Count; j++)
                        {
                            _COL += "[" + _IDT.Columns[j].ColumnName + "],";
                            _VAL += "'" + _IDT.Rows[i][j].ToString() + "',";
                        }
                        _COL = _COL.RemoveEndChar(",");
                        _VAL = _VAL.RemoveEndChar(",");
                        _SQL = _SQL.Replace("@COLUMN", _COL).Replace("@VALUES", _VAL);
                        if (_INoRepeatColumn.IsNotNullOrEmpty())
                        {
                            List<string> _NoRepeatDL = _INoRepeatColumn.SplitString(",").ToList();
                            string _NoRepatWhere = "";
                            foreach (string _NItem in _NoRepeatDL)
                            {
                                for (int j = 0; j < _IDT.Columns.Count; j++)
                                {
                                    if (_NItem == _IDT.Columns[j].ColumnName)
                                    {
                                        _NoRepatWhere += _NItem + "='" + _IDT.Rows[i][j].ToString() + "' OR ";
                                    }
                                }
                            }
                            if (_NoRepatWhere.IsNotNullOrEmpty())
                            {
                                _NoRepatWhere = _NoRepatWhere.RemoveEndChar("OR");
                                _SQL = @"IF NOT EXISTS(SELECT TOP 1 * FROM " + _ITableName + " WHERE " + _NoRepatWhere + @")
                                       BEGIN
                                         " + _SQL + @"
                                       END";
                            }
                        }
                        _SBSQL.AppendLine(_SQL);
                    }
                    try
                    {
                        SqlHelper _DBHelper = new SqlHelper(_ConnStrng);
                        reVal = _DBHelper.ExecuteNonQuery(_SBSQL.ToString()) > 0;
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("Soruce", "GenericDataSqlServer.Insert");
                        ex.Data.Add("ConnString", _ConnStrng);
                        ex.Data.Add("SQL", _SBSQL.ToString());
                        throw ex;
                    }
                }
            }
            return reVal;
        }
        #endregion 新增

        #region 更新
        /// <summary>
        /// 单个更新
        /// </summary>
        public bool Update(DataTable _IDT, string _IUpdateColumn = "*", string _IWhere = "", string _IConnString = "", string _ITableName = "")
        {
            bool reVal = false;
            if (_IDT != null)
            {
                string _ConnStrng = _IConnString.IsNullOrEmpty() ? Config.SqlServerReadDBConnectionString : _IConnString;
                _ITableName = _ITableName.IsNullOrEmpty() ? typeof(T).Name : _ITableName;
                SuperSqlServer _SuperDB = new SuperSqlServer();
                List<string> StoredProcedureDT = _SuperDB.GetStoredProcedureDT();
                string _ThisProcName = _ITableName.ToUpper() + "_UPDATE";
                if (_ThisProcName.In(StoredProcedureDT))
                {
                    try
                    {
                        SqlParameter[] Params = new SqlParameter[] { new SqlParameter() { ParameterName = "@UPDATEDT", SqlDbType = SqlDbType.Structured, Value = _IDT } };
                        SqlHelper _DBHelper = new SqlHelper(_ConnStrng);
                        DataTable ResultDT = _DBHelper.RunProcedureDataTable(_ThisProcName, Params);
                        int RecordCount = ResultDT.Rows[0][0].ToString().ToInt();
                        if (RecordCount >= 1)
                        {
                            reVal = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("Soruce", "GenericDataSqlServer.Upate");
                        ex.Data.Add("ConnString", _ConnStrng);
                        ex.Data.Add("SQL", _ThisProcName);
                        throw ex;
                    }
                }
                else
                {
                    string _UpdateSQL = string.Empty;
                    string _UpdateSQLTemplate = "UPDATE [" + _ITableName + "] SET @SetColumn WHERE @Where";
                    _IWhere = _IWhere.Trim().ToUpper().RemoveStartChar("WHERE");
                    if (_IWhere.IsNullOrEmpty())
                    {
                        _IWhere = " [ID]=@ID;";
                    }
                    foreach (DataRow DW in _IDT.Rows)
                    {

                    }
                }
            }
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
            if (_IDelKeyList.IsNotNullOrEmpty())
            {
                string _WhereString = string.Empty;
                _WhereString = _IDelKeyList.Replace(",", "','");
                reVal = DeleteByWhere(_WhereString, _IConnString, _ITableName);
            }
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
            if (_IWhereString.IsNotNullOrEmpty())
            {
                string _ConnStrng = _IConnString.IsNullOrEmpty() ? Config.SqlServerReadDBConnectionString : _IConnString;
                _ITableName = _ITableName.IsNullOrEmpty() ? typeof(T).Name : _ITableName;
                _IWhereString = _IWhereString.Trim().ToUpper();
                _IWhereString = _IWhereString.RemoveStartChar("WHERE").Replace(" INSERT ", "").Replace(" UPDATE ", "").Replace(" DELETE ", "");
                StringBuilder _SB = new StringBuilder("DELETE FROM [" + _ITableName + "] WHERE ");
                _SB.Append(_IWhereString);
                try
                {
                    SqlHelper _DbHelper = new SqlHelper(_ConnStrng);
                    reVal = _DbHelper.ExecuteNonQuery(_SB.ToString()) > 0;
                }
                catch (Exception ex)
                {
                    ex.Data.Add("Soruce", "GenericDataSqlServer.Upate");
                    ex.Data.Add("ConnString", _ConnStrng);
                    ex.Data.Add("SQL", _SB.ToString());
                }
            }
            return reVal;
        }
        #endregion 关联条件删除

        #endregion 删除
    }
    #endregion SqlServer泛型数据类
}