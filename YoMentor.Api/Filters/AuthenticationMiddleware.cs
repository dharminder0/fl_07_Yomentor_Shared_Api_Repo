
using Hub.Common.Settings;
using Microsoft.AspNetCore.Http;

using System.Text;


namespace Hub.Web.Api.Filters {
    public class AuthenticationMiddleware {
        private readonly RequestDelegate _next;
        private const string CookieName = "ElmahBasicAuthorization";

        public AuthenticationMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task Invoke(HttpContext context) {
            //var userName = ConfigurationManager.AppSettings["ElmahBasicAuth.UserName"];
            var userName = GlobalSettings.ElmahBasicUserName;
            //var password = ConfigurationManager.AppSettings["ElmahBasicAuth.Password"];
            var password = GlobalSettings.ElmahBasicUserPassword;
            //var elmah = ConfigurationManager.AppSettings["elmah.mvc.route"];
            var elmah = GlobalSettings.ElmahMvcRoute;
            if (string.IsNullOrWhiteSpace(userName) && string.IsNullOrWhiteSpace(password)) {
                await _next.Invoke(context);
            }
            else if (!context.Request.Path.StartsWithSegments("/" + elmah, StringComparison.OrdinalIgnoreCase)) {
                await _next.Invoke(context);
            }
            else {
                var authHeader = string.Empty;

                // see if the user already basic authorized
                var cookie = context.Request.Cookies[CookieName];
                if (cookie != null && !string.IsNullOrWhiteSpace(cookie)) {
                    authHeader = cookie;
                }
                else {
                    // See if they've supplied credentials
                    authHeader = context.Request.Headers["Authorization"];
                }

                // decode basic auth
                if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Basic")) {
                    // Parse username and password out of the HTTP headers
                    var authHeaderDecoded = authHeader.Substring("Basic".Length).Trim();
                    var authHeaderBytes = Convert.FromBase64String(authHeaderDecoded);
                    authHeaderDecoded = Encoding.UTF8.GetString(authHeaderBytes);
                    var authUserName = authHeaderDecoded.Split(':')[0];
                    var authPassword = authHeaderDecoded.Split(':')[1];

                    // Validate login attempt
                    if (userName.Equals(authUserName, StringComparison.OrdinalIgnoreCase) && password.Equals(authPassword)) {
                        context.Response.Cookies.Append(CookieName, authHeader);
                        await _next.Invoke(context);
                    }
                }

                // Force the browser to pop up the login prompt
                context.Response.StatusCode = 401;
                context.Response.Headers.Add("WWW-Authenticate", "Basic");
            }
        }
    }
}
