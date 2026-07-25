namespace srj.Application.Dtos.Response.Authentication;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;

    public DateTime Expiry { get; set; }

    public string UserName { get; set; } = string.Empty;
}