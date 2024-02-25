using IGaming.Core.Models;
namespace IGaming.Core.Interfaces;

public interface IBetService
{
    Task CreateBet(BetServiceModel service, string token, CancellationToken cancellationToken);
}