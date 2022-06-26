namespace Apis.Settings;

public class JwtSettings
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string RefreshKey { get; set; }
    public int AccessExpiredTime { get; set; } // Second
    public int RefreshExpiredTime { get; set; } // Second
}