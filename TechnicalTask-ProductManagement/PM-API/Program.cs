using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PM_API.Middleware;
using PM_Application.Authorization;
using PM_Application.Interfaces;
using PM_Application.Services;
using PM_Application.MappingProfiles;
using PM_Domain.Entities;
using PM_Domain.Interfaces;
using PM_Infrastructure.AuthServices;
using PM_Infrastructure.Data;
using PM_Infrastructure.Interfaces;
using PM_Infrastructure.Repositories;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    var connectionStringProvider = serviceProvider.GetRequiredService<IConnectionStringRepository>();
    options.UseSqlServer(connectionStringProvider.GetConnectionString());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.WithOrigins("https://localhost:44360", "http://localhost:4200", "https://localhost:7005") 
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(CategoryMapper));


builder.Services.AddScoped<Argon2PasswordHasher>();
builder.Services.AddScoped<IConnectionStringRepository, ConnectionStringRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IPasswordHasher<ApplicationUser>, Argon2PasswordHasher>();
builder.Services.AddScoped<JwtHelper>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddOptions();

//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
//    {
//        Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
//        In = "header",
//        Name = "Authorization",
//        Type = "apiKey"
//    });

//    options.OperationFilter<SecurityRequirementsOperationFilter>();
//});

builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {

        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "JWT Authorization header using the Bearer scheme."
    };

    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });

    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, jwtSecurityScheme);

    options.OperationFilter<SecurityRequirementsOperationFilter>();

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            jwtSecurityScheme, new string[] {}
        }
    });
});



//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("UserPolicy", policy =>
//        policy.RequireAuthenticatedUser().RequireRole("User"));

//    options.AddPolicy("AdminPolicy", policy =>
//        policy.RequireAuthenticatedUser().RequireRole("Admin"));
//});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();


builder.Logging.ClearProviders();
builder.Logging.AddSerilog();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseRouting();


app.UseAuthentication(); 
app.UseAuthorization();

//app.UseMiddleware<AuthMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>(); 
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DataSeeder.SeedRolesAndUsersAsync(services);
}


app.Run(); 

Log.CloseAndFlush();