using CodeNotion.Academy.OrderScheduling.Cqrs.Commands;
using CodeNotion.Academy.OrderScheduling.Database;
using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNotion.Academy.OrderScheduling.Tests;

public class OrderTests
{
    [Fact]
    public async Task Should_CreateOrder_When_OrderIsCorrect()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddDbContext<DatabaseContext>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        var provider = services.BuildServiceProvider().CreateScope().ServiceProvider;
        var mediator = provider.GetRequiredService<IMediator>();
        var orderToCreate = new Order
        {
            Customer = "test",
            OrderNumber = "123"
        };

        // Act
        var result = await mediator.Send(new CreateOrderCommand(orderToCreate));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal(orderToCreate.Customer, result.Customer);
        Assert.Equal(orderToCreate.OrderNumber, result.OrderNumber);
    }
}