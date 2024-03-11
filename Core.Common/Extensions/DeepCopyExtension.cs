namespace Core.Common.Extensions {
    public static class DeepCopyExtension {

        public static T DeepCopySerialization<T>(this T obj) {
            var json = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(json);
        }



    }
}
