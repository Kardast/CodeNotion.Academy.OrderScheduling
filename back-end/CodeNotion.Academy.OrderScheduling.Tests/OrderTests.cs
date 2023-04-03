using CodeNotion.Academy.OrderScheduling.Cqrs.Commands;
using CodeNotion.Academy.OrderScheduling.Database;
using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System.Data;

namespace CodeNotion.Academy.OrderScheduling.Tests;

[Collection(nameof(SharedTestCollection))]
public class OrderTests : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;
    private readonly IMediator _mediator;
    private readonly IServiceProvider _provider;

    public OrderTests(OrderApiFactory factory)
    {
        _client = factory.HttpClient;
        _resetDatabase = factory.ResetDatabaseAsync;
        _provider = factory.Services.CreateScope().ServiceProvider;
        _mediator = _provider.GetRequiredService<IMediator>();

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        var context = _provider.GetRequiredService<DatabaseContext>();
        context.Orders.AddRange(new Order[]
        {
            new() { Customer = "Alex", OrderNumber = "XR123", CuttingDate = new(2023, 03, 01), PreparationDate = new(2023, 03, 02), BendingDate = new(2023, 03, 03), AssemblyDate = new(2023, 03, 04) },
        });
        context.SaveChanges();
    }

    [Fact]
    public async Task Should_ReturnAllOrders()
    {
        // Arrange
        var context = _provider.GetRequiredService<DatabaseContext>();
        var expectedOrders = await context.Orders.ToListAsync();

        // Act & Assert
        var response = (await _client.GetAsync("/api/Order/List")).EnsureSuccessStatusCode();
        var actualOrders = await response.Content.ReadAsAsync<List<Order>>();
        Assert.Equal(expectedOrders, actualOrders);
        // add more assertions as needed
    }

    [Fact]
    public async Task Should_CreateOrder_When_OrderIsCorrect()
    {
        // Arrange
        var context = _provider.GetRequiredService<DatabaseContext>();
        var orderToCreate = new Order
        {
            Customer = "Felix",
            OrderNumber = "PW898"
        };
        var initialOrdersCount = await context.Orders.CountAsync();
        var excpectedOrdersCount = initialOrdersCount + 1;

        // Act
        var result = await _mediator.Send(new CreateOrderCommand(orderToCreate));

        // Assert
        Assert.True(result.Id >= 0);
        Assert.Equal(orderToCreate, result);

        var createdOrder = await context.Orders.FindAsync(result.Id);
        Assert.Equal(orderToCreate, createdOrder);

        var actualOrdersCount = await context.Orders.CountAsync();
        Assert.Equal(excpectedOrdersCount, actualOrdersCount);
    }

    [Fact]
    public async Task Should_UpdateOrder_When_OrderIsCorrect()
    {
        // Arrange
        var context = _provider.GetRequiredService<DatabaseContext>();
        var lastOrderId = await context.Orders.OrderBy(x => x.Id).Select(x => x.Id).LastAsync();
        var orderToUpdate = new Order()
        {
            Id = lastOrderId,
            Customer = string.Empty,
            OrderNumber = string.Empty
        };
        var excpectedOrdersCount = await context.Orders.CountAsync();

        // Act
        var result = await _mediator.Send(new UpdateOrderCommand(orderToUpdate));

        // Assert
        Assert.Equal(orderToUpdate, result);

        var updatedOrder = await context.Orders.FindAsync(result.Id);
        Assert.Equal(orderToUpdate, updatedOrder);

        var actualOrdersCount = await context.Orders.CountAsync();
        Assert.Equal(excpectedOrdersCount, actualOrdersCount);
    }

    [Fact]
    public async Task Should_DeleteOrder_When_OrderExists()
    {
        // Arrange
        var context = _provider.GetRequiredService<DatabaseContext>();
        var orderToDeleteId = await context.Orders.Select(x => x.Id).FirstAsync();
        var initialOrdersCount = await context.Orders.CountAsync();
        var expectedOrdersCount = initialOrdersCount - 1;

        // Act
        await _mediator.Send(new DeleteOrderCommand(orderToDeleteId));

        // Assert
        var deletedOrder = await context.Orders.FindAsync(orderToDeleteId);
        Assert.Null(deletedOrder); // verify that the order was deleted

        var actualOrdersCount = await context.Orders.CountAsync();
        Assert.Equal(expectedOrdersCount, actualOrdersCount);
    }

    [Fact]
    public async Task Should_NotThrow_When_DeletingNonExistingOrder()
    {
        // Arrange
        var context = _provider.GetRequiredService<DatabaseContext>();
        var excpectedOrdersCount = await context.Orders.CountAsync();
        var lastOrderId = await context.Orders.OrderBy(x => x.Id).Select(x => x.Id).LastAsync();
        var nonExistedOrderId = lastOrderId + 1;

        // Act
        await _mediator.Send(new DeleteOrderCommand(nonExistedOrderId));

        // Assert
        var actualOrdersCount = await context.Orders.CountAsync();
        Assert.Equal(excpectedOrdersCount, actualOrdersCount);
    }

    [Fact]
    public async Task Should_Throw_When_UpdatingNonExistingOrder()
    {
        // Arrange
        var context = _provider.GetRequiredService<DatabaseContext>();
        var excpectedOrdersCount = await context.Orders.CountAsync();
        var lastOrderId = await context.Orders.OrderBy(x => x.Id).Select(x => x.Id).LastAsync();
        var nonExistingOrder = new Order()
        {
            Id = lastOrderId + 1,
            Customer = string.Empty,
            OrderNumber = string.Empty
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _mediator.Send(new UpdateOrderCommand(nonExistingOrder)));

        var actualOrdersCount = await context.Orders.CountAsync();
        Assert.Equal(excpectedOrdersCount, actualOrdersCount);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}