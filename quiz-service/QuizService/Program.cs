using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using QuizService.Interfaces.IRepositories;
using QuizService.Interfaces.IServices;
using QuizService.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using QuizService.Services;
using QuizService.Helpers;
using QuizService.Data;
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
builder.Services.AddHttpClient<UserClient>(client =>
{
    
    client.BaseAddress = new Uri("http://localhost:5000");
});

builder.Services.AddScoped<IQuizRepository, QuizRepo>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepo>();
builder.Services.AddScoped<IChoiceRepository, ChoiceRepo>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepo>();

builder.Services.AddScoped<IQuizService, QuizServices>();
builder.Services.AddScoped<IQuestionService, QuestionService>();


builder.Services.AddAutoMapper(cfg => cfg.AddProfile<QuizAutoMapperProfiles>());

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
