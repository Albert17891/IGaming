using System.Security.Claims;
namespace IGaming.Core.Interfaces;

public interface IJwtService
{
    string GetJwtToken(string userName);
    ClaimsPrincipal GetClaimsPrincipalFromToken(string token);
}