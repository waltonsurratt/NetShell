using NetShell.Internal;
using NetShell.Security;
using NetShell.Models;
using NetShell.Parsing;

namespace NetShell;

public static class NetShWlan
{
    public static IReadOnlyList<WlanProfileSummary> ShowProfilesParsed()
    {
        var result = NetShExecutor.Execute(
            new NetShCommand
            {
                Requirement = NetShCommandRequirement.User,
                Arguments = ["wlan", "show", "profiles"]
            });

        return WlanProfileParser.ParseProfiles(result.StandardOutput);
    }

    public static WlanProfileDetails ShowProfileClearKey(string ssid)
    {
        var result = NetShExecutor.Execute(
            new NetShCommand
            {
                Requirement = NetShCommandRequirement.Administrator,
                Arguments = ["wlan", "show", "profile", $"name={ssid}", "key=clear"]
            });

        return WlanProfileParser.ParseProfile(result.StandardOutput);
    }
}