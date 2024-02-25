using IGaming.Core.Models;

namespace IGaming.Core.Interfaces;

public interface IUserService
{
    Task<string> Login(string username, string password);
    Task Register(UserServiceModel user, CancellationToken cancellationToken);
    Task<UserServiceModel> GetUserInfo(string token);
}