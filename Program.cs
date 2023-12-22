using AutoMapper;
using postgreanddotnet.Data;
using restaurant_app_API.Container;
using restaurant_app_API.Helper;
using restaurant_app_API.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//register the service
builder.Services.AddTransient<IUserService, UserService>();

//db connection
builder.Services.AddDbContext<AppDbContex>();

//mappper
var AutoMapper = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperHandler());
});

IMapper mapper = AutoMapper.CreateMapper();
builder.Services.AddSingleton(mapper);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
