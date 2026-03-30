using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Sessions;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Sessions.Operations;
using Itmo.ObjectOrientedProgramming.Lab5.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace Itmo.ObjectOrientedProgramming.Lab5.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost("login/user")]
    public IActionResult UserLogin([FromBody] LoginUserRequest request)
    {
        LoginUser.Result result = _sessionService.LoginUser(new LoginUser.Request(request.AccountNumber, request.PinCode));

        return result switch
        {
            LoginUser.Result.Success success => Ok(new { success.SessionKey }),
            LoginUser.Result.AccountNotFound => NotFound(new { Message = "Account not found" }),
            LoginUser.Result.InvalidCredentials => Unauthorized(new { Message = "Invalid credentials" }),
            _ => BadRequest(),
        };
    }

    [HttpPost("login/admin")]
    public IActionResult AdminLogin([FromBody] LoginAdminRequest request)
    {
        LoginAdmin.Result result = _sessionService.LoginAdmin(new LoginAdmin.Request(request.SystemPassword));

        return result switch
        {
            LoginAdmin.Result.Success success => Ok(new { success.SessionKey }),
            LoginAdmin.Result.InvalidCredentials => Unauthorized(new { Message = "Invalid credentials" }),
            _ => BadRequest(),
        };
    }

    [HttpPost("logout")]
    public IActionResult Logout([FromHeader(Name = "X-Session-Key")] System.Guid sessionKey)
    {
        _sessionService.Logout(sessionKey);
        return Ok(new { Message = "Logged out" });
    }
}