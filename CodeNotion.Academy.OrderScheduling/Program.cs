using System.Reflection;
using CodeNotion.Academy.OrderScheduling.Configurations;
using CodeNotion.Academy.OrderScheduling.Cqrs.Decorators;
using CodeNotion.Academy.OrderScheduling.Database;
using MediatR;
using Timer = CodeNotion.Academy.OrderScheduling.Cqrs.Decorators.Timer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Dependency Injection
builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddTransient<Timer>();
builder.Services.AddSwagger();
builder.Services.AddNSwag();

// Mediator
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Decorators
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExecutionTimerDecorator<,>));

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseApplicationSwagger();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();