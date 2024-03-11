using Core.Common.Configuration;
using Hub.Common.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;




namespace Hub.Web.Api.Filters {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class RequireAuthorization : Attribute, IAuthorizationFilter {

        /// <summary>
        /// this will have default bearer
        /// Default Bearer authorization key is (AuthorizationBearer) or pass a new BearerKey to attribute properties
        /// </summary>
        public string BearerKey { get; set; }

        /// <summary>
        /// check for authorize caller by header value
        /// </summary>
        /// <param name="actionContext"></param>
        public void OnAuthorization(AuthorizationFilterContext context) {
            // check for authorization enabled first
            //var authorizationEnabled = bool.Parse(ConfigurationManager.AppSettings["AuthorizationEnabled"]);
            var authorizationEnabled = GlobalSettings.AuthorizationEnabled;
            if (!authorizationEnabled) {
                return;
            }
            context.HttpContext.Items.Add("RequestId", Guid.NewGuid().ToString());
            //var defaultAuthorizationValue = ConfigurationManager.AppSettings["AuthorizationBearer"];
            var defaultAuthorizationValue = GlobalSettings.AuthorizationBearer;
            var bearerAutorizeKeys = !string.IsNullOrWhiteSpace(BearerKey) ? BearerKey.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) : null;
            var bearerAuthorizeValues = new List<string>();
            if (bearerAutorizeKeys != null && bearerAutorizeKeys.Any()) {
                bearerAutorizeKeys.ToList().ForEach(k => {
                    if (!string.IsNullOrWhiteSpace(Core.Common.Configuration.ConfigurationManager.AppSettings[k]))
                        bearerAuthorizeValues.Add(Core.Common.Configuration.ConfigurationManager.AppSettings[k]);
                });
            }
            if (context.HttpContext.Request.Headers.TryGetValue("authorization", out var value) && (value.First() == defaultAuthorizationValue || (bearerAuthorizeValues != null && bearerAuthorizeValues.Contains(value.First())))) {
                return;
            }

            // if public call return forbidden 403 instead of unauthorized
            if (!string.IsNullOrWhiteSpace(BearerKey)) {
                context.Result = new ForbidResult();
            }
            else {
                // if not valid bearer don't return Unauthorized (401)
                context.Result = new UnauthorizedResult();
            }
        }

        //private string GetCallerIp() {
        //    var ip = HttpContext.Current.Request.RequestContext.HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null ? HttpContext.Current.Request.RequestContext.HttpContext.Request["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.RequestContext.HttpContext.Request["REMOTE_ADDR"];
        //    switch (ip) {
        //        case "::1":
        //            return ip;//local
        //        default: //online don't include port and the double ip address and proxies the ip is the first one client1, proxy1, proxy2, ...
        //            return Regex.Replace(ip, @"((\:.*)|(\,.*))", "", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        //    }
        //}
    }
}