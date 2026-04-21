using NetShell.Models;

namespace NetShell.Parsing;

internal static class IpConfigParser
{
    public static IReadOnlyList<InterfaceIpConfig> Parse(string output)
    {
        var list = new List<InterfaceIpConfig>();
        InterfaceIpConfig? current = null;

        foreach (var rawLine in output.SplitLines())
        {
            var line = rawLine.Trim();

            // Interface header:
            // Configuration for interface "Ethernet"
            if (line.StartsWith(
                "Configuration for interface",
                StringComparison.OrdinalIgnoreCase))
            {
                if (current != null)
                    list.Add(current);

                var name = line.Split('"').ElementAtOrDefault(1) ?? string.Empty;
                current = new InterfaceIpConfig
                {
                    InterfaceName = name
                };
                continue;
            }

            if (current == null || !line.Contains(":"))
                continue;

            var parts = line.Split(':', 2);
            if (parts.Length != 2)
                continue;

            var key = parts[0].Trim().ToLowerInvariant();
            var value = parts[1].Trim();

            switch (key)
            {
                case "dhcp enabled":
                    current.DhcpEnabled =
                        value.Equals("yes", StringComparison.OrdinalIgnoreCase);
                    break;

                case "ip address":
                    current.IpAddress = value;
                    break;

                case "default gateway":
                    current.DefaultGateway = value;
                    break;

                case "subnet prefix":
                    // Example:
                    // 255.255.255.0 (mask 255.255.255.0)
                    if (value.Contains("mask", StringComparison.OrdinalIgnoreCase))
                    {
                        var maskPart = value.Split("mask", StringSplitOptions.RemoveEmptyEntries);
                        if (maskPart.Length > 1)
                            current.SubnetMask = maskPart[1].Trim(' ', ')');
                    }
                    break;
            }
        }

        if (current != null)
            list.Add(current);

        return list;
    }
}