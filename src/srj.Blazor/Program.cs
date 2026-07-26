using srj.Blazor.Components;
using srj.Blazor.Services;
using srj.Blazor.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;

});

builder.Services.AddSingleton<TokenService>();

builder.Services.AddTransient<JwtHttpHandler>();

builder.Services.AddHttpClient("Api", client =>
    {
        client.BaseAddress = new Uri(
            builder.Configuration["ApiSettings:BaseUrl"]!);
    })
    .AddHttpMessageHandler<JwtHttpHandler>();

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>()
        .CreateClient("Api"));

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

#endregion

var app = builder.Build();

#region Middleware

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

#endregion

app.Run();