using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;

namespace Super.Framework
{
    #region 安全相关的类集合
    ///安全相关的类集合
    public static class Secret
    {
        #region DES加密钥匙
        private static string EncryptString = "Yga67^4~";
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        #endregion DES加密钥匙

        #region 防SQL注入正则表达式
        private static Regex SQLKeyWordRegex = new Regex(@"(select|insert|delete|from|count\(|drop|table|update|truncate|asc\(|mid\(|char\(|xp_cmdshell|exec|master|net|local|group|administrators|user|or|and|-|;|,|\(|\)|\[|\]|\{|\}|%|\*|!|\')", RegexOptions.IgnoreCase);
        #endregion 防SQL注入正则表达式

        #region 散列加密
        /// <summary>
        /// 散列加密
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        public static string EncryptHash(this string sourceString)
        {
            string reVal = string.Empty;
            reVal = MD5N(sourceString, 183);

            return reVal;
        }
        #endregion 散列加密

        #region MD5加密函数
        /// <summary>
        /// I型加密函数
        /// </summary>
        /// <param name="sourceString">要加密的源字符串</param>
        public static string MD5(string sourceString)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(sourceString, "MD5").ToUpper();
        }
        #endregion MD5加密函数

        #region MD5加密N次
        /// <summary>
        /// MD5加密N次
        /// </summary>
        /// <param name="sourceString">要加密的源字符串</param>
        /// <param name="times">重复加密次数</param>
        public static string MD5N(string sourceString, int times)
        {
            string reVal = MD5(sourceString);
            for (int i = 0; i < times - 1; i++)
            {
                reVal = MD5(reVal);
            }

            return reVal;
        }
        #endregion MD5加密N次

        #region 盐值加密
        /// <summary>
        /// 盐值加密
        /// </summary>
        /// <param name="InputString">要加密的源字符串</param>
        /// <param name="InputSalt">盐值</param>
        public static string SaltEncrypt(string InputString, string InputSalt)
        {
            string RealSalt = MD5(InputSalt);
            string reVal = MD5(InputString + RealSalt);

            return reVal;
        }
        #endregion 盐值加密

        #region 可逆加密
        /// <summary>
        /// 可逆加密
        /// </summary>
        /// <param name="InputEncryptString"></param>
        /// <returns></returns>
        public static string EncryptDES(this string InputEncryptString, string SecretKey = "")
        {
            try
            {
                if (SecretKey.IsNullOrEmpty())
                {
                    SecretKey = EncryptString;
                }
                InputEncryptString = InputEncryptString.Trim();
                byte[] rgbKey = Encoding.UTF8.GetBytes(SecretKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(InputEncryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return InputEncryptString;
            }
        }
        #endregion 可逆加密

        #region 可逆解密
        /// <summary>
        /// 可逆解密
        /// </summary>
        /// <param name="InputDecryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功获取解密后的字符串，失败返源串</returns>
        public static string DecryptDES(this string InputDecryptString, string SecretKey = "")
        {
            try
            {
                if (SecretKey.IsNullOrEmpty())
                {
                    SecretKey = EncryptString;
                }
                InputDecryptString = InputDecryptString.Trim();
                byte[] rgbKey = Encoding.UTF8.GetBytes(SecretKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(InputDecryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return InputDecryptString;
            }
        }
        #endregion 可逆解密

        #region 计算文件的MD5值
        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        /// <param name="InputFileName">要计算 MD5 值的文件名和路径</param>
        /// <returns>MD5值16进制字符串</returns>
        public static string MD5File(this string InputFileName)
        {
            return HashFile(InputFileName, "md5");
        }
        #endregion 计算文件的 MD5 值

        #region 计算文件的哈希值
        /// <summary>
        /// 计算文件的哈希值
        /// </summary>
        /// <param name="InputFileName">要计算哈希值的文件名和路径</param>
        /// <param name="AlgName">算法:sha1,md5</param>
        /// <returns>哈希值16进制字符串</returns>
        public static string HashFile(string InputFileName, string AlgName)
        {
            if (!System.IO.File.Exists(InputFileName))
                return string.Empty;

            FileStream fs = new FileStream(InputFileName, FileMode.Open, FileAccess.Read);
            byte[] hashBytes = HashData(fs, AlgName);
            fs.Close();
            return ByteArrayToHexString(hashBytes);
        }
        #endregion 计算文件的哈希值

        #region 计算哈希值
        /// <summary>
        /// 计算哈希值
        /// </summary>
        /// <param name="InputStream">要计算哈希值的 Stream</param>
        /// <param name="InputAlgName">算法:sha1,md5</param>
        /// <returns>哈希值字节数组</returns>
        public static byte[] HashData(Stream InputStream, string InputAlgName)
        {
            HashAlgorithm algorithm;
            if (InputAlgName == null)
            {
                throw new ArgumentNullException("算法名称不能为 null");
            }
            if (string.Compare(InputAlgName, "sha1", true) == 0)
            {
                algorithm = SHA1.Create();
            }
            else
            {
                if (string.Compare(InputAlgName, "md5", true) != 0)
                {
                    throw new Exception("算法只能使用SHA1或MD5");
                }
                algorithm = System.Security.Cryptography.MD5.Create();
            }
            return algorithm.ComputeHash(InputStream);
        }
        #endregion 计算哈希值

        #region 获取对象的MD5哈希值
        /// <summary>
        /// 获取对象的MD5哈希值
        /// </summary>
        public static string GetMD5Hash(object obj)
        {
            StringBuilder builder = new StringBuilder();
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj.ToString());
                var bytes = ms.GetBuffer();
                using (MD5 md = new MD5CryptoServiceProvider())
                {
                    var hashBytes = md.ComputeHash(bytes);
                    foreach (var hashbyte in hashBytes)
                    {
                        builder.Append(hashbyte.ToString("x2"));
                    }
                    md.Clear();
                }
            }
            return builder.ToString();
        }
        #endregion 获取对象的MD5哈希值

        #region 字节数组转换为16进制表示的字符串
        /// <summary>
        /// 字节数组转换为16进制表示的字符串
        /// </summary>
        public static string ByteArrayToHexString(byte[] buf)
        {
            string returnStr = "";
            if (buf != null)
            {
                for (int i = 0; i < buf.Length; i++)
                {
                    returnStr += buf[i].ToString("X2");
                }
            }
            return returnStr;
        }
        #endregion 字节数组转换为16进制表示的字符串

        #region 判断当前字符串是否存在SQL注入
        /// <summary>
        /// 判断当前字符串是否存在SQL注入
        /// </summary>
        /// <returns></returns>
        public static bool IsSafeSqlString(string SqlString)
        {
            //防SQL注入正则表达式
            Regex _SqlKeyWordRegex = new Regex(@"(select|insert|delete|from|count\(|drop|table|update|truncate|asc\(|mid\(|char\(|xp_cmdshell|exec|master|net|local|group|administrators|user|or|and|-|;|,|\(|\)|\[|\]|\{|\}|%|\*|!|\')", RegexOptions.IgnoreCase);
            if (SqlString != null && SQLKeyWordRegex.IsMatch(SqlString))
                return false;
            return true;
        }
        #endregion 判断当前字符串是否存在SQL注入

        #region  删除SQL注入特殊字符
        /// <summary>
        /// 删除SQL注入特殊字符
        /// </summary>
        public static string DelSQLInjection(string SqlString)
        {
            string reVal = SqlString;
            if (SqlString.IsNotNullOrEmpty())
            {
                //过滤 ' --  
                string pattern1 = @"(\%27)|(\')|(\-\-)";

                //防止执行 ' or  
                string pattern2 = @"((\%27)|(\'))\s*((\%6F)|o|(\%4F))((\%72)|r|(\%52))";

                //防止执行sql server 内部存储过程或扩展存储过程  
                string pattern3 = @"\s+exec(\s|\+)+(s|x)p\w+";
                reVal = Regex.Replace(reVal, pattern1, string.Empty, RegexOptions.IgnoreCase);
                reVal = Regex.Replace(reVal, pattern2, string.Empty, RegexOptions.IgnoreCase);
                reVal = Regex.Replace(reVal, pattern3, string.Empty, RegexOptions.IgnoreCase);
            }
            return reVal;
        }
        #endregion 删除SQL注入特殊字符

        #region 判断是否是Base64字符串
        /// <summary>
        /// 判断是否是Base64字符串
        /// </summary>
        public static bool IsBase64String(this string Base64String)
        {
            Regex _Base64Regex = new Regex(@"[A-Za-z0-9\=\/\+]");
            if (Base64String != null)
                return _Base64Regex.IsMatch(Base64String);
            return true;
        }
        #endregion 判断是否是Base64字符串
    }
    #endregion 安全相关的类集合
}