using CookieAPI.Handler;
using CookieAPI.PolicyProvider;
using CookieAPI.Requirement;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"c:\PATH TO COMMON KEY RING FOLDER"))
    .SetApplicationName("SharedCookieApp");

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", config =>
    {
        config.Cookie.Name = "MyCookie";
        config.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
        config.Cookie.Path = "/";
        config.LoginPath = "/Home/Login";
        config.AccessDeniedPath = "/Home";
    });

builder.Services.AddAuthorization(p =>
{
    p.AddPolicy("depPolicy", config =>
    {
        config.RequireClaim("DPT", "IT");
    });

    p.AddPolicy("minimumAgeRequired", config =>
    {
        config.Requirements.Add(new MinimumAgeRequirement(18));
    });
});

builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowAll", p =>
    {
        p.WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddControllers();

builder.Services.AddHttpClient().AddHttpContextAccessor();

builder.Services.AddSingleton<IAuthorizationPolicyProvider, MinimumAgePolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();


var app = builder.Build();

app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(p =>
{
    p.MapDefaultControllerRoute();
});
app.Run();
