using CodeNotion.Academy.OrderScheduling.Database;
using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;

namespace CodeNotion.Academy.OrderScheduling.Commands;

public record UpdateOrderCommand(int Id, Order Order) : IRequest<Order>;

public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, Order>
{
    private readonly DatabaseContext _db;
    private readonly Timer _timer;

    public UpdateOrderHandler(DatabaseContext db, Timer timer)
    {
        _db = db;
        _timer = timer;
    }

    public Task<Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        _timer.StartTime();
        var orderFromDb = _db.Orders.FirstOrDefault(o => o.Id == request.Id);
        if (orderFromDb != null)
        {
            orderFromDb.Customer = request.Order.Customer;
            orderFromDb.OrderNumber = request.Order.OrderNumber;
            orderFromDb.CuttingDate = request.Order.CuttingDate;
            orderFromDb.PreparationDate = request.Order.PreparationDate;
            orderFromDb.BendingDate = request.Order.BendingDate;
            orderFromDb.AssemblyDate = request.Order.AssemblyDate;
        }
        _db.SaveChanges();
        _timer.EndTime();
        return Task.FromResult(orderFromDb ?? throw new InvalidOperationException());
    }
}