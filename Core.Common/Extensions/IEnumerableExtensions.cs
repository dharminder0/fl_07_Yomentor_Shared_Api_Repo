using System.Text;

namespace Core.Common.Extensions {
    public static class IEnumerableExtensions {
        public static IEnumerable<List<T>> Partition<T>(this IEnumerable<T> source, int size) {
            for (int i = 0; i < Math.Ceiling(source.Count() / (Double)size); i++)
                yield return new List<T>(source.Skip(size * i).Take(size));
        }

        public static string GetAsCsvString<T>(this IEnumerable<T> data) {

            var props = typeof(T).GetProperties();
            var csvContent = new StringBuilder();
            csvContent.AppendLine(string.Join(",", props.Select(p => p.Name)));
            foreach (var item in data) {
                csvContent.AppendLine($"\"{string.Join("\",\"", props.Select(p => p.GetValue(item)?.ToString() ?? ""))}\"");
            }

            return csvContent.ToString();
        }
    }
}
