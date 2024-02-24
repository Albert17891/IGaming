using IGaming.Core.Interfaces;
using IGaming.Core.Models;
using IGaming.Domain.Exceptions;
using IGaming.Domain.Models;
using IGaming.Infrastructure.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace IGaming.Core.Services;
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;

    public UserService(IUnitOfWork unitOfWork, IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public async Task<UserServiceModel> GetUserInfo(string token)
    {
        var userClaims = _jwtService.GetClaimsPrincipalFromToken(token);

        var userName = userClaims?.FindFirst("name")?.Value;

        var user = await _unitOfWork.Repository<User>().Table
                                              .SingleOrDefaultAsync(x => x.UserName == userName) ?? throw new UserNotFoundException("User Not Found");

        return user.Adapt<UserServiceModel>();
    }

    public async Task<string> Login(string userName, string password)
    {
        var user = await _unitOfWork.Repository<User>().Table
                                              .SingleOrDefaultAsync(x => x.UserName == userName);

        if (user != null && VerifyPassword(password, user.Password))
        {
            return _jwtService.GetJwtToken(userName);
        }

        throw new UserNotFoundException("User Not Found");
    }

    public async Task Register(UserServiceModel userServiceModel, CancellationToken cancellationToken)
    {
        if (userServiceModel.Password != userServiceModel.ConfirmPassword)
        {
            throw new ArgumentException("ConfirmPassword must be equal to Password");
        }


        var isExist = await _unitOfWork.Repository<User>().Table.AnyAsync(x => x.UserName == userServiceModel.UserName, cancellationToken);

        if (isExist)
        {
            throw new SameUserNameExceptions($"The username  {userServiceModel.UserName} exist,Use Another Name");
        }



        var user = userServiceModel.Adapt<User>();

        user.Password = HashPassword(userServiceModel.Password);

        await _unitOfWork.Repository<User>().AddAsync(user, cancellationToken);

        await _unitOfWork.SaveChangeAsync();
    }

    private string HashPassword(string password)
    {
        using SHA256 sha256 = SHA256.Create();

        byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string enteredPassword, string storedHashedPassword)
    {
        string enteredHash = HashPassword(enteredPassword);

        return storedHashedPassword.Equals(enteredHash);
    }
}
