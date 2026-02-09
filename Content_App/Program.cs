using System.Text;
using Content_App.App.Interfaces;
using Content_App.App.Services;
using Content_App.Controllers;
using Content_App.Domain.Enums;
using Content_App.Infrastructure.Data;
using Content_App.Infrastructure.Security;
using Content_App.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Đăng ký DbContext sử dụng Npgsql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));


//builder.Services.AddScoped<IRefreshTokenStore, RefreshTokenStore>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<AuthService>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)
        ),

        RoleClaimType = JwtClaimConstants.Role,
        NameClaimType = JwtClaimConstants.UserCode
    };

    options.Events = new()
    {
        OnMessageReceived = ctx =>
        {
            ctx.Token = ctx.Request.Cookies["access_token"];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireAssertion(ctx =>
            Enum.Parse<RoleCode>(
                ctx.User.FindFirst(JwtClaimConstants.Role)!.Value
            ) >= RoleCode.admin));

    options.AddPolicy("ManagerUp", policy =>
        policy.RequireAssertion(ctx =>
            Enum.Parse<RoleCode>(
                ctx.User.FindFirst(JwtClaimConstants.Role)!.Value
            ) >= RoleCode.manager));

    options.AddPolicy("DevOnly", policy =>
        policy.RequireAssertion(ctx =>
            Enum.Parse<RoleCode>(
                ctx.User.FindFirst(JwtClaimConstants.Role)!.Value
            ) == RoleCode.dev));


});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<JwtFromCookieMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
