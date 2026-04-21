namespace NetShell.Models;

public sealed class WlanProfileDetails
{
    public string Name { get; set; } = string.Empty;
    public string Authentication { get; set; } = string.Empty;
    public string Cipher { get; set; } = string.Empty;
    public string KeyContent { get; set; } = string.Empty;
}