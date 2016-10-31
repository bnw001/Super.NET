namespace Super.Framework
{
    #region 缓存策略接口
    /// <summary>
    /// 缓存策略接口
    /// </summary>
    public interface ICacheStrategy
    {
        #region 缓存过期时间
        /// <summary>
        /// 缓存过期时间
        /// </summary>
        int TimeOut { set; get; }
        #endregion 缓存过期时间

        #region 获得指定键的缓存值
        /// <summary>
        /// 获得指定键的缓存值
        /// </summary>
        /// <param name="Key">缓存键</param>
        /// <returns>缓存值</returns>
        object Get(string Key);
        #endregion 获得指定键的缓存值

        #region 从缓存中移除指定键的缓存值
        /// <summary>
        /// 从缓存中移除指定键的缓存值
        /// </summary>
        /// <param name="Key">缓存键</param>
        void Remove(string Key);
        #endregion 从缓存中移除指定键的缓存值

        #region 清空所有缓存对象
        /// <summary>
        /// 清空所有缓存对象
        /// </summary>
        void Clear();
        #endregion 清空所有缓存对象

        #region 将指定键的对象添加到缓存中
        /// <summary>
        /// 将指定键的对象添加到缓存中
        /// </summary>
        /// <param name="Key">缓存键</param>
        /// <param name="Data">缓存值</param>
        void Insert(string Key, object Data);
        #endregion 将指定键的对象添加到缓存中

        #region 将指定键的对象添加到缓存中，并指定过期时间
        /// <summary>
        /// 将指定键的对象添加到缓存中，并指定过期时间
        /// </summary>
        /// <param name="Key">缓存键</param>
        /// <param name="Data">缓存值</param>
        /// <param name="CacheTime">缓存过期时间</param>
        void Insert(string Key, object Data, int CacheTime);
        #endregion 将指定键的对象添加到缓存中，并指定过期时间
    }
    #endregion 缓存策略接口
}