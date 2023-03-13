using System.Reflection;
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

// Mediator
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Decorators
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExecutionTimerDecorator<,>));

builder.Services.AddSwaggerGen();

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