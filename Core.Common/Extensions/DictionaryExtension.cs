namespace Core.Common.Extensions {
    public static class DictionaryExtension {
        public static string GetDictionarykeyValue(this Dictionary<string, string> dictionaryList, string key) {
            if (dictionaryList != null) {
                var keyvalue = dictionaryList.Where(v => v.Key.EqualsCI(key)).Select(v => v.Value).FirstOrDefault();
                return keyvalue == null ? null : keyvalue.ToString();
            }
            return null;
        }

        public static string GetDictionarykeyValue(this Dictionary<string, object> dictionaryList, string key) {
            if (dictionaryList != null) {
                var keyvalue = dictionaryList.Where(v => v.Key.EqualsCI(key)).Select(v => v.Value).FirstOrDefault();
                return keyvalue == null ? null : keyvalue.ToString();
            }
            return null;
        }

        public static bool CheckDictionarykeyExist(this Dictionary<string, object> dictionaryList, string key) {
            if (dictionaryList != null) {
                return dictionaryList.Any(v => v.Key.EqualsCI(key));
            }
            return false;
        }

        public static bool CheckDictionarykeyExist(this Dictionary<string, string> dictionaryList, string key) {
            if (dictionaryList != null) {
                return dictionaryList.Any(v => v.Key.EqualsCI(key));
            }
            return false;
        }

        public static Dictionary<string, object> AddDictionarykey(this Dictionary<string, object> dictionaryList, string key, string value) {
            if (dictionaryList != null) {
                if (!dictionaryList.Any(v => v.Key.EqualsCI(key))) {
                    dictionaryList.Add(key, value);
                }
            }
            return dictionaryList;
        }

        public static Dictionary<string, object> UpdateDictionarykey(this Dictionary<string, object> dictionaryList, string key, string value) {
            if (dictionaryList != null) {
                if (dictionaryList.Any(v => v.Key.EqualsCI(key))) {
                    var keyname = dictionaryList.Where(v => v.Key.EqualsCI(key)).Select(v => v.Key).First();
                    dictionaryList[keyname] = value;
                }
            }
            return dictionaryList;
        }

        public static Dictionary<string, string> AddORUpdateDictionarykey(this Dictionary<string, string> dictionaryList, string key, string value) {
            if (dictionaryList != null) {
                if (dictionaryList.Any(v => v.Key.EqualsCI(key))) {
                    var keyname = dictionaryList.Where(v => v.Key.EqualsCI(key)).Select(v => v.Key).First();
                    dictionaryList[keyname] = value;
                }
                else {
                    dictionaryList.Add(key, value);
                }
            }
            return dictionaryList;
        }

        public static Dictionary<string, string> AddORUpdateDictionarykey(this Dictionary<string, string> dictionaryList, string key, object value) {
            if (dictionaryList != null) {
                if (dictionaryList.Any(v => v.Key.EqualsCI(key))) {
                    var keyname = dictionaryList.Where(v => v.Key.EqualsCI(key)).Select(v => v.Key).First();
                    dictionaryList[keyname] = value == null ? null : Convert.ToString(value);
                }
                else {
                    dictionaryList.Add(key, value == null ? null : Convert.ToString(value));
                }
            }
            return dictionaryList;
        }

        public static Dictionary<string, object> AddORUpdateDictionaryObjectkey(this Dictionary<string, object> dictionaryList, string key, object value) {
            if (dictionaryList != null) {
                if (dictionaryList.Any(v => v.Key.EqualsCI(key))) {
                    var keyname = dictionaryList.Where(v => v.Key.EqualsCI(key)).Select(v => v.Key).First();
                    dictionaryList[keyname] = value;
                }
                else {
                    dictionaryList.Add(key, value);
                }
            }
            return dictionaryList;
        }

        public static Dictionary<string, object> AddOrUpdateDictionaryPhoneKey(this Dictionary<string, object> dictionaryList, string key) {
            string phoneValue = string.Empty;
            try {
                if (dictionaryList != null) {
                    if (dictionaryList.Any(v => v.Key.EqualsCI(key))) {
                        var phoneValueobject = dictionaryList.Where(v => v.Key.EqualsCI(key)).Select(v => v.Value).FirstOrDefault();
                        if (phoneValueobject != null) {
                            phoneValue = phoneValueobject.ToString();
                            if (phoneValue.Length < 5) {
                                dictionaryList[key] = null;
                            }
                        }
                    }
                }
            } catch { }
            return dictionaryList;
        }

        //public static class DictionaryExtensions {
        //	public static Dictionary<string, object> AddOrUpdateDateTime(this Dictionary<string, object> dictionary, string key, DateTime value) {
        //		if (dictionary == null) {
        //			dictionary = new Dictionary<string, object>();
        //		}
        //		dictionary[key] = value.ToIso8601DateTimeString();
        //		return dictionary;
        //	}
        //}

        public static Dictionary<string, object> AddOrUpdateIso8601DateTime(this Dictionary<string, object> dictionary, string key, DateTime value) {
            if (dictionary == null) {
                dictionary = new Dictionary<string, object>();
            }
            if (dictionary.Any(v => v.Key.EqualsCI(key))) {
                var keyname = dictionary.Where(v => v.Key.EqualsCI(key)).Select(v => v.Key).First();
                dictionary[keyname] = value.SFDateTimeFormat();
            }
            return dictionary;
        }



        public static bool CheckDictionarykeyExistStringObject(this Dictionary<string, object> dictionaryList, string key) {
            if (dictionaryList != null) {
                return dictionaryList.Any(v => v.Key.EqualsCI(key));
            }
            return false;
        }

        public static string GetDictionarykeyValueStringObject(this Dictionary<string, object> dictionaryList, string key) {
            if (dictionaryList != null) {
                var keyvalue = dictionaryList.Where(v => v.Key.EqualsCI(key)).Select(v => v.Value).FirstOrDefault();
                if (keyvalue != null)
                    return keyvalue.ToString();
            }
            return null;
        }

        public static JArray GetDictionarykeyValueStringObjectToArray(this Dictionary<string, object> dictionaryList, string key) {
            if (dictionaryList != null) {
                var keyvalue = dictionaryList.Where(v => v.Key.EqualsCI(key) && (v.Value != null)).Select(v => v.Value).FirstOrDefault();
                if (keyvalue != null) {
                    return keyvalue.ToJArray();
                }

            }
            return null;
        }

        public static bool CheckIDictionarykeyExistStringJToken(this IDictionary<string, JToken> dictionaryList, string key) {
            if (dictionaryList != null) {
                return dictionaryList.Any(v => v.Key.EqualsCI(key));
            }
            return false;
        }

        public static bool CheckIDictionarykeyExistStringJToken(this Dictionary<Type, Func<long, object>> dictionaryList, Type key) {
            if (dictionaryList != null) {
                return dictionaryList.Any(v => v.Key.ToDynamic().EqualsCI(key));
            }
            return false;
        }

    }
}
