using IGaming.Core.Models;

namespace IGaming.Core.Interfaces;

public interface IUserService
{
    Task<string> Login(string username, string password, CancellationToken cancellationToken);
    Task Register(UserServiceModel user, CancellationToken cancellationToken);
    Task<UserServiceModel> GetUserInfo(string token, CancellationToken cancellationToken);
}