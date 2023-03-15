using CodeNotion.Academy.OrderScheduling.Database;
using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNotion.Academy.OrderScheduling.Cqrs.Queries;

public record GetListQuery(string? Customer, string? OrderNumber) : IRequest<List<Order>>;

internal class GetListHandler : IRequestHandler<GetListQuery, List<Order>>
{
    private readonly DatabaseContext _db;

    public GetListHandler(DatabaseContext db)
    {
        _db = db;
    }

    public async Task<List<Order>> Handle(GetListQuery request, CancellationToken cancellationToken)
    {
        var orders = _db.Orders;

        if (request is { Customer: { }, OrderNumber: { } })
        {
            return await orders.Where(order =>
                    request.Customer != null &&
                    order.Customer.ToLower().Contains(request.Customer.ToLower()) &&
                    order.OrderNumber.ToLower().Contains(request.OrderNumber.ToLower()))
                .ToListAsync(cancellationToken: cancellationToken);
        }
        else if (request.Customer != null)
        {
            return await orders.Where(order => order.Customer.ToLower().Contains(request.Customer.ToLower()))
                .ToListAsync(cancellationToken: cancellationToken);
        }
        else if (request.OrderNumber != null)
        {
            return await orders.Where(order => order.OrderNumber.ToLower().Contains(request.OrderNumber.ToLower()))
                .ToListAsync(cancellationToken: cancellationToken);
        }

        return await orders.ToListAsync(cancellationToken: cancellationToken);
    }
}