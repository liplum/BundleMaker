using System.Diagnostics.CodeAnalysis;

namespace BundleMaker.Core.Utils;
public static class Utils {
    public static IEnumerable<(int, T)> WithIndex<T>(this IEnumerable<T> collection) {
        var i = 0;
        foreach (var element in collection) {
            yield return (i, element);
            i++;
        }
    }

    public static void AddAll<TK, TV>(this IDictionary<TK, TV> self, IDictionary<TK, TV> addition) {
        foreach (var pair in addition) {
            self.Add(pair);
        }
    }
    [return: NotNull]
    public static string Sub(this string parent, string sub) => Path.Combine(parent, sub);
}