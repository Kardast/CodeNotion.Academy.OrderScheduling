using CodeNotion.Academy.OrderScheduling.Database;
using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNotion.Academy.OrderScheduling.Cqrs.Queries;

public record GetListQuery(string? Customer, string? OrderNumber) : IRequest<Order[]>;

internal class GetListHandler : IRequestHandler<GetListQuery, Order[]>
{
    private readonly DatabaseContext _db;

    public GetListHandler(DatabaseContext db)
    {
        _db = db;
    }

    public async Task<Order[]> Handle(GetListQuery request, CancellationToken ct)
    {
        var orders = _db.Orders.AsQueryable();

        if (request is { Customer: { }, OrderNumber: { } })
        {
            orders = orders.Where(order =>
                order.Customer.ToLower().Contains(request.Customer.ToLower()) &&
                order.OrderNumber.ToLower().Contains(request.OrderNumber.ToLower()));
        }
        if (request.Customer is not null)
        {
            orders = orders.Where(order => order.Customer.ToLower().Contains(request.Customer.ToLower()));
        }
        if (request.OrderNumber is not null)
        {
            orders = orders.Where(order => order.OrderNumber.ToLower().Contains(request.OrderNumber.ToLower()));
        }

        return await orders.ToArrayAsync(ct);
    }
}