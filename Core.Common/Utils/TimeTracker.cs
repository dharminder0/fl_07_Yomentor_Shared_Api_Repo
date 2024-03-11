using System.Text;

namespace Core.Common.Utils {
    public class TimeTracker {
        private List<KeyValuePair<DateTime, string>> _timeTracker = new List<KeyValuePair<DateTime, string>>();

        public TimeTracker() {
            _timeTracker.Add(new KeyValuePair<DateTime, string>(DateTime.UtcNow, "Started"));
        }

        public void LogTime(string message = null) {
            _timeTracker.Add(new KeyValuePair<DateTime, string>(DateTime.UtcNow, message));
        }

        public void Reset() {
            _timeTracker.Clear();
            _timeTracker.Add(new KeyValuePair<DateTime, string>(DateTime.UtcNow, "Started"));
        }

        public string GetTimeStatsAsText() {
            var timeLogText = new StringBuilder();

            timeLogText.AppendLine($"{_timeTracker[0].Value} @ {_timeTracker[0].Key}");

            if (_timeTracker.Count > 1) {
                for (int i = 1; i < _timeTracker.Count; i++) {
                    var currentTime = _timeTracker[i].Key;
                    var previousTime = _timeTracker[i - 1].Key;
                    var timeSpent = currentTime.Subtract(previousTime);
                    timeLogText.AppendLine($"{_timeTracker[i].Value} in {timeSpent}");
                }
            }

            return timeLogText.ToString();
        }

        public double GetElapsedMilliseconds() {
            return DateTime.UtcNow.Subtract(_timeTracker[0].Key).TotalMilliseconds;
        }
    }
}
