using Core.Common.Configuration;
using System.Collections.Specialized;

namespace Hub.Common.Settings {
    public class GlobalSettings {
        private static NameValueCollection _appSettings = ConfigurationManager.AppSettings;

        public static string AuthorizationBearers => GetString("AuthorizationBearer");
        public static bool AuthorizationEnabled => GetBool("AuthorizationEnabled");
        public static string AuthorizationBearer => GetString("AuthorizationBearer");
        public static string AllowedOrigins => GetString("AllowedOrigins");
       
        public static bool IsUserTokenEncryptionEnabled => GetBool("UserTokenEncryptionEnabled");
        public static string BlobSymmetricSecretKey = GetString("BlobSymmetricSecretKey");
        public static string JwtIssuer { get; set; }
        public static string JwtAudience { get; set; }
        public static string BlobAssessment => GetString("BlobAssessment");
        public static string BlobAssignment => GetString("BlobAssignment");
        public static string BlobStorageAccount => GetString("BlobStorageAccount");
        public static string BlobContainerName => GetString("BlobContainerName");
        public static string UserContainerName => GetString("UserContainerName");

        public static string GetKeyValues(string key) {
            return !string.IsNullOrWhiteSpace(_appSettings[key]) ? _appSettings[key] : null;
        }
        #region Utils
        private static string GetString(string key, string defaultVal = null) {
            return !string.IsNullOrWhiteSpace(_appSettings[key]) ? _appSettings[key] : defaultVal;
        }

        private static int GetInt(string key, int defaultVal = 0) {
            return !string.IsNullOrWhiteSpace(_appSettings[key]) ? Convert.ToInt32(_appSettings[key]) : defaultVal;
        }

        private static bool GetBool(string key, bool defaultVal = false) {
            return !string.IsNullOrWhiteSpace(_appSettings[key]) ? Convert.ToBoolean(_appSettings[key]) : defaultVal;
        }

        private static IEnumerable<string> GetListValues(string key, bool defaultVal = false) {
            return !string.IsNullOrWhiteSpace(_appSettings[key]) ? _appSettings[key].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
        }

        #endregion
    }
}
