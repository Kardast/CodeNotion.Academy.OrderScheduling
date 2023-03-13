using CodeNotion.Academy.OrderScheduling.Database;
using MediatR;

namespace CodeNotion.Academy.OrderScheduling.Cqrs.Commands;

public record DeleteOrderCommand(int Id) : IRequest<int>;

internal class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, int>
{
    private readonly DatabaseContext _db;

    public DeleteOrderHandler(DatabaseContext db)
    {
        _db = db;
    }

    public Task<int> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var orderToRemove = _db.Orders.FirstOrDefault(o => o.Id == request.Id);
        if (orderToRemove != null) _db.Orders.Remove(orderToRemove);
        _db.SaveChanges();
        return Task.FromResult(request.Id);
    }
}