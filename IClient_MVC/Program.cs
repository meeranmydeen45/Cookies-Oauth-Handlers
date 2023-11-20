using Microsoft.AspNetCore.Authentication.OAuth;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection service = builder.Services;

service.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = "ClientCookie";
    o.DefaultSignInScheme = "ClientCookie";
    o.DefaultChallengeScheme = "Oauth";
})
    .AddCookie("ClientCookie")
    .AddOAuth("Oauth", o =>
    {
        o.ClientId = "client_id";
        o.ClientSecret = "client_secret";
        o.CallbackPath = "/oauth/callback";
        o.AuthorizationEndpoint = "https://localhost:4000/oauth/authorize";
        o.TokenEndpoint = "https://localhost:4000/oauth/token";

        o.SaveTokens = true;

        o.Events = new OAuthEvents
        {
            OnCreatingTicket = context =>
            {
                string? accessToken = context.AccessToken;
                var token = new JwtSecurityToken(accessToken);
                foreach(var claim in token.Claims)
                {
                    context?.Identity?.AddClaim(claim);
                }
                return Task.CompletedTask;
            }
        };
    });

service.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

service.AddHttpContextAccessor().AddHttpClient();

service.AddRazorPages();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints?.MapDefaultControllerRoute();
    endpoints?.MapRazorPages();
});

app.Run();
