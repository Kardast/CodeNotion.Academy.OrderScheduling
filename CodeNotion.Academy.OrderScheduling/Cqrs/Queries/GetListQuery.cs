using CodeNotion.Academy.OrderScheduling.Database;
using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNotion.Academy.OrderScheduling.Cqrs.Queries;

public record GetListQuery : IRequest<List<Order>>;

internal class GetListHandler : IRequestHandler<GetListQuery, List<Order>>
{
    private readonly DatabaseContext _db;

    public GetListHandler(DatabaseContext db)
    {
        _db = db;
    }

    public async Task<List<Order>> Handle(GetListQuery request, CancellationToken cancellationToken)
    {
        var orders = await _db.Orders.ToListAsync(cancellationToken: cancellationToken);
        return orders;
    }
}