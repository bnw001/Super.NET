using Super.Framework;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace DotNetCache.Session.Strategy
{
    #region 基于.Net缓存的会话状态策略
    /// <summary>
    /// 基于.Net缓存的会话状态策略
    /// </summary>
    [StrategyCode("Session")]
    [StrategyTitle("基于.Net缓存的会话状态策略")]
    [StrategyVersion("1.0")]
    public class SessionStrategy : ISessionStrategy
    {
        #region 变量
        private Cache _Cache;
        private int _TimeOut=600;
        #endregion 变量

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        public SessionStrategy()
        {
            _Cache = HttpRuntime.Cache;
        }
        #endregion 初始化

        #region 过期时间(单位为秒)
        /// <summary>
        /// 过期时间(单位为秒)
        /// </summary>
        public int TimeOut
        {
            get
            {
                return _TimeOut;
            }
        }
        #endregion 过期时间(单位为秒)

        #region 获得用户会话状态数据
        /// <summary>
        /// 获得用户会话状态数据
        /// </summary>
        /// <param name="SID">用户SessionID</param>
        /// <returns>用户Session对象</returns>
        public Dictionary<string, object> GetSession(string SID)
        {
            Dictionary<string, object> reVal = new Dictionary<string, object>();
            object CacheObj = _Cache.Get(SID);
            if(CacheObj!=null)
            {
                reVal = (Dictionary<string, object>)CacheObj;
            }
            else
            {
                Dictionary<string, object> NewSession = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                _Cache.Insert(SID, NewSession, null, DateTime.Now.AddSeconds(_TimeOut), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                reVal= NewSession;
            }

            return reVal;
        }
        #endregion 获得用户会话状态数据

        #region 移除用户会话状态数据
        /// <summary>
        /// 移除用户会话状态数据
        /// </summary>
        /// <param name="SID">用户SessionID</param>
        public void RemoveSession(string SID)
        {
            _Cache.Remove(SID);
        }
        #endregion 移除用户会话状态数据

        #region 获得用户会话状态数据的数据项的值
        /// <summary>
        /// 获得用户会话状态数据的数据项的值
        /// </summary>
        /// <param name="SID">用户SessionID</param>
        /// <param name="Key">键</param>
        public object GetValue(string SID, string Key)
        {
            object reVal = null;
            object Session = _Cache.Get(SID);
            if(Session!=null)
            {
                Dictionary<string, object> ObjSession = (Dictionary<string, object>)Session;
                ObjSession.TryGetValue(Key, out reVal);
            }

            return reVal;
        }
        #endregion 获得用户会话状态数据的数据项的值

        #region 设置用户会话状态数据的数据项
        /// <summary>
        /// 设置用户会话状态数据的数据项
        /// </summary>
        /// <param name="SID">用户SessionID</param>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public void SetItem(string SID, string Key, object Value)
        {
            Dictionary<string, object> Session = GetSession(SID);
            if (Session.ContainsKey(Key))
            {
                Session[Key] = Value;
            }
            else
            { 
                Session.Add(Key, Value);
            }
            _Cache[SID] = Session;
        }
        #endregion 设置用户会话状态数据的数据项

        #region 移除用户会话状态数据的数据项
        /// <summary>
        /// 移除用户会话状态数据的数据项
        /// </summary>
        /// <param name="SID">用户SessionID</param>
        /// <param name="Key">键</param>
        public void RemoveItem(string SID, string Key)
        {
            Dictionary<string, object> Session = GetSession(SID);
            if (Session.ContainsKey(Key))
            {
                Session.Remove(Key);
            }
            _Cache[SID] = Session;
        }
        #endregion 移除用户会话状态数据的数据项
    }
    #endregion 基于.Net缓存的会话状态策略
}