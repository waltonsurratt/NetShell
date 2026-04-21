namespace NetShell.Models;

public sealed class InterfaceIpConfig
{
    public string InterfaceName { get; set; } = string.Empty;
    public bool DhcpEnabled { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string SubnetMask { get; set; } = string.Empty;
    public string DefaultGateway { get; set; } = string.Empty;
}