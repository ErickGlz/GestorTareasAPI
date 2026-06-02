using FluentValidation;
using GestorTareasAPI.Models.Entities;
using GestorTareasAPI.Repositories;
using GestorTareasAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var cs = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<RegistrotareasContext>(x =>
{
    x.UseMySql(cs, ServerVersion.AutoDetect(cs));
}); 
var jwtKey = builder.Configuration["Jwt:Key"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddScoped(typeof(Repository<>), typeof(Repository<>));
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TareasService>();

builder.Services.AddAutoMapper(x =>
{

}, typeof(Program).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();