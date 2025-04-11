
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StoreManageAPI.Config;
using StoreManageAPI.Context;
using StoreManageAPI.DatabaseMigrations;
using StoreManageAPI.Mddleware;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Models;
using StoreManageAPI.Services.Auth;
using StoreManageAPI.Services.Dish;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.Services.OrderTables;
using StoreManageAPI.Services.Reports;
using StoreManageAPI.Services.Roles;
using StoreManageAPI.Services.Shop;
using StoreManageAPI.Services.Store;
using StoreManageAPI.Services.UserManager;
using StoreManageAPI.Services.VnPay;
using StoreManageAPI.Websoket;
using System.Text;
using VNPAY.NET;
using static StoreManageAPI.Helpers.SendEmail.SendEmail;

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
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<CloudinaryMiddle>();

// DI Services
builder.Services.AddScoped<IVnpay, Vnpay>();

builder.Services.AddScoped<IVnpayService, VnpayService>();
builder.Services.AddScoped<IAuthenService, AuthenService>();
builder.Services.AddScoped<IJwtService , JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDatabaseMigrationsService, DatabaseMigrationsService>();
builder.Services.AddScoped<IShopSerVice , ShopService>();
builder.Services.AddScoped<IAreasService, AreasService>(); 
builder.Services.AddScoped<ITablesService, TablesService>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IMenuGroupService, MenuGroupService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<ITableAreaService, TableAreaService>();
builder.Services.AddScoped<ITableDishService,  TableDishService>();

builder.Services.AddScoped<IReportAbortedService, ReportAbortedService>();
builder.Services.AddScoped<IReportBillService, ReportBillService>();
builder.Services.AddScoped<IStatisticalService, StatisticalService>();

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

// Register websoket
builder.Services.AddSignalR();

// Config CORS
builder.Services.AddCors(options =>
 {
     options.AddPolicy(Config.PolicyName, op =>
     {
       op.WithOrigins(builder.Configuration.GetSection(Config.AllowedOrigins).Get<string[]>() ?? throw new InvalidOperationException("AllowedOrigins is not found"))
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials();
     });
 });

/*
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AppRoles.Administrator, policy =>
    {
        policy.RequireRole(AppRoles.Administrator);
    });
    options.AddPolicy(AppRoles.Developer, policy =>
    {
        policy.RequireRole(AppRoles.Developer );
        policy.RequireRole(AppRoles.Administrator );
    });


    options.AddPolicy(AppRoles.Owner, policy =>
    {
        policy.RequireRole(AppRoles.Owner);
    });

    options.AddPolicy(AppRoles.Staff, policy =>
    {
        policy.RequireRole(AppRoles.Staff);
    });
   
    options.AddPolicy(AppRoles.Sashier, policy =>
    {
        policy.RequireRole(AppRoles.Sashier);
    });
});*/


// Config JWT 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

})
    .AddJwtBearer( options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ClockSkew = TimeSpan.Zero,

            ValidIssuer = builder.Configuration[Config.Issues] ?? throw new InvalidOperationException("Issues is not found"),
            ValidAudience = builder.Configuration[Config.Audience] ?? throw new InvalidOperationException("Audience is not found"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[Config.AccessTokenSecret] ?? throw new InvalidOperationException("SecurityKey is not found")))
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

// Cho phép mọi URL endpoint đều chuyển thành chữ thường
builder.Services.AddRouting(option =>
{
    option.LowercaseUrls = true;
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

app.Urls.Add("http://0.0.0.0:5000");

app.UseHttpsRedirection();

app.UseCors(Config.PolicyName);
app.MapHub<WsOrderTableArea>("/ordertablearea");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
