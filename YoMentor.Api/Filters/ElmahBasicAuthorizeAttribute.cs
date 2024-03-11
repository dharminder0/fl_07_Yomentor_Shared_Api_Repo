//
//
//
//using System.Configuration;
//
//using System.Net;
//using System.Text;
//
//using System.Web.Mvc;

namespace Hub.Web.Api.Filters {
    //public class ElmahBasicAuthorizeAttribute : AuthorizeAttribute {
    //    private const string CookieName = "ElmahBasicAuthorization";

    //    public override void OnAuthorization(AuthorizationContext filterContext) {
    //        var userName = ConfigurationManager.AppSettings["ElmahBasicAuth.UserName"];
    //        var password = ConfigurationManager.AppSettings["ElmahBasicAuth.Password"];
    //        var elmah = ConfigurationManager.AppSettings["elmah.mvc.route"];

    //        if (string.IsNullOrWhiteSpace(userName) && string.IsNullOrWhiteSpace(password)) {
    //            return;
    //        }
    //        else if (!filterContext.HttpContext.Request.RawUrl.StartsWith("/" + elmah, StringComparison.OrdinalIgnoreCase)) {
    //            return;
    //        }
    //        else {
    //            var authHeader = string.Empty;

    //            // see if the user already basic authorized
    //            var cookie = filterContext.HttpContext.Request.Cookies[CookieName];
    //            if (!string.IsNullOrWhiteSpace(cookie?.Value)) {
    //                authHeader = cookie.Value;
    //            }
    //            else {
    //                // See if they've supplied credentials
    //                authHeader = filterContext.HttpContext.Request.Headers["Authorization"];
    //            }

    //            // decode basic auth
    //            if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Basic")) {
    //                // Parse username and password out of the HTTP headers
    //                var authHeaderDecoded = authHeader.Substring("Basic".Length).Trim();
    //                var authHeaderBytes = Convert.FromBase64String(authHeaderDecoded);
    //                authHeaderDecoded = Encoding.UTF7.GetString(authHeaderBytes);
    //                var authUserName = authHeaderDecoded.Split(':')[0];
    //                var authPassword = authHeaderDecoded.Split(':')[1];

    //                // Validate login attempt
    //                if (userName.EqualsCI(authUserName)
    //                    && password.Equals(authPassword)) {
    //                    cookie = new HttpCookie(CookieName) { Value = authHeader };
    //                    filterContext.HttpContext.Response.Cookies.Add(cookie);
    //                    return;
    //                }
    //            }

    //            // Force the browser to pop up the login prompt
    //            filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
    //            filterContext.HttpContext.Response.StatusCode = 401;
    //            filterContext.HttpContext.Response.AppendHeader("WWW-Authenticate", "Basic");

    //            // This gets shown if they click "Cancel" to the login prompt
    //            filterContext.HttpContext.Response.Write("You must log in to access this URL.");
    //        }
    //    }
    //}
}