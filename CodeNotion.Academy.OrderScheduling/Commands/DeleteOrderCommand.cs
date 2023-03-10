using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;

namespace CodeNotion.Academy.OrderScheduling.Commands;

public class DeleteOrderCommand : IRequest<Order>
{
    public int Id { get; }

    public DeleteOrderCommand(int id)
    {
        this.Id = id;
    }
}