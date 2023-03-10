using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;

namespace CodeNotion.Academy.OrderScheduling.Commands;

public class CreateOrderCommand : IRequest<Order>
{
    public Order? Order { get; }
    public CreateOrderCommand(Order? order)
    {
        this.Order = order;
    }
}