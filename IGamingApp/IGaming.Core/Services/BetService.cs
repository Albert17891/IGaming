using IGaming.Core.Interfaces;
using IGaming.Core.Models;
using IGaming.Domain.Models;
using IGaming.Infrastructure.Interfaces;
using Mapster;

namespace IGaming.Core.Services;

public class BetService : IBetService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public BetService(IUnitOfWork unitOfWork, IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
    }
    public async Task CreateBet(BetServiceModel betModel, string token, CancellationToken cancellationToken)
    {
        var initialBetAmount = 1000M;  //Can be set in conf

        var bet = betModel.Adapt<Bet>();

        var user = await _userService.GetUserInfo(token);

        bet.Amount += initialBetAmount;

        bet.UserId = user.Id;

        await _unitOfWork.Repository<Bet>().AddAsync(bet, cancellationToken);

        await _unitOfWork.SaveChangeAsync();
    }
}