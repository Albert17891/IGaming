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
    private const int Iterations = 10000;
    private const int SaltSize = 16;
    private const int HashSize = 20;

    public UserService(IUnitOfWork unitOfWork, IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }
    public async Task<string> Login(string username, string password)
    {
        var user = await _unitOfWork.Repository<User>().Table
                                              .SingleOrDefaultAsync(x => x.UserName == username);

        if (user != null && VerifyPassword(password, user.Password))
        {
            return _jwtService.GetJwtToken(username);
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
