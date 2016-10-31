using System.Collections.Generic;
using System;

namespace Super.Framework
{
    #region 内置缓存管理
    /// <summary>
    /// 内置缓存管理
    /// </summary>
    public class OwnCacheManage
    {
        #region 放入缓存
        /// <summary>
        /// 放入缓存
        /// </summary>
        public static void SaveCache<T>(Dictionary<string, T[]> _ICache, Dictionary<string, DateTime> _ICacheTime, string _IKey = "", T[] _ITA = null)
        {
            if (_ITA != null && _IKey.IsNotNullOrEmpty())
            {
                lock (_ICache)
                {
                    if (!_ICache.ContainsKey(_IKey))
                    {
                        _ICache.Add(_IKey, _ITA);
                    }
                    else
                    {
                        _ICache[_IKey] = _ITA;
                    }
                    if (!_ICacheTime.ContainsKey(_IKey))
                    {
                        _ICacheTime.Add(_IKey, BaseTool.Now);
                    }
                    else
                    {
                        _ICacheTime[_IKey] = BaseTool.Now;
                    }
                }
            }
        }

        /// <summary>
        /// 放入缓存
        /// </summary>
        public static void SaveCache<T>(Dictionary<string, T> _ICache, Dictionary<string, DateTime> _ICacheTime, string _IKey, T _IT)
        {
            if (_IT != null && _IKey.IsNotNullOrEmpty())
            {
                lock (_ICache)
                {
                    if (!_ICache.ContainsKey(_IKey))
                    {
                        _ICache.Add(_IKey, _IT);
                    }
                    else
                    {
                        _ICache[_IKey] = _IT;
                    }
                    if (!_ICacheTime.ContainsKey(_IKey))
                    {
                        _ICacheTime.Add(_IKey, BaseTool.Now);
                    }
                    else
                    {
                        _ICacheTime[_IKey] = BaseTool.Now;
                    }
                }
            }
        }

        #endregion 放入缓存

        #region 清除缓存
        /// <summary>
        /// 清除缓存
        /// </summary>
        public static void ClearCache<T>(Dictionary<string, T[]> _ICache, Dictionary<string, DateTime> _ITime, int _ITimeOut = 0, string _IKey = "") where T : new()
        {
            DateTime _Now = BaseTool.Now;
            if (_IKey.IsNotNullOrEmpty() && _ICache.ContainsKey(_IKey) && _ITime.ContainsKey(_IKey))
            {
                TimeSpan _TS = _TS = _Now - _ITime[_IKey];
                int _DiffSec = _TS.Days * 86400 + _TS.Hours * 3600 + _TS.Minutes * 60 + _TS.Seconds;
                if (_DiffSec >= _ITimeOut)
                {
                    if (_ICache.ContainsKey(_IKey))
                    {
                        _ICache.Remove(_IKey);
                    }
                }
            }
            else
            {
                List<string> _DelList = new List<string>();
                foreach (string _FDelKey in _ITime.Keys)
                {
                    TimeSpan _TS = _TS = _Now - _ITime[_FDelKey];
                    int _DiffSec = _TS.Days * 86400 + _TS.Hours * 3600 + _TS.Minutes * 60 + _TS.Seconds;
                    if (_DiffSec >= _ITimeOut)
                    {
                        _DelList.Add(_FDelKey);
                    }
                }
                foreach (string _FKey in _DelList)
                {
                    _ITime.Remove(_FKey);
                    if (_ICache.ContainsKey(_FKey))
                    {
                        _ICache.Remove(_FKey);
                    }
                }
            }
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public static void ClearCache<T>(Dictionary<string, T> _ICache, Dictionary<string, DateTime> _ITime, int _ITimeOut = 0, string _IKey = "") where T : new()
        {
            DateTime _Now = BaseTool.Now;
            if (_IKey.IsNotNullOrEmpty() && _ICache.ContainsKey(_IKey) && _ITime.ContainsKey(_IKey))
            {
                TimeSpan _TS = _TS = _Now - _ITime[_IKey];
                int _DiffSec = _TS.Days * 86400 + _TS.Hours * 3600 + _TS.Minutes * 60 + _TS.Seconds;
                if (_DiffSec >= _ITimeOut)
                {
                    if (_ICache.ContainsKey(_IKey))
                    {
                        _ICache.Remove(_IKey);
                    }
                }
            }
            else
            {
                List<string> _DelList = new List<string>();
                foreach (string _FDelKey in _ITime.Keys)
                {
                    TimeSpan _TS = _TS = _Now - _ITime[_FDelKey];
                    int _DiffSec = _TS.Days * 86400 + _TS.Hours * 3600 + _TS.Minutes * 60 + _TS.Seconds;
                    if (_DiffSec >= _ITimeOut)
                    {
                        _DelList.Add(_FDelKey);
                    }
                }
                foreach (string _FKey in _DelList)
                {
                    _ITime.Remove(_FKey);
                    if (_ICache.ContainsKey(_FKey))
                    {
                        _ICache.Remove(_FKey);
                    }
                }
            }
        }
        #endregion 清除缓存
    }
    #endregion 内置缓存管理
}