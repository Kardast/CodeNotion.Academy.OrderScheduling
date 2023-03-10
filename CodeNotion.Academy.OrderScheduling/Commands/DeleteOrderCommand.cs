using CodeNotion.Academy.OrderScheduling.Database;
using MediatR;

namespace CodeNotion.Academy.OrderScheduling.Commands;

public record DeleteOrderCommand(int Id) : IRequest<int>;

public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, int>
{
    private readonly DatabaseContext _db;
    private readonly Timer _timer;
    
    public DeleteOrderHandler(DatabaseContext db, Timer timer)
    {
        _db = db;
        _timer = timer;
    }

    public Task<int> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        _timer.StartTime();
        var orderToRemove = _db.Orders.FirstOrDefault(o => o.Id == request.Id);
        if (orderToRemove != null) _db.Orders.Remove(orderToRemove);
        _db.SaveChanges();
        _timer.EndTime();
        return Task.FromResult(request.Id);
    }
}
