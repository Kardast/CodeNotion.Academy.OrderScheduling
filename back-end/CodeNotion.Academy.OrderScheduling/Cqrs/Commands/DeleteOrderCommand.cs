using CodeNotion.Academy.OrderScheduling.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNotion.Academy.OrderScheduling.Cqrs.Commands;

public record DeleteOrderCommand(int Id) : IRequest<int>;

internal class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, int>
{
    private readonly DatabaseContext _db;

    public DeleteOrderHandler(DatabaseContext db)
    {
        _db = db;
    }

    public async Task<int> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var orderToRemove = await _db.Orders.FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
        if (orderToRemove != null)
        {
            _db.Orders.Remove(orderToRemove);
            await _db.SaveChangesAsync(cancellationToken);
        }

        return request.Id;
    }
}