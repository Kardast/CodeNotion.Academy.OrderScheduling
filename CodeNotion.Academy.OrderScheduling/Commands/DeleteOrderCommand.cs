using CodeNotion.Academy.OrderScheduling.Models;
using MediatR;

namespace CodeNotion.Academy.OrderScheduling.Commands;

public record DeleteOrderCommand(int Id) : IRequest<Order>;