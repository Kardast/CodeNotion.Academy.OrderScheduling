using CodeNotion.Academy.OrderScheduling.Cqrs.Commands;
using CodeNotion.Academy.OrderScheduling.Cqrs.Queries;
using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeNotion.Academy.OrderScheduling.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = new CreateOrderCommand(order);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet]
    [Route("[action]")]
    [ProducesResponseType(typeof(List<Order>), 200)]
    public async Task<IActionResult> List([FromQuery] string? customer, [FromQuery] string? orderNumber)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var query = new GetListQuery(customer, orderNumber);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Order order)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = new UpdateOrderCommand(order);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete]
    [Route("[action]/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = new DeleteOrderCommand(id);
        await _mediator.Send(command);
        return Ok("File removed");
    }
}