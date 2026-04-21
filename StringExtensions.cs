namespace NetShell;

internal static class StringExtensions
{
    public static IEnumerable<string> SplitLines(this string input) =>
        input.Split(
            new[] { "\r\n", "\n" },
            StringSplitOptions.RemoveEmptyEntries);
}