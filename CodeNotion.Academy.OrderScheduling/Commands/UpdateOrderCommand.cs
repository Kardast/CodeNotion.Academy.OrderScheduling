using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;

namespace CodeNotion.Academy.OrderScheduling.Commands;

public class UpdateOrderCommand : IRequest<Order>
{
    public int Id { get; }
    public Order Order { get; }
    
    public UpdateOrderCommand(int id, Order order)
    {
        Id = id;
        Order = order;
    }
}