using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace Super.Framework
{
    #region 超级网站程序类
    /// <summary>
    /// 超级网站程序类
    /// </summary>
    public class SuperWeb : SuperManager
    {
        #region 信息服务软件名称和版本
        /// <summary>
        /// 信息服务软件名称和版本
        /// </summary>
        public static string ServerSoft
        {
            get
            {
                string reVal = string.Empty;
                HttpContext httpCurrent = HttpContext.Current;
                reVal = httpCurrent.Request.ServerVariables["SERVER_SOFTWARE"];

                return reVal;
            }
        }
        #endregion 信息服务软件名称和版本

        #region 网站根目录的物理路径
        /// <summary>
        /// 网站根目录的物理路径
        /// </summary>
        public static string RootPath
        {
            get
            {
                string reVal = string.Empty;
                HttpContext httpCurrent = HttpContext.Current;
                if (httpCurrent != null)
                {
                    reVal = httpCurrent.Server.MapPath("~");
                }
                else
                {
                    reVal = AppDomain.CurrentDomain.BaseDirectory;
                    if (!Regex.Match(reVal, @"\\$", RegexOptions.Compiled).Success)
                        reVal += "\\";
                }

                return reVal;
            }
        }
        #endregion 网站根目录的物理路径

        #region 网站运行的URL根目录
        /// <summary>
        /// 网站运行的URL根目录
        /// </summary>
        public static string RootUrl
        {
            get
            {
                string reVal = string.Empty;
                HttpContext HttpCurrent = HttpContext.Current;
                HttpRequest httpRequest;
                if (HttpCurrent != null)
                {
                    httpRequest = HttpCurrent.Request;
                    string urlAuthority = httpRequest.Url.GetLeftPart(UriPartial.Authority);
                    if (httpRequest.ApplicationPath == null || httpRequest.ApplicationPath == "/")
                        reVal = urlAuthority;
                    else
                        reVal = urlAuthority + httpRequest.ApplicationPath;
                }

                return reVal + "/";
            }
        }
        #endregion 网站运行的URL根目录

        #region 系统语言
        /// <summary>
        /// 系统语言
        /// </summary>
        public static string Language
        {
            get
            {
                string reVal = "ZH-CN";
                if (!GetCookie("WEB_LANGUAGE").IsNullOrEmpty() && LanguageCodeList.Contains(GetCookie("WEB_LANGUAGE")))
                {
                    reVal = GetCookie("WEB_LANGUAGE");
                }
                else
                {
                    if (LanguageCodeList.Count > 0 && !LanguageCodeList.Contains("ZH-CN"))
                    {
                        reVal = LanguageCodeList[0];
                    }
                }

                if (!reVal.IsNullOrEmpty())
                {
                    SetCookie("WEB_LANGUAGE", reVal, 30 * 24 * 60);
                }
                return reVal;
            }
            set
            {
                if (!value.IsNullOrEmpty() && LanguageCodeList.Contains(value))
                {
                    SetCookie("WEB_LANGUAGE", value, 30 * 24 * 60);
                }
            }
        }
        #endregion 系统语言

        #region 环境检测
        /// <summary>
        /// 环境检测
        /// </summary>
        public static bool EnvironmentCheck(out Dictionary<string, bool> _OResult)
        {
            bool reVal = false;
            _OResult = new Dictionary<string, bool>();

            return reVal;
        }
        #endregion 环境检测

        #region 初始化设置
        /// <summary>
        /// 初始化设置
        /// </summary>
        public static void InitSetting()
        {

        }
        #endregion 初始化设置

        #region 获取系统的所有语言编码列表
        /// <summary>
        /// 获取系统的所有语言编码列表
        /// </summary>
        public static List<string> LanguageCodeList
        {
            get
            {
                List<string> reVal = new List<string>();
                string LangPath = SuperManager.FileFullPath("~/Lang/");
                string[] LangDirs = Directory.GetDirectories(LangPath);
                foreach (string Lang in LangDirs)
                {
                    string LangInfoPath = SuperManager.FileFullPath("~/Lang/" + Lang + "/Self.Info");
                    if (File.Exists(LangInfoPath))
                    {
                        string NodeInfoPath = "//root/Name";
                        XmlNode Node = XMLHelper.SingleNode(LangInfoPath, NodeInfoPath);
                        if (Node != null && !Node.InnerText.IsNullOrEmpty())
                        {
                            reVal.Add(Lang);
                        }
                    }
                }

                return reVal;
            }
        }
        #endregion 获取系统的所有语言编码列表

        #region 系统语言列表
        /// <summary>
        /// 系统语言列表
        /// </summary>
        public static Dictionary<string, string> LanguageList
        {
            get
            {
                Dictionary<string, string> reVal = new Dictionary<string, string>();
                string LangPath = SuperManager.FileFullPath("~/Lang/");
                string[] LangDirs = Directory.GetDirectories(LangPath);
                foreach (string Lang in LangDirs)
                {
                    string LangInfoPath = SuperManager.FileFullPath("~/Lang/" + Lang + "/Self.Info");
                    if (File.Exists(LangInfoPath))
                    {
                        string NodeInfoPath = "//root/Name";
                        XmlNode Node = XMLHelper.SingleNode(LangInfoPath, NodeInfoPath);
                        if (Node != null && !Node.InnerText.IsNullOrEmpty())
                        {
                            reVal.Add(Lang, Node.InnerText.TBBRTrim());
                        }
                    }
                }

                return reVal;
            }
        }
        #endregion 系统语言列表

        #region 主机名
        /// <summary>
        /// 主机名
        /// </summary>
        public static string MachineName
        {
            get
            {
                string reVal = string.Empty;
                HttpContext httpCurrent = HttpContext.Current;
                reVal = httpCurrent.Server.MachineName;

                return reVal;
            }
        }
        #endregion 主机名

        #region 服务器域名
        /// <summary>
        /// 服务器域名
        /// </summary>
        public static string DomainName
        {
            get
            {
                string reVal = string.Empty;
                HttpContext httpCurrent = HttpContext.Current;
                reVal = httpCurrent.Request.ServerVariables["SERVER_NAME"];

                return reVal;
            }
        }
        #endregion 服务器域名

        #region 服务器IP地址
        /// <summary>
        /// 服务器IP地址
        /// </summary>
        public static string ServerIP
        {
            get
            {
                string reVal = string.Empty;
                HttpContext httpCurrent = HttpContext.Current;
                reVal = httpCurrent.Request.ServerVariables["LOCAL_ADDR"];

                return reVal;
            }
        }
        #endregion 服务器IP地址

        #region 网站运行所在端口
        /// <summary>
        /// 网站运行所在端口
        /// </summary>
        public static string WebPort
        {
            get
            {
                string reVal = string.Empty;
                HttpContext httpCurrent = HttpContext.Current;
                reVal = httpCurrent.Request.ServerVariables["SERVER_PORT"];

                return reVal;
            }
        }
        #endregion 网站运行所在端口

        #region HTML编码
        /// <summary>
        /// HTML编码
        /// </summary>
        public static string HtmlEncode(string HtmlString)
        {
            return HttpUtility.HtmlEncode(HtmlString);
        }
        #endregion HTML编码

        #region HTML解码
        /// <summary>
        /// HTML解码
        /// </summary>
        public static string HtmlDecode(string HtmlString)
        {
            return HttpUtility.HtmlDecode(HtmlString);
        }
        #endregion HTML解码

        #region URL编码
        /// <summary>
        /// URL编码
        /// </summary>
        public static string UrlEncode(string UrlString)
        {
            return HttpUtility.UrlEncode(UrlString);
        }
        #endregion URL编码

        #region URL解码
        /// <summary>
        /// URL解码
        /// </summary>
        public static string UrlDecode(string UrlString)
        {
            return HttpUtility.UrlDecode(UrlString);
        }
        #endregion URL解码

        #region 获取指定名称的Cookie值
        /// <summary>
        /// 获取指定名称的Cookie值
        /// </summary>
        public static string GetCookie(string CookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieName];
            if (cookie != null)
            {
                return cookie.Value;
            }

            return string.Empty;
        }
        #endregion 获取指定名称的Cookie值

        #region 获得指定名称的Cookie中特定键的值
        /// <summary>
        /// 获得指定名称的Cookie中特定键的值
        /// </summary>
        public static string GetCookie(string CookieName, string CookieKey)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieName];
            if (cookie != null && cookie.HasKeys)
            {
                string Value = cookie[CookieKey];
                if (Value != null)
                    return Value;
            }

            return string.Empty;
        }
        #endregion 获得指定名称的Cookie中特定键的值

        #region 添加一个Cookie（24小时过期）
        /// <summary>
        /// 添加一个Cookie（24小时过期）
        /// </summary>
        public static void SetCookie(string CookieName, string CookieValue)
        {
            SetCookie(CookieName, CookieValue, DateTime.Now.AddDays(1.0));
        }
        #endregion 添加一个Cookie（24小时过期）

        #region 添加一个Cookie
        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        public static void SetCookie(string CookieName, string CookieValue, DateTime CookieExpires)
        {
            HttpCookie cookie = new HttpCookie(CookieName)
            {
                Value = CookieValue,
                Expires = CookieExpires
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        #endregion 添加一个Cookie

        #region 添加一个Cookie
        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        public static void SetCookie(string CookieName, string CookieValue, double CookieExpires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieName];
            if (cookie == null)
                cookie = new HttpCookie(CookieName);

            cookie.Value = CookieValue;
            cookie.Expires = DateTime.Now.AddMinutes(CookieExpires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion 添加一个Cookie

        #region 添加一个Cookie
        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        public static void SetCookie(string CookieName, string CookieKey, string CookieValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieName];
            if (cookie == null)
                cookie = new HttpCookie(CookieName);

            cookie[CookieKey] = CookieValue;
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion 添加一个Cookie

        #region 添加一个Cookie
        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        public static void SetCookie(string CookieName, string CookieKey, string CookieValue, double CookieExpires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieName];
            if (cookie == null)
                cookie = new HttpCookie(CookieName);

            cookie[CookieKey] = CookieValue;
            cookie.Expires = DateTime.Now.AddMinutes(CookieExpires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion 添加一个Cookie

        #region 设置Session
        /// <summary>
        /// 设置Session
        /// </summary>
        public static void SetSession(string Key, object Value)
        {
            HttpContext.Current.Session[Key] = Value.ToString();
        }
        #endregion 设置Session

        #region 获取Session
        /// <summary>
        /// 获取Session
        /// </summary>
        public static string GetSession(string Key)
        {
            if (HttpContext.Current.Session[Key] != null)
            {
                return HttpContext.Current.Session[Key].ToString();
            }

            return string.Empty;
        }
        #endregion 获取Session

        #region 清空Session
        public static void ClearSession()
        {
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Clear();
            }
        }
        #endregion 清空Session

        #region 移除指定名称的Session
        /// <summary>
        /// 移除指定名称的Session
        /// </summary>
        public static void RemoveSession(string Key)
        {
            if (HttpContext.Current.Session[Key] != null)
            {
                HttpContext.Current.Session.Remove(Key);
            }
        }
        #endregion 移除指定名称的Session

        #region 移除指定名称的Cookie
        /// <summary>
        /// 移除指定名称的Cookie
        /// </summary>
        public static void RemoveCookie(string CookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-3);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
        #endregion 移除指定名称的Cookie

        #region 是否是Get请求
        /// <summary>
        /// 是否是GET请求
        /// </summary>
        public static bool IsGet
        {
            get
            {
                return HttpContext.Current.Request.HttpMethod == "GET";
            }
        }
        #endregion 是否是Get请求

        #region 是否是POST请求
        /// <summary>
        /// 是否是POST请求
        /// </summary>
        public static bool IsPost
        {
            get
            {
                return HttpContext.Current.Request.HttpMethod == "POST";
            }
        }
        #endregion 是否是POST请求

        #region 是否是Ajax请求
        /// <summary>
        /// 是否是Ajax请求
        /// </summary>
        public static bool IsAjax
        {
            get
            {
                return HttpContext.Current.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            }
        }
        #endregion 是否是Ajax请求

        #region 获得查询字符串中的值
        /// <summary>
        /// 获得查询字符串中的值
        /// </summary>
        public static string GetQueryString(string QueryKey, string DefaultValue)
        {
            string reVal = DefaultValue;

            if (HttpContext.Current.Request.QueryString[QueryKey] != null)
            {
                reVal = HttpContext.Current.Request.QueryString[QueryKey];
                if (reVal.IsNullOrEmpty())
                {
                    reVal = DefaultValue;
                }
            }

            return reVal;
        }
        #endregion 获得查询字符串中的值

        #region 获得查询字符串中的值
        /// <summary>
        /// 获得查询字符串中的值
        /// </summary>
        public static string GetQueryString(string QueryKey)
        {
            return GetQueryString(QueryKey, "");
        }
        #endregion 获得查询字符串中的值

        #region 获取表单字符串中的值
        /// <summary>
        /// 获取表单字符串中的值
        /// </summary>
        public static string GetFormString(string FormKey, string DefaultValue)
        {
            string reVal = DefaultValue;
            if (HttpContext.Current.Request.Form[FormKey] != null)
            {
                reVal = HttpContext.Current.Request.Form[FormKey];
                if (reVal.IsNullOrEmpty())
                {
                    reVal = DefaultValue;
                }
            }

            return reVal;
        }
        #endregion 获取表单字符串中的值

        #region 获取表单字符串中的值
        /// <summary>
        /// 获取表单字符串中的值
        /// </summary>
        public static string GetFormString(string FormKey)
        {
            return GetFormString(FormKey, "");
        }
        #endregion 获取表单字符串中的值

        #region 获得请求中的值
        /// <summary>
        /// 获得请求中的值
        /// </summary>
        public static string GetRequestString(string RequestKey, string DefaultValue)
        {
            if (HttpContext.Current.Request.Form[RequestKey] != null)
                return GetFormString(RequestKey, DefaultValue);
            else
                return GetQueryString(RequestKey, DefaultValue);
        }
        #endregion 获得请求中的值

        #region 获得请求中的值
        /// <summary>
        /// 获得请求中的值
        /// </summary>
        public static string GetRequestString(string RequestKey)
        {
            if (HttpContext.Current.Request.Form[RequestKey] != null)
                return GetFormString(RequestKey);
            else
                return GetQueryString(RequestKey);
        }
        #endregion 获得请求中的值

        #region 获取请求参数列表
        /// <summary>
        /// 获取请求参数列表
        /// </summary>
        public static Dictionary<string, string> RequestParams
        {
            get
            {
                Dictionary<string, string> reVal = new Dictionary<string, string>();
                var QS = HttpContext.Current.Request.QueryString;
                var RF = HttpContext.Current.Request.Form;
                if (QS["DataColumn"] != null && QS["DataValue"] != null)
                {
                    List<string> DataColumnList = QS["DataColumn"].SplitString(",").ToList();
                    List<string> DataValueList = QS["DataValue"].SplitString(",").ToList();
                    if (DataColumnList.Count == DataValueList.Count)
                    {
                        int ListCount = DataColumnList.Count;
                        for (int i = 0; i < ListCount; i++)
                        {
                            if (!reVal.ContainsKey(DataColumnList[i]))
                            {
                                reVal.Add(DataColumnList[i], DataValueList[i]);
                            }
                        }
                    }
                }
                else if (RF["DataColumn"] != null && RF["DataValue"] != null)
                {
                    List<string> DataColumnList = RF["DataColumn"].SplitString(",").ToList();
                    List<string> DataValueList = RF["DataValue"].SplitString(",").ToList();
                    if (DataColumnList.Count == DataValueList.Count)
                    {
                        int ListCount = DataColumnList.Count;
                        for (int i = 0; i < ListCount; i++)
                        {
                            if (!reVal.ContainsKey(DataColumnList[i]))
                            {
                                reVal.Add(DataColumnList[i], DataValueList[i]);
                            }
                        }
                    }
                }
                else
                {
                    foreach (string QSKey in QS)
                    {
                        if (!reVal.ContainsKey(QSKey))
                        {
                            reVal.Add(QSKey, QS[QSKey]);
                        }
                    }

                    foreach (string RFKey in RF)
                    {
                        if (!reVal.ContainsKey(RFKey))
                        {
                            reVal.Add(RFKey, RF[RFKey]);
                        }
                    }
                }
                return reVal;
            }
        }
        #endregion 获取请求参数列表

        #region 上次请求的Url
        /// <summary>
        /// 上次请求的Url
        /// </summary>
        public static string UrlReferrer
        {
            get
            {
                Uri uri = HttpContext.Current.Request.UrlReferrer;
                if (uri == null)
                    return string.Empty;

                return uri.ToString();
            }
        }
        #endregion 上次请求的Url

        #region 请求的Url
        /// <summary>
        /// 请求的Url
        /// </summary>
        public static string Url
        {
            get
            {
                return HttpContext.Current.Request.Url.ToString();
            }
        }
        #endregion 请求的Url

        #region 原始请求的Url
        /// <summary>
        /// 原始请求的Url
        /// </summary>
        public static string GetRawUrl
        {
            get
            {
                return HttpContext.Current.Request.RawUrl;
            }
        }
        #endregion 原始请求的Url

        #region 客户端IP地址
        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public static string IP
        {
            get
            {
                string reVal = string.Empty;
                if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                    reVal = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                else
                    reVal = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

                if (string.IsNullOrEmpty(reVal) || !ValidateHelper.IsIP(reVal))
                    reVal = "127.0.0.1";
                return reVal;
            }
        }
        #endregion 客户端IP地址

        #region 请求的浏览器类型
        /// <summary>
        /// 请求的浏览器类型
        /// </summary>
        public static string BrowserType
        {
            get
            {
                string BrowserType = HttpContext.Current.Request.Browser.Type;
                if (string.IsNullOrEmpty(BrowserType) || BrowserType == "unknown")
                    return "未知";

                return BrowserType.ToLower();
            }
        }
        #endregion 请求的浏览器类型

        #region 请求的浏览器名称
        /// <summary>
        /// 请求的浏览器名称
        /// </summary>
        public static string BrowserName
        {
            get
            {
                string name = HttpContext.Current.Request.Browser.Browser;
                if (string.IsNullOrEmpty(name) || name == "unknown")
                    return "未知";

                return name.ToLower();
            }
        }
        #endregion 请求的浏览器名称

        #region 获得请求的浏览器版本
        /// <summary>
        /// 获得请求的浏览器版本
        /// </summary>
        public static string BrowserVersion
        {
            get
            {
                string reVal = HttpContext.Current.Request.Browser.Version;
                if (string.IsNullOrEmpty(reVal) || reVal == "unknown")
                    return "未知";

                return reVal;
            }
        }
        #endregion 获得请求的浏览器版本

        #region 获得请求客户端的操作系统类型
        /// <summary>
        /// 获得请求客户端的操作系统类型
        /// </summary>
        public static string OSType
        {
            get
            {
                string reVal = "未知";
                string UserAgent = HttpContext.Current.Request.UserAgent;

                if (UserAgent.Contains("NT 6.1"))
                {
                    reVal = "Windows 7";
                }
                else if (UserAgent.Contains("NT 5.1"))
                {
                    reVal = "Windows XP";
                }
                else if (UserAgent.Contains("NT 6.2"))
                {
                    reVal = "Windows 8";
                }
                else if (UserAgent.Contains("android"))
                {
                    reVal = "Android";
                }
                else if (UserAgent.Contains("iphone"))
                {
                    reVal = "IPhone";
                }
                else if (UserAgent.Contains("Mac"))
                {
                    reVal = "Mac";
                }
                else if (UserAgent.Contains("NT 6.0"))
                {
                    reVal = "Windows Vista";
                }
                else if (UserAgent.Contains("NT 5.2"))
                {
                    reVal = "Windows 2003";
                }
                else if (UserAgent.Contains("NT 5.0"))
                {
                    reVal = "Windows 2000";
                }
                else if (UserAgent.Contains("98"))
                {
                    reVal = "Windows 98";
                }
                else if (UserAgent.Contains("95"))
                {
                    reVal = "Windows 95";
                }
                else if (UserAgent.Contains("Me"))
                {
                    reVal = "Windows Me";
                }
                else if (UserAgent.Contains("NT 4"))
                {
                    reVal = "Windows NT4";
                }
                else if (UserAgent.Contains("Unix"))
                {
                    reVal = "UNIX";
                }
                else if (UserAgent.Contains("Linux"))
                {
                    reVal = "Linux";
                }
                else if (UserAgent.Contains("SunOS"))
                {
                    reVal = "SunOS";
                }

                return reVal;
            }
        }
        #endregion 获得请求客户端的操作系统类型

        #region 获得请求客户端的操作系统名称
        /// <summary>
        /// 获得请求客户端的操作系统名称
        /// </summary>
        public static string OSName
        {
            get
            {
                string reVal = HttpContext.Current.Request.Browser.Platform;
                if (string.IsNullOrEmpty(reVal))
                    return "未知";

                return reVal;
            }
        }
        #endregion 获得请求客户端的操作系统名称

        #region 判断是否是浏览器请求
        /// <summary>
        /// 判断是否是浏览器请求
        /// </summary>
        public static bool IsBrowser
        {
            get
            {
                string Name = BrowserName;
                string[] BrowserList = new string[] { "IE", "Chrome", "Mozilla", "Netscape", "Firefox", "Opera", "Konqueror" };
                foreach (string Item in BrowserList)
                {
                    if (Name.Contains(Item))
                        return true;
                }
                return false;
            }
        }
        #endregion 判断是否是浏览器请求

        #region 是否是移动设备请求
        /// <summary>
        /// 是否是移动设备请求
        /// </summary>
        public static bool IsMobile
        {
            get
            {
                if (HttpContext.Current.Request.Browser.IsMobileDevice)
                    return true;

                bool IsTablet = false;
                if (bool.TryParse(HttpContext.Current.Request.Browser["IsTablet"], out IsTablet) && IsTablet)
                    return true;

                return false;
            }
        }
        #endregion 是否是移动设备请求

        #region 判断是否是搜索引擎爬虫请求
        /// <summary>
        /// 判断是否是搜索引擎爬虫请求
        /// </summary>
        public static bool IsCrawler()
        {
            bool reVal = HttpContext.Current.Request.Browser.Crawler;
            if (!reVal)
            {
                string Referrer = UrlReferrer;
                string[] SearchEngineList = new string[] { "baidu", "google", "360", "sogou", "bing", "msn", "sohu", "soso", "sina", "163", "yahoo", "jikeu" };
                if (Referrer.Length > 0)
                {
                    foreach (string item in SearchEngineList)
                    {
                        if (Referrer.Contains(item))
                            return true;
                    }
                }
            }
            return reVal;
        }
        #endregion 判断是否是搜索引擎爬虫请求

        #region 获得参数列表
        /// <summary>
        /// 获得参数列表
        /// </summary>
        public static NameValueCollection GetParmList(string Data)
        {
            NameValueCollection ParmList = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(Data))
            {
                int Length = Data.Length;
                for (int i = 0; i < Length; i++)
                {
                    int StartIndex = i;
                    int EndIndex = -1;
                    while (i < Length)
                    {
                        char c = Data[i];
                        if (c == '=')
                        {
                            if (EndIndex < 0)
                                EndIndex = i;
                        }
                        else if (c == '&')
                        {
                            break;
                        }
                        i++;
                    }
                    string key;
                    string value;
                    if (EndIndex >= 0)
                    {
                        key = Data.Substring(StartIndex, EndIndex - StartIndex);
                        value = Data.Substring(EndIndex + 1, (i - EndIndex) - 1);
                    }
                    else
                    {
                        key = Data.Substring(StartIndex, i - StartIndex);
                        value = string.Empty;
                    }
                    ParmList[key] = value;
                    if ((i == (Length - 1)) && (Data[i] == '&'))
                        ParmList[key] = string.Empty;
                }
            }
            return ParmList;
        }
        #endregion 获得参数列表

        #region 获取上传控件上传的是否是图片

        #region 获取上传控件(FileUpload)上传的是否是图片
        /// <summary>
        /// 获取上传控件(FileUpload)上传的是否是图片
        /// </summary>
        public static bool IsUploadImage(FileUpload UploadControl)
        {
            bool reValue = false;
            if (UploadControl.HasFile)
            {
                string fileExtension = System.IO.Path.GetExtension(UploadControl.FileName).ToLower();
                string[] allowExtension = { ".jpg", ".gif", ".jpeg", ",png", ".bmp" };
                foreach (string ext in allowExtension)
                {
                    if (fileExtension == ext)
                    {
                        reValue = true;
                        break;
                    }
                }

                if (reValue == true)
                {
                    string type = UploadControl.PostedFile.ContentType.ToLower();
                    if (!type.Contains("image"))
                    {
                        reValue = false;
                    }
                }
            }
            return reValue;
        }
        #endregion 获取上传控件(FileUpload)上传的是否是图片

        #region 获取上传控件(HttpPostedFile)上传的是否是图片
        /// <summary>
        /// 获取上传控件(HttpPostedFile)上传的是否是图片
        /// </summary>
        public static bool IsUploadImage(HttpPostedFile UploadControl)
        {
            bool reValue = false;
            if (UploadControl.ContentLength > 3)
            {
                string fileExtension = System.IO.Path.GetExtension(UploadControl.FileName).ToLower();
                string[] allowExtension = { ".jpg", ".gif", ".jpeg", ",png", ".bmp" };
                foreach (string ext in allowExtension)
                {
                    if (fileExtension == ext)
                    {
                        reValue = true;
                        break;
                    }
                }

                if (reValue == true)
                {
                    string type = UploadControl.ContentType.ToLower();
                    if (!type.Contains("image"))
                    {
                        reValue = false;
                    }
                }
            }
            return reValue;
        }
        #endregion 获取上传控件(HttpPostedFile)上传的是否是图片

        #endregion 获取上传控件上传的是否是图片

        #region 判断远程资源是否可访问
        /// <summary>
        /// 判断远程资源是否可访问
        /// </summary>
        public static bool IsRemoteResourcesCanRead(string Url)
        {
            bool reValue = true;
            if (Url.Length > 1)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(Url));
                ServicePointManager.Expect100Continue = false;
                try
                {
                    ((HttpWebResponse)request.GetResponse()).Close();
                }
                catch (System.Exception ex)
                {
                    reValue = false;
                    throw ex;
                }
            }
            else
            {
                reValue = false;
            }
            return reValue;
        }
        #endregion 判断远程资源是否可访问

        #region 获得Http请求数据
        /// <summary>
        /// 获得Http请求数据
        /// </summary>
        public static string GetRequestData(string Url, string PostData)
        {
            return GetRequestData(Url, "Post", PostData);
        }
        #endregion 获得Http请求数据

        #region 获得Http请求数据
        /// <summary>
        /// 获得Http请求数据
        /// </summary>
        public static string GetRequestData(string Url, string Method, string PostData)
        {
            return GetRequestData(Url, Method, PostData, Encoding.UTF8);
        }
        #endregion 获得Http请求数据

        #region 获得Http请求数据
        /// <summary>   
        /// 获得Http请求数据
        /// </summary>
        public static string GetRequestData(string Url, string Method, string PostData, Encoding Encoding)
        {
            return GetRequestData(Url, Method, PostData, Encoding, 20000);
        }
        #endregion 获得Http请求数据

        #region 获得Http请求数据
        /// <summary>
        /// 获得Http请求数据
        /// </summary>
        public static string GetRequestData(string Url, string Method, string PostData, Encoding Encoding, int TimeOut)
        {
            if (!(Url.Contains("http://") || Url.Contains("https://")))
                Url = "http://" + Url;
            Regex _MetaRegex = new Regex("<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(Url);
            Request.Method = Method.Trim().ToLower();
            Request.Timeout = TimeOut;
            Request.AllowAutoRedirect = true;
            Request.ContentType = "text/html";
            Request.Accept = "text/html, application/xhtml+xml, */*,zh-CN";
            Request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
            Request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);

            try
            {
                if (!string.IsNullOrEmpty(PostData) && Request.Method == "post")
                {
                    byte[] buffer = Encoding.GetBytes(PostData);
                    Request.ContentLength = buffer.Length;
                    Request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)Request.GetResponse())
                {
                    if (Encoding == null)
                    {
                        MemoryStream stream = new MemoryStream();
                        if (response.ContentEncoding != null && response.ContentEncoding.Equals("Gzip", StringComparison.InvariantCultureIgnoreCase))
                            new GZipStream(response.GetResponseStream(), CompressionMode.Decompress).CopyTo(stream, 10240);
                        else
                            response.GetResponseStream().CopyTo(stream, 10240);

                        byte[] RawResponse = stream.ToArray();
                        string Temp = Encoding.Default.GetString(RawResponse, 0, RawResponse.Length);
                        Match meta = _MetaRegex.Match(Temp);
                        string charter = (meta.Groups.Count > 2) ? meta.Groups[2].Value : string.Empty;
                        charter = charter.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty);
                        if (charter.Length > 0)
                        {
                            charter = charter.ToLower().Replace("iso-8859-1", "gbk");
                            Encoding = Encoding.GetEncoding(charter);
                        }
                        else
                        {
                            if (response.CharacterSet.ToLower().Trim() == "iso-8859-1")
                            {
                                Encoding = Encoding.GetEncoding("gbk");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(response.CharacterSet.Trim()))
                                {
                                    Encoding = Encoding.UTF8;
                                }
                                else
                                {
                                    Encoding = Encoding.GetEncoding(response.CharacterSet);
                                }
                            }
                        }
                        return Encoding.GetString(RawResponse);
                    }
                    else
                    {
                        StreamReader Reader = null;
                        if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                        {
                            using (Reader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress), Encoding))
                            {
                                return Reader.ReadToEnd();
                            }
                        }
                        else
                        {
                            using (Reader = new StreamReader(response.GetResponseStream(), Encoding))
                            {
                                try
                                {
                                    return Reader.ReadToEnd();
                                }
                                catch (Exception ex)
                                {
                                    ex.Data.Add("Url", Url);
                                    throw ex;
                                }

                            }
                        }
                    }
                }

            }
            catch (WebException ex)
            {
                ex.Data.Add("Url", Url);
                throw ex;
            }
        }

        #endregion
    }
    #endregion 超级网站程序类
}
