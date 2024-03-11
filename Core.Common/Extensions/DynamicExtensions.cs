using System.Text.RegularExpressions;

namespace Core.Common.Extensions {
    public static class DynamicExtensions {
        public static string DynamicVariablesReplace(string message, Dictionary<string, object> contactObjs, Dictionary<string, object> staticobjects) {

            if (contactObjs == null && staticobjects == null) {
                return message;
            }

           

            var list = new List<string>();
            if (!string.IsNullOrWhiteSpace(message)) {
               // message = message.Replace("{{", "%").Replace("}}", "%");
                var mOdd = Regex.Matches(message, @"{{([a-zA-Z0-9.-_])+}}");
                foreach (Match match in mOdd) {
                    message= message.RegexReplace(match.Value, match.Value.Replace("{{", "%").Replace("}}", "%"));
                }
                var m1 = Regex.Matches(message, @"%([a-zA-Z0-9.-_])+%");

                foreach (Match match in m1) {
                    list.Add(match.Value.ToLower());
                }

                //var m1bracket = Regex.Matches(message, @"{{([a-zA-Z0-9.-_])+}}");

                //foreach (Match match in m1bracket) {
                //    match.Value.Replace("{{", "%").Replace("}}", "%");
                //    list.Add(match.Value.ToLower());
                //}
            }

            if (contactObjs == null) {
                contactObjs = new Dictionary<string, object>();
            }

            var exObjpair = contactObjs.ToDictionary(k => "%" + k.Key.ToLower() + "%", k => k.Value);
            exObjpair.AddDictionarykey("%fname%", exObjpair.GetDictionarykeyValueStringObject("%firstname%"));
            exObjpair.AddDictionarykey("%lname%", exObjpair.GetDictionarykeyValueStringObject("%lastname%"));
            exObjpair.AddDictionarykey("%phone%", exObjpair.GetDictionarykeyValueStringObject("%telephone%"));
            exObjpair.AddDictionarykey("%email%", exObjpair.GetDictionarykeyValueStringObject("%email%"));



            //exObjpair.AddDictionarykey("%rfname%", exObjpair.GetDictionarykeyValueStringObject("%firstname%"));
            //exObjpair.AddDictionarykey("%rphone%", exObjpair.GetDictionarykeyValueStringObject("%telephone%"));

            foreach (var item in list) {
                if (exObjpair.CheckDictionarykeyExistStringObject(item)) {
                    // message = message.Replace(item, Convert.ToString(exObjpair[item]));
                    //var ula2 = exObjpair[item];
                    //var ula2Key = item.Replace("")
                    message = Regex.Replace(message, item, Convert.ToString(exObjpair[item]), RegexOptions.IgnoreCase);
                }



                else if (staticobjects != null && staticobjects.CheckDictionarykeyExistStringObject(item)) {
                    //message = message.Replace(item, Convert.ToString(staticobjects[item]));
                    message = Regex.Replace(message, item, Convert.ToString(staticobjects[item]), RegexOptions.IgnoreCase);
                }

                //else
                //{
                //    //message = message.Replace(item, string.Empty);
                //    message = Regex.Replace(message, item, string.Empty, RegexOptions.IgnoreCase);
                //}
            }

            return message;
        }
    }
}
