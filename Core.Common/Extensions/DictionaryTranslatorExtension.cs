namespace Core.Common.Extensions {
    public static class DictionaryTranslatorExtension {

        public static Dictionary<string, object> CreateObjectDictionaryTranslator(this object Object) {
            if (Object != null) {
                Dictionary<string, object> exObj = null;
                if (Object.GetType().Name.EqualsCI("Dictionary")) {
                    exObj = (Dictionary<string, object>)Object;
                    return exObj;

                }
                else if (Object.GetType().Name.EqualsCI("Dictionary`2")) {
                    exObj = (Dictionary<string, object>)Object;
                    return exObj;

                }

                else {
                    var exObjtemp = (JsonConvert.DeserializeObject<ExpandoObject>(Object.ToString()) as dynamic) as IDictionary<string, object>;
                    if (exObjtemp != null) {
                        exObj = exObjtemp.ToDictionary(k => k.Key.ToLower(), k => k.Value);
                        return exObj;
                    }
                }
            }
            return null;
        }
    }
}
