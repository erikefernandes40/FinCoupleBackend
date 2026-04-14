using System.Text;
using FinCouple.Application.Services;
using FinCouple.Application.Services.Interfaces;
using FinCouple.Infrastructure.Data;
using FinCouple.Infrastructure.HostedServices;
using FinCouple.Infrastructure.Repositories;
using FinCouple.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Redis
var redisConnection = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(redisConnection));

// Repositories
builder.Services.AddScoped<FinCouple.Domain.Repositories.IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<FinCouple.Domain.Repositories.IRecurringExpenseRepository, RecurringExpenseRepository>();
builder.Services.AddScoped<FinCouple.Domain.Repositories.IUserRepository, UserRepository>();
builder.Services.AddScoped<FinCouple.Domain.Repositories.ICoupleRepository, CoupleRepository>();
builder.Services.AddScoped<FinCouple.Domain.Repositories.IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<FinCouple.Domain.Repositories.ICategoryRepository, CategoryRepository>();

// Services
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IRecurringExpenseService, RecurringExpenseService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IEventPublisher, RedisEventPublisher>();

// Hosted Services
builder.Services.AddHostedService<RecurrenceHostedService>();

// JWT Authentication
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT:Secret not configured");
var key = Encoding.UTF8.GetBytes(jwtSecret);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// CORS
var frontendUrl = builder.Configuration["FrontendUrl"] ?? "http://localhost:5173";
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
        policy.WithOrigins(frontendUrl)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
