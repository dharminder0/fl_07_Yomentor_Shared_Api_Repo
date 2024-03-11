namespace Core.Common.Extensions {
    public class StringIEqualityComparer : IEqualityComparer<string> {
        public bool Equals(string x, string y) {
            return x.Equals(y, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(string obj) {
            return obj.GetHashCode();
        }
    }
}
