using CodeNotion.Academy.OrderScheduling.Database;
using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;

namespace CodeNotion.Academy.OrderScheduling.Commands;

public record CreateOrderCommand(Order? Order) : IRequest<Order>;

internal class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Order>
{
    private readonly DatabaseContext _db;
    private readonly Timer _timer;

    public CreateOrderHandler(DatabaseContext db, Timer timer)
    {
        _db = db;
        _timer = timer;
    }

    public Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        _timer.StartTime();
        if (request.Order != null)
        {
            _db.Orders.Add(request.Order);
        }
        _db.SaveChanges();
        _timer.EndTime();
        return Task.FromResult(request.Order ?? throw new InvalidOperationException());
    }
}