using System;
using System.IO;
using System.Management;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml.Serialization;

namespace Super.Framework
{
    #region 超级管理类
    /// <summary>
    /// 超级管理类
    /// </summary>
    public class SuperManager
    {
        #region 获取GUID值
        /// <summary>
        /// 获取GUID值
        /// </summary>
        public static string NewGUID
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }
        #endregion 获取GUID值

        #region 程序运行模式
        /// <summary>
        /// 程序运行模式
        /// </summary>
        public static RunMode WorkingMode
        {
            get
            {
                RunMode reVal = RunMode.Unknown;
                if (AppDomain.CurrentDomain.SetupInformation.ConfigurationFile.ToLower().EndsWith("web.config"))
                {
                    reVal = RunMode.Web;
                }
                else if (AppDomain.CurrentDomain.SetupInformation.ConfigurationFile.ToLower().Length > 0)
                {
                    reVal = RunMode.App;
                }

                return reVal;
            }
        }
        #endregion 程序运行模式

        #region 获取当前应用程序域
        /// <summary>
        /// 获取当前应用程序域
        /// </summary>
        public static AppDomain CurrentAppDomain
        {
            get
            {
                return Thread.GetDomain();
            }
        }
        #endregion 获取当前应用程序域

        #region 获取系统换行符
        /// <summary>
        /// 获取系统换行符
        /// </summary>
        public static string NewLine
        {
            get
            {
                return Environment.NewLine;
            }
        }
        #endregion 获取系统换行符

        #region 文件全路径
        /// <summary>
        /// 文件全路径
        /// </summary>
        /// <param name="FilePath">文件路径信息</param>
        /// <returns>返回值：文件全路径</returns>
        public static string FileFullPath(string FilePath)
        {
            string reVal = string.Empty;
            if (FilePath.IndexOf(":") > 0)
            {
                reVal = FilePath;
            }
            else
            {
                RunMode AppRunMode = SuperManager.WorkingMode;
                if (AppRunMode == RunMode.Unknown)
                {
                    reVal = string.Empty;
                }
                else if (AppRunMode == RunMode.Web)
                {
                    if (HttpContext.Current != null)
                    {
                        reVal = HttpContext.Current.Server.MapPath(FilePath);
                    }
                    else
                    {
                        return System.Web.Hosting.HostingEnvironment.MapPath(FilePath);
                    }
                }
                else
                {
                    FilePath = FilePath.Replace("/", "\\");
                    if (FilePath.StartsWith("~"))
                    {
                        FilePath = FilePath.RemoveStartChar("~");
                    }
                    if (!FilePath.StartsWith("\\"))
                    {
                        FilePath = "\\" + FilePath;
                    }
                    reVal = SuperApp.WorkDir + FilePath;
                }
            }
            return reVal;
        }
        #endregion 文件全路径

        #region 验证目录是否合法
        /// <summary>
        /// 验证目录是否合法
        /// </summary>
        /// <param name="InputDirectoryPath">目录路径</param>
        public bool IsLegalDirectory(string InputDirectoryPath)
        {
            bool reVal = false;
            if (Directory.Exists(InputDirectoryPath))
            {
                reVal = true;
            }
            else
            {
                try
                {
                    InputDirectoryPath = FileFullPath(InputDirectoryPath);
                    Directory.CreateDirectory(InputDirectoryPath);
                    reVal = true;
                }
                catch
                {
                    reVal = false;
                }
            }
            return reVal;
        }
        #endregion 验证目录是否合法

        #region 序列化

        #region XML序列化
        /// <summary>
        /// XML序列化
        /// </summary>
        /// <param name="InputObj">序列对象</param>
        /// <param name="FilePath">XML文件路径</param>
        /// <returns>是否成功</returns>
        public static bool SerializeToXml(object InputObj, string FilePath)
        {
            bool reVal = false;
            FileStream FS = null;
            try
            {
                FS = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(InputObj.GetType());
                serializer.Serialize(FS, InputObj);
                reVal = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (FS != null)
                    FS.Close();
            }
            return reVal;

        }
        #endregion XML序列化

        #region XML反序列化
        /// <summary>
        /// XML反序列化
        /// </summary>
        /// <param name="InputType">目标类型(Type类型)</param>
        /// <param name="InputFilePath">XML文件路径</param>
        /// <returns>序列对象</returns>
        public static object DeserializeFromXML(Type InputType, string InputFilePath)
        {
            FileStream FS = null;
            try
            {
                FS = new FileStream(InputFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(InputType);
                return serializer.Deserialize(FS);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (FS != null)
                    FS.Close();
            }
        }
        #endregion XML反序列化

        #endregion 序列化

        #region 操作系统版本
        /// <summary>
        /// 操作系统版本
        /// </summary>
        public static string ServerOS
        {
            get
            {
                string reVal = "unknow";
                string Agent = Environment.OSVersion.ToString();
                if (Agent.IndexOf("NT 4.0") > 0)
                {
                    reVal = "Windows NT 4.0";
                }
                else if (Agent.IndexOf("NT 5.0") > 0)
                {
                    reVal = "Windows 2000";
                }
                else if (Agent.IndexOf("NT 5.1") > 0)
                {
                    reVal = "Windows XP";
                }
                else if (Agent.IndexOf("NT 5.2") > 0)
                {
                    reVal = "Windows Server 2003";
                }
                else if (Agent.IndexOf("NT 6.0") > 0)
                {
                    reVal = "Windows Server 2008";
                }
                else if (Agent.IndexOf("NT 6.1") > 0)
                {
                    reVal = "Windows 7";
                }
                else if (Agent.IndexOf("NT 6.2") > 0)
                {
                    reVal = "Windows 8";
                }
                else if (Agent.IndexOf("WindowsCE") > 0)
                {
                    reVal = "Windows CE";
                }
                else if (Agent.IndexOf("Linux") > 0)
                {
                    reVal = "Linux";
                }
                else if (Agent.IndexOf("SunOS") > 0)
                {
                    reVal = "SunOS";
                }
                else if (Agent.IndexOf("Mac") > 0)
                {
                    reVal = "Mac";
                }
                else if (Agent.IndexOf("Windows") > 0)
                {
                    reVal = "Windows";
                }

                return reVal;
            }
        }
        #endregion 操作系统版本

        #region 映射到进程的物理内存量
        /// <summary>
        /// 映射到进程的物理内存量
        /// </summary>
        public static string ProcessWorkingMemory
        {
            get
            {
                string reVal = "0";
                reVal = Environment.WorkingSet.ToString();
                return reVal;
            }
        }
        #endregion 当前进程使用的内存

        #region 开机运行时长
        /// <summary>
        /// 开机运行时长
        /// </summary>
        public static string ServerOpenTimeSpan
        {
            get
            {
                string reVal = string.Empty;
                try
                {
                    int temp = (int)Environment.TickCount / (1000 * 60 * 60 * 24);
                    if (temp > 0)
                    {
                        reVal += temp.ToString() + "天";
                    }
                    temp = ((int)Environment.TickCount / 3600000) % 24;
                    if (temp > 0)
                    {
                        reVal += temp + "小时";
                    }
                    temp = ((int)Environment.TickCount / 60000) % (24 * 60) % 60;
                    if (temp > 0)
                    {
                        reVal += temp + "分钟";
                    }
                }
                catch
                {
                    reVal = "unknow";
                }

                return reVal;
            }
        }
        #endregion 开机运行时长

        #region .NET 版本
        /// <summary>
        /// .NET 版本
        /// </summary>
        public static string DotNetVersion
        {
            get
            {
                string reVal = string.Empty;
                int build, major, minor, revision;
                build = Environment.Version.Build;
                major = Environment.Version.Major;
                minor = Environment.Version.Minor;
                revision = Environment.Version.Revision;
                reVal = major + "." + minor + "." + build + "." + revision;

                return reVal;
            }
        }
        #endregion .NET 版本

        #region 机器码
        /// <summary>
        /// 机器码
        /// </summary>
        public static string MachineCode
        {
            get
            {
                try
                {
                    ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
                    ManagementObjectCollection queryCollection = query.Get();
                    foreach (ManagementObject mo in queryCollection)
                    {
                        if (mo["IPEnabled"].ToString() == "True")
                            return mo["MacAddress"].ToString();
                    }
                    return "";
                }
                catch
                {
                    return "";
                }

            }
        }
        #endregion 机器码

        #region 验证码

        #region 产生字母和数字随机码
        /// <summary>
        /// 产生字母和数字随机码
        /// </summary>
        /// <param name="InputValidCodeLength">指定长度</param>
        public static string GenerateRandomChareNumString(int InputValidCodeLength)
        {
            string reVal = string.Empty;
            if (InputValidCodeLength < 4)
            {
                InputValidCodeLength = 4;
            }
            var RandomChars = "ABCDEFGHIJKLMNPQRSTUVWXYZ0123456789";
            System.Random random = new Random();
            StringBuilder Result = new StringBuilder();
            for (int i = 0; i < InputValidCodeLength; i++)
            {
                int index = random.Next(RandomChars.Length);
                Result.Append(RandomChars[index]);
            }
            reVal = Result.ToString();

            return reVal;
        }
        #endregion 产生字母和数字随机码

        #region 产生验证码图片二进制流
        /// <summary>
        /// 产生验证码图片二进制流
        /// </summary>
        /// <param name="CodeLength">验证码长度</param>
        /// <param name="InputImageWidth">图片宽度</param>
        /// <param name="InputImageHeight">图片高度</param>
        /// <param name="VelidateCode">验证码的真实值</param>
        public static byte[] CreateValidateCode(int CodeLength, int InputImageWidth, int InputImageHeight, out string VelidateCode)
        {
            VelidateCode = GenerateRandomChareNumString(CodeLength);
            byte[] reVal = null;
            #region 画出验证码
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(InputImageWidth, InputImageHeight);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            try
            {
                System.Random random = new Random();
                g.Clear(System.Drawing.Color.White);
                System.Drawing.Font font = new System.Drawing.Font("Microsoft Yahei", 16, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new System.Drawing.Rectangle(0, 0, image.Width, image.Height), System.Drawing.Color.Blue, System.Drawing.Color.DarkRed, 1.2f, true);
                g.DrawString(VelidateCode, font, brush, 2, 2);
                for (int i = 0; i < InputImageWidth * InputImageHeight / 5; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, System.Drawing.Color.FromArgb(random.Next()));
                }
                g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Black), 0, 0, image.Width - 1, image.Height - 1);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                reVal = ms.ToArray();
                return reVal;
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
            #endregion 画出验证码
        }
        #endregion 产生验证码图片二进制流

        #endregion 验证码
    }
    #endregion 超级管理类
}