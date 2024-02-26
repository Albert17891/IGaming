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

    public async Task<UserServiceModel> GetUserInfo(string token, CancellationToken cancellationToken)
    {
        var userClaims = _jwtService.GetClaimsPrincipalFromToken(token);

        var userName = userClaims?.FindFirst("name")?.Value;

        var user = await _unitOfWork.Repository<User>().Table
                                              .SingleOrDefaultAsync(x => x.UserName == userName, cancellationToken) ?? throw new UserNotFoundException("User Not Found");

        return user.Adapt<UserServiceModel>();
    }

    public async Task<string> Login(string userName, string password, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<User>().Table
                                              .SingleOrDefaultAsync(x => x.UserName == userName, cancellationToken);

        if (user != null && VerifyPassword(password, user.Password, user.PasswordSalt))
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

        user.Password = HashPassword(userServiceModel.Password, out byte[] salt);
        user.PasswordSalt = salt;

        await _unitOfWork.Repository<User>().AddAsync(user, cancellationToken);

        await _unitOfWork.SaveChangeAsync();
    }

    //Can be Set in Configuration
    const int keySize = 64;
    const int iterations = 350000;
    readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

    private string HashPassword(string password, out byte[] salt)
    {
        salt = RandomNumberGenerator.GetBytes(keySize);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            iterations,
            hashAlgorithm,
            keySize);

        return Convert.ToHexString(hash);
    }

    private bool VerifyPassword(string password, string hash, byte[] salt)
    {
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);

        return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
    }
}