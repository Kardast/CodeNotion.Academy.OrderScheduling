using CodeNotion.Academy.OrderScheduling.Commands;
using CodeNotion.Academy.OrderScheduling.Models;
using CodeNotion.Academy.OrderScheduling.Models.Repositories;
using MediatR;

namespace CodeNotion.Academy.OrderScheduling.Handlers;

public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, Order>
{
    private readonly IDbOrderRepository _orderRepository;

    public UpdateOrderHandler(IDbOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public Task<Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderFromDb = _orderRepository.GetById(request.Id);
        return Task.FromResult(_orderRepository.Update(orderFromDb ?? throw new InvalidOperationException(), request.Order));
    }
}