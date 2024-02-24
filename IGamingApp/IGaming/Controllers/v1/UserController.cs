using Asp.Versioning;
using IGaming.Core.Interfaces;
using IGaming.Core.Models;
using IGaming.Models.Request;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace IGaming.Controllers.v1;

[ApiVersion(1)]
[Route("api/v{v:apiVersion}/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [MapToApiVersion(1)]
    [Route("authenticate")]
    [HttpPost]
    public async Task<IActionResult> Authenticate(string userName, string password)
    {
        var token = await _userService.Login(userName, password);

        return Ok(token);
    }

    [MapToApiVersion(1)]
    [Route("Register")]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest registerRequest, CancellationToken cancellationToken = default)
    {
        await _userService.Register(registerRequest.Adapt<UserServiceModel>(), cancellationToken);

        return Ok();
    }
}
