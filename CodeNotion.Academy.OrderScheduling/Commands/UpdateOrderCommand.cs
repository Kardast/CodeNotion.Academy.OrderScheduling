using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;

namespace CodeNotion.Academy.OrderScheduling.Commands;

public record UpdateOrderCommand(int Id, Order Order) : IRequest<Order>;