using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace Super.Framework
{
    #region 超级SQLite类
    /// <summary>
    /// 超级SQLite类
    /// </summary>
    public class SuperSQLite
    {
        #region 系统数据表名列表
        /// <summary>
        /// 系统数据表名列表
        /// </summary>
        public List<string> GetSystemDT(SQLiteHelper DbHelper = null)
        {
            List<string> reVal = new List<string>();
            string SelectSQL = @"SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;";
            if (DbHelper == null)
            {
                DbHelper = new SQLiteHelper(Config.SQLiteConnectionString);
            }
            DataTable DT = DbHelper.ExecuteDataTable(SelectSQL);
            string[] KeyTable = new string[] { "sqlite_stat1", "sqlite_sequence" };
            foreach (DataRow dw in DT.Rows)
            {
                if (!dw[0].ToString().In(KeyTable))
                {
                    reVal.Add(dw[0].ToString());
                }
            }
            return reVal;
        }

        /// <summary>
        /// 系统数据表名列表
        /// </summary>
        public List<string> GetSystemDT(string DBConnectionString)
        {
            if (!DBConnectionString.IsNullOrEmpty())
            {
                SQLiteHelper DBHelper = new SQLiteHelper(DBConnectionString);
                return this.GetSystemDT(DBHelper);
            }
            else
            {
                return this.GetSystemDT();
            }
        }
        #endregion 系统数据表名列表

        #region 表的栏位名称列表
        /// <summary>
        /// 表的栏位名称列表
        /// </summary>
        /// <param name="TableName">表名</param>
        public List<string> ColumnNameList(string TableName, SQLiteHelper DbHelper = null)
        {
            List<string> reVal = new List<string>();
            string SelectSQL = @"SELECT * FROM " + TableName.Replace(" ", "").Replace("<", "").Replace(">", "");
            if (DbHelper == null)
            {
                DbHelper = new SQLiteHelper(Config.SQLiteConnectionString);
            }
            DataTable DT = DbHelper.ExecuteDataTable(SelectSQL);
            foreach (DataColumn dc in DT.Columns)
            {
                reVal.Add(dc.ColumnName);
            }

            return reVal;
        }

        public List<string> ColumnNameList(string TableName, string DBConnectionString)
        {
            if (!DBConnectionString.IsNullOrEmpty())
            {
                SQLiteHelper DBHelper = new SQLiteHelper(DBConnectionString);
                return this.ColumnNameList(TableName, DBHelper);
            }
            else
            {
                return this.ColumnNameList(TableName);
            }
        }
        #endregion 表的栏位名称列表

        #region 表的栏位列表
        /// <summary>
        /// 表的栏位列表
        /// </summary>
        /// <param name="TableName">表名</param>
        public List<DataColumnModel> ColumnList(string TableName, SQLiteHelper DbHelper = null)
        {
            List<DataColumnModel> reVal = new List<DataColumnModel>();
            string SelectSQL = @"PRAGMA table_info([@TableName]) ".Replace("@TableName", TableName);
            if (DbHelper == null)
            {
                DbHelper = new SQLiteHelper(Config.SQLiteConnectionString);
            }
            DataTable SystemDT = DbHelper.ExecuteDataTable(SelectSQL);
            DataColumnModel DataColumnM = null;
            foreach (DataRow dw in SystemDT.Rows)
            {
                int length = 0;
                string DataType = "";
                Regex Rx = new Regex(@"^\w+", RegexOptions.IgnoreCase);
                var MC = Rx.Matches(dw["type"].ToString());
                DataType = MC[0].Value;
                Rx = new Regex(@"(\d+)");
                if (Rx.IsMatch(dw["type"].ToString()))
                {
                    MC = Rx.Matches(dw["type"].ToString());
                    length = MC[0].Value.ToInt();
                }
                bool IsIdentity = false;
                if (dw["pk"].ToString() == "1" && DataType == "INTEGER")
                {
                    IsIdentity = true;
                }
                DataColumnM = new DataColumnModel()
                {
                    Name = dw["name"].ToString(),
                    DataType = DataType,
                    Length = length,
                    IsAllowNull = !dw["notnull"].ToString().ToBool(),
                    DisplayName = string.Empty,
                    IsIdentity = IsIdentity,
                    IsPrimaryKey = dw["pk"].ToString().ToBool(),
                    DecimalDigits = 0,
                    DefaultValue = dw["dflt_value"].ToString()
                };

                if (!reVal.Contains(DataColumnM))
                {
                    reVal.Add(DataColumnM);
                }
            }

            return reVal;
        }

        public List<DataColumnModel> ColumnList(string TableName, string DBConnectionString)
        {
            if (!DBConnectionString.IsNullOrEmpty())
            {
                SQLiteHelper DBHelper = new SQLiteHelper(DBConnectionString);
                return this.ColumnList(TableName, DBHelper);
            }
            else
            {
                return this.ColumnList(TableName);
            }
        }
        #endregion 表的栏位列表

        #region 数据库模型转SQL脚本
        /// <summary>
        /// 数据库模型转SQL脚本
        /// </summary>
        /// <param name="DbModel">数据库模型对象</param>
        /// <returns>返回值：转SQL脚本</returns>
        public string TrunSqlScript(DataBaseModel DbModel)
        {
            string reVal = string.Empty;
            StringBuilder SqlScript = new StringBuilder();

            #region 脚本版权声明
            string ScriptCopyRight = @"/*
SuperSQLite SQL Script

脚本类型 : SQLite
日期: $DateTime
*/".Replace("$DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            #endregion 脚本版权声明

            #region 创建表的SQL
            string CreateTableScript = string.Empty;
            #endregion 创建表的SQL

            #region 创建表的脚本模板
            string CreateDataTableTemplate = @"/*@TableNote*/
CREATE TABLE ""@TableName"" (
@ColumnCreateScript);";
            #endregion 创建表的脚本模板

            #region 产生SQL脚本
            if (DbModel.DTMList.Count > 0)
            {
                int DTMListCount = DbModel.DTMList.Count;
                int DtCount = 1;
                foreach (DataTableModel DtModel in DbModel.DTMList)
                {
                    string CreateColumnScript = string.Empty;
                    int DCMListCount = DtModel.ColumnList.Count;
                    int DcCount = 1;
                    foreach (DataColumnModel DcModel in DtModel.ColumnList)
                    {
                        if (!string.IsNullOrEmpty(DcModel.Name))
                        {
                            if (!DcModel.Name.In(SystemKey.SqlKey))
                            {
                                CreateColumnScript += @"""" + DcModel.Name.Trim() + @""" " + ConfigDbType2Type(DcModel.DataType);
                                if (DcModel.DataType == "nvarchar" && DcModel.Length > 0)
                                {
                                    CreateColumnScript += "(" + DcModel.Length + ") ";
                                }
                                if (DcModel.IsPrimaryKey)
                                {
                                    CreateColumnScript += " PRIMARY KEY";
                                }
                                if (DcModel.DataType == "number" && DcModel.IsIdentity)
                                {
                                    CreateColumnScript += " AUTOINCREMENT";
                                }
                                if (!DcModel.IsAllowNull)
                                {
                                    CreateColumnScript += " NOT NULL";
                                }
                                if (!DcModel.DefaultValue.IsNullOrEmpty())
                                {
                                    CreateColumnScript += " DEFAULT " + ConfigDefault2SqliteDefault(DcModel.DefaultValue);
                                }
                                if (DcCount < DCMListCount)
                                {
                                    CreateColumnScript += ",";
                                }
                                if (!DcModel.DisplayName.IsNullOrEmpty())
                                {
                                    CreateColumnScript += "  /*" + DcModel.DisplayName + "*/";
                                }
                                CreateColumnScript += Environment.NewLine;
                                DcCount++;
                            }
                            else
                            {
                                Exception ex = new Exception("模型对象的文件出错，栏位名称不能为系统关键字");
                                ex.Data.Add("Name", DcModel.Name.Trim());
                                throw ex;
                            }
                        }
                        else
                        {
                            throw new Exception("模型对象的文件出错，栏位名称不能为空");
                        }
                    }
                    CreateTableScript += CreateDataTableTemplate.Replace("@TableNote", DtModel.DisplayName).Replace("@TableName", DtModel.Name).Replace("@ColumnCreateScript", CreateColumnScript);
                    if (DtCount < DTMListCount)
                    {
                        CreateTableScript += Environment.NewLine + Environment.NewLine;
                    }
                    DtCount++;
                }
            }
            reVal = ScriptCopyRight + Environment.NewLine + Environment.NewLine + CreateTableScript;
            #endregion 产生SQL脚本

            return reVal;
        }
        #endregion 数据库模型转SQL脚本

        #region 类型转换

        #region 配置类型转换为数据库类型
        /// <summary>
        /// 配置类型转换为数据库类型
        /// </summary>
        /// <param name="configType">配置文件的数据类型</param>
        /// <returns>返回值：SQLite数据类型</returns>
        private static string ConfigDbType2Type(string configType)
        {
            string reVal = "nvarchar";
            switch (configType)
            {
                case "number":
                    reVal = "INTEGER";
                    break;
                case "GUID":
                    reVal = "nvarchar";
                    break;
                default:
                    reVal = "nvarchar";
                    break;
            }
            return reVal;
        }
        #endregion 配置类型转换为数据库类型

        #endregion 类型转换

        #region 预设值转换
        /// <summary>
        /// 预设值转换
        /// </summary>
        /// <param name="ConfigDefault">配置预设值</param>
        /// <returns>返回值：预设值</returns>
        public static string ConfigDefault2SqliteDefault(string ConfigDefault)
        {
            string reVal = string.Empty;
            if (ConfigDefault.ToLower().Trim() == "Now()".ToLower())
            {
                reVal = "(datetime('now','localtime'))";
            }
            else if (ConfigDefault.ToLower().Trim() == "NewGUID()".ToLower())
            {
                reVal = @"''";
            }
            else if (ConfigDefault.ToLower().Trim() == "EmptyGUID()".ToLower())
            {
                reVal = "'00000000-0000-0000-0000-000000000000'";
            }
            else
            {
                if (ConfigDefault.StartsWith("'") && ConfigDefault.EndsWith("'"))
                {
                    reVal += "'" + ConfigDefault + "'";
                }
                else
                {
                    reVal = ConfigDefault;
                }
            }
            return reVal;
        }
        #endregion 预设值转换
    }
    #endregion 超级SQLite类
}