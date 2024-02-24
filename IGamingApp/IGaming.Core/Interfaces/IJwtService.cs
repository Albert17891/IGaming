namespace IGaming.Core.Interfaces;

public interface IJwtService
{
    string GetJwtToken(string userName);
}
