using CodeNotion.Academy.OrderScheduling.Cqrs.Commands;
using CodeNotion.Academy.OrderScheduling.Cqrs.Queries;
using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeNotion.Academy.OrderScheduling.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [Route("")]
    [HttpGet]
    [ProducesResponseType(typeof(List<Order>), 200)]
    public async Task<IActionResult> List()
    {
        var query = new GetListQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [Route("{id:int}")]
    [HttpPost]
    public async Task<IActionResult> Update(int id, Order order)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = new UpdateOrderCommand(id, order);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [Route("{id:int}")]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteOrderCommand(id);
        await _mediator.Send(command);
        return Ok("File removed");
    }
}