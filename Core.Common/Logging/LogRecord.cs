using Core.Common.Data;

namespace Core.Common.Logging {
    [Alias(Name = "AppLog")]
    public class LogRecord {
        [Key(AutoNumber = true)]
        public int Id { get; set; }
        public DateTime AddedOn { get; set; }
        public string Message { get; set; }
        public string LogType { get; set; }
        public string Source { get; set; }
    }
}
