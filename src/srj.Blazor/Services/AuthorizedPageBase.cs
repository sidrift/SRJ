using Microsoft.AspNetCore.Components;

namespace srj.Blazor.Services;

public abstract class AuthorizedPageBase : ComponentBase
{
    [Inject]
    protected TokenService TokenService { get; set; } = default!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        var token = TokenService.GetToken();

        if (string.IsNullOrWhiteSpace(token))
        {
            NavigationManager.NavigateTo("/login", true);
            return;
        }

        base.OnInitialized();
    }
}