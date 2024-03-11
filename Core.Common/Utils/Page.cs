namespace Core.Common.Utils {
    public class Page<T> {
        public int Total { get; set; }
        public IEnumerable<T> Result { get; set; }
    }
}
