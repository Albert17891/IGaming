namespace IGaming.Domain.Options;
public class AccessTokenConfiguration
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int AccessTokenExpiresMinutes { get; set; }
    public string Key { get; set; }
}
