using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection service = builder.Services;
IConfiguration configuration = builder.Configuration;


//service.AddSwaggerGen(opt =>
//{
//    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
//    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Description = "Please enter token",
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        BearerFormat = "JWT",
//        Scheme = "bearer"
//    });

//    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type=ReferenceType.SecurityScheme,
//                    Id="Bearer"
//                }
//            },
//            new string[]{}
//        }
//    });
//});


service.AddAuthentication("cookie")
    .AddCookie("cookie", config =>
    {
        config.Cookie.Name = "MyAppleCookie";
        config.LoginPath = "/Home";
        config.AccessDeniedPath = "/Home";
    })
    .AddJwtBearer("jwt", config =>
    {
        byte[] secretBytes = Encoding.UTF8.GetBytes(Constants.SecretKey);
        var key = new SymmetricSecurityKey(secretBytes);

        config.Events = new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                if(context.Request.Query.ContainsKey("access_token"))
                {
                    context.Token = context.Request.Query["access_token"];
                }
                return Task.CompletedTask;
            }
        };

        config.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = Constants.Issuer,
            ValidAudience = Constants.Audience,
            IssuerSigningKey = key,
        };
    });

service.AddAuthorization(policy =>
{
    policy.AddPolicy("myDPTPolicy", policyBuilder =>
    {
        policyBuilder.RequireClaim("DPT");
    });
});

service.AddHttpContextAccessor();
service.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

service.AddRazorPages();


var app = builder.Build();

//app.UseSwagger();
//app.UseSwaggerUI();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
});

app.Run();
