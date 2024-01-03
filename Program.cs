using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using postgreanddotnet.Data;
using restaurant_app_API.Container;
using restaurant_app_API.Helper;
using restaurant_app_API.Model;
using restaurant_app_API.Service;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//keys
var _authKey = builder.Configuration.GetValue<string>("JwtSettings:Secret");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//register the service
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRefreshHandler, RefreshHandler>();

//authentication
// builder.Services.AddAuthentication("BasicAuthentication")
//     .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

//enable jwt authentication
builder.Services.AddAuthentication(item =>
{
    item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(item =>
{
    item.RequireHttpsMetadata = true;
    item.SaveToken = true;
    item.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

//db connection
builder.Services.AddDbContext<AppDbContex>();

//mappper
var AutoMapper = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperHandler());
});

IMapper mapper = AutoMapper.CreateMapper();
builder.Services.AddSingleton(mapper);

// enable cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.WithOrigins("*")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

//jwt authentication
var _jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(_jwtSettings);

//rate limit
// builder.Services.AddRateLimiter(
//     _=> _.AddFixedWindowLimiter(policyName: "FixedWindow", option =>
//     {
//         option.Window = TimeSpan.FromSeconds(10);
//         option.PermitLimit = 1;
//         option.QueueLimit = 0;
//         option.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
//     })
// );


//logger
// Serilog configuration
// string logPath = builder.Configuration.GetSection("LogPath").Value;
// var _logger = new LoggerConfiguration()
//     .MinimumLevel.Error()
//     .MinimumLevel.Debug()
//     .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
//     .Enrich.FromLogContext()
//     .WriteTo.File(logPath)
//     .CreateLogger();

// builder.Services.AddSingleton<Serilog.ILogger>(_logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//use static files
app.UseStaticFiles();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
