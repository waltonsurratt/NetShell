using System.Diagnostics;
using System.Text;
using NetShell.Internal;
using NetShell.Security;

namespace NetShell;

internal static class NetShExecutor
{
    private const string NetShExe = "netsh";

    public static NetShResult Execute(NetShCommand command)
    {
        if (command.Requirement == NetShCommandRequirement.Administrator &&
            !AdminRights.IsAdministrator())
        {
            throw new UnauthorizedAccessException(
                "This NetSH command requires Administrator privileges.");
        }

        var psi = new ProcessStartInfo
        {
            FileName = NetShExe,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        foreach (var arg in command.Arguments)
            psi.ArgumentList.Add(arg);

        using var process = Process.Start(psi)
            ?? throw new InvalidOperationException("Failed to start netsh.");

        var stdout = new StringBuilder();
        var stderr = new StringBuilder();

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data is not null)
                stdout.AppendLine(e.Data);
        };

        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data is not null)
                stderr.AppendLine(e.Data);
        };

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();

        return new NetShResult
        {
            ExitCode = process.ExitCode,
            StandardOutput = stdout.ToString().Trim(),
            StandardError = stderr.ToString().Trim()
        };
    }
}