//using Core.Common.Data;
//using Core.Common.Extensions;
//using Hub.Common.Settings;
//using System;

//namespace Core.Common.Logging {
//    public class DbLogger {
//        private static readonly DataRepository<LogRecord> _dataRepository;
//        //private static readonly bool _dbLogEnabled = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["DbLog.Enabled"]) ? Convert.ToBoolean(ConfigurationManager.AppSettings["DbLog.Enabled"]) : false;
//        private static readonly bool _dbLogEnabled = !string.IsNullOrWhiteSpace(GlobalSettings._dbLogEnabled) ? Convert.ToBoolean(GlobalSettings._dbLogEnabled) : false;

//        static DbLogger() {
//            _dataRepository = new DataRepository<LogRecord>("LogDb");
//        }

//        public static void LogInfo(string message, string source = null) {
//            if (_dbLogEnabled) {
//                _dataRepository.Add(new LogRecord {
//                    LogType = "INFO",
//                    AddedOn = DateTime.UtcNow,
//                    Message = message,
//                    Source = source
//                });
//            }
//        }

//        public static void LogError(string message, string source = null) {
//            if (_dbLogEnabled) {
//                _dataRepository.Add(new LogRecord {
//                    LogType = "ERROR",
//                    AddedOn = DateTime.UtcNow,
//                    Message = message,
//                    Source = source
//                });
//            }
//        }

//        public static void LogError(Exception error, string source = null) {
//            if (_dbLogEnabled) {
//                var message = string.Empty;
//                var indent = "\t";
//                var count = 0;
//                do {
//                    message += $@"
//{indent.Times(count++)}{error.ToString()}: {error.Message}
//";
//                    error = error.InnerException;
//                } while (error != null);
//                _dataRepository.Add(new LogRecord {
//                    LogType = "ERROR",
//                    AddedOn = DateTime.UtcNow,
//                    Message = message,
//                    Source = source
//                });
//            }
//        }
//    }
//}
