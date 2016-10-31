using System;
using System.Text.RegularExpressions;

namespace Super.Framework
{
    #region 验证辅助类
    /// <summary>
    /// 验证辅助类
    /// </summary>
    public static class ValidateHelper
    {
        #region 正则表达式定义
        //邮件正则表达式
        private static Regex EmailRegex = new Regex(@"^[a-z]([a-z0-9]*[-_]?[a-z0-9]+)*@([a-z0-9]*[-_]?[a-z0-9]+)+[\.][a-z]{2,3}([\.][a-z]{2})?$", RegexOptions.IgnoreCase);
        //手机号正则表达式
        private static Regex MobileRegex = new Regex("^(13|15|18)[0-9]{9}$");
        //固话号正则表达式
        private static Regex PhoneRegex = new Regex(@"^(\d{3,4}-?)?\d{7,8}$");
        //IP正则表达式
        private static Regex IPRegex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
        //日期正则表达式
        private static Regex DateRegex = new Regex(@"(\d{4})-(\d{1,2})-(\d{1,2})");
        //数值(包括整数和小数)正则表达式
        private static Regex NumericRegex = new Regex(@"^[-]?[0-9]+(\.[0-9]+)?$");
        //数值（小于1的小数)
        private static Regex PercentNumberRegex = new Regex(@"^0(\.[0-9]+)?$");
        //整数
        private static Regex IntRegex = new Regex(@"^[-]?([0-9]{1,})$");
        //邮政编码正则表达式
        private static Regex ZIPcodeRegex = new Regex(@"^\d{6}$");
        #endregion 正则表达式定义

        #region 是否为邮箱名
        /// <summary>
        /// 是否为邮箱名
        /// </summary>
        public static bool IsEmail(this string EmailString)
        {
            if (string.IsNullOrEmpty(EmailString))
                return false;
            return EmailRegex.IsMatch(EmailString);
        }
        #endregion 是否为邮箱名

        #region 是否为手机号
        /// <summary>
        /// 是否为手机号
        /// </summary>
        public static bool IsMobile(this string MobileString)
        {
            if (string.IsNullOrEmpty(MobileString))
                return false;
            return MobileRegex.IsMatch(MobileString);
        }
        #endregion 是否为手机号

        #region 是否为固话号
        /// <summary>
        /// 是否为固话号
        /// </summary>
        public static bool IsPhone(this string PhoneString)
        {
            if (string.IsNullOrEmpty(PhoneString))
                return false;
            return PhoneRegex.IsMatch(PhoneString);
        }
        #endregion 是否为固话号

        #region 是否为IP
        /// <summary>
        /// 是否为IP
        /// </summary>
        public static bool IsIP(this string IPString)
        {
            return IPRegex.IsMatch(IPString);
        }
        #endregion 是否为IP

        #region 是否是身份证号
        /// <summary>
        /// 是否是身份证号
        /// </summary>
        public static bool IsIdCard(this string ID)
        {
            if (string.IsNullOrEmpty(ID))
                return false;
            if (ID.Length == 18)
                return CheckIDCard18(ID);
            else if (ID.Length == 15)
                return CheckIDCard15(ID);
            else
                return false;
        }
        #endregion 是否是身份证号

        #region 是否为18位身份证号
        /// <summary>
        /// 是否为18位身份证号
        /// </summary>
        private static bool CheckIDCard18(this string ID)
        {
            long n = 0;
            if (long.TryParse(ID.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(ID.Replace('x', '0').Replace('X', '0'), out n) == false)
                return false;//数字验证

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(ID.Remove(2)) == -1)
                return false;//省份验证

            string birth = ID.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
                return false;//生日验证

            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = ID.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());

            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != ID.Substring(17, 1).ToLower())
                return false;//校验码验证

            return true;//符合GB11643-1999标准
        }
        #endregion 是否为18位身份证号

        #region 是否为15位身份证号
        /// <summary>
        /// 是否为15位身份证号
        /// </summary>
        private static bool CheckIDCard15(this string ID)
        {
            long n = 0;
            if (long.TryParse(ID, out n) == false || n < Math.Pow(10, 14))
                return false;//数字验证

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(ID.Remove(2)) == -1)
                return false;//省份验证

            string birth = ID.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
                return false;//生日验证

            return true;//符合15位身份证标准
        }
        #endregion 是否为15位身份证号

        #region 是否为日期
        /// <summary>
        /// 是否为日期
        /// </summary>
        public static bool IsDate(this string DateString)
        {
            return DateRegex.IsMatch(DateString);
        }
        #endregion 是否为日期

        #region 是否是整数
        public static bool IsInt(this string IntString)
        {
            return IntRegex.IsMatch(IntString);
        }
        #endregion 是否是整数

        #region 是否是数值(包括整数和小数)
        /// <summary>
        /// 是否是数值(包括整数和小数)
        /// </summary>
        public static bool IsNumeric(this string numericStr)
        {
            return NumericRegex.IsMatch(numericStr);
        }
        #endregion 是否是数值(包括整数和小数)

        #region 是否是百分比数
        /// <summary>
        /// 是否是百分比数
        /// </summary>
        /// <param name="PercentNumberString">百分比字符串</param>
        public static bool IsPercentNumber(this string PercentNumberString)
        {
            return PercentNumberRegex.IsMatch(PercentNumberString);
        }
        #endregion 是否是百分比数

        #region 是否为邮政编码
        /// <summary>
        /// 是否为邮政编码
        /// </summary>
        public static bool IsZIPCode(this string ZIPCode)
        {
            if (string.IsNullOrEmpty(ZIPCode))
                return false;
            return ZIPcodeRegex.IsMatch(ZIPCode);
        }
        #endregion 是否为邮政编码

        #region 是否是图片文件名
        /// <summary>
        /// 是否是图片文件名
        /// </summary>
        /// <returns> </returns>
        public static bool IsImgFileName(this string FileName)
        {
            if (FileName.IndexOf(".") == -1)
                return false;

            string tempFileName = FileName.Trim().ToLower();
            string extension = tempFileName.Substring(tempFileName.LastIndexOf("."));
            return extension == ".png" || extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif";
        }
        #endregion 是否是图片文件名

        #region 判断一个IP是否在另一个IP内
        /// <summary>
        /// 判断一个IP是否在另一个IP内
        /// </summary>
        /// <param name="SourceIP">检测IP</param>
        /// <param name="TargetIP">匹配IP</param>
        public static bool InIP(this string SourceIP, string TargetIP)
        {
            if (string.IsNullOrEmpty(SourceIP) || string.IsNullOrEmpty(TargetIP))
                return false;

            string[] SourceIPBlockList = SourceIP.SplitString(".");
            string[] TargetIPBlockList = TargetIP.SplitString(".");

            int sourceIPBlockListLength = SourceIPBlockList.Length;

            for (int i = 0; i < sourceIPBlockListLength; i++)
            {
                if (TargetIPBlockList[i] == "*")
                    return true;

                if (SourceIPBlockList[i] != TargetIPBlockList[i])
                {
                    return false;
                }
                else
                {
                    if (i == 3)
                        return true;
                }
            }
            return false;
        }
        #endregion 判断一个IP是否在另一个IP内

        #region 判断一个IP是否在另一个IP内
        /// <summary>
        /// 判断一个IP是否在另一个IP内
        /// </summary>
        /// <param name="SourceIP">检测IP</param>
        /// <param name="TargetIPList">匹配IP列表</param>
        public static bool InIPList(this string SourceIP, string[] TargetIPList)
        {
            if (TargetIPList != null && TargetIPList.Length > 0)
            {
                foreach (string targetIP in TargetIPList)
                {
                    if (InIP(SourceIP, targetIP))
                        return true;
                }
            }
            return false;
        }
        #endregion 判断一个IP是否在另一个IP内

        #region 判断一个IP是否在另一个IP内
        /// <summary>
        /// 判断一个IP是否在另一个IP内
        /// </summary>
        /// <param name="SourceIP">检测IP</param>
        /// <param name="TargetIP">匹配IP</param>
        /// <returns></returns>
        public static bool InIPList(this string SourceIP, string TargetIP)
        {
            string[] TargetIPList = TargetIP.SplitString("\n");
            return InIPList(SourceIP, TargetIPList);
        }
        #endregion 判断一个IP是否在另一个IP内

        #region 判断当前时间是否在指定的时间段内
        /// <summary>
        /// 判断当前时间是否在指定的时间段内
        /// </summary>
        /// <param name="PeriodList">指定时间段</param>
        /// <param name="LiePeriod">所处时间段</param>
        public static bool BetweenPeriod(this string[] PeriodList, out string LiePeriod)
        {
            if (PeriodList != null && PeriodList.Length > 0)
            {
                DateTime StartTime;
                DateTime EndTime;
                DateTime NowTime = DateTime.Now;
                DateTime NowDate = NowTime.Date;

                foreach (string Period in PeriodList)
                {
                    int index = Period.IndexOf("-");
                    StartTime = Period.Substring(0, index).ToDateTime();
                    EndTime = Period.Substring(index + 1).ToDateTime();

                    if (StartTime < EndTime)
                    {
                        if (NowTime > StartTime && NowTime < EndTime)
                        {
                            LiePeriod = Period;
                            return true;
                        }
                    }
                    else
                    {
                        if ((NowTime > StartTime && NowTime < NowDate.AddDays(1)) || (NowTime < EndTime))
                        {
                            LiePeriod = Period;
                            return true;
                        }
                    }
                }
            }
            LiePeriod = string.Empty;
            return false;
        }
        #endregion 判断当前时间是否在指定的时间段内

        #region 判断当前时间是否在指定的时间段内
        /// <summary>
        /// 判断当前时间是否在指定的时间段内
        /// </summary>
        /// <param name="PeriodStr">指定时间段</param>
        /// <param name="LiePeriod">所处时间段</param>
        /// <returns></returns>
        public static bool BetweenPeriod(this string PeriodStr, out string LiePeriod)
        {
            string[] periodList = PeriodStr.SplitString("\n");
            return BetweenPeriod(periodList, out LiePeriod);
        }
        #endregion 判断当前时间是否在指定的时间段内

        #region 判断当前时间是否在指定的时间段内
        /// <summary>
        /// 判断当前时间是否在指定的时间段内
        /// </summary>
        /// <param name="PeriodList">指定时间段</param>
        /// <returns></returns>
        public static bool BetweenPeriod(this string PeriodList)
        {
            string liePeriod = string.Empty;
            return BetweenPeriod(PeriodList, out liePeriod);
        }
        #endregion 判断当前时间是否在指定的时间段内

        #region 是否是数值(包括整数和小数)
        /// <summary>
        /// 是否是数值(包括整数和小数)
        /// </summary>
        public static bool IsNumericArray(this string[] NumericStrList)
        {
            if (NumericStrList != null && NumericStrList.Length > 0)
            {
                foreach (string numberStr in NumericStrList)
                {
                    if (!IsNumeric(numberStr))
                        return false;
                }
                return true;
            }
            return false;
        }
        #endregion 是否是数值(包括整数和小数)

        #region 是否是数值(包括整数和小数)
        /// <summary>
        /// 是否是数值(包括整数和小数)
        /// </summary>
        public static bool IsNumericRule(this string NumericRuleStr, string SplitChar)
        {
            return IsNumericArray(NumericRuleStr.SplitString(SplitChar));
        }
        #endregion 是否是数值(包括整数和小数)

        #region 是否是数值(包括整数和小数)
        /// <summary>
        /// 是否是数值(包括整数和小数)
        /// </summary>
        public static bool IsNumericRule(this string NumericRuleStr)
        {
            return IsNumericRule(NumericRuleStr, ",");
        }
        #endregion 是否是数值(包括整数和小数)
    }
    #endregion 验证辅助类
}