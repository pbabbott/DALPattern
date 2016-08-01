using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace MyDotNetPatterns.Lib.CachingPattern.Core
{
    public class InMemoryCache
    {
        #region Properties

        private int _DefaultCacheTimeInHours { get; set; }
        private ObjectCache _Cache { get; set; }

        #endregion Properties

        #region Constructor

        protected InMemoryCache(int defaultCacheTimeInHours)
        {
            this._DefaultCacheTimeInHours = 4;
            this._Cache = MemoryCache.Default;
        }

        #endregion Constructor

        #region Methods

        public void AddOrUpdateItem(string key, object value, TimeSpan lifeTime)
        {
            if (this.ContainsKey(key))
            {
                this._Cache.Add(key, value, new CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTime.Now + lifeTime
                });
            }
            else
            {
                this._Cache[key] = value;
            }
        }

        public void AddOrUpdateItem(string key, object value)
        {
            AddOrUpdateItem(key, value, new TimeSpan(_DefaultCacheTimeInHours, 0, 0));
        }

        public T GetItem<T> (string key)
        {
            if(this.ContainsKey(key))
            {
                return (T)this._Cache[key];
            }
            else
            {
                return default(T);
            }
        }

        public T GetAndRemoveItem<T>(string key)
        {
            if (this.ContainsKey(key))
            {
                T result = (T)this._Cache[key];
                this.RemoveItem(key);
                return result;
            }
            return default(T);
        }

        public bool ContainsKey(string key)
        {
            return this._Cache.Contains(key);
        }

        public void Clear()
        {
            foreach (KeyValuePair<string, object> item in this._Cache)
            {
                this.RemoveItem(item.Key);
            }
        }

        public void RemoveItem(string key)
        {
            if (ContainsKey(key))
            {
                this._Cache.Remove(key);
            }
        }

        #endregion Methods

        #region Static

        private static InMemoryCache _Instance;

        public static InMemoryCache Instance(int defaultCacheTimeInHours = 4)
        {
            if (_Instance == null)
            {
                _Instance = new InMemoryCache(defaultCacheTimeInHours);
            }
            return _Instance;
        }

        #endregion Static
    }
}