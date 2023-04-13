using System.Runtime.CompilerServices;
using System.Reflection;
using CodeNotion.Academy.OrderScheduling.Configurations;
using CodeNotion.Academy.OrderScheduling.Cqrs.Decorators;
using CodeNotion.Academy.OrderScheduling.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Timer = CodeNotion.Academy.OrderScheduling.Cqrs.Decorators.Timer;

[assembly: InternalsVisibleTo("CodeNotion.Academy.OrderScheduling.Tests")]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Dependency Injection
builder.Services.AddDbContext<DatabaseContext>(options => options
    .UseSqlServer("data source=db5eb13a5470;initial catalog=master;user id=sa;password=myPassword7941;TrustServerCertificate=True;"));

builder.Services.AddTransient<Timer>();
builder.Services.AddSwagger();
builder.Services.AddNSwag();

builder.Services.AddCors();
// Mediator
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Decorators
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExecutionTimerDecorator<,>));

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.EnvironmentName != "IntegrationTesting")
{
    app.Services.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>().Database.Migrate();
}

app.UseCors(b => b
    .WithOrigins("http://localhost:4200")
    .AllowAnyHeader()
    .AllowCredentials()
    .AllowAnyMethod());

// Configure the HTTP request pipeline.
app.UseApplicationSwagger();

app.UseAuthorization();

app.MapControllers();

app.Run();

// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0#basic-tests-with-the-default-webapplicationfactory
public partial class Program { }