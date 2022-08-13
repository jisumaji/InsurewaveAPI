using LogicLayer;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

/*var connectionString = builder.Configuration.GetConnectionString("Constr");
builder.Services.AddDbContext<InsurewaveContext>(options =>
    options.UseSqlServer(connectionString));*/

builder.Services.AddScoped<IUser, User>();
builder.Services.AddScoped<IBuyer, Buyer>();
builder.Services.AddScoped<IBroker, Broker>();
builder.Services.AddScoped<IInsurer, Insurer>();
builder.Services.AddScoped<IPolicy, Policy>();
builder.Services.AddScoped<IRequest, Request>();
builder.Services.AddScoped<ICountry, Country>();

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
}));

/*services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);//You can set Time   
});*/

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

app.UseHttpsRedirection();

//app.UseSession();

app.UseCors("MyPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
