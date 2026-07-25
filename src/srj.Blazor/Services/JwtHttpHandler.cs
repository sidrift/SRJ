using System.Net.Http.Headers;

namespace srj.Blazor.Services;

public class JwtHttpHandler : DelegatingHandler
{
    private readonly TokenService _tokenService;

    public JwtHttpHandler(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = _tokenService.GetToken();

        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}