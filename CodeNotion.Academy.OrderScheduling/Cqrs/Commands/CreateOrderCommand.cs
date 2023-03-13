using CodeNotion.Academy.OrderScheduling.Database;
using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;

namespace CodeNotion.Academy.OrderScheduling.Cqrs.Commands;

public record CreateOrderCommand(Order? Order) : IRequest<Order>;

internal class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Order>
{
    private readonly DatabaseContext _db;

    public CreateOrderHandler(DatabaseContext db)
    {
        _db = db;
    }

    public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        if (request.Order != null)
        {
            _db.Orders.Add(request.Order);
        }

        await _db.SaveChangesAsync(cancellationToken);
        return request.Order ?? throw new InvalidOperationException();
    }
}