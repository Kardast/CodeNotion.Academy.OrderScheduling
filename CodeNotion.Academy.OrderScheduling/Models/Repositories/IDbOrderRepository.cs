using System.Collections.Generic;

namespace CodeNotion.Academy.OrderScheduling.Models.Repositories;

public interface IDbOrderRepository
{
    void StartTime();
    void EndTime();
    Order? GetById(int id);
    Order Create(Order? order);
    List<Order> All();
    void Update(Order order, Order modifiedOrder);
    Order Delete(Order order);
}