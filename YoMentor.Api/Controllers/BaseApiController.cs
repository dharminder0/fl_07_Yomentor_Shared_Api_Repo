





using Hub.Common.Settings;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

namespace Hub.Web.Api.Controllers {
    public abstract class BaseApiController : ControllerBase {

        protected IActionResult JsonExt<T>(T content) {
            var jsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings() {
                ContractResolver = new DefaultContractResolver {
                    NamingStrategy = new CamelCaseNamingStrategy { ProcessDictionaryKeys = true }
                }
            };
            return new JsonResult(content, jsonSerializerSettings);
        }



        protected IActionResult JsonExt2<T>(T content) {
            var jsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings() {
                ContractResolver = new DefaultContractResolver {
                    NamingStrategy = new CamelCaseNamingStrategy { ProcessDictionaryKeys = true }
                }
            };
            return new JsonResult(content, jsonSerializerSettings);
        }

        protected IActionResult JsonExt3<T>(T content) {
            var jsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings();

            return new JsonResult(content, jsonSerializerSettings);
        }

        protected BadRequestErrorMessageResult BadRequest(Exception error) {
            string message = "";
            var ex = error;
            while (ex != null) {
                message += string.Format("{0}: {1}\r\n", ex.ToString(), ex.Message);
                ex = ex.InnerException;
            }
            return new BadRequestErrorMessageResult(message);
        }

        //protected string GetUserTokenFromHeader() {
        //    try {

        //        var token = Request.Headers["token"];
        //        var symmetricSecretKey = ConfigurationManager.AppSettings["SymmetricSecretKey"];
        //        var symmetricSecretKey = GlobalSettings.SymmetricSecretKey;
        //        var decryptedToken = JwtSecurityService.Decrypt(symmetricSecretKey, token);
        //        if (!string.IsNullOrWhiteSpace(decryptedToken)) {
        //            var userToken = JwtSecurityService.Decode(decryptedToken);
        //            return userToken;
        //        }
        //        return token;
        //    } catch (Exception) {

        //        return null;
        //    }
        //}
        protected string GetSFInstanceHeader() {
            try {

                var token = Request.Headers["instanceurl"];
                return token;
            } catch (Exception) {

                return null;
            }
        }

        protected string GetJwtTokenFromHeader() {
            var token = Request.Headers["token"];
            return token;
        }

        protected string RequestId {
            get {
                object obj = null;
                HttpContext.Items.TryGetValue("RequestId", out obj);

                return obj.ToString();
            }
        }
    }

    public class BadRequestErrorMessageResult : BadRequestResult {
        public BadRequestErrorMessageResult(string message) {
            Message = message;
        }
        public string Message { get; set; }
    }
}
