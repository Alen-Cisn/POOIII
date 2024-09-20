
using System.Text.RegularExpressions;

namespace Gema.Server;

public partial class ConfigurationValidator(ILogger logger)
{
    private readonly ILogger _logger = logger;

    public bool Validate(IConfiguration configuration)
    {
        bool isValid = true;

        if (string.IsNullOrEmpty(configuration["address"]) || !ValidateIpAddress(configuration["address"]!))
        {
            _logger.LogError("Invalid IP address in settings.");
            isValid = false;
        }

        if (string.IsNullOrEmpty(configuration["port"]) || !int.TryParse(configuration["port"], out int port) || port < 1024 || port > 65535)
        {
            _logger.LogError("Invalid port number in settings.");
            isValid = false;
        }

        if (string.IsNullOrEmpty(configuration["certificationFingerprint"]) || !ValidateFingerprint(configuration["certificationFingerprint"]!))
        {
            _logger.LogError("Invalid certification fingerprint in settings.");
            isValid = false;
        }

        return isValid;
    }

    private static bool ValidateIpAddress(string ipAddress)
        => Ipv4Regex().IsMatch(ipAddress);

    private static bool ValidateFingerprint(string fingerprint)
        => FingerprintRegex().IsMatch(fingerprint);

    [GeneratedRegex(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$")]
    private static partial Regex Ipv4Regex();

    [GeneratedRegex(@"^((([0-9A-Fa-f]{2}:){19}[0-9A-Fa-f]{2})|[0-9A-Fa-f]{40})$")]
    private static partial Regex FingerprintRegex();
}