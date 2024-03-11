using ElmahCore;
using System.Text.RegularExpressions;
using System.Threading;

namespace Core.Common.Logging {
    public static class ElmahErrorLog {
        private static bool _cleanring = false;
        //private static bool _logEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["ElmahLogEnabled"]);
        //private static bool _infoLogEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["ElmahInfoLogEnabled"]);

        private static bool _logEnabled = GlobalSettings.ElmahLogEnabled;
        private static bool _infoLogEnabled = GlobalSettings.ElmahInfoLogEnabled;

        /// <summary>
        /// Log error to Elmah
        /// </summary>
        public static void LogError(Exception ex) {
            if (_logEnabled) {
                try {
                    ElmahExtensions.RaiseError(ex);

                    ClearOldLogs();
                } catch {
                    // uh oh! just keep going
                }
            }
        }

        public static void LogInfo(string message) {
            if (_logEnabled && _infoLogEnabled) {
                try {
                    ElmahExtensions.RaiseError(new InfoException(message));
                    ClearOldLogs();
                } catch {
                    // uh oh! just keep going
                }
            }
        }

        public static void ClearOldLogs() {
            if (_cleanring) return;
            _cleanring = true;
            ThreadPool.QueueUserWorkItem(state => {
                try {
                    // deleting old logs
                    var appRoot = GetApplicationRoot();
                    var directoryInfo = new DirectoryInfo(Path.Combine(appRoot, "ElmahLog"));
                    if (directoryInfo.Exists) {
                        var old = DateTime.UtcNow.AddMonths(-1);
                        var fileSystemInfoArray = directoryInfo.GetFiles("error-*.xml");
                        foreach (var fileInfo in fileSystemInfoArray) {
                            if (fileInfo.CreationTimeUtc < old && fileInfo.LastAccessTimeUtc < old && fileInfo.LastWriteTimeUtc < old) {
                                fileInfo.Delete();
                            }
                        }
                    }
                } catch {
                    // uh oh! just keep going
                } finally {
                    _cleanring = false;
                }
            });
        }

        public static void ClearAllLogs() {
            ThreadPool.QueueUserWorkItem(state => {
                try {
                    // deleting old logs
                    var appRoot = GetApplicationRoot();
                    var directoryInfo = new DirectoryInfo(Path.Combine(appRoot, "ElmahLog"));
                    if (directoryInfo.Exists) {
                        var fileSystemInfoArray = directoryInfo.EnumerateFiles("error-*.xml");
                        foreach (var fileInfo in fileSystemInfoArray) {
                            fileInfo.Delete();
                        }
                    }
                } catch {
                    // uh oh! just keep going
                }
            });
        }

        public static string GetApplicationRoot() {
            var exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            return appRoot;
        }
    }

    public class InfoException : Exception {
        public InfoException(string message) : base(message) {

        }
        public InfoException(string message, Exception innerException) : base(message, innerException) {

        }
    }
}
