using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"c:\PATH TO COMMON KEY RING FOLDER"))
    .SetApplicationName("SharedCookieApp");

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", config =>
    {
        config.Cookie.Name = "MyCookie";
    });

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
