using CodeNotion.Academy.OrderScheduling.Cqrs.Commands;
using CodeNotion.Academy.OrderScheduling.Database;
using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNotion.Academy.OrderScheduling.Tests;

public class OrderTests : IClassFixture<OrderApiFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;
    private readonly IMediator _mediator;
    private readonly IServiceProvider _provider;

    public OrderTests(OrderApiFactory factory)
    {
        _client = factory.HttpClient;
        _resetDatabase = factory.ResetDatabaseAsync;
        var services = new ServiceCollection();
        //services.AddDbContext<DatabaseContext>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        _provider = services.BuildServiceProvider().CreateScope().ServiceProvider;
        _mediator = _provider.GetRequiredService<IMediator>();
    }

    [Fact]
    public async Task TestGetOrders()
    {
        // Arrange
        List<Order> expectedOrders = new()
        {
            /* fill in expected orders */
        };
        if (expectedOrders == null) throw new ArgumentNullException(nameof(expectedOrders));

        // Act
        var response = await _client.GetAsync("/api/Order/List");
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadAsAsync<List<Order>>();

        // Assert
        Assert.Equal(expectedOrders.Count, orders.Count);
        // add more assertions as needed
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
            .OrderByDescending(x => x.Id)
            .FirstOrDefault();

        var newOrder = new Order
        {
            Id = order!.Id,
            Customer = "new test",
            OrderNumber = "456"
        };

        // Act
        var result = await _mediator.Send(new UpdateOrderCommand(newOrder));
        var updatedOrder = await _provider.GetRequiredService<DatabaseContext>().Orders.FindAsync(result.Id);

        // Assert
        Assert.NotNull(updatedOrder);
        Assert.True(updatedOrder.Id > 0);
        Assert.Equal(newOrder.Customer, updatedOrder.Customer);
        Assert.Equal(newOrder.OrderNumber, updatedOrder.OrderNumber);
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
        var originalOrderCount = await _provider.GetRequiredService<DatabaseContext>().Orders.CountAsync();
        await _mediator.Send(new DeleteOrderCommand(order!.Id));
        var orderCount = await _provider.GetRequiredService<DatabaseContext>().Orders.CountAsync();
        var deletedOrder = await _provider.GetRequiredService<DatabaseContext>().Orders.FindAsync(order.Id);

        // Assert
        Assert.Null(deletedOrder); // Verify that the order was deleted
        Assert.Equal(originalOrderCount - 1, orderCount);
    }

    [Fact]
    public async Task Should_NotThrowError_When_DeletingNonExistingOrder()
    {
        // Arrange
        var order = _provider
            .GetRequiredService<DatabaseContext>()
            .Orders
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .FirstOrDefault();

        // Act
        var originalOrderCount = await _provider.GetRequiredService<DatabaseContext>().Orders.CountAsync();
        await _mediator.Send(new DeleteOrderCommand(order!.Id + 1));
        var orderCount = await _provider.GetRequiredService<DatabaseContext>().Orders.CountAsync();

        // Assert
        Assert.Equal(originalOrderCount, orderCount);
    }

    [Fact]
    public async Task Should_ThrowError_When_UpdatingNonExistingOrder()
    {
        // Arrange
        var order = _provider
            .GetRequiredService<DatabaseContext>()
            .Orders
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .FirstOrDefault();

        var newOrder = new Order
        {
            Id = order!.Id + 1,
            Customer = "new test",
            OrderNumber = "456"
        };

        // Act
        var originalOrderCount = await _provider.GetRequiredService<DatabaseContext>().Orders.CountAsync();
        async Task UpdateOrder() => await _mediator.Send(new UpdateOrderCommand(newOrder));
        var orderCount = await _provider.GetRequiredService<DatabaseContext>().Orders.CountAsync();

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(UpdateOrder);
        Assert.Equal(originalOrderCount, orderCount);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}