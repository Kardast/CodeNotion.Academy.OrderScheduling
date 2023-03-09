using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;

namespace CodeNotion.Academy.OrderScheduling.Commands;

public class CreateOrderCommand : IRequest<Order>
{
    public CreateOrderCommand(Order? order)
    {
        this.order = order;
    }

    public Order? order { get; set; }
}