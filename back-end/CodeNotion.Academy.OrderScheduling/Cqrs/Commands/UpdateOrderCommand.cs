using CodeNotion.Academy.OrderScheduling.Database;
using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNotion.Academy.OrderScheduling.Cqrs.Commands;

public record UpdateOrderCommand(int Id, Order Order) : IRequest<Order>;

internal class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, Order>
{
    private readonly DatabaseContext _db;

    public UpdateOrderHandler(DatabaseContext db)
    {
        _db = db;
    }

    public async Task<Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderFromDb = await _db.Orders.FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken: cancellationToken);
        if (orderFromDb != null)
        {
            orderFromDb.Customer = request.Order.Customer;
            orderFromDb.OrderNumber = request.Order.OrderNumber;
            orderFromDb.CuttingDate = request.Order.CuttingDate;
            orderFromDb.PreparationDate = request.Order.PreparationDate;
            orderFromDb.BendingDate = request.Order.BendingDate;
            orderFromDb.AssemblyDate = request.Order.AssemblyDate;
        }

        await _db.SaveChangesAsync(cancellationToken);
        return orderFromDb ?? throw new InvalidOperationException();
    }
}