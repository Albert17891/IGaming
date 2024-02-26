using Asp.Versioning;
using IGaming.Core.Interfaces;
using IGaming.Core.Models;
using IGaming.Models.Request;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IGaming.Controllers.v1;

[ApiVersion(1)]
[Authorize]
[Route("api/v{v:apiVersion}/bets")]
[ApiController]
public class BetController : ControllerBase
{
    private readonly IBetService _betService;

    public BetController(IBetService betService)
    {
        _betService = betService;
    }

    /// <summary>
    /// Place Pet
    /// </summary>
    /// <param name="betRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [MapToApiVersion(1)]
    [Route("profile")]
    [HttpPost]
    public async Task<IActionResult> PlaceBet(BetRequest betRequest, CancellationToken cancellationToken = default)
    {
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token == null)
        {
            return BadRequest();
        }

        var betServiceModel = betRequest.Adapt<BetServiceModel>();

        await _betService.CreateBet(betServiceModel, token, cancellationToken);

        return Ok();
    }
}