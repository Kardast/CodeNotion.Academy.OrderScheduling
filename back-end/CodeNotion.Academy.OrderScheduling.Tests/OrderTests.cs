using CodeNotion.Academy.OrderScheduling.Cqrs.Commands;
using CodeNotion.Academy.OrderScheduling.Database;
using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNotion.Academy.OrderScheduling.Tests;

public class OrderTests
{
    private readonly IMediator _mediator;
    private readonly IServiceProvider _provider;

    public OrderTests()
    {
        var services = new ServiceCollection();
        services.AddDbContext<DatabaseContext>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        _provider = services.BuildServiceProvider().CreateScope().ServiceProvider;
        _mediator = _provider.GetRequiredService<IMediator>();
    }

    [Fact]
    public async Task Should_CreateOrder_When_OrderIsCorrect()
    {
        // Arrange
        var order = new Order
        {
            Customer = "test",
            OrderNumber = "123"
        };

        // Act
        var result = await _mediator.Send(new CreateOrderCommand(order));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal(order.Customer, result.Customer);
        Assert.Equal(order.OrderNumber, result.OrderNumber);
    }

    [Fact]
    public async Task Should_UpdateOrder_When_OrderIsCorrect()
    {
        // Arrange
        var order = _provider
            .GetRequiredService<DatabaseContext>()
            .Orders
            .AsNoTracking()
            .OrderByDescending(x => x.Id).FirstOrDefault();

        // Act

        // Assert
    }

    [Fact]
    public async Task Should_DeleteOrder_When_OrderExists()
    {
        // Arrange
        var order = _provider
            .GetRequiredService<DatabaseContext>()
            .Orders
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .FirstOrDefault();

        // Act

        // Assert
    }

    [Fact]
    public async Task Should_NotThrowError_When_DeletingNonExistingOrder()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public async Task Should_ThrowError_When_UpdatingNonExistingOrder()
    {
        // Arrange

        // Act

        // Assert
    }
}