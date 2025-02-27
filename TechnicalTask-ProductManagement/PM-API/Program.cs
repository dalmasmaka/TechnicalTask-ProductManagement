using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PM_Domain.Entities;
using PM_Domain.Interfaces;
using PM_Infrastructure.Configurations;
using PM_Infrastructure.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container. Adding dependencies

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register your DbContext with dependency injection

// Register the ConnectionStringProvider to inject the connection string
builder.Services.AddScoped<IConnectionStringProvider, ConnectionStringProvider>();

// Register the DbContext
builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    var connectionStringProvider = serviceProvider.GetRequiredService<IConnectionStringProvider>();
    options.UseSqlServer(connectionStringProvider.GetConnectionString());
});

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


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
