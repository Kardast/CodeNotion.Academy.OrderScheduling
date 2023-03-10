using CodeNotion.Academy.OrderScheduling.Commands;
using CodeNotion.Academy.OrderScheduling.Models;
using CodeNotion.Academy.OrderScheduling.Models.Repositories;
using CodeNotion.Academy.OrderScheduling.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeNotion.Academy.OrderScheduling.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IDbOrderRepository _orderRepository;
    private readonly IMediator _mediator;
    
    public OrderController (IDbOrderRepository orderRepository, IMediator mediator)
    {
        _orderRepository = orderRepository;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderCommand command)
    {
        _orderRepository.StartTime();
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _mediator.Send(command);
        _orderRepository.EndTime();
        return Ok(result);
    }
    
    [Route("")]
    [HttpGet]
    public async Task<IActionResult> List()
    {
        _orderRepository.StartTime();
        var query = new GetListQuery();
        var result = await _mediator.Send(query);
        _orderRepository.EndTime();
        return Ok(result);
    }
    
    [Route("{id:int}")]
    [HttpPost]
    public async Task<IActionResult> Update(int id, Order order)
    {
        _orderRepository.StartTime();
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = new UpdateOrderCommand(id, order);
        await _mediator.Send(command);
        _orderRepository.EndTime();
        return Ok(order);
    }
    
    [Route("{id:int}")]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        _orderRepository.StartTime();
        var command = new DeleteOrderCommand(id);
        await _mediator.Send(command);
        _orderRepository.EndTime();
        return Ok("Removed file");
    }
}