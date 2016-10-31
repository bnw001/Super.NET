using System;
using System.Collections.Generic;
using System.IO;

namespace Super.Framework
{
    #region 文件型日志
    /// <summary>
    /// 文件型日志
    /// </summary>
    internal class FileLog : ILog
    {
        #region 日志文件
        /// <summary>
        /// 日志文件
        /// </summary>
        private static string _LogFile { get; set; }
        #endregion 日志文件

        #region 信息队列
        /// <summary>
        /// 信息队列
        /// </summary>
        private static Queue<LogMessage> _QLM = new Queue<LogMessage>();
        #endregion 信息队列

        #region 出队锁
        private static bool _IsOut = false;
        #endregion 出队锁

        #region 入队
        /// <summary>
        /// 入队
        /// </summary>
        private static void In(LogMessage _ILM)
        {
            _QLM.Enqueue(_ILM);
            if (!_IsOut)
            {
                Out();
            }
        }
        #endregion 入队

        #region 出队
        /// <summary>
        /// 出队
        /// </summary>
        private static void Out()
        {
            _IsOut = true;
            LogMessage _LM = null;
            if (_QLM.Count > 0)
            {
                _LM = _QLM.Dequeue();
                if (_LM.Type == LogType.Info)
                {
                    WriteMessByMode("Info", _LM.ID, _LM.Message);
                }
                else if (_LM.Type == LogType.Warning)
                {
                    WriteMessByMode("Warning", _LM.ID, _LM.Message);
                }
                else if (_LM.Type == LogType.Error)
                {
                    WriteMessByMode("Error", _LM.ID, _LM.Message);
                }
                else if (_LM.Type == LogType.Debug)
                {
                    WriteMessByMode("Debug", _LM.ID, _LM.Message);
                }
                if (_IsOut)
                {
                    Out();
                }
            }
            _IsOut = false;
        }
        #endregion 出队

        #region 写入一般信息
        /// <summary>
        /// 写入一般信息
        /// </summary>
        public void Info(string _IMessage)
        {
            LogMessage _LM = new LogMessage()
            {
                Type = LogType.Info,
                Message = _IMessage
            };
            In(_LM);
        }
        #endregion 写入一般信息

        #region 写入警告信息
        /// <summary>
        /// 写入警告信息
        /// </summary>
        public void Warning(string _IMessage)
        {
            LogMessage _LM = new LogMessage()
            {
                Type = LogType.Warning,
                Message = _IMessage
            };
            In(_LM);
        }
        #endregion 写入警告信息

        #region 写入错误信息
        /// <summary>
        /// 写入错误信息
        /// </summary>
        public void Error(string _IMessage)
        {
            LogMessage _LM = new LogMessage()
            {
                Type = LogType.Error,
                Message = _IMessage
            };
            In(_LM);
        }
        #endregion 写入错误信息

        #region 写入调试信息
        /// <summary>
        /// 写入调试信息
        /// </summary>
        public void Debug(string _IMessage)
        {
            LogMessage _LM = new LogMessage()
            {
                Type = LogType.Debug,
                Message = _IMessage
            };
            In(_LM);
        }
        #endregion 写入调试信息

        #region 模式写入信息
        /// <summary>
        /// 模式写入信息
        /// </summary>
        private static void WriteMessByMode(string _Mode, string _ID, string _Message)
        {
            CreateLogFile(_Mode);
            StreamWriter streamwrite = File.AppendText(_LogFile.Replace(".log", "." + _Mode + ".log"));
            streamwrite.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] [" + _ID + "] [" + _Mode + "] " + _Message);
            streamwrite.Close();
        }
        #endregion 模式写入信息

        #region 创建日志文件
        /// <summary>
        /// 创建日志文件
        /// </summary>
        private static void CreateLogFile(string mode)
        {
            string LogDirectory = SuperManager.FileFullPath("~\\Log\\");
            if (LogDirectory.IsNullOrEmpty())
            {
                LogDirectory = "Log";
            }
            if (SuperManager.WorkingMode == RunMode.Web)
            {
                if (!Directory.Exists(LogDirectory))
                {
                    Directory.CreateDirectory(LogDirectory);
                }
                _LogFile = Path.Combine(LogDirectory, DateTime.Now.ToString("yyyy-MM-dd") + ".log");

            }
            else
            {
                if (!Directory.Exists(LogDirectory))
                {
                    Directory.CreateDirectory(LogDirectory);
                }
                _LogFile = Path.Combine(LogDirectory, DateTime.Now.ToString("yyyy-MM-dd") + ".log");
            }
            if (!File.Exists(_LogFile.Replace(".log", "." + mode + ".log")))
            {
                FileStream fs = File.Create(_LogFile.Replace(".log", "." + mode + ".log"));
                fs.Close();
            }
        }
        #endregion 创建文件
    }
    #endregion 文件型日志
}