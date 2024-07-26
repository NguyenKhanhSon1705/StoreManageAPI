using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StoreManageAPI.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connect MySql
builder.Services.AddDbContext<DataStore>(option =>
{
    string connect = builder.Configuration.GetConnectionString("ConnectionStrings") ?? "";
    option.UseMySQL(connect);
});

// DI D Injection

// Config Login for Identity 
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<DataStore>()
    .AddDefaultTokenProviders();

// Config LogIn Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    // Thiết lập yêu cầu về mật khẩu
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;

    // Thiết lập khóa người dùng
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // Thiết lập yêu cầu về người dùng
    options.User.RequireUniqueEmail = true;
});

// Config CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowGlobal",
            builder => builder
                .WithOrigins()
                .AllowAnyMethod()
                .AllowAnyHeader());
});

// Config JWT 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"] ?? ""))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowGlobal");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
