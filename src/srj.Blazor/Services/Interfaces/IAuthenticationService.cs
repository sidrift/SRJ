using srj.Application.Dtos.Request.Authentication;
using srj.Application.Dtos.Response.Authentication;

namespace srj.Blazor.Services.Interfaces;

public interface IAuthenticationService
{
    public Task<LoginResponse> LoginAsync(LoginRequest request);

    public Task RegisterAsync(RegisterRequest request);
}