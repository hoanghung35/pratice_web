using System.Text;
using System.Text.Json;
using Content_App.App.Interfaces;
using Content_App.App.Services;
using Content_App.Controllers;
using Content_App.Domain.Enums;
using Content_App.Infrastructure.Data;
using Content_App.Infrastructure.Security;
using Content_App.Shared.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});


builder.Services.AddMemoryCache();

IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json").Build();
var connection = configuration.GetConnectionString("Connections");

//add scoped
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ActionService>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<MailService>();
builder.Services.AddScoped<RequestService>();
builder.Services.AddScoped<ApproveService>();
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<RefreshTokenStore>();
buidder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<FileService>();
builder.Service.AddScoped<DateConverter>();
builder.Services.AddScoped<IAuthService, JwtService>();

//Db context
builder.Services.AddDbContext<LogDbContext>(option => option.UseNpgsql(connection));

builder.Services.AddHttpClient();

//Jwt config
builder.Services.AddAuthentication(options => 
{
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"]!.ToString(),
        ValidAudience = builder.Configuration["Jwt:Audience"]!.ToString(),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!.ToString())),
        RoleClaimType = JwtClaimConstant.Role,
        NameClaimType = JwtClaimConstant.UserName,

        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var authHeader = context.Request.Headers.Authorization.ToString();

            if (!string.IsNullOrEmpty(authHeader)) return Task.CompletedTask;

            if (context.Request.Cookies.TryGetValue("access_token", out var token))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin", policy =>
    {
        policy.RequireAssertion(ctx =>
        {
            return ctx.User.Identity.IsAuthenticated && Enum.Parse<RoleKey>(ctx.User.FindFirst(JwtClaimConstant.Role)!.Value) == RoleKey.admin;
        });
    });

    options.AddPolicy("manager", policy =>
    {
        policy.RequireAssertion(ctx =>
        {
            return ctx.User.Identity.IsAuthenticated && Enum.Parse<RoleKey>(ctx.User.FindFirst(JwtClaimConstant.Role)!.Value) == RoleKey.manager;
        });
    });

    options.AddPolicy("super", policy =>
    {
        policy.RequireAssertion(ctx =>
        {
            return ctx.User.Identity.IsAuthenticated && Enum.Parse<RoleKey>(ctx.User.FindFirst(JwtClaimConstant.Role)!.Value) == RoleKey.super;
        });
    });

    options.AddPolicy("dev", policy =>
    {
        policy.RequireAssertion(ctx =>
        {
            return ctx.User.Identity.IsAuthenticated && Enum.Parse<RoleKey>(ctx.User.FindFirst(JwtClaimConstant.Role)!.Value) == RoleKey.dev;
        });
    });

    options.AddPolicy("gm", policy => 
    {
       policy.RequireAssertion(ctx =>
        {
            return ctx.User.Identity.IsAuthenticated && Enum.Parse<RoleKey>(ctx.User.FindFirst(JwtClaimConstant.Role)!.Value) == RoleKey.general_manager;
        });
    });

    options.AddPolicy("CanCreate", policy =>
    {
        policy.RequireAssertion(ctx =>
        {
            return ctx.User.Identity.IsAuthenticated && Enum.Parse<RoleKey>(ctx.User.FindFirst(JwtClaimConstant.Role)!.Value) == RoleKey.dev ||
            Enum.Parse<RoleKey>(ctx.User.FindFirst(JwtClaimConstant.Role)!.Value) == RoleKey.admin;
        });
    });

    options.AddPolicy("CanApproval", policy => 
    {
        policy.RequireAssertion(ctx => 
        {
            return ctx.User.Identity.IsAuthenticated && Enum.Parse<RoleKey>(ctx.User.FindFirst(JwtClaimConstant.Role)!.Value) == RoleKey.general_manager ||
                Enum.Parse<RoleKey>(ctx.User.FindFirst(JwtClaimConstant.Role)!.Value) == RoleKey.dev ||
                Enum.Parse<RoleKey>(ctx.User.FindFirst(JwtClaimConstant.Role)!.Value) == RoleKey.super ||
                Enum.Parse<RoleKey>(ctx.User.FindFirst(JwtClaimConstant.Role)!.Value) == RoleKey.manager;
        });
    });
});

//config CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCors", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Config the HTTP request pipeline
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRouting();
app.UseCors("AllowCors");


app.UseMiddleware<JwtFromCookieMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
