using srj.Application.Dtos.Request.Authentication;
using srj.Application.Dtos.Response.Authentication;

namespace srj.Application.Interface.Services.Authentication;

public interface IAuthenticationService
{
    Task RegisterAsync(RegisterRequest request);

    Task<LoginResponse> LoginAsync(LoginRequest request);
}