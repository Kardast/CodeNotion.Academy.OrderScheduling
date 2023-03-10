using CodeNotion.Academy.OrderScheduling.Commands;
using CodeNotion.Academy.OrderScheduling.Models;
using CodeNotion.Academy.OrderScheduling.Models.Repositories;
using MediatR;

namespace CodeNotion.Academy.OrderScheduling.Handlers;

public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, Order>
{
    private readonly IDbOrderRepository _orderRepository;

    public DeleteOrderHandler(IDbOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public Task<Order> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = _orderRepository.GetById(request.Id);
        return Task.FromResult<Order>(_orderRepository.Delete(order ?? throw new InvalidOperationException()));
    }
}
