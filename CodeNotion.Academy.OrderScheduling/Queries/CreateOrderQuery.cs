using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;

namespace CodeNotion.Academy.OrderScheduling.Queries;

public class CreateOrderQuery : IRequest<Order>
{
    public Order order { get; }
    
    public CreateOrderQuery(Order order)
    {
        this.order = order;
    }
}