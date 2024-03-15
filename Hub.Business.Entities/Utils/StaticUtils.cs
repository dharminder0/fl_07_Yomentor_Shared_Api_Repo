using System.Text.RegularExpressions;

namespace Core.Business.Entites.Utils
{
    public static class StaticUtils
    {
        public static string ExtractInnerException(this Exception ex)
        {
            string errorMessage = String.Empty;
            if (ex.Message != null)
            {
                errorMessage = $"/Msg/-{ex.Message}{Environment.NewLine}";
                if (ex.Source != null)
                {
                    errorMessage += $"/Src/-{ex.Source}{Environment.NewLine}";
                }
                if (ex.StackTrace != null)
                {
                    errorMessage += $"/Stack/-{ex.StackTrace}{Environment.NewLine}";
                }
            }
            return errorMessage;
        }


        public static string GenerateSlug(this string phrase)
        {
            var s = phrase.ToLower();
            s = Regex.Replace(s, @"[^a-z0-9\s-.]", "");                      // remove invalid characters
            s = Regex.Replace(s, @"\s+", " ").Trim();                       // single space
            s = Regex.Replace(s, @"\s", "-");                               // insert hyphens
            s = Regex.Replace(s, @"\-+", "-");
            return s.ToLower();
        }
    }
}
