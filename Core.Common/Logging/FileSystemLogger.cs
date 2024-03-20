namespace Core.Common.Logging {
    public class FileSystemLogger {
        private static readonly string _logFile = null;
        private const string _infoLogTemplate = @"
{0}
INFO: {1} 
------------------------------------------------
";
        private const string _errorLogTemplate = @"
{0}
ERROR: {1} 
------------------------------------------------
";
        //static FileSystemLogger() {
        //    //if(!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["FileLogDirectory"]))
        //        if (!string.IsNullOrWhiteSpace(GlobalSettings.FileLogDirectory))
        //            _logFile = Path.Combine(GlobalSettings.FileLogDirectory, "AppLocalLog.txt");
        //    else
        //        _logFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "AppLocalLog.txt");
        //}

        public static void LogInfo(string message) {
            File.AppendAllText(_logFile, string.Format(_infoLogTemplate, DateTime.UtcNow, message));
        }

        public static void LogError(string message) {
            File.AppendAllText(_logFile, string.Format(_errorLogTemplate, DateTime.UtcNow, message));
        }

        public static void LogError(Exception error) {
            var message = string.Empty;
            var indent = "\t";
            var count = 0;
            do {
                message += $@"
{indent.Times(count++)}{error.ToString()}: {error.Message}
";
                error = error.InnerException;
            } while (error != null);
            File.AppendAllText(_logFile, string.Format(_errorLogTemplate, DateTime.UtcNow, message));
        }
    }
}
