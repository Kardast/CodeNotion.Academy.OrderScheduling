using CodeNotion.Academy.OrderScheduling.Models;
using CodeNotion.Academy.OrderScheduling.Models.Repositories;
using MediatR;

namespace CodeNotion.Academy.OrderScheduling.Commands;

public record CreateOrderCommand(Order? Order) : IRequest<Order>;

internal class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Order>
{
    private readonly IDbOrderRepository _orderRepository;

    public CreateOrderHandler(IDbOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken) =>
        Task.FromResult(_orderRepository.Create(request.Order));
}