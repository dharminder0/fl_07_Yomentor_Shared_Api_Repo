//namespace Core.Common {
//    public class EncryptionDecryption {
//        public static string DecryptedValue(string keyValue) {
//            keyValue = HttpUtility.UrlDecode(keyValue);
//            //var symmetricSecretKey = Core.Common.Configuration.ConfigurationManager.AppSettings["SymmetricSecretKey"];
//            var symmetricSecretKey = GlobalSettings.SymmetricSecretKey;
//            return JwtSecurityService.Decrypt(symmetricSecretKey, keyValue);

//        }

//        public static string Encryptedvalue(string keyValue) {
//            //var symmetricSecretKey = Core.Common.Configuration.ConfigurationManager.AppSettings["SymmetricSecretKey"];
//            var symmetricSecretKey = GlobalSettings.SymmetricSecretKey;
//            return HttpUtility.UrlEncode(JwtSecurityService.Encrypt(symmetricSecretKey, keyValue));
//        }
//    }
//}
