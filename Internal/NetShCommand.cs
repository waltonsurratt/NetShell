using NetShell.Security;

namespace NetShell.Internal;

internal sealed class NetShCommand
{
    public string[] Arguments { get; init; } = [];
    public NetShCommandRequirement Requirement { get; init; }
}