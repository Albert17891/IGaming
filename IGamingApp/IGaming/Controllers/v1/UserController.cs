﻿using Asp.Versioning;
using IGaming.Core.Interfaces;
using IGaming.Core.Models;
using IGaming.Models.Request;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IGaming.Controllers.v1;

[ApiVersion(1)]
[Authorize]
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
    [AllowAnonymous]
    [Route("authenticate")]
    [HttpPost]
    public async Task<IActionResult> Authenticate(string userName, string password)
    {
        if (userName == null || password == null)
        {
            return BadRequest("UserName or Password is Null");
        }

        var token = await _userService.Login(userName, password);

        return Ok(token);
    }

    [MapToApiVersion(1)]
    [AllowAnonymous]
    [Route("Register")]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest registerRequest, CancellationToken cancellationToken = default)
    {
        if (registerRequest == null)
        {
            return BadRequest("User Info is Null");
        }

        await _userService.Register(registerRequest.Adapt<UserServiceModel>(), cancellationToken);

        return Ok();
    }

    [MapToApiVersion(1)]
    [Route("profile")]
    [HttpPost]
    public async Task<IActionResult> GetProfileInfo()
    {
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            return BadRequest();
        }

        var user = await _userService.GetUserInfo(token);

        return Ok(user);
    }
}
