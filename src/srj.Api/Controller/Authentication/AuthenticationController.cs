using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using srj.Application.Dtos.Request.Authentication;
using srj.Application.Dtos.Response.Authentication;
using srj.Application.Interface.Services.Authentication;

namespace TheSRJProject.Controller.Authentication;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _service;

    public AuthenticationController(
        IAuthenticationService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterRequest request)
    {
        await _service.RegisterAsync(request);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(
        LoginRequest request)
    {
        return Ok(await _service.LoginAsync(request));
    }
}