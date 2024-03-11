namespace Core.Common.Data {
    public class ApiUsageLogRequest {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Body { get; set; }
        public string Url { get; set; }
        public string CallerIp { get; set; }
        public DateTime RequestDate { get; set; }
        public string Response { get; set; }
        public string Headers { get; set; }
    }
}
