namespace Core.Common.Caching {
    public static class AppLocalCache {
        private static Dictionary<string, CacheObject> _cache = new Dictionary<string, Caching.CacheObject>();
        //private static bool _isCacheEnabled = ConfigurationManager.AppSettings["AppLocalCacheEnabled"]?.ToBool() ?? false;
        //private static int _defaultCacheHours = ConfigurationManager.AppSettings["DefaultAppLocalCacheHours"]?.ToInt() ?? 5;
        private static bool _isCacheEnabled = GlobalSettings._isCacheEnabled;
        private static int _defaultCacheHours = GlobalSettings._defaultCacheHours;


        public static void Add(string key, CacheObject obj) {
            key = key.ToLower();
            lock (_cache) {
                if (_cache.ContainsKey(key)) {
                    _cache.Remove(key);
                }
                _cache.Add(key, obj);
            }
        }

        private static void Add<T>(string key, CacheObject<T> obj) {
            if (!_isCacheEnabled) return;
            lock (_cache) {
                if (_cache.ContainsKey(key)) {
                    _cache.Remove(key);
                }
                _cache.Add(key, obj);
            }
        }

        private static CacheObject<T> Get<T>(string key) {
            if (!_isCacheEnabled) return null;
            if (!_cache.ContainsKey(key))
                return null;
            if (_cache[key].ExpireDate < DateTime.Now) {
                Remove(key);
                return null;
            }
            return (CacheObject<T>)_cache[key];
        }

        public static bool KeyExist(string key) {
            key = key.ToLower();
            if (!_isCacheEnabled) return false;
            lock (_cache) {
                if (_cache.ContainsKey(key))
                    return true;

            }
            return false;
        }

        public static CacheObject Get(string key) {
            key = key.ToLower();
            if (!_isCacheEnabled) return null;
            if (!_cache.ContainsKey(key))
                return null;
            if (_cache[key].ExpireDate < DateTime.Now) {
                Remove(key);
                return null;
            }
            return _cache[key];
        }

        public static void RemoveLikeKeys(string key) {
            try {
                key = key.ToLower();
                if (!_isCacheEnabled) return;
                lock (_cache) {
                    foreach (var item in _cache.Keys.ToList()) {
                        if (item.Contains(key)) {
                            Remove(item);
                        }
                    }
                }

            } catch (Exception) {
            }
        }

        public static void Remove(string key) {
            key = key.ToLower();
            if (!_isCacheEnabled) return;
            lock (_cache) {
                if (_cache.ContainsKey(key))
                    _cache.Remove(key);
            }
        }

        public static void RemoveByClientcode(string clientcode) {
            clientcode = clientcode.ToUpper();
            var cache = GetAllCahedObjects();
            if (!_isCacheEnabled) return;

            foreach (var key in cache) {
                try {
                    dynamic jsonObject = key.Value;
                    string resposeClientCode = jsonObject.Data.ClientCode;
                    if (!string.IsNullOrWhiteSpace(resposeClientCode)) {
                        if (resposeClientCode.EqualsCI(clientcode)) {
                            _cache.Remove(key.Key);
                        }
                    }
                } catch (Exception ex) { }
            }
        }

        public static void Remove(IEnumerable<string> keys) {
            if (!_isCacheEnabled) return;
            lock (_cache) {
                foreach (var key in keys) {
                    if (_cache.ContainsKey(key.ToLower()))
                        _cache.Remove(key.ToLower());
                }
            }
        }

        public static void Clear() {
            if (!_isCacheEnabled) return;
            lock (_cache) {
                _cache.Clear();
            }
        }

        public static IEnumerable<string> GetAllKeys() {
            return _cache.Keys;
        }

        public static bool ContainsKey(string key) {

            return _cache.ContainsKey(key);
        }

        public static T GetOrCache<T>(string key, Func<T> f) {
            key = key.ToLower();
            return GetOrCache(key, _defaultCacheHours * 60, f);
        }

        public static T GetOrCache<T>(string key, int minutes, Func<T> f) {
            key = key.ToLower();
            var result = Get<T>(key);
            if (result == null) {
                var data = f();
                if (data != null) {
                    Add<T>(key, new CacheObject<T> { ExpireDate = DateTime.Now.AddMinutes(minutes), Data = data });
                }
                return data;
            }
            return result.Data;
        }

        public static T GetOrCacheSecond<T>(string key, int seconds, Func<T> f) {
            key = key.ToLower();
            var result = Get<T>(key);
            if (result == null) {
                var data = f();
                if (data != null) {
                    Add<T>(key, new CacheObject<T> { ExpireDate = DateTime.Now.AddSeconds(seconds), Data = data });
                }
                return data;
            }
            return result.Data;
        }

        public static Dictionary<string, CacheObject> GetAllCahedObjects() {
            return _cache;
        }
    }
}
