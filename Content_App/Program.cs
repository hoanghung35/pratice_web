using System.Text;
using Content_App.App.Interfaces;
using Content_App.App.Services;
using Content_App.Controllers;
using Content_App.Domain.Enums;
using Content_App.Infrastructure.Data;
using Content_App.Infrastructure.Security;
using Content_App.Shared.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

builder.Services.AddMemoryCache();

IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json").Build();

var connection = configuration.GetConnectionString("Connections");

//Add Scoped
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<RefreshTokenStore>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<ApproveService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ActionService>();

//DbContext
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connection));
builder.Services.AddHttpClient();

//Jwt congig
builder.Services.AddAuthentication().AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"]!.ToString(),
        ValidAudience = builder.Configuration["Jwt:Audience"]!.ToString(),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]!.ToString())),
        RoleClaimType = JwtClaimConstants.Role,
        NameClaimType = JwtClaimConstants.UserCode,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents { 
        OnMessageReceived = context =>
        {
            //If header already provided, keep it
            var authHeader = context.Request.Headers.Authorization.ToString();
            if(!string.IsNullOrEmpty(authHeader))
            {
                return Task.CompletedTask;
            }

            if(context.Request.Cookies.TryGetValue("access_token", out var token))
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
            var roleValue = ctx.User.FindFirst(JwtClaimConstants.Role)?.Value;
            return roleValue != null && Enum.TryParse<RoleKey>(roleValue, out var role) && role == RoleKey.admin;
        });
    });

    options.AddPolicy("dev", policy =>
    {
        policy.RequireAssertion(ctx =>
        {
            var roleValue = ctx.User.FindFirst(JwtClaimConstants.Role)?.Value;
            return roleValue != null && Enum.TryParse<RoleKey>(roleValue, out var role) && role == RoleKey.dev;
        });
    });

    options.AddPolicy("manager", policy =>
    {
        policy.RequireAssertion(ctx =>
        {
            var roleValue = ctx.User.FindFirst(JwtClaimConstants.Role)?.Value;
            return roleValue != null && Enum.TryParse<RoleKey>(roleValue, out var role) && role == RoleKey.manager;
        });
    });
});

//Congig CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCors", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
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
