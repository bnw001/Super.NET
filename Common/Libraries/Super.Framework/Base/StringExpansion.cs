using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Super.Framework
{
    #region 字符串相关扩展类
    /// <summary>
    /// 字符串相关扩展类
    /// </summary>
    public static class StringExpansion
    {
        #region 变量

        #region 空格、回车、换行符、制表符正则表达式
        /// <summary>
        /// 空格、回车、换行符、制表符正则表达式
        /// </summary>
        private static Regex _TbbrRegex = new Regex(@"\s*|\t|\r|\n", RegexOptions.IgnoreCase);
        #endregion 空格、回车、换行符、制表符正则表达式

        #endregion 变量

        #region 是否SQLite
        /// <summary>
        /// 是否SQLite
        /// </summary>
        public static bool IsSQLite(this string _IString)
        {
            bool reVal = false;
            if (_IString.Trim().ToUpper() == "SQLITE")
            {
                reVal = true;
            }
            return reVal;
        }
        #endregion 是否SQLite

        #region 是否SqlServer
        /// <summary>
        /// 是否SqlServer
        /// </summary>
        public static bool IsSqlServer(this string _IString)
        {
            bool reVal = false;
            if (_IString.Trim().ToUpper() == "SQLSERVER" || _IString.Trim().ToUpper() == "MSSQL")
            {
                reVal = true;
            }
            return reVal;
        }
        #endregion 是否SqlServer

        #region 是否MySQL
        /// <summary>
        /// 是否MySQL
        /// </summary>
        public static bool IsMySQL(this string _IString)
        {
            bool reVal = false;
            if (_IString.Trim().ToUpper() == "MYSQL")
            {
                reVal = true;
            }
            return reVal;
        }
        #endregion 是否MySQL

        #region 是否Oracle
        /// <summary>
        /// 是否Oracle
        /// </summary>
        public static bool IsOracle(this string _IString)
        {
            bool reVal = false;
            if (_IString.Trim().ToUpper() == "ORACLE")
            {
                reVal = true;
            }
            return reVal;
        }
        #endregion 是否Oracle

        #region 是否数据库类型
        /// <summary>
        /// 是否数据库类型
        /// </summary>
        public static bool IsDBType(this string _IString)
        {
            bool reVal = false;
            reVal = (_IString.IsSQLite() || _IString.IsSqlServer() || _IString.IsMySQL() || _IString.IsOracle());

            return reVal;
        }
        #endregion 是否数据库类型

        #region 是否MD5字符串
        /// <summary>
        /// 是否MD5字符串
        /// </summary>
        public static bool IsMD5(this string _IString)
        {
            bool reVal = false;
            if (_IString.IsNotNullOrEmpty() && _IString.Trim().Trim().Length == 32)
            {
                reVal = true;
            }
            return reVal;
        }
        #endregion 是否MD5字符串

        #region 转标准的数据类型字符串
        /// <summary>
        /// 转标准的数据类型字符串
        /// </summary>
        public static string ToStandardDbType(this string _IString)
        {
            string reVal = _IString.Trim();
            if (_IString.IsSQLite())
            {
                reVal = "SQLite";
            }
            else if (_IString.IsSqlServer())
            {
                reVal = "SqlServer";
            }
            else if (_IString.IsMySQL())
            {
                reVal = "MySql";
            }
            else if (_IString.IsOracle())
            {
                reVal = "Oracle";
            }

            return reVal;
        }
        #endregion 转标准的数据类型字符串

        #region 从字符串中寻找指定属性的值
        /// <summary>
        /// 从字符串中寻找指定属性的值
        /// </summary>
        public static string GetVlaue(this string JsonString, string Key)
        {
            string reVal = string.Empty;
            JsonString = JsonString.RemoveEndChar("}");
            JsonString.RemoveStartChar("[").RemoveEndChar("]").RemoveStartChar("{").RemoveEndChar("}");
            string[] JsonStringArr = JsonString.SplitString(",");
            foreach (string KeyValue in JsonStringArr)
            {
                string[] IKeyVal = KeyValue.Split(',');
                if (IKeyVal.Length == 2)
                {
                    IKeyVal[0] = IKeyVal[0].TakeHat("\"");
                    if (IKeyVal[0] == Key)
                    {
                        IKeyVal[1] = IKeyVal[1].TakeHat("\"");
                        reVal = IKeyVal[1];
                        break;
                    }
                }
            }

            return reVal;
        }
        #endregion 从指定字符串中寻找指定属性的值

        #region 获取一个字符串是否在字符串数组中
        /// <summary>
        /// 获取一个字符串是否在字符串数组中
        /// </summary>
        /// <param name="StringArray">字符串数组</param>
        /// <param name="SourceString">字符串</param>
        /// <returns>是否存在于数组中</returns>
        public static bool In(this System.String SourceString, System.String[] StringArray)
        {
            foreach (System.String strTemp in StringArray)
            {
                if (SourceString.Trim().ToLower() == strTemp.Trim().ToLower())
                {
                    return true;
                }
            }

            return false;
        }
        #endregion 获取一个字符串是否在字符串数组中

        #region 获取一个字符串是否在一个字符串列表中
        /// <summary>
        /// 获取一个字符串是否在一个字符串列表中
        /// </summary>
        /// <param name="InputString">字符串</param>
        /// <param name="StringList">字符串列表</param>
        public static bool In(this System.String InputString, List<System.String> StringList)
        {
            bool reVal = false;

            foreach (System.String Item in StringList)
            {
                if (InputString.Trim().ToLower() == Item.Trim().ToLower())
                {
                    reVal = true;
                }
            }

            return reVal;
        }
        #endregion 获取一个字符串是否在一个字符串列表中

        #region 获取一个字符串是否在一个字符串列表中
        /// <summary>
        /// 获取一个字符串是否在一个字符串列表中
        /// </summary>
        /// <param name="InputString">字符串</param>
        /// <param name="StringList">字符串列表</param>
        public static bool In(this System.String InputString, List<System.String> StringList, out string StringItem)
        {
            bool reVal = false;
            StringItem = InputString.TBBRTrim();
            foreach (System.String Item in StringList)
            {
                if (InputString.Trim().ToLower() == Item.Trim().ToLower())
                {
                    StringItem = Item.TBBRTrim();
                    reVal = true;
                }
            }

            return reVal;
        }
        #endregion 获取一个字符串是否在一个字符串列表中

        #region 获取一个字符串是否在一个另一个字符串中（分隔符非字母和数字的固定字符)
        /// <summary>
        /// 获取一个字符串是否在一个另一个字符串中（分隔符非字母和数字的固定字符)
        /// </summary>
        /// <param name="InputString"></param>
        /// <param name="StringList"></param>
        /// <returns></returns>
        public static bool In(this System.String InputString, System.String StringList)
        {
            return StringList.TBBRTrim().ToLower().Contains(InputString.Trim().ToLower());
        }
        #endregion 获取一个字符串是否在一个另一个字符串中（分隔符非字母和数字的固定字符)

        #region 获取一个字符串是否为空
        /// <summary>
        /// 获取一个字符串是否为空
        /// </summary>
        /// <param name="SourceString">原始字符串</param>
        /// <returns>是否为空</returns>
        public static bool IsNullOrEmpty(this System.String SourceString)
        {
            if (SourceString != null)
            {
                return System.String.IsNullOrEmpty(SourceString.Trim());
            }
            else
            {
                return true;
            }
        }
        #endregion 获取一个字符串是否为空

        #region 获取一个字符串是否不为空
        /// <summary>
        /// 获取一个字符串是否不为空
        /// </summary>
        /// <param name="InputString">原始字符串</param>
        public static bool IsNotNullOrEmpty(this System.String InputString)
        {
            if (InputString != null)
            {
                return !System.String.IsNullOrEmpty(InputString.Trim());
            }
            else
            {
                return false;
            }
        }
        #endregion 获取一个字符串是否不为空

        #region 多个空格变一个空格
        /// <summary>
        /// 多个空格变一个空格
        /// </summary>
        public static string Multiple2OneSpace(this string InputString)
        {
            string reVal = InputString;
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);
            reVal = regex.Replace(reVal, " ");

            return reVal;
        }
        #endregion 多个空格变一个空格

        #region 多个连续字符变成一个字符
        /// <summary>
        /// 多个连续字符变成一个字符
        /// </summary>
        /// <param name="InputString">原始字符串</param>
        /// <param name="ReplaceString">要替换的字符</param>
        public static string More2One(this string InputString, string ReplaceString)
        {
            string reVal = InputString;
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[" + ReplaceString + "]{2,}", options);
            reVal = regex.Replace(reVal, ReplaceString);

            return reVal;
        }
        #endregion 多个连续字符变成一个字符

        #region 截取字符串
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="SourceString">原始字符串</param>
        /// <param name="StartIndex">开始索引</param>
        /// <param name="Length">长度</param>
        public static System.String SubString(this System.String SourceString, int StartIndex, int Length)
        {
            System.String reVal = System.String.Empty;
            if (StartIndex < 0)
            {
                StartIndex = 0;
            }
            if (SourceString.Length > StartIndex + Length)
            {
                reVal = SourceString.Substring(StartIndex, Length);
            }
            else if (SourceString.Length > StartIndex)
            {
                reVal = SourceString.Substring(StartIndex, SourceString.Length - StartIndex);
            }
            else
            {
                reVal = SourceString.SubString(0, SourceString.Length);
            }

            return reVal;
        }
        #endregion 截取字符串

        #region 补足最小长度
        /// <summary>
        /// 补足最小长度
        /// </summary>
        /// <param name="SourceString">原始字符串</param>
        /// <param name="SupplementChar">要填充的字符</param>
        /// <param name="Length">最小长度</param>
        public static System.String Supplement(this System.String SourceString, System.String SupplementChar, int Length)
        {
            System.String reVal = System.String.Empty;
            reVal = SourceString;
            if (SourceString.Length < Length)
            {
                for (int i = 0; i < SourceString.Length; i++)
                {
                    reVal += SupplementChar;
                }
            }
            return reVal;
        }
        #endregion 补足最小长度

        #region 脱帽(去除开头和结尾多余字符串)
        /// <summary>
        /// 脱帽(去除开头和结尾多余字符串)
        /// </summary>
        public static System.String TakeHat(this System.String SourceString, System.String ExcessChar)
        {
            SourceString = SourceString.TBBRTrim();
            ExcessChar = ExcessChar.TBBRTrim();
            System.String reVal = SourceString;

            if (SourceString.Length > 0)
            {
                if (SourceString.StartsWith(ExcessChar))
                {
                    reVal = SourceString.Remove(ExcessChar.Length, SourceString.Length - ExcessChar.Length);
                }
                if (SourceString.EndsWith(ExcessChar))
                {
                    reVal = SourceString.Remove(SourceString.Length - ExcessChar.Length, ExcessChar.Length);
                }
            }
            return reVal;
        }
        #endregion 脱帽(去除开头和结尾多余字符串)

        #region 去除开头多余字符串
        /// <summary>
        /// 去除开头多余字符串
        /// </summary>
        public static System.String RemoveStartChar(this System.String SourceString, System.String ExcessChar)
        {
            SourceString = SourceString.TBBRTrim();
            ExcessChar = ExcessChar.TBBRTrim();
            System.String reVal = SourceString;

            if (SourceString.Length > 0 && SourceString.StartsWith(ExcessChar))
            {
                reVal = SourceString.Remove(0, ExcessChar.Length);
            }

            return reVal;
        }
        #endregion 去除开头多余字符串

        #region 去除最后多余字符串
        /// <summary>
        /// 去除最后多余字符串
        /// </summary>
        public static System.String RemoveEndChar(this System.String SourceString, System.String ExcessChar)
        {
            SourceString = SourceString.TBBRTrim();
            ExcessChar = ExcessChar.TBBRTrim();
            System.String reVal = SourceString;

            if (SourceString.Length > 0 && SourceString.EndsWith(ExcessChar))
            {
                reVal = SourceString.Remove(SourceString.Length - ExcessChar.Length, ExcessChar.Length);
            }

            return reVal;
        }
        #endregion 去除最后多余字符串

        #region 逻辑类型转中文字符串输出
        /// <summary>
        /// 逻辑类型转中文字符串输出
        /// </summary>
        public static System.String BoolTrunChinese(this System.String BoolString)
        {
            bool isBool = bool.Parse(BoolString);
            if (isBool)
            {
                return "是";
            }
            else
            {
                return "否";
            }
        }
        #endregion 逻辑类型转中文字符串输出

        #region 字符串转成整型
        /// <summary>
        /// 字符串转成整型
        /// </summary>
        /// <param name="IntString">字符串</param>
        /// <returns>返回值：转型的整数，失败会转为预设值0</returns>
        public static int ToInt(this System.String IntString, int DefaultValue = 0)
        {
            int reVal = DefaultValue;
            int.TryParse(IntString, out reVal);
            return reVal;
        }
        #endregion 字符串转成整型

        #region 字符串转成长整型
        /// <summary>
        /// 字符串转成长整型
        /// </summary>
        /// <param name="LongIntString">字符串</param>
        /// <returns>返回值：转型的整数，失败会转为预设值0</returns>
        public static long ToLong(this System.String LongIntString, long DefaultValue = 0)
        {
            long reVal = DefaultValue;
            long.TryParse(LongIntString, out reVal);
            return reVal;
        }
        #endregion 字符串转成长整型

        #region 字符串转成逻辑类型
        /// <summary>
        /// 字符串转成逻辑类型（如果转型失败会返回逻辑假，使用过程中请注意）
        /// </summary>
        /// <param name="boolString">逻辑值字符串</param>
        /// <returns>返回值：转型完的逻辑值</returns>
        public static bool ToBool(this System.String boolString, bool DefalutValue = false)
        {
            bool reVal = DefalutValue;
            if (boolString == "1")
            {
                reVal = true;
            }
            else if (boolString == "0")
            {
                reVal = false;
            }
            else
            {
                bool.TryParse(boolString, out reVal);
            }
            return reVal;
        }
        #endregion 字符串转成逻辑类型

        #region 分割字符串
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="SourceString">源字符串</param>
        /// <param name="SplitString">分隔字符串</param>
        /// <returns>返回值：分隔完的字符串数组</returns>
        public static System.String[] SplitString(this System.String SourceString, System.String SplitString)
        {
            if (System.String.IsNullOrEmpty(SourceString) || System.String.IsNullOrEmpty(SplitString))
                return new System.String[0] { };

            if (SourceString.IndexOf(SplitString) == -1)
                return new System.String[] { SourceString };

            if (SplitString.Length == 1)
                return SourceString.Split(SplitString[0]);
            else
                return Regex.Split(SourceString, Regex.Escape(SplitString), RegexOptions.IgnoreCase);
        }
        #endregion 分割字符串

        #region 分割字符串
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="SourceString">源字符串</param>
        /// <returns>返回值：分隔完的字符串数组</returns>
        public static System.String[] SplitString(System.String SourceString)
        {
            return SplitString(SourceString, ",");
        }
        #endregion 分割字符串

        #region 合并字符
        /// <summary>
        /// 合并字符
        /// </summary>
        /// <param name="InputList">字符串列表</param>
        /// <param name="JoinString">连接符，预设为英文逗号</param>
        public static string Merge(this List<string> InputList, string JoinString = ",")
        {
            string reVal = string.Empty;
            JoinString = JoinString.TBBRTrim();
            foreach (string Item in InputList)
            {
                reVal += Item + JoinString;
            }
            reVal = reVal.RemoveEndChar(JoinString);

            return reVal;
        }
        #endregion 合并字符

        #region 合并字符
        /// <summary>
        /// 合并字符
        /// </summary>
        /// <param name="InputList">字符串列表</param>
        /// <param name="JoinString">连接符，预设为英文逗号</param>
        public static string Merge(this string[] InputList, string JoinString = ",")
        {
            string reVal = string.Empty;
            JoinString = JoinString.TBBRTrim();
            foreach (string Item in InputList)
            {
                reVal += Item + JoinString;
            }
            reVal = reVal.RemoveEndChar(JoinString);

            return reVal;
        }
        #endregion 合并字符

        #region 将字符串列表转大写
        /// <summary>
        /// 将字符串列表转大写
        /// </summary>
        public static List<string> ToUpper(this List<string> _IStringList)
        {
            List<string> reVal = new List<string>();
            foreach (string _FSLI in _IStringList)
            {
                reVal.Add(_FSLI.ToUpper());
            }
            return reVal;
        }
        #endregion 将字符串列表转大写

        #region 将字符串列表转小写
        /// <summary>
        /// 将字符串列表转小写
        /// </summary>
        public static List<string> ToLower(this List<string> _IStringList)
        {
            List<string> reVal = new List<string>();
            foreach (string _FSLI in _IStringList)
            {
                reVal.Add(_FSLI.ToLower());
            }
            return reVal;
        }
        #endregion 将字符串列表转小写

        #region 获得字符串在字符串数组中的位置
        /// <summary>
        /// 获得字符串在字符串数组中的位置
        /// </summary>
        public static int IndexInArray(this System.String SourceString, System.String[] StringArray, bool ignoreCase)
        {
            if (System.String.IsNullOrEmpty(SourceString) || StringArray == null || StringArray.Length == 0)
                return -1;

            int index = 0;
            System.String temp = null;

            if (ignoreCase)
                SourceString = SourceString.ToLower();

            foreach (System.String item in StringArray)
            {
                if (ignoreCase)
                    temp = item.ToLower();
                else
                    temp = item;

                if (SourceString == temp)
                    return index;
                else
                    index++;
            }

            return -1;
        }
        #endregion 获得字符串在字符串数组中的位置

        #region 获得字符串在字符串数组中的位置
        /// <summary>
        /// 获得字符串在字符串数组中的位置
        /// </summary>
        public static int IndexInArray(this System.String SourceString, System.String[] StringArray)
        {
            return SourceString.IndexInArray(StringArray, false);
        }
        #endregion 获得字符串在字符串数组中的位置

        #region 判断字符串是否在字符串数组中
        /// <summary>
        /// 判断字符串是否在字符串数组中
        /// </summary>
        public static bool IsInArray(this System.String SourceString, System.String[] StringArray, bool ignoreCase)
        {
            return SourceString.IndexInArray(StringArray, ignoreCase) > -1;
        }
        #endregion 判断字符串是否在字符串数组中

        #region 判断字符串是否在字符串数组中
        /// <summary>
        /// 判断字符串是否在字符串数组中
        /// </summary>
        public static bool IsInArray(this System.String SourceString, System.String[] array)
        {
            return IsInArray(SourceString, array, false);
        }
        #endregion 判断字符串是否在字符串数组中

        #region 判断字符串是否在字符串中
        /// <summary>
        /// 判断字符串是否在字符串中
        /// </summary>
        public static bool IsInArray(this System.String SourceString, System.String StringArray, System.String SplitStr, bool ignoreCase)
        {
            return IsInArray(SourceString, StringArray.SplitString(SplitStr), ignoreCase);
        }
        #endregion 判断字符串是否在字符串中

        #region 判断字符串是否在字符串中
        /// <summary>
        /// 判断字符串是否在字符串中
        /// </summary>
        public static bool IsInArray(this System.String SourceString, System.String StringArray, System.String SplitStr)
        {
            return SourceString.IsInArray(StringArray, SplitStr, false);
        }
        #endregion 判断字符串是否在字符串中

        #region 判断字符串是否在字符串中
        /// <summary>
        /// 判断字符串是否在字符串中
        /// </summary>
        public static bool IsInArray(this System.String SourceString, System.String StringArray)
        {
            return SourceString.IsInArray(StringArray, ",");
        }
        #endregion 判断字符串是否在字符串中

        #region 将对象数组拼接成字符串
        /// <summary>
        /// 将对象数组拼接成字符串
        /// </summary>
        public static System.String ToArrayString(this object[] ObjArray, System.String SplitStr)
        {
            if (ObjArray == null || ObjArray.Length == 0)
                return "";

            StringBuilder reVal = new StringBuilder();
            for (int i = 0; i < ObjArray.Length; i++)
                reVal.AppendFormat("{0}{1}", ObjArray[i], SplitStr);

            return reVal.Remove(reVal.Length - SplitStr.Length, SplitStr.Length).ToString();
        }
        #endregion 将对象数组拼接成字符串

        #region 将对象数组拼接成字符串
        /// <summary>
        /// 将对象数组拼接成字符串
        /// </summary>
        public static System.String ToArrayString(this object[] ObjArray)
        {
            return ToArrayString(ObjArray, ",");
        }
        #endregion 将对象数组拼接成字符串

        #region 将字符串数组拼接成字符串
        /// <summary>
        /// 将字符串数组拼接成字符串
        /// </summary>
        public static System.String ToArrayString(this System.String[] StringArray, System.String SplitStr)
        {
            return StringArray.ToArrayString(SplitStr);
        }
        #endregion 将字符串数组拼接成字符串

        #region 将字符串数组拼接成字符串
        /// <summary>
        /// 将字符串数组拼接成字符串
        /// </summary>
        public static System.String ToArrayString(this System.String[] StringArray)
        {
            return StringArray.ToArrayString(",");
        }
        #endregion 将字符串数组拼接成字符串

        #region 将字符串数组转成列表
        /// <summary>
        /// 将字符串数组转成列表
        /// </summary>
        /// <param name="InputStringArray">输入字符串</param>
        public static List<string> ToList(this System.String[] InputStringArray)
        {
            List<string> reVal = new List<string>();
            foreach (string IString in InputStringArray)
            {
                reVal.Add(IString);
            }
            return reVal;
        }
        #endregion 将字符串数组转成列表

        #region 将整数数组拼接成字符串
        /// <summary>
        /// 将整数数组拼接成字符串
        /// </summary>
        public static System.String ToArrayString(this int[] IntArray, System.String SplitStr)
        {
            if (IntArray == null || IntArray.Length == 0)
                return "";

            StringBuilder reVal = new StringBuilder();
            for (int i = 0; i < IntArray.Length; i++)
                reVal.AppendFormat("{0}{1}", IntArray[i], SplitStr);

            return reVal.Remove(reVal.Length - SplitStr.Length, SplitStr.Length).ToString();
        }
        #endregion 将整数数组拼接成字符串

        #region 将整数数组拼接成字符串
        /// <summary>
        /// 将整数数组拼接成字符串
        /// </summary>
        public static System.String ToArrayString(this int[] IntArray)
        {
            return ToArrayString(IntArray, ",");
        }
        #endregion 将整数数组拼接成字符串

        #region 转Decimal

        #region 将string类型转换成decimal类型
        /// <summary>
        /// 将string类型转换成decimal类型
        /// </summary>
        /// <param name="InputString">目标字符串</param>
        /// <param name="DefaultValue">预设值</param>
        public static decimal ToDecimal(this string InputString, decimal DefaultValue)
        {
            if (!string.IsNullOrWhiteSpace(InputString))
            {
                decimal result;
                if (decimal.TryParse(InputString, out result))
                    return result;
            }

            return DefaultValue;
        }
        #endregion 将string类型转换成decimal类型

        #region 将string类型转换成decimal类型
        /// <summary>
        /// 将string类型转换成decimal类型
        /// </summary>
        /// <param name="InputString">目标字符串</param>
        public static decimal ToDecimal(this string InputString)
        {
            return ToDecimal(InputString, 0m);
        }
        #endregion 将string类型转换成decimal类型

        #endregion 转Decimal

        #region 移除数组中的指定项
        /// <summary>
        /// 移除数组中的指定项
        /// </summary>
        /// <param name="array">源数组</param>
        /// <param name="RemoveItem">要移除的项</param>
        /// <param name="removeBackspace">是否移除空格</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static System.String[] RemoveArrayItem(this System.String[] StringArray, System.String RemoveItem, bool RemoveBackspace, bool ignoreCase)
        {
            if (StringArray != null && StringArray.Length > 0)
            {
                StringBuilder ArrayStr = new StringBuilder();
                if (ignoreCase)
                    RemoveItem = RemoveItem.ToLower();
                System.String temp = "";
                foreach (System.String item in StringArray)
                {
                    if (ignoreCase)
                        temp = item.ToLower();
                    else
                        temp = item;

                    if (temp != RemoveItem)
                        ArrayStr.AppendFormat("{0}_", RemoveBackspace ? item.Trim() : item);
                }

                return ArrayStr.Remove(ArrayStr.Length - 1, 1).ToString().SplitString("_");
            }

            return StringArray;
        }
        #endregion 移除数组中的指定项

        #region 移除数组中的指定项
        /// <summary>
        /// 移除数组中的指定项
        /// </summary>
        /// <param name="array">源数组</param>
        /// <returns></returns>
        public static System.String[] RemoveArrayItem(this System.String[] StringArray)
        {
            return StringArray.RemoveArrayItem("", true, false);
        }
        #endregion 移除数组中的指定项

        #region 去除字符串首尾处的空格、回车、换行符、制表符
        /// <summary>
        /// 去除字符串首尾处的空格、回车、换行符、制表符
        /// </summary>
        public static System.String TBBRTrim(this System.String SourceString)
        {
            if (!System.String.IsNullOrEmpty(SourceString))
                return SourceString.Trim().Trim('\r').Trim('\n').Trim('\t');
            return System.String.Empty;
        }
        #endregion 去除字符串首尾处的空格、回车、换行符、制表符

        #region 去除字符串中的空格、回车、换行符、制表符
        /// <summary>
        /// 去除字符串中的空格、回车、换行符、制表符
        /// </summary>
        public static System.String ClearTBBR(this System.String SourceString)
        {
            if (!System.String.IsNullOrEmpty(SourceString))
                return _TbbrRegex.Replace(SourceString, "");

            return System.String.Empty;
        }
        #endregion 去除字符串中的空格、回车、换行符、制表符

        #region 删除字符串中的空行
        /// <summary>
        /// 删除字符串中的空行
        /// </summary>
        public static System.String DeleteNullOrSpaceRow(this System.String SourceString)
        {
            if (System.String.IsNullOrEmpty(SourceString))
                return "";

            System.String[] TempArray = SourceString.SplitString("\r\n");
            StringBuilder result = new StringBuilder();
            foreach (System.String item in TempArray)
            {
                if (!System.String.IsNullOrWhiteSpace(item))
                    result.AppendFormat("{0}\r\n", item);
            }
            if (result.Length > 0)
                result.Remove(result.Length - 2, 2);
            return result.ToString();
        }
        #endregion 删除字符串中的空行

        #region 获得邮箱提供者
        /// <summary>
        ///获得邮箱提供者
        /// </summary>
        /// <param name="Email">邮箱</param>
        public static System.String GetEmailProvider(this System.String Email)
        {
            int index = Email.LastIndexOf('@');
            if (index > 0)
                return Email.Substring(index + 1);
            return System.String.Empty;
        }
        #endregion 获得邮箱提供者

        #region 转义正则表达式
        /// <summary>
        /// 转义正则表达式
        /// </summary>
        public static System.String EscapeRegex(this System.String SourceString)
        {
            System.String[] oList = { "\\", ".", "+", "*", "?", "{", "}", "[", "^", "]", "$", "(", ")", "=", "!", "<", ">", "|", ":" };
            System.String[] eList = { "\\\\", "\\.", "\\+", "\\*", "\\?", "\\{", "\\}", "\\[", "\\^", "\\]", "\\$", "\\(", "\\)", "\\=", "\\!", "\\<", "\\>", "\\|", "\\:" };
            for (int i = 0; i < oList.Length; i++)
                SourceString = SourceString.Replace(oList[i], eList[i]);
            return SourceString;
        }
        #endregion 转义正则表达式

        #region 将IP地址转换成Int64类型
        /// <summary>
        /// 将IP地址转换成Int64类型
        /// </summary>
        /// <param name="IP">IP地址</param>
        public static Int64 ConvertIPToInt64(this System.String IP)
        {
            return Convert.ToInt64(IP.Replace(".", System.String.Empty));
        }
        #endregion 将IP地址转换成Int64类型

        #region 隐藏邮箱
        /// <summary>
        /// 隐藏邮箱
        /// </summary>
        public static System.String HideEmail(this System.String Email)
        {
            int index = Email.LastIndexOf('@');

            if (index == 1)
                return "*" + Email.Substring(index);
            if (index == 2)
                return Email[0] + "*" + Email.Substring(index);

            StringBuilder sb = new StringBuilder();
            sb.Append(Email.Substring(0, 2));
            int count = index - 2;
            while (count > 0)
            {
                sb.Append("*");
                count--;
            }
            sb.Append(Email.Substring(index));
            return sb.ToString();
        }
        #endregion 隐藏邮箱

        #region 隐藏手机
        /// <summary>
        /// 隐藏手机
        /// </summary>
        public static System.String HideMobile(this System.String Mobile)
        {
            return Mobile.Substring(0, 3) + "*****" + Mobile.Substring(8);
        }
        #endregion 隐藏手机

        #region 将String类型转换成DateTime类型
        /// <summary>
        /// 将String类型转换成DateTime类型
        /// </summary>
        /// <param name="TimeString">目标字符串</param>
        /// <param name="DefaultValue">预设值</param>
        public static System.DateTime ToDateTime(this string TimeString, DateTime DefaultValue)
        {
            if (!string.IsNullOrWhiteSpace(TimeString))
            {
                DateTime result;
                if (System.DateTime.TryParse(TimeString, out result))
                    return result;
            }
            return DefaultValue;
        }
        #endregion 将String类型转换成DateTime类型

        #region 将String类型转换成DateTime类型
        /// <summary>
        /// 将String类型转换成DateTime类型
        /// </summary>
        /// <param name="TimeString">目标字符串</param>
        /// <returns></returns>
        public static System.DateTime ToDateTime(this string TimeString)
        {
            return TimeString.ToDateTime(System.DateTime.Now);
        }
        #endregion 将String类型转换成DateTime类型
    }
    #endregion 字符串相关扩展类
}