using System.Globalization;

namespace Core.Common.Extensions {
    public static class DateTimeExtensions {
        public static double GetAmsterdamUTCTimeOffset(this DateTime date) {
            var wEurope = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            var utcTime = date.ToUniversalTime();
            var offSet = wEurope.GetUtcOffset(utcTime);
            return offSet.TotalHours;
        }

        public static DateTime ToAmsterdamTime(this DateTime date) {
            var wEurope = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            var utcTime = date.ToUniversalTime();
            var offSet = wEurope.GetUtcOffset(utcTime);
            return date.AddHours(offSet.TotalHours);
        }

        public static DateTime FromUtcToAmsterdamTime(this DateTime date) {
            var wEurope = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            var utcTime = TimeZoneInfo.ConvertTimeFromUtc(date, wEurope);
            return utcTime;
        }

        public static DateTime? CheckDateTimeNullable(DateTime? date) {
            if (date == DateTime.MinValue || date == default(DateTime)) {
                return null;
            }
            else if (date.ToString() == "0001-01-01T00:00:00") {
                return null;
            }
            else {
                return date;
            }

        }

        public static string SFDateTimeFormat(this DateTime dateTime) {
            if (dateTime == DateTime.MinValue || dateTime == default(DateTime)) {
                return null;
            }
            else if (dateTime.ToString() == "0001-01-01T00:00:00") {
                return null;
            }

            return dateTime.ToString("yyyy-MM-dd'T'HH:mm:ss");
        }
        public static string SFDateFormat(this DateTime dateTime) {
            if (dateTime == DateTime.MinValue || dateTime == default(DateTime)) {
                return null;
            }
            else if (dateTime.ToString() == "0001-01-01T00:00:00") {
                return null;
            }

            return dateTime.ToString("yyyy-MM-dd");
        }



        public static DateTime? AsDate(this string value) {



            DateTime? datetimeExact = null;
            if (!value.ContainsCI("/")) {
                var date = DateTimeExtensions.NormalParseStringToDate(value);
                if (date != null && date.HasValue) {
                    datetimeExact = date.Value;
                }
            }

            if (!datetimeExact.HasValue) {
                var date = DateTimeExtensions.DateFormatParseStingToDate(value);
                if (date != null && date.HasValue) {
                    datetimeExact = date.Value;
                }
            }

            return DateTimeExtensions.CheckDateTimeNullable(datetimeExact);
        }



        public static DateTime? NormalParseStringToDate(string value) {
            try {
                DateTime date = new DateTime();
                DateTime.TryParse(value, out date);
                return CheckDateTimeNullable(date);
            } catch (Exception) {

                return null;
            }


        }

        public static DateTime? DateFormatParseStingToDate(string value) {
            DateTime date = new DateTime();
            CultureInfo provider = CultureInfo.InvariantCulture;
            try {
                bool isSuccess6 = DateTime.TryParseExact(value, new string[] {
                                    "dd/MM/yyyy","d/MM/yyyy","d/MM/yyyy" ,"dd/MM/yyyy hh:mm:ss tt","d/MM/yyyy hh:mm:ss tt","d/M/yyyy hh:mm:ss tt"
                                    ,"dd/MM/yyyy h:mm:ss tt","d/MM/yyyy h:mm:ss tt","d/M/yyyy h:mm:ss tt","dd/MM/yyyy h:m:ss tt","d/MM/yyyy h:m:s tt","d/M/yyyy h:mm:s tt"
                                    ,"dd/MM/yyyy hh:m:ss tt","d/MM/yyyy hh:m:s tt","d/M/yyyy hh:mm:s tt",

                                    "dd-MM-yyyy","d-MM-yyyy","d-MM-yyyy" ,"dd-MM-yyyy hh:mm:ss tt","d-MM-yyyy hh:mm:ss tt","d-M-yyyy hh:mm:ss tt"
                                    ,"dd-MM-yyyy h:mm:ss tt","d-MM-yyyy h:mm:ss tt","d-M-yyyy h:mm:ss tt","dd-MM-yyyy h:m:ss tt","d-MM-yyyy h:m:s tt","d-M-yyyy h:mm:s tt"
                                    ,"dd-MM-yyyy hh:m:ss tt","d-MM-yyyy hh:m:s tt","d-M-yyyy hh:mm:s tt","dd-MM-yyyy HH:mm:ss",

                                    "dd.MM.yyyy","d.MM.yyyy","d.MM.yyyy" ,"dd.MM.yyyy hh:mm:ss tt","d.MM.yyyy hh:mm:ss tt","d.M.yyyy hh:mm:ss tt"
                                    ,"dd.MM.yyyy h:mm:ss tt","d.MM.yyyy h:mm:ss tt","d.M.yyyy h:mm:ss tt","dd.MM.yyyy h:m:ss tt","d.MM.yyyy h:m:s tt","d.M.yyyy h:mm:s tt"
                                    ,"dd.MM.yyyy hh:m:ss tt","d.MM.yyyy hh:m:s tt","d.M.yyyy hh:mm:s tt",

                                    "yyyy-MM-ddTHH:mm:ss.fffZ"

                                    }, provider, DateTimeStyles.None, out date);
                if (isSuccess6) {
                    return CheckDateTimeNullable(date);
                }
            } catch (Exception) {
            }

            return null;
        }
        private static void AddPropertyJObject(JObject jObj, string propertyName, JToken value) {
            if (jObj != null && jObj[propertyName] == null) {
                jObj.Add(propertyName, value);
            }
            else if (jObj != null && jObj[propertyName] != null) {
                jObj[propertyName] = value;
            }
        }

        public static string ElasticDateTimeFormat(this DateTime dateTime) {
            if (dateTime == DateTime.MinValue || dateTime == default(DateTime)) {
                return null;
            }
            else if (dateTime.ToString() == "0001-01-01T00:00:00") {
                return null;
            }


            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ");
        }
    }
}