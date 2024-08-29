
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StoreManageAPI.Config;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Models;
using StoreManageAPI.Services;
using StoreManageAPI.Services.Interfaces;
using System.Text;
using static StoreManageAPI.Functions.SendEmail.SendEmail;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connect MySql
builder.Services.AddDbContext<DataStore>(option =>
{
    string? connect = builder.Configuration.GetConnectionString("connect");
    option.UseMySQL(connect?? "");
});


// DI D Injection

// DI Services
builder.Services.AddScoped<IAuthenService, AuthenService>();
builder.Services.AddScoped<IJwtService , JwtService>();

// SendMail
var maisetting = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(maisetting);
builder.Services.AddScoped<ISendMail, SendMail>();

// Config Login for Identity 
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<DataStore>()
    .AddDefaultTokenProviders();

// Config LogIn Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    // Thiết lập yêu cầu về mật khẩu
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    // Thiết lập khóa người dùng
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // Thiết lập yêu cầu về người dùng
    options.User.RequireUniqueEmail = true;

    // SignIn

    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedAccount = true;

});


// Config CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(ConfigAppSetting.PolicyName, op =>
    {
       op.WithOrigins(builder.Configuration.GetSection(ConfigAppSetting.AllowedOrigins).Get<string[]>() ?? throw new InvalidOperationException("AllowedOrigins is not found"))
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();

    });
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

            ClockSkew = TimeSpan.Zero,

            ValidIssuer = builder.Configuration[ConfigAppSetting.Issues] ?? throw new InvalidOperationException("Issues is not found"),
            ValidAudience = builder.Configuration[ConfigAppSetting.Audience] ?? throw new InvalidOperationException("Audience is not found"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[ConfigAppSetting.AccessTokenSecret] ?? throw new InvalidOperationException("SecurityKey is not found")))
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                // Call this to skip the default logic and avoid using the default response
                context.HandleResponse();
                // Write to the response in any way you wish
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(
                    new ApiResponse
                    {
                        StatusCode = 401,
                        IsSuccess = false,
                        Message = "You are not authorized, please login to get access"
                    }
                );
            }
        };
    });

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;/*
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;*/
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

    });

builder.Services.AddHttpContextAccessor();


builder.Services.AddSwaggerGen(opt =>
{
    opt.EnableAnnotations();

    opt.SwaggerDoc("v1", new OpenApiInfo 
    {
        Title = "My API",
        Version = "v1",
        Description = "This is a sample API for demonstration purposes.",
        Contact = new OpenApiContact
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    }
    );

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    
    app.UseSwaggerUI(config =>
    {
        config.SwaggerEndpoint("v1/swagger.json", "Test");
    });

}




app.UseHttpsRedirection();

app.UseCors(ConfigAppSetting.PolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
