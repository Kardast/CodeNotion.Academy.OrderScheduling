using CodeNotion.Academy.OrderScheduling.Database;
using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;

namespace CodeNotion.Academy.OrderScheduling.Queries;

public record GetListQuery : IRequest<List<Order>>;

public class GetListHandler : IRequestHandler<GetListQuery, List<Order>>
{
    private readonly DatabaseContext _db;
    private readonly Timer _timer;

    public GetListHandler(DatabaseContext db, Timer timer)
    {
        _db = db;
        _timer = timer;
    }

    public Task<List<Order>> Handle(GetListQuery request, CancellationToken cancellationToken)
    {
        _timer.StartTime();
        var orders = _db.Orders.ToList();
        _timer.EndTime();
        return Task.FromResult(orders);
    }
}