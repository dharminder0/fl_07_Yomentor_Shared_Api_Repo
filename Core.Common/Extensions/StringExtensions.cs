using Core.Common.ExternalApp;

namespace Core.Common.Extensions {
    public static class StringExtensions {

        public static int ToInt(this string input) {
            var n = 0;
            int.TryParse(input, out n);
            return n;
        }

        public static bool ToBool(this string input) {
            var n = false;
            bool.TryParse(input, out n);
            return n;
        }

        public static List<string> StringToList(this string input) {
            return !string.IsNullOrWhiteSpace(input) ? input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
        }

        public static bool EqualsCI(this string input, string comparedInput) {
            return !string.IsNullOrEmpty(input) && input.Equals(comparedInput, StringComparison.OrdinalIgnoreCase);
        }

        public static bool ContainsCI(this string source, string toCheck) {
            return !string.IsNullOrEmpty(source) && source.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        //public static DateTime? AsDate(this string input) {
        //    if (!string.IsNullOrWhiteSpace(input)) {
        //        DateTime date = DateTime.Now;
        //        if (DateTime.TryParse(input, out date))
        //            return date;
        //    }
        //    return null;
        //}


        //public static DateTime? AsDate(this string input) {
        //    if (!string.IsNullOrWhiteSpace(input)) {
        //        DateTime date = new DateTime();
        //        DateTime.TryParse(input, out date);

        //        if (date == DateTime.MinValue || date == default(DateTime) || (date != null && date.ToString() == "0001-01-01T00:00:00")) {
        //            CultureInfo provider = CultureInfo.InvariantCulture;
        //            bool isSuccess6 = DateTime.TryParseExact(input, new string[] {
        //                            "dd/MM/yyyy","d/MM/yyyy","d/MM/yyyy" ,"dd/MM/yyyy hh:mm:ss tt","d/MM/yyyy hh:mm:ss tt","d/M/yyyy hh:mm:ss tt"
        //                            ,"dd/MM/yyyy h:mm:ss tt","d/MM/yyyy h:mm:ss tt","d/M/yyyy h:mm:ss tt","dd/MM/yyyy h:m:ss tt","d/MM/yyyy h:m:s tt","d/M/yyyy h:mm:s tt"
        //                            ,"dd/MM/yyyy hh:m:ss tt","d/MM/yyyy hh:m:s tt","d/M/yyyy hh:mm:s tt",

        //                            "dd-MM-yyyy","d-MM-yyyy","d-MM-yyyy" ,"dd-MM-yyyy hh:mm:ss tt","d-MM-yyyy hh:mm:ss tt","d-M-yyyy hh:mm:ss tt"
        //                            ,"dd-MM-yyyy h:mm:ss tt","d-MM-yyyy h:mm:ss tt","d-M-yyyy h:mm:ss tt","dd-MM-yyyy h:m:ss tt","d-MM-yyyy h:m:s tt","d-M-yyyy h:mm:s tt"
        //                            ,"dd-MM-yyyy hh:m:ss tt","d-MM-yyyy hh:m:s tt","d-M-yyyy hh:mm:s tt",

        //                            "dd.MM.yyyy","d.MM.yyyy","d.MM.yyyy" ,"dd.MM.yyyy hh:mm:ss tt","d.MM.yyyy hh:mm:ss tt","d.M.yyyy hh:mm:ss tt"
        //                            ,"dd.MM.yyyy h:mm:ss tt","d.MM.yyyy h:mm:ss tt","d.M.yyyy h:mm:ss tt","dd.MM.yyyy h:m:ss tt","d.MM.yyyy h:m:s tt","d.M.yyyy h:mm:s tt"
        //                            ,"dd.MM.yyyy hh:m:ss tt","d.MM.yyyy hh:m:s tt","d.M.yyyy hh:mm:s tt",

        //                            "yyyy-MM-ddTHH:mm:ss.fffZ"

        //                            }, provider, DateTimeStyles.None, out date);

        //            return date;

        //        }
        //        else {
        //            return date;
        //        }
        //    }
        //    return null;
        //}


        public static string IfThen(this string input, string compareVal, string thenVal) {
            if (input == compareVal)
                return thenVal;
            return input;
        }

        public static string IfNull(this string input, string thenVal) {
            if (string.IsNullOrWhiteSpace(input))
                return thenVal;
            return input;
        }

        public static T IfNull<T>(this string input, T trueValue, T falseValue) {
            if (string.IsNullOrWhiteSpace(input))
                return trueValue;
            else
                return falseValue;
        }

        public static string ToCamelCaseName(this string str) {
            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }


        public static string Times(this string txt, int count) {
            var output = string.Empty;
            for (int i = 0; i < count; i++) {
                output += txt;
            }
            return output;
        }

        public static T Deserialize<T>(this string txt) {
            return JsonConvert.DeserializeObject<T>(txt);
        }

        public static string CreateExternalContactId(this string contactid, string clientCode, string ContactType, bool externalContact, string triggerLocation) {
            if (!externalContact) return contactid;
            if (string.IsNullOrWhiteSpace(contactid)) return contactid;
            if (contactid.ContainsCI("SF-")) return contactid;
            if (string.IsNullOrWhiteSpace(ContactType)) return contactid;
            string Id = string.Empty;
            if (contactid.ContainsCI("UNK-")) {
                return contactid;
            }
            if (contactid.ContainsCI(AbbreviationsKey.UnknownAutomationContactAbbreviation)) {
                return contactid;
            }
            if (triggerLocation.EqualsCI("SF")) //  Entities.DTOs.Enums.TriggerLocation.SalesforceLocation
            {
                Id = "SF-";
            }
            if (ContactType.EqualsCI("lead") || ContactType.EqualsCI("application")) //LeadType.lead
            {
                return string.Format("{0}{1}", Id, contactid);
            }
            else {
                contactid = string.Format("{0}EC-{1}", Id, contactid);
                return contactid;
            }
        }

        public static string ExtractExternalContactId(this string contactid) {
            if (string.IsNullOrWhiteSpace(contactid)) return contactid;
            if (contactid.Contains("SF-")) {
                contactid = contactid.Replace("SF-", string.Empty);
            }
            if (contactid.Contains("EC-")) {
                contactid = contactid.Replace("EC-", string.Empty);
            }

            return contactid;
        }

        public static string RegexReplace(this string message, string item, string replacedItem) {
            if (string.IsNullOrWhiteSpace(message)) {
                return message;
            }
            if (string.IsNullOrWhiteSpace(replacedItem)) {
                replacedItem = string.Empty;
            }
            message = System.Text.RegularExpressions.Regex.Replace(message, item, replacedItem, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return message;


        }
    }
}
