using FluentValidation;
using GestorTareas.Models.Entities;
using GestorTareas.Repositories;
using GestorTareasAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var cs = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<RegistroTareasContext>(x =>
{
    x.UseMySql(cs, ServerVersion.AutoDetect(cs));
});

builder.Services.AddScoped(typeof(Repository<>), typeof(Repository<>));

builder.Services.AddScoped<TareasService>();

builder.Services.AddAutoMapper(x =>
{

}, typeof(Program).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

var app = builder.Build();

app.MapControllers();

app.Run();