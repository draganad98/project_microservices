using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Data;
using UserService.Helpers;
using UserService.Interfaces.IRepositories;
using UserService.Interfaces.IServices;
using UserService.Repositories;
using UserService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
string? secretKey = builder.Configuration.GetSection("AppSettings:Key").Value;
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("JwtScheme", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = key,
            NameClaimType = ClaimTypes.Name
        };
    });

builder.Services.AddAuthorization(options =>
{
    var policyBuilder = new AuthorizationPolicyBuilder("JwtScheme");
    options.AddPolicy("JwtSchemePolicy", policyBuilder.RequireAuthenticatedUser().Build());
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IUsersRepository, UsersRepo>();
builder.Services.AddScoped<IUsersService, UsersService>();


builder.Services.AddAutoMapper(cfg => cfg.AddProfile<UserAutoMapperProfiles>());

builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(m => m.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
