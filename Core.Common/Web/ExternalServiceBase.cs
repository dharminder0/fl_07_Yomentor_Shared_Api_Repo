namespace Core.Common.Web {
    public abstract class ExternalServiceBase {
        protected HttpService _httpService;
        public ExternalServiceBase(string rootUrl, string authorizationHeader) {
            _httpService = new HttpService(rootUrl);
            _httpService.AddHeader("Authorization", authorizationHeader);
            _httpService.AddHeader("ApiSecret", authorizationHeader);
        }
    }
}
