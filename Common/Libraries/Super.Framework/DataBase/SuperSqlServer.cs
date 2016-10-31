﻿using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Super.Framework
{
    #region 超级SqlServer类
    /// <summary>
    /// 超级SqlServer类
    /// </summary>
    public class SuperSqlServer
    {
        #region 系统数据表
        /// <summary>
        /// 系统数据表
        /// </summary>
        public List<string> GetSystemDT(string DBConnectionString = null)
        {
            List<string> reVal = new List<string>();
            if (DBConnectionString.IsNullOrEmpty())
            {
                DBConnectionString = Config.SqlServerConnectionString;
            }

            string SelectSQL = "SELECT NAME FROM SYSOBJECTS WHERE XTYPE='U'";
            SqlHelper DbHelp = new SqlHelper(DBConnectionString);
            DataTable SystemDT = DbHelp.ExecuteDataTable(SelectSQL);
            foreach (DataRow dw in SystemDT.Rows)
            {
                reVal.Add(dw[0].ToString());
            }

            return reVal;
        }
        #endregion 系统数据表

        #region 用户视图名称列表
        /// <summary>
        /// 用户视图名称列表
        /// </summary>
        public List<string> GetViewDT(string DBConnectionString = null)
        {
            List<string> reVal = new List<string>();
            if (DBConnectionString.IsNullOrEmpty())
            {
                DBConnectionString = Config.SqlServerConnectionString;
            }
            SqlHelper DbHelper = new SqlHelper(DBConnectionString);
            string SelectSQL = @"SELECT A.NAME FROM SYS.ALL_OBJECTS A,SYS.SQL_MODULES B WHERE A.IS_MS_SHIPPED=0 and A.OBJECT_ID = B.OBJECT_ID AND A.[TYPE] IN ('V') ORDER BY A.[NAME] ASC";
            DataTable SelectDT = DbHelper.ExecuteDataTable(SelectSQL);
            foreach (DataRow dw in SelectDT.Rows)
            {
                reVal.Add(dw[0].ToString());
            }

            return reVal;
        }
        #endregion 用户视图名称列表

        #region 用户视图模型
        /// <summary>
        /// 用户视图模型
        /// </summary>
        public List<DataViewModel> GetViewSimpleDT(string DBConnectionString = null)
        {
            List<DataViewModel> reVal = new List<DataViewModel>();
            string SqlScript = @"SELECT A.NAME,A.[TYPE],B.[DEFINITION] FROM SYS.ALL_OBJECTS A,SYS.SQL_MODULES B 
WHERE A.IS_MS_SHIPPED=0 and A.OBJECT_ID = B.OBJECT_ID AND A.[TYPE] IN ('V') 
ORDER BY A.[NAME] ASC";
            if (DBConnectionString.IsNullOrEmpty())
            {
                DBConnectionString = Config.SqlServerConnectionString;
            }
            SqlHelper DbHelper = new SqlHelper(DBConnectionString);
            DataTable SelectDT = DbHelper.ExecuteDataTable(SqlScript);
            foreach (DataRow dw in SelectDT.Rows)
            {
                reVal.Add(new DataViewModel() { Name = dw["NAME"].ToString(), Context = dw["DEFINITION"].ToString() });
            }

            return reVal;
        }
        #endregion 用户视图模型

        #region 用户存储过程列表
        /// <summary>
        /// 用户存储过程列表
        /// </summary>
        public List<string> GetStoredProcedureDT(string DBConnectionString = null)
        {
            List<string> reVal = new List<string>();
            if (DBConnectionString.IsNullOrEmpty())
            {
                DBConnectionString = Config.SqlServerConnectionString;
            }
            SqlHelper DbHelper = new SqlHelper(DBConnectionString);
            string SelectSQL = @"SELECT A.NAME FROM SYS.ALL_OBJECTS A,SYS.SQL_MODULES B WHERE A.IS_MS_SHIPPED=0 and A.OBJECT_ID = B.OBJECT_ID AND A.[TYPE] IN ('P') ORDER BY A.[NAME] ASC";
            DataTable SelectDT = DbHelper.ExecuteDataTable(SelectSQL);
            foreach (DataRow dw in SelectDT.Rows)
            {
                reVal.Add(dw[0].ToString());
            }

            return reVal;
        }
        #endregion 用户存储过程列表

        #region 表的栏位名称列表
        /// <summary>
        /// 表的栏位名称列表
        /// </summary>
        /// <param name="TableName">表名</param>
        public List<string> ColumnNameList(string TableName, string DBConnectionString = null)
        {
            List<string> reVal = new List<string>();
            if (DBConnectionString.IsNullOrEmpty())
            {
                DBConnectionString = Config.SqlServerConnectionString;
            }
            SqlHelper DbHelper = new SqlHelper(DBConnectionString);
            string SelectSQL = "SELECT * FROM SYS.COLUMNS WHERE OBJECT_ID=OBJECT_ID('@TableName')".Replace("@TableName", TableName);
            DataTable SelectDT = DbHelper.ExecuteDataTable(SelectSQL);
            foreach (DataRow dw in SelectDT.Rows)
            {
                reVal.Add(dw["name"].ToString());
            }

            return reVal;
        }
        #endregion 表的栏位名称列表

        #region 存储过程内容
        /// <summary>
        /// 存储过程内容
        /// </summary>
        public string GetStoredProcedureContent(string _ProcName, string _IDBConnectionString = null)
        {
            string reVal = "";
            if (_IDBConnectionString.IsNullOrEmpty())
            {
                _IDBConnectionString = Config.SqlServerConnectionString;
            }
            string SelectSQL = @"IF EXISTS(SELECT A.NAME FROM SYS.ALL_OBJECTS A,SYS.SQL_MODULES B WHERE A.IS_MS_SHIPPED=0 and A.OBJECT_ID = B.OBJECT_ID AND A.NAME='@ProcName' AND A.[TYPE] IN ('P'))
                                    BEGIN
                                    EXEC SP_HELPTEXT '@ProcName';
                                    END
                                    ELSE
                                    BEGIN
                                    SELECT ''
                                    END".Replace("@ProcName", _ProcName);
            SqlHelper DbHelper = new SqlHelper(_IDBConnectionString);
            DataTable SelectDT = DbHelper.ExecuteDataTable(SelectSQL);
            StringBuilder _SB = new StringBuilder();
            foreach (DataRow _DW in SelectDT.Rows)
            {
                _SB.AppendLine(_DW[0].ToString());
            }
            reVal = _SB.ToString();

            return reVal;
        }
        #endregion 存储过程内容

        #region 表模型列表
        /// <summary>
        /// 表模型列表
        /// </summary>

        public List<DataTableModel> GetSystemSimpleDT(string DBConnectionString = null)
        {
            List<DataTableModel> reVal = new List<DataTableModel>();
            if (DBConnectionString.IsNullOrEmpty())
            {
                DBConnectionString = Config.SqlServerConnectionString;
            }
            string SelectSQL = @"SELECT DISTINCT
            [Name]=CASE WHEN A.COLORDER=1 THEN B.NAME ELSE '' END,
            [Description]=CASE WHEN A.COLORDER=1 THEN ISNULL(C.VALUE,'') ELSE '' END
            FROM SYSCOLUMNS A
            LEFT JOIN SYSOBJECTS B ON A.ID=A.ID  AND B.XTYPE='U' AND  B.NAME<>'DTPROPERTIES'
            LEFT JOIN SYS.EXTENDED_PROPERTIES C ON B.ID=C.MAJOR_ID AND C.MINOR_ID=0
            WHERE A.COLORDER=1 AND B.NAME IS NOT NULL;";
            SqlHelper DbHelper = new SqlHelper(DBConnectionString);
            DataTable SelectDT = DbHelper.ExecuteDataTable(SelectSQL);
            foreach (DataRow dw in SelectDT.Rows)
            {
                reVal.Add(new DataTableModel() { Name = dw["Name"].ToString(), DisplayName = dw["Description"].ToString() });
            }

            return reVal;
        }
        #endregion 表模型列表

        #region 表的栏位列表
        /// <summary>
        /// 表的栏位列表
        /// </summary>
        /// <param name="TableName">表名</param>
        public List<DataColumnModel> ColumnList(string TableName, string DBConnectionString = null)
        {
            List<DataColumnModel> reVal = new List<DataColumnModel>();
            if (DBConnectionString.IsNullOrEmpty())
            {
                DBConnectionString = Config.SqlServerConnectionString;
            }
            string SelectSQL = @"SELECT
            [DisplayOrder]=[A].[COLORDER],
            [Name]=[A].[Name],
            [IsIdentity]=CASE WHEN COLUMNPROPERTY([A].[ID], [A].[Name],'IsIdentity')=1 THEN '1'ELSE '0' END,
            [IsPrimaryKey]=CASE WHEN EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE='PK' AND PARENT_OBJ=A.ID AND NAME IN (
            SELECT [Name] FROM SYSINDEXES WHERE [INDID] IN(
            SELECT [INDID] FROM SYSINDEXKEYS WHERE [ID]=[A].[ID] AND [COLID]=[A].[COLID]
            ))) THEN '1' ELSE '0' END,
            [DataType]=[B].[Name],
            [OccupancyByte]=[A].[length],
            [Length]=COLUMNPROPERTY(A.ID,A.name,'PRECISION'),
            [DecimalDigits]=isnull(COLUMNPROPERTY(A.ID,A.name,'Scale'),0),
            [IsAllowNull]=CASE WHEN [A].[IsNullAble]=1 THEN '1' ELSE '0' END,
            [DefaultValue]=ISNULL(E.text,''),
            [Description]=ISNULL(g.[value],'')
            FROM SYSCOLUMNS [A]
            LEFT JOIN systypes [B] ON [A].[XUSERTYPE]=[B].[XUSERTYPE]
            INNER JOIN SYSOBJECTS [D] ON [A].[ID]=[D].[ID]  AND [D].[XTYPE]='U' AND  [D].[Name]<>'dtproperties'
            LEFT JOIN syscomments [E] ON [A].[CDEFAULT]=[E].[ID]
            LEFT JOIN SYS.EXTENDED_PROPERTIES [G] ON [A].[ID]=[G].[MAJOR_ID] AND [A].[COLID]=[G].[MINOR_ID] 
            LEFT JOIN SYS.EXTENDED_PROPERTIES [F] ON [D].[ID]=[F].[MAJOR_ID] AND [F].[MINOR_ID]=0
            WHERE [D].[NAME]='@TableName'".Replace("@TableName", TableName);
            SqlHelper DbHelper = new SqlHelper(DBConnectionString);
            DataTable SelectDT = DbHelper.ExecuteDataTable(SelectSQL);
            DataColumnModel DataColumnM = null;
            foreach (DataRow dw in SelectDT.Rows)
            {
                DataColumnM = new DataColumnModel()
                {
                    Name = dw["Name"].ToString(),
                    DataType = dw["DataType"].ToString(),
                    Length = dw["Length"].ToString().ToInt(),
                    IsAllowNull = dw["IsAllowNull"].ToString().ToBool(),
                    DisplayName = dw["Description"].ToString(),
                    IsIdentity = dw["IsIdentity"].ToString().ToBool(),
                    IsPrimaryKey = dw["IsPrimaryKey"].ToString().ToBool(),
                    DecimalDigits = dw["DecimalDigits"].ToString().ToInt(),
                    DefaultValue = dw["DefaultValue"].ToString()
                };

                if (!reVal.Contains(DataColumnM))
                {
                    reVal.Add(DataColumnM);
                }
            }

            return reVal;
        }
        #endregion 表的栏位列表

        #region 建表的SQL
        /// <summary>
        /// 建表的SQL
        /// </summary>
        public string CreateDTSQL(bool _IsReBuild = false, string _IDBConnectionString = null)
        {
            string reVal = "";
            if (_IDBConnectionString.IsNullOrEmpty())
            {
                _IDBConnectionString = Config.SqlServerConnectionString;
            }
            string _SQL = "SELECT NAME FROM SYSOBJECTS WHERE XTYPE='U'";
            SqlHelper _DbHelper = new SqlHelper(_IDBConnectionString);
            DataTable _SYSDT = _DbHelper.ExecuteDataTable(_SQL);
            DataTable _SubDT = new DataTable();
            foreach (DataRow DW in _SYSDT.Rows)
            {
                string _TableName = DW[0].ToString();

                #region 定义SQL
                string _DTSQL = @"SELECT
            [DisplayOrder]=[A].[COLORDER],
            [Name]=[A].[Name],
            [IsIdentity]=CASE WHEN COLUMNPROPERTY([A].[ID], [A].[Name],'IsIdentity')=1 THEN '1'ELSE '0' END,
            [IsPrimaryKey]=CASE WHEN EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE='PK' AND PARENT_OBJ=A.ID AND NAME IN (
            SELECT [Name] FROM SYSINDEXES WHERE [INDID] IN(
            SELECT [INDID] FROM SYSINDEXKEYS WHERE [ID]=[A].[ID] AND [COLID]=[A].[COLID]
            ))) THEN '1' ELSE '0' END,
            [DataType]=[B].[Name],
            [OccupancyByte]=[A].[length],
            [Length]=COLUMNPROPERTY(A.ID,A.name,'PRECISION'),
            [DecimalDigits]=isnull(COLUMNPROPERTY(A.ID,A.name,'Scale'),0),
            [IsAllowNull]=CASE WHEN [A].[IsNullAble]=1 THEN '1' ELSE '0' END,
            [DefaultValue]=ISNULL(E.text,''),
            [Description]=ISNULL(g.[value],'')
            FROM SYSCOLUMNS [A]
            LEFT JOIN systypes [B] ON [A].[XUSERTYPE]=[B].[XUSERTYPE]
            INNER JOIN SYSOBJECTS [D] ON [A].[ID]=[D].[ID]  AND [D].[XTYPE]='U' AND  [D].[Name]<>'dtproperties'
            LEFT JOIN syscomments [E] ON [A].[CDEFAULT]=[E].[ID]
            LEFT JOIN SYS.EXTENDED_PROPERTIES [G] ON [A].[ID]=[G].[MAJOR_ID] AND [A].[COLID]=[G].[MINOR_ID] 
            LEFT JOIN SYS.EXTENDED_PROPERTIES [F] ON [D].[ID]=[F].[MAJOR_ID] AND [F].[MINOR_ID]=0
            WHERE [D].[NAME]='@TableName'".Replace("@TableName", _TableName);
                #endregion 定义SQL

                string _CreateSQL = "";
                if (_IsReBuild)
                {
                    _CreateSQL = @"
IF EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE='U' AND NAME='@TableName')
BEGIN
  DROP TABLE @TableName;
END
CREATE TABLE [@TableName] (
";
                }
                else
                {
                    _CreateSQL = @"
IF NOT EXISTS(SELECT NAME FROM SYSOBJECTS WHERE XTYPE='U' AND NAME='@TableName')
BEGIN
CREATE TABLE [@TableName] (
";
                }
                _SubDT = _DbHelper.ExecuteDataTable(_DTSQL);
                foreach (DataRow _SDW in _SubDT.Rows)
                {
                    string _ISQL = "  [" + _SDW["Name"].ToString() + "] [" + _SDW["DataType"].ToString().ToUpper() + "] ";
                    if (_SDW["DataType"].ToString() == "varchar" || _SDW["DataType"].ToString() == "nvarchar")
                    {
                        _ISQL += "(" + _SDW["Length"].ToString() + ") ";
                    }
                    else if (_SDW["DataType"].ToString() == "decimal")
                    {
                        _ISQL += "(" + _SDW["Length"].ToString() + "," + _SDW["DecimalDigits"].ToString() + ") ";
                    }

                    if (_SDW["IsIdentity"].ToString() == "1")
                    {
                        _ISQL += " IDENTITY(1,1) ";
                    }

                    if (_SDW["IsPrimaryKey"].ToString() == "1")
                    {
                        _ISQL += " PRIMARY KEY ";
                    }

                    if (_SDW["IsAllowNull"].ToString() == "1")
                    {
                        _ISQL += " NULL,";
                    }
                    else
                    {
                        _ISQL += " NOT NULL,";
                    }
                    _CreateSQL += _ISQL + @"
";
                }
            }

            return reVal;
        }
        #endregion 建表的SQL

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        public int Insert(string TableName, Dictionary<string, string> SqlParms, string DBConnectionString = null)
        {
            int reVal = 0;
            if (DBConnectionString.IsNullOrEmpty())
            {
                DBConnectionString = Config.SqlServerConnectionString;
            }
            if (SqlParms != null && SqlParms.Count > 0)
            {
                string InsertSQL = "INSERT INTO " + TableName + "(";
                string InsertColumn = string.Empty;
                List<string> TableClounm = this.ColumnNameList(TableName);
                foreach (string ParmKey in SqlParms.Keys)
                {
                    if (TableClounm.Contains(ParmKey))
                    {
                        InsertColumn += "@" + ParmKey + " ,";
                    }
                }
                InsertColumn = InsertColumn.RemoveEndChar(",");
                InsertSQL += InsertColumn.Replace("@", string.Empty) + ") VALUES(" + InsertColumn + ")";
                SqlHelper DbHelper = new SqlHelper(DBConnectionString);
                reVal = DbHelper.ExecuteNonQuery(InsertSQL, SqlParms);
            }
            else
            {
                throw new System.Exception("错误的参数对象");
            }
            return reVal;
        }
        #endregion 新增

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="SqlParms">参数列表</param>
        public int Update(string TableName, Dictionary<string, string> SqlParms, string DBConnectionString = null)
        {
            int reVal = 0;
            if (DBConnectionString.IsNullOrEmpty())
            {
                DBConnectionString = Config.SqlServerConnectionString;
            }
            if (SqlParms != null && SqlParms.Count > 0)
            {
                string UpdateSQL = "UPDATE " + TableName + " SET ";
                string UpdateSTR = string.Empty;
                string[] KeyColumns = new string[] { "ID", "GUID" };
                string WhereSTR = string.Empty;
                foreach (string ParmKey in SqlParms.Keys)
                {
                    if (!ParmKey.In(KeyColumns))
                    {
                        UpdateSTR += ParmKey + "=@" + ParmKey + " , ";
                    }
                    else
                    {
                        WhereSTR += ParmKey + "=@" + ParmKey + " AND";
                    }
                }
                UpdateSTR = UpdateSTR.RemoveEndChar(",");
                WhereSTR = " WHERE " + WhereSTR.RemoveEndChar("AND");
                UpdateSQL += UpdateSTR + WhereSTR;
                SqlHelper DbHelper = new SqlHelper(DBConnectionString);
                reVal = DbHelper.ExecuteNonQuery(UpdateSQL, SqlParms);
            }
            else
            {
                throw new System.Exception("错误的参数对象");
            }
            return reVal;
        }
        #endregion 更新

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="SqlParms">参数列表</param>
        public int Delete(string TableName, Dictionary<string, string> SqlParms = null, string DBConnectionString = null)
        {
            int reVal = 0;
            if (DBConnectionString.IsNullOrEmpty())
            {
                DBConnectionString = Config.SqlServerConnectionString;
            }
            string DeleteSQL = "DELETE * FROM @TableName @WHERE;";
            string WhereSTR = string.Empty;
            if (SqlParms != null && SqlParms.Count > 0)
            {
                foreach (string ParmKey in SqlParms.Keys)
                {
                    WhereSTR += ParmKey + "=@" + ParmKey + " AND";
                }
                WhereSTR = WhereSTR.RemoveEndChar("AND");
                DeleteSQL = DeleteSQL.Replace("@TableName", TableName).Replace("@WHERE", WhereSTR);
                SqlHelper DbHelper = new SqlHelper(DBConnectionString);
                reVal = DbHelper.ExecuteNonQuery(DeleteSQL, SqlParms);
            }

            return reVal;
        }

        #endregion 删除
    }
    #endregion 超级SqlServer类
}