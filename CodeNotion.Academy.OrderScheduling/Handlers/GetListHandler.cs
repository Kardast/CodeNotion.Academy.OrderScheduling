using CodeNotion.Academy.OrderScheduling.Models;
using CodeNotion.Academy.OrderScheduling.Models.Repositories;
using CodeNotion.Academy.OrderScheduling.Queries;
using MediatR;

namespace CodeNotion.Academy.OrderScheduling.Handlers;

public class GetListHandler : IRequestHandler<GetListQuery, List<Order>>
{
    private readonly IDbOrderRepository _orderRepository;
    
    public GetListHandler(IDbOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public Task<List<Order>> Handle(GetListQuery request, CancellationToken cancellationToken)
    {
        var orders = _orderRepository.All();
        return Task.FromResult(orders);
    }
}