# NetShell
This a new project written in C# and built using only the .NET 10.0 standard/core libraries. It acts as a wrapper for the well-known Windows **netsh** command-line utility used to configure and monitor network settings. The library should be used as a high-level interface for two or more known contexts (Example: IPConfig and WLAN) and can be dropped into other projects and applications.

# Version: 1.0
The library currently includes the following features:
* Invoking netsh.exe and capturing stdout/stderr
* .NET 10.0 class library
* Zero third-party dependencies
* Returns structured output
* Safe argument handling
* Admin-required command awareness


# Explanation
✅ Fully supported
+ Runs inside a normal user-launched Windows process
+ Can operate in:
  + Standard user context (read‑only NetSH commands)
  + Elevated Administrator context (full NetSH surface)
+ UAC behavior is owned by the host app, not the library

**Why it works**

+ netsh.exe is available on all supported Windows SKUs
+ Standard output/error capture works normally
+ Admin detection/enforcement functions correctly
✅ This is the primary intended context


⚠️ Admin Rights Reminder
Many commands require elevation:

+ Firewall rules
+ Interface changes
+ Winsock reset

# API Calls & Examples
#### Check Administrator Status (Recommended First Step).
Many NetSH commands require elevation. The API will throw if you’re not elevated.
~~~
using NetShell.Security;

if (!AdminRights.IsAdministrator())
{
    Console.WriteLine("This application is not running as Administrator.");
    Console.WriteLine("Some NetShell commands will fail.");
}
~~~

#### List Saved Wi‑Fi Profiles
Equivalent to:
~~~
netsh wlan show profiles
~~~
~~~
using NetShell;

var profiles = NetShWlan.ShowProfilesParsed();

foreach (var profile in profiles)
{
    Console.WriteLine($"SSID: {profile.Name}");
}
~~~
Example Output:
```
SSID: CorpWiFi
SSID: GuestWiFi
SSID: HomeNetwork
```

#### Show Wi‑Fi Profile Details (Including Clear Key)
Equivalent to:
~~~
netsh wlan show profile name="CorpWiFi" key=clear
~~~
⚠ Requires Administrator privileges
~~~
using NetShell;

try
{
    var details = NetShWlan.ShowProfileClearKey("CorpWiFi");

    Console.WriteLine($"SSID: {details.Name}");
    Console.WriteLine($"Authentication: {details.Authentication}");
    Console.WriteLine($"Cipher: {details.Cipher}");

    if (!string.IsNullOrEmpty(details.KeyContent))
        Console.WriteLine($"Password: {details.KeyContent}");
    else
        Console.WriteLine("Password not available (admin required)");
}
catch (UnauthorizedAccessException ex)
{
    Console.WriteLine(ex.Message);
}
~~~

#### Enable / Disable a Network Interface
Equivalent to:
~~~
netsh interface set interface "Ethernet" admin=disable
~~~
⚠ Requires Administrator privileges

**Disable Interface**
~~~
using NetShell;

var result = NetShExecutor.Execute(
    new NetShell.Internal.NetShCommand
    {
        Requirement = NetShell.Security.NetShCommandRequirement.Administrator,
        Arguments = ["interface", "set", "interface", "name=Ethernet", "admin=disable"]
    });

Console.WriteLine(result.StandardOutput);
~~~

# NetShell Project Structure
```
NetShell/
│
├─ NetShell.csproj
│
├─ NetShResult.cs
├─ NetShExecutor.cs
├─ StringExtensions.cs
│
├─ Security/
│   ├─ AdminRights.cs
│   └─ NetShCommandRequirement.cs
│
├─ Internal/
│   └─ NetShCommand.cs
│
├─ Models/
│   ├─ WlanProfileSummary.cs
│   ├─ WlanProfileDetails.cs
│   └─ InterfaceIpConfig.cs
│
├─ Parsing/
│   ├─ WlanProfileParser.cs
│   └─ IpConfigParser.cs
│
└─ NetShWlan.cs
```
