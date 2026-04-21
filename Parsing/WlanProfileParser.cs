using NetShell.Models;

namespace NetShell.Parsing;

internal static class WlanProfileParser
{
    public static IReadOnlyList<WlanProfileSummary> ParseProfiles(string output)
    {
        var list = new List<WlanProfileSummary>();

        foreach (var line in output.SplitLines())
        {
            // Example:
            //     All User Profile     : MyWifi
            if (!line.Contains(":"))
                continue;

            if (!line.Contains("Profile", StringComparison.OrdinalIgnoreCase))
                continue;

            var parts = line.Split(':', 2);
            if (parts.Length != 2)
                continue;

            var name = parts[1].Trim();
            if (string.IsNullOrWhiteSpace(name))
                continue;

            list.Add(new WlanProfileSummary
            {
                Name = name
            });
        }

        return list;
    }

    public static WlanProfileDetails ParseProfile(string output)
    {
        var result = new WlanProfileDetails();

        foreach (var line in output.SplitLines())
        {
            if (!line.Contains(":"))
                continue;

            var parts = line.Split(':', 2);
            if (parts.Length != 2)
                continue;

            var key = parts[0].Trim().ToLowerInvariant();
            var value = parts[1].Trim();

            switch (key)
            {
                case "profile name":
                    result.Name = value;
                    break;

                case "authentication":
                    result.Authentication = value;
                    break;

                case "cipher":
                    result.Cipher = value;
                    break;

                case "key content":
                    result.KeyContent = value;
                    break;
            }
        }

        return result;
    }
}