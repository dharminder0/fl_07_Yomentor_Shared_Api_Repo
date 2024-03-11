﻿namespace Core.Common.Extensions {
    public static class JObjectExtension {
        public static JToken GetPropertyFromPath(this JToken token, string path) {
            if (token == null) {
                return null;
            }
            string[] pathParts = path.Split('.');
            JToken current = token;
            foreach (string part in pathParts) {
                current = current.GetProperty(part);
                if (current == null) {
                    return null;
                }
            }
            return current;
        }

        public static JToken GetProperty(this JToken token, string name) {
            if (token == null) {
                return null;
            }
            var obj = token as JObject;
            JToken match;
            if (obj.TryGetValue(name, StringComparison.OrdinalIgnoreCase, out match)) {
                return match;
            }
            return null;
        }


        public static JObject AddORUpdateJObjectkey(this JObject source, string key, string value) {
            if (!CheckJObjectkeyExist(source, key)) {
                source.Add(key, value);
            }
            else {
                source[key] = value;
            }
            return source;
        }

        public static bool CheckJObjectkeyExist(this JObject source, string key) {
            if (source == null)
                return false;
            var dictionaryList = source.ToObject<Dictionary<string, object>>();
            if (dictionaryList != null) {
                return dictionaryList.Any(v => v.Key.EqualsCI(key));
            }
            return false;
        }

        public static string GetJobjectValue(this JObject source, string key) {
            if (source == null)
                return null;
            var dictionaryList = source.ToObject<Dictionary<string, object>>();
            if (dictionaryList != null) {
                var keyvalue = dictionaryList.Where(v => v.Key.EqualsCI(key)).Select(v => v.Value).FirstOrDefault();
                if (keyvalue != null)
                    return keyvalue.ToString();
            }
            return null;
        }

        public static IDictionary<string, object> ToDictionary(this JObject @object) {
            var result = @object.ToObject<Dictionary<string, object>>();

            var JObjectKeys = (from r in result
                               let key = r.Key
                               let value = r.Value
                               where value.GetType() == typeof(JObject)
                               select key).ToList();

            var JArrayKeys = (from r in result
                              let key = r.Key
                              let value = r.Value
                              where value.GetType() == typeof(JArray)
                              select key).ToList();

            JArrayKeys.ForEach(key => result[key] = ((JArray)result[key]).Values().Select(x => ((JValue)x).Value).ToArray());
            JObjectKeys.ForEach(key => result[key] = ToDictionary(result[key] as JObject));

            return result;
        }

    }
}
