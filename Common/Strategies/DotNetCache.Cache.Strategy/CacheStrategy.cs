using Super.Framework;
using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace DotNetCache.Cache.Strategy
{
    #region 基于Asp.Net内置缓存的缓存策略
    /// <summary>
    /// 基于Asp.Net内置缓存的缓存策略
    /// </summary>
    [StrategyCode("Cache")]
    [StrategyTitle("基于Asp.Net内置缓存的缓存策略")]
    [StrategyVersion("1.0")]
    public class CacheStrategy : ICacheStrategy
    {
        #region 变量

        #region 缓存
        /// <summary>
        /// 缓存
        /// </summary>
        private System.Web.Caching.Cache _Cache;
        #endregion 缓存

        #region 缓存过期时间
        /// <summary>
        /// 缓存过期时间，单位秒
        /// </summary>
        private int _TimeOut = 3600;
        #endregion 缓存过期时间

        #endregion 变量

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        public CacheStrategy()
        {
            _Cache = HttpRuntime.Cache;
        }
        #endregion 初始化

        #region 获得指定键的缓存值
        /// <summary>
        /// 获得指定键的缓存值
        /// </summary>
        /// <param name="Key">缓存键</param>
        /// <returns>缓存值</returns>
        public object Get(string InputKey)
        {
            return _Cache.Get(InputKey);
        }
        #endregion 获得指定键的缓存值

        #region 将指定键的对象添加到缓存中
        /// <summary>
        /// 将指定键的对象添加到缓存中
        /// </summary>
        /// <param name="InputKey">缓存键</param>
        /// <param name="InputData">缓存值</param>
        public void Insert(string InputKey, object InputData)
        {
            _Cache.Insert(InputKey, InputData, null, DateTime.Now.AddSeconds(_TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }
        #endregion 将指定键的对象添加到缓存中

        #region 将指定键的对象添加到缓存中，并指定过期时间
        /// <summary>
        /// 将指定键的对象添加到缓存中，并指定过期时间
        /// </summary>
        /// <param name="InputKey">缓存键</param>
        /// <param name="InputData">缓存值</param>
        /// <param name="InputCacheTime">缓存过期时间</param>
        public void Insert(string InputKey, object InputData, int InputCacheTime)
        {
            _Cache.Insert(InputKey, InputData, null, DateTime.Now.AddSeconds(InputCacheTime), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }
        #endregion 将指定键的对象添加到缓存中，并指定过期时间

        #region 从缓存中移除指定键的缓存值
        /// <summary>
        /// 从缓存中移除指定键的缓存值
        /// </summary>
        /// <param name="InputKey">缓存键</param>
        public void Remove(string InputKey)
        {
            _Cache.Remove(InputKey);
        }
        #endregion 从缓存中移除指定键的缓存值

        #region 清空所有缓存对象
        /// <summary>
        /// 清空所有缓存对象
        /// </summary>
        public void Clear()
        {
            IDictionaryEnumerator CacheEnum = _Cache.GetEnumerator();
            while (CacheEnum.MoveNext())
                _Cache.Remove(CacheEnum.Key.ToString());
        }
        #endregion 清空所有缓存对象

        #region 缓存过期时间
        /// <summary>
        /// 缓存过期时间
        /// </summary>
        public int TimeOut
        {
            get
            {
                return _TimeOut;
            }
            set
            {
                _TimeOut = value > 0 ? value : 3600;
            }
        }
        #endregion 缓存过期时间
    }
    #endregion 基于Asp.Net内置缓存的缓存策略
}