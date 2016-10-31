using System;
using System.Collections.Generic;
using System.Data;

namespace Super.Framework
{
    #region 简易查询
    /// <summary>
    /// 简易查询
    /// </summary>
    public class SimpleQuery
    {
        #region 数据库类型
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType { get; set; }
        #endregion 数据库类型

        #region 数据库连接字符串
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnString { get; set; }
        #endregion 数据库连接字符串

        #region 数据表名
        /// <summary>
        /// 数据表名
        /// </summary>
        public string TableName { get; set; }
        #endregion 数据表名

        #region 过滤条件
        /// <summary>
        /// 过滤条件
        /// </summary>
        public string Filter { get; set; }
        #endregion 过滤条件

        #region 排序字段
        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderColumn { get; set; }
        #endregion 排序字段

        #region 排序方向
        /// <summary>
        /// 排序方向
        /// </summary>
        public string OrderDirection { get; set; }
        #endregion 排序方向

        #region 页索引
        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex { get; set; }
        #endregion 页索引

        #region 页大小
        /// <summary>
        /// 页大小
        /// </summary>

        public int PageSize { get; set; }
        #endregion 页大小

        #region 查询字段
        /// <summary>
        /// 查询字段
        /// </summary>
        public string SelectColumn { get; set; }
        #endregion 查询字段

        #region 设置数据库类型
        /// <summary>
        /// 设置数据库类型
        /// </summary>
        public SimpleQuery DBType(string _IDBType)
        {
            this.DbType = _IDBType;
            if (Config.IsRWSeparate)
            {
                if (DbType.IsSQLite())
                {
                    ConnString = Config.SQLiteReadConnectionString;
                }
                else if (DbType.IsSqlServer())
                {
                    ConnString = Config.SqlServerReadDBConnectionString;
                }
                else if (DbType.IsOracle())
                {
                    ConnString = Config.OracleReadDBConnectionString;
                }
                else if (DbType.IsMySQL())
                {
                    ConnString = Config.MySqlConnectionString;
                }
            }
            else
            {
                if (DbType.IsSQLite())
                {
                    ConnString = Config.SQLiteConnectionString;
                }
                else if (DbType.IsSqlServer())
                {
                    ConnString = Config.SqlServerConnectionString;
                }
                else if (DbType.IsOracle())
                {
                    ConnString = Config.OracleConnectionString;
                }
                else if (DbType.IsMySQL())
                {
                    ConnString = Config.MySqlConnectionString;
                }
            }
            return this;
        }
        #endregion 设置数据库类型

        #region 设置数据表表名
        /// <summary>
        /// 设置数据表表名
        /// </summary>
        public SimpleQuery Table(string _ITable)
        {
            TableName = _ITable;
            return this;
        }
        #endregion 设置数据表表名

        #region 设置过滤条件
        /// <summary>
        /// 设置过滤条件
        /// </summary>
        public SimpleQuery Where(string whereSql)
        {
            Filter = whereSql;
            return this;
        }
        #endregion 设置过滤条件

        #region 设置排序
        /// <summary>
        /// 设置排序
        /// </summary>
        public SimpleQuery OrderBy(string _IOrderColumn)
        {
            List<string> _OrderInfo = _IOrderColumn.SplitString(",").ToList();
            if (_IOrderColumn.IsNotNullOrEmpty())
            {
                OrderColumn = string.Empty;
                OrderDirection = string.Empty;
                foreach (string _FKOrderKey in _OrderInfo)
                {
                    string _Col = string.Empty;
                    string _Dir = string.Empty;
                    if (_FKOrderKey.SplitString(" ").Length == 2)
                    {
                        _Col = _FKOrderKey.SplitString(" ")[0].Trim();
                        _Dir = _FKOrderKey.SplitString(" ")[1].Trim();
                        if (_Dir.ToUpper() != "ASC" && _Dir.ToUpper() != "DESC")
                        {
                            _Dir = "ASC";
                        }
                    }
                    else if (_FKOrderKey.SplitString(" ").Length == 1 && _FKOrderKey.IndexOf(" ") < 0)
                    {
                        _Col = _FKOrderKey.Trim();
                        _Dir = "ASC";
                    }
                    if (_Col.IsNotNullOrEmpty() && _Dir.IsNotNullOrEmpty())
                    {
                        OrderColumn += _Col + ",";
                        OrderDirection += _Dir + ",";
                    }
                }
                OrderColumn = OrderColumn.RemoveEndChar(",");
                OrderDirection = OrderDirection.RemoveEndChar(",");
            }
            return this;
        }
        #endregion 设置排序

        #region 设置索引页
        /// <summary>
        /// 设置索引页
        /// </summary>
        public SimpleQuery PageI(int _IPageIndex)
        {
            if (_IPageIndex >= 0)
            {
                PageIndex = _IPageIndex;
            }
            return this;
        }
        #endregion 设置索引页

        #region 设置页面容量
        /// <summary>
        /// 设置页面容量
        /// </summary>
        public SimpleQuery PageS(int _IPageSize)
        {
            if (_IPageSize > 0)
            {
                this.PageSize = _IPageSize;
            }

            return this;
        }
        #endregion 设置页面容量

        #region 设置查询字段
        /// <summary>
        /// 设置查询字段
        /// </summary>
        public SimpleQuery Column(string _IColumn)
        {
            if (_IColumn.IsNotNullOrEmpty())
            {
                _IColumn = _IColumn.Trim();
                SelectColumn = "";
                List<string> _ColumnList = _IColumn.SplitString(",").ToList();
                foreach (string _FKColKey in _ColumnList)
                {
                    if (_FKColKey.Trim().IndexOf("*") < 0)
                    {
                        if (!_FKColKey.StartsWith("[") && !_FKColKey.EndsWith("]"))
                        {
                            SelectColumn += "[" + _FKColKey + "],";
                        }
                        else
                        {
                            SelectColumn += "[" + _FKColKey.Replace("[", "").Replace("]", "") + "],";
                        }
                    }
                    else
                    {
                        SelectColumn += _FKColKey;
                    }
                }
                SelectColumn = SelectColumn.RemoveEndChar(",");
            }
            return this;
        }
        #endregion 设置查询字段

        #region 查询SQL语句
        /// <summary>
        /// 查询SQL语句
        /// </summary>
        public string SQL
        {
            get
            {
                string reVal = string.Empty;
                if (DbType == "SQLite")
                {
                    reVal = SQLiteSQL;
                }
                else if (DbType == "SqlServer")
                {
                    reVal = SqlServerSQL;
                }
                else if (DbType == "MySql")
                {
                    reVal = MySQL;
                }
                else if (DbType == "Oracle")
                {
                    reVal = OracleSQL;
                }
                return reVal;
            }
        }
        #endregion 查询SQL语句

        #region SQLiteSQL语句
        /// <summary>
        /// SqliteSQL语句
        /// </summary>
        public string SQLiteSQL
        {
            get
            {
                string reVal = string.Empty;
                if (TableName.IsNotNullOrEmpty() && SelectColumn.IsNotNullOrEmpty())
                {
                    List<string> _ColList = SelectColumn.SplitString(",").ToList();
                    string _ColS = string.Empty;
                    foreach (string _FKCol in _ColList)
                    {
                        if (_FKCol.StartsWith("[") && _FKCol.EndsWith("]"))
                        {
                            _ColS += _FKCol + ",";
                        }
                        else
                        {
                            _ColS += "[" + _FKCol.Replace("[", "").Replace("]", "") + "]";
                        }
                    }
                    _ColS = _ColS.RemoveEndChar(",");
                    string _WhereS = string.Empty;
                    if (Filter.IsNotNullOrEmpty())
                    {
                        _WhereS = " WHERE " + Filter;
                    }
                    string _LimitS = string.Empty;
                    if (PageIndex >= 0 && PageSize > 0)
                    {
                        _LimitS = " Limit " + PageSize + "  Offset " + PageSize * PageIndex + " ";
                    }
                    string _OrderS = string.Empty;
                    if (OrderColumn.IsNotNullOrEmpty() && OrderDirection.IsNotNullOrEmpty() && OrderColumn.SplitString(",").Length == OrderDirection.SplitString(",").Length)
                    {
                        List<string> _OrdCol = OrderColumn.SplitString(",").ToList();
                        List<string> _OrdDir = OrderDirection.SplitString(",").ToList();
                        for (int i = 0; i < _OrdCol.Count; i++)
                        {
                            _OrderS += " " + _OrdCol + " " + _OrdDir + ",";
                        }
                        _OrderS = _OrderS.RemoveEndChar(",");
                    }
                    reVal = "SELECT " + _ColS + " From [" + TableName + "] " + _WhereS + " " + _OrderS + " " + _LimitS;
                }
                return reVal;
            }
        }
        #endregion SQLiteSQL语句

        #region SqlServerSQL语句
        /// <summary>
        /// SqlServerSQL语句
        /// </summary>
        public string SqlServerSQL
        {
            get
            {
                string reVal = string.Empty;
                if (TableName.IsNotNullOrEmpty() && SelectColumn.IsNotNullOrEmpty())
                {
                    List<string> _ColList = SelectColumn.SplitString(",").ToList();
                    string _ColS = string.Empty;
                    foreach (string _FKCol in _ColList)
                    {
                        if (_FKCol.IndexOf("*") < 0)
                        {
                            if (_FKCol.StartsWith("[") && _FKCol.EndsWith("]"))
                            {
                                _ColS += _FKCol + ",";
                            }
                            else
                            {
                                _ColS += "[" + _FKCol.Replace("[", "").Replace("]", "") + "]";
                            }
                        }
                        else
                        {
                            _ColS = _FKCol + ",";
                        }
                    }
                    _ColS = _ColS.RemoveEndChar(",");
                    string _WhereS = string.Empty;
                    if (Filter.IsNotNullOrEmpty())
                    {
                        Filter = Filter.Trim().ToUpper().RemoveStartChar("WHERE");
                        _WhereS = " WHERE " + Filter + " ";
                    }

                    string _OrderS = string.Empty;
                    if (OrderColumn.IsNotNullOrEmpty() && OrderDirection.IsNotNullOrEmpty() && OrderColumn.SplitString(",").Length == OrderDirection.SplitString(",").Length)
                    {
                        List<string> _OrdCol = OrderColumn.SplitString(",").ToList();
                        List<string> _OrdDir = OrderDirection.SplitString(",").ToList();
                        for (int i = 0; i < _OrdCol.Count; i++)
                        {
                            _OrderS += " " + _OrdCol[i] + " " + _OrdDir[i] + ",";
                        }
                        _OrderS = _OrderS.RemoveEndChar(",");
                        if (_OrderS.IsNotNullOrEmpty())
                        {
                            _OrderS = " ORDER BY " + _OrderS;
                        }
                    }
                    if (PageSize > 0 && PageIndex >= 0)
                    {
                        reVal = "SELECT " + _ColS + " FROM(" + "SELECT " + _ColS + ",ROW_NUMBER() OVER (" + _OrderS + ") AS [RowNumber] FROM [" + TableName + "]" + _WhereS + ") " + TableName + "Page WHERE [RowNumber]>" + PageIndex * PageSize + " AND [RowNumber]<" + (PageIndex + 1) * PageSize + _WhereS.Replace("WHERE", "AND") + _OrderS;
                    }
                    else
                    {
                        reVal = "SELECT " + _ColS + " FROM[" + TableName + "]" + _WhereS + _OrderS;
                    }
                }
                return reVal;
            }
        }
        #endregion SqlServerSQL语句

        #region MySQL语句
        /// <summary>
        /// MySQL语句
        /// </summary>
        public string MySQL
        {
            get
            {
                string reVal = string.Empty;
                List<string> _ColList = SelectColumn.SplitString(",").ToList();
                string _ColS = string.Empty;
                foreach (string _FKCol in _ColList)
                {
                    _ColS = _FKCol + ",";
                }
                _ColS = _ColS.RemoveEndChar(",");
                string _WhereS = string.Empty;
                if (Filter.IsNotNullOrEmpty())
                {
                    Filter = Filter.Trim().ToUpper().RemoveStartChar("WHERE");
                    _WhereS = " WHERE " + Filter;
                }
                string _LimitS = string.Empty;
                if (PageIndex >= 0 && PageSize > 0)
                {
                    _LimitS = " Limit " + PageSize * PageIndex + " , " + PageSize + " ";
                }
                string _OrderS = string.Empty;
                if (OrderColumn.IsNotNullOrEmpty() && OrderDirection.IsNotNullOrEmpty() && OrderColumn.SplitString(",").Length == OrderDirection.SplitString(",").Length)
                {
                    List<string> _OrdCol = OrderColumn.SplitString(",").ToList();
                    List<string> _OrdDir = OrderDirection.SplitString(",").ToList();
                    for (int i = 0; i < _OrdCol.Count; i++)
                    {
                        _OrderS += " " + _OrdCol[i] + " " + _OrdDir[i] + ",";
                    }
                    _OrderS = _OrderS.RemoveEndChar(",");
                    if (_OrderS.IsNotNullOrEmpty())
                    {
                        _OrderS = " ORDER BY " + _OrderS;
                    }
                }
                reVal = "SELECT " + _ColS + " From " + TableName + " " + _WhereS + " " + _OrderS + " " + _LimitS;
                return reVal;
            }
        }
        #endregion MySQL语句

        #region OracleSQL语句
        /// <summary>
        /// OracleSQL语句
        /// </summary>
        public string OracleSQL
        {
            get
            {
                string reVal = string.Empty;
                if (TableName.IsNotNullOrEmpty() && SelectColumn.IsNotNullOrEmpty())
                {
                    List<string> _ColList = SelectColumn.SplitString(",").ToList();
                    string _ColS = string.Empty;
                    foreach (string _FKCol in _ColList)
                    {
                        if (_FKCol.StartsWith("[") && _FKCol.EndsWith("]"))
                        {
                            _ColS += _FKCol + ",";
                        }
                        else
                        {
                            _ColS += "[" + _FKCol.Replace("[", "").Replace("]", "") + "]";
                        }
                    }
                    _ColS = _ColS.RemoveEndChar(",");
                    string _WhereS = string.Empty;
                    if (Filter.IsNotNullOrEmpty())
                    {
                        _WhereS = " WHERE " + Filter + " ";
                    }

                    string _OrderS = string.Empty;
                    if (OrderColumn.IsNotNullOrEmpty() && OrderDirection.IsNotNullOrEmpty() && OrderColumn.SplitString(",").Length == OrderDirection.SplitString(",").Length)
                    {
                        List<string> _OrdCol = OrderColumn.SplitString(",").ToList();
                        List<string> _OrdDir = OrderDirection.SplitString(",").ToList();
                        for (int i = 0; i < _OrdCol.Count; i++)
                        {
                            _OrderS += " " + _OrdCol + " " + _OrdDir + ",";
                        }
                        _OrderS = _OrderS.RemoveEndChar(",");
                    }
                    if (PageSize > 0 && PageIndex >= 0)
                    {
                        reVal = "SELECT " + _ColS + " FROM ( SELECT A.*, ROWNUM RN  FROM (SELECT " + _ColS + " FROM [" + TableName + "]) A WHERE ROWNUM <= " + (PageIndex + 1) * PageSize + _WhereS.Replace("WHERE", "AND") + " ) WHERE RN > " + (PageIndex) * PageSize + _WhereS.Replace("WHERE", "AND");
                    }
                    else
                    {
                        reVal = "SELECT " + _ColS + " FROM[" + TableName + "]" + _WhereS + _OrderS;
                    }
                }
                return reVal;
            }
        }
        #endregion OracleSQL语句

        #region 连接字符串
        /// <summary>
        /// 连接字符串
        /// </summary>
        public SimpleQuery ConnS(string _IConnString)
        {
            this.ConnString = _IConnString;
            return this;
        }
        #endregion 连接字符串

        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        public DataTable Select()
        {
            DataTable reVal = new DataTable();
            if (DbType.IsSQLite())
            {
                try
                {
                    SQLiteHelper _DbHelp = new SQLiteHelper(ConnString);
                    reVal = _DbHelp.ExecuteDataTable(SQLiteSQL);
                }
                catch (Exception ex)
                {
                    ex.Data.Add("SimpleQuery-ConnString(SQLite)", ConnString);
                    ex.Data.Add("SimpleQuery-SQLiteSQL", SQLiteSQL);
                    throw ex;
                }
            }
            else if (DbType.IsSqlServer())
            {
                try
                {
                    SqlHelper _DbHelp = new SqlHelper(ConnString);
                    reVal = _DbHelp.ExecuteDataTable(SqlServerSQL);
                }
                catch (Exception ex)
                {
                    ex.Data.Add("SimpleQuery-ConnString(SqlServer)", ConnString);
                    ex.Data.Add("SimpleQuery-SqlServerSQL", SqlServerSQL);
                    throw ex;
                }
            }
            else if (DbType.IsMySQL())
            {
                try
                {
                    MySqlHelper _DbHelp = new MySqlHelper(ConnString);
                    reVal = _DbHelp.ExecuteDataTable(MySQL);
                }
                catch (Exception ex)
                {
                    ex.Data.Add("SimpleQuery-ConnString(MySQL)", ConnString);
                    ex.Data.Add("SimpleQuery-MySQL", MySQL);
                    throw ex;
                }
            }
            else if (DbType.IsOracle())
            {
                try
                {
                    OracleHelper _DbHelp = new OracleHelper(ConnString);
                    reVal = _DbHelp.ExecuteDataTable(SQLiteSQL);
                }
                catch (Exception ex)
                {
                    ex.Data.Add("SimpleQuery-ConnString(Oracle)", ConnString);
                    ex.Data.Add("SimpleQuery-OracleSQL", OracleSQL);
                    throw ex;
                }
            }
            return reVal;
        }
        #endregion 查询

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            OrderColumn = "ID";
            OrderDirection = "ASC";
            SelectColumn = " * ";
            PageIndex = 0;
            PageSize = 100000;
            DbType = Config.DBType;
            if (Config.IsRWSeparate)
            {
                if (DbType.IsSQLite())
                {
                    ConnString = Config.SQLiteReadConnectionString;
                }
                else if (DbType.IsSqlServer())
                {
                    ConnString = Config.SqlServerReadDBConnectionString;
                }
                else if (DbType.IsOracle())
                {
                    ConnString = Config.OracleReadDBConnectionString;
                }
                else if (DbType.IsMySQL())
                {
                    ConnString = Config.MySqlConnectionString;
                }
            }
            else
            {
                if (DbType.IsSQLite())
                {
                    ConnString = Config.SQLiteConnectionString;
                }
                else if (DbType.IsSqlServer())
                {
                    ConnString = Config.SqlServerConnectionString;
                }
                else if (DbType.IsOracle())
                {
                    ConnString = Config.OracleConnectionString;
                }
                else if (DbType.IsMySQL())
                {
                    ConnString = Config.MySqlConnectionString;
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public SimpleQuery()
        {
            Init();
        }
        #endregion 初始化
    }
    #endregion 简易查询
}