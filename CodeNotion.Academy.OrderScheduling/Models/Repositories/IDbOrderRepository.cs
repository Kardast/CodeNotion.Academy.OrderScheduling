using System.Collections.Generic;

namespace CodeNotion.Academy.OrderScheduling.Models.Repositories;

public interface IDbOrderRepository
{
    void StartTime();
    void EndTime();
    Order? GetById(int id);
    void Create(Order? order);
    List<Order> All();
    void Update(Order order, Order modifiedOrder);
    void Delete(Order order);
}