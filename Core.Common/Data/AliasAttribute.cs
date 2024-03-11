namespace Core.Common.Data {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class AliasAttribute : Attribute {
        public string Name { get; set; }
    }
    public class ActionMassegeResponse {
        public bool Response { get; set; }
        public string Message { get; set; }
        public object Content { get; set; }
    }
}
