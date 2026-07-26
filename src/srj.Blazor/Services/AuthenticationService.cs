using srj.Application.Dtos.Request.Authentication;
using srj.Application.Dtos.Response.Authentication;
using srj.Blazor.Services.Interfaces;

namespace srj.Blazor.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;

    public AuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var response =
            await _httpClient.PostAsJsonAsync(
                "auth/login",
                request);


        if (!response.IsSuccessStatusCode)
        {
            var apiError =
                await response.Content.ReadFromJsonAsync<ApiError>();

            throw new Exception(
                apiError?.Message ?? "Login failed.");
        }


        return (await response.Content
            .ReadFromJsonAsync<LoginResponse>())!;
    }



    public async Task RegisterAsync(RegisterRequest request)
    {
        var response =
            await _httpClient.PostAsJsonAsync(
                "auth/register",
                request);


        if (!response.IsSuccessStatusCode)
        {
            var apiError =
                await response.Content.ReadFromJsonAsync<ApiError>();

            throw new Exception(
                apiError?.Message ?? "Registration failed.");
        }

    }



    private class ApiError
    {
        public string? Message { get; set; }
    }
}