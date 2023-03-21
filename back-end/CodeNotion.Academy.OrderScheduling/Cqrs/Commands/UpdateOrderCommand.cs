using CodeNotion.Academy.OrderScheduling.Database;
using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNotion.Academy.OrderScheduling.Cqrs.Commands;

public record UpdateOrderCommand(Order Order) : IRequest<Order>;

internal class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, Order>
{
    private readonly DatabaseContext _db;

    public UpdateOrderHandler(DatabaseContext db)
    {
        _db = db;
    }

    public async Task<Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderFromDb = await GetById(request.Order.Id);
        if (orderFromDb is null)
        {
            throw new InvalidOperationException();
        }

        orderFromDb.Customer = request.Order.Customer;
        orderFromDb.OrderNumber = request.Order.OrderNumber;
        orderFromDb.CuttingDate = request.Order.CuttingDate;
        orderFromDb.PreparationDate = request.Order.PreparationDate;
        orderFromDb.BendingDate = request.Order.BendingDate;
        orderFromDb.AssemblyDate = request.Order.AssemblyDate;

        await _db.SaveChangesAsync(cancellationToken);
        return orderFromDb ?? throw new InvalidOperationException();
    }

    private Task<Order?> GetById(int id) => _db.Orders.FirstOrDefaultAsync(or => or.Id == id);
}