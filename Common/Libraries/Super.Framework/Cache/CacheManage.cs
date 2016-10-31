using System;
using System.IO;
using System.Reflection;

namespace Super.Framework
{
    #region 缓存管理类
    /// <summary>
    /// 缓存管理类
    /// </summary>
    public class CacheManage
    {
        #region 变量

        #region 锁对象
        /// <summary>
        /// 锁对象
        /// </summary>
        private static object _Locker = new object();
        #endregion 锁对象

        #region 缓存策略
        /// <summary>
        /// 缓存策略
        /// </summary>
        private static ICacheStrategy _CacheStrategy = null;
        #endregion 缓存策略

        #endregion 变量

        #region 缓存管理
        /// <summary>
        /// 缓存管理
        /// </summary>
        static CacheManage()
        {
            Load();
        }
        #endregion 缓存管理

        #region 加载缓存策略
        /// <summary>
        /// 加载缓存策略
        /// </summary>
        public static void Load()
        {
            try
            {
                TargetStrategyInfo TargetStrategy = StrategyManage.GetTargetStrategy("Cache");
                string TargetStrategyFilePath = SuperManager.FileFullPath("~/Strategy/" + TargetStrategy.StrategyFile);
                Assembly _Assembly = Assembly.LoadFile(TargetStrategyFilePath);
                if (_Assembly.IsHasAttributeClass<StrategyCodeAttribute>())
                {
                    string StrategyDomainName = TargetStrategy.StrategyFile.RemoveEndChar(".dll");
                    string StrategyClassFullName = StrategyDomainName + "." + TargetStrategy.ClassName;
                    Assembly asmb = Assembly.LoadFrom(TargetStrategyFilePath);
                    var AttributeType = asmb.GetType(StrategyClassFullName);
                    _CacheStrategy = (ICacheStrategy)Activator.CreateInstance(AttributeType);
                }
                else
                {
                    string StrategyDirectory = SuperManager.FileFullPath("~/Strategy/");
                    string[] StrategyFileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "*.Strategy.*.dll", SearchOption.TopDirectoryOnly);
                }
            }
            catch (Exception ex)
            {
                ex.ToLog();
            }
        }
        #endregion 加载缓存策略

        #region 缓存过期时间
        /// <summary>
        /// 缓存过期时间
        /// </summary>
        public static int TimeOut
        {
            get
            {
                return _CacheStrategy.TimeOut;
            }
            set
            {
                lock (_Locker)
                {
                    _CacheStrategy.TimeOut = value;
                }
            }
        }
        #endregion 缓存过期时间

        #region 获得指定键的缓存值
        /// <summary>
        /// 获得指定键的缓存值
        /// </summary>
        /// <param name="InputKey">缓存键</param>
        /// <returns>缓存值</returns>
        public static object Get(string InputKey)
        {
            if (InputKey.IsNullOrEmpty())
                return null;
            return _CacheStrategy.Get(InputKey);
        }
        #endregion 获得指定键的缓存值

        #region 将指定键的对象添加到缓存中
        /// <summary>
        /// 将指定键的对象添加到缓存中
        /// </summary>
        /// <param name="InputKey">缓存键</param>
        /// <param name="InputData">缓存值</param>
        public static void Insert(string InputKey, object InputData)
        {
            if (string.IsNullOrWhiteSpace(InputKey) || InputData == null)
                return;
            lock (_Locker)
            {
                _CacheStrategy.Insert(InputKey, InputData);
            }
        }
        #endregion 将指定键的对象添加到缓存中

        #region 将指定键的对象添加到缓存中，并指定过期时间
        /// <summary>
        /// 将指定键的对象添加到缓存中，并指定过期时间
        /// </summary>
        /// <param name="InputKey">缓存键</param>
        /// <param name="InputData">缓存值</param>
        /// <param name="InputCacheTime">缓存过期时间</param>
        public static void Insert(string InputKey, object InputData, int InputCacheTime)
        {
            if (string.IsNullOrWhiteSpace(InputKey) || InputData == null)
                return;
            lock (_Locker)
            {
                _CacheStrategy.Insert(InputKey, InputData, InputCacheTime);
            }
        }
        #endregion 将指定键的对象添加到缓存中，并指定过期时间

        #region 从缓存中移除指定键的缓存值
        /// <summary>
        /// 从缓存中移除指定键的缓存值
        /// </summary>
        /// <param name="Key">缓存键</param>
        public static void Remove(string Key)
        {
            if (string.IsNullOrWhiteSpace(Key))
                return;
            lock (_Locker)
            {
                _CacheStrategy.Remove(Key);
            }
        }
        #endregion 从缓存中移除指定键的缓存值

        #region 清空缓存所有对象
        /// <summary>
        /// 清空缓存所有对象
        /// </summary>
        public static void Clear()
        {
            lock (_Locker)
            {
                _CacheStrategy.Clear();
            }
        }
        #endregion 清空缓存所有对象
    }
    #endregion 缓存管理类
}