using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CodeNotion.Academy.OrderScheduling.Database;

namespace CodeNotion.Academy.OrderScheduling.Models.Repositories;

public class DbOrderRepository : IDbOrderRepository
{
    private readonly DatabaseContext _db;

    public DbOrderRepository(DatabaseContext db)
    {
        _db = db;
    }

    private readonly Stopwatch _stopwatch = new();

    public void StartTime()
    {
        _stopwatch.Start();
    }

    public void EndTime()
    {
        _stopwatch.Stop();
        Console.WriteLine($"Time taken: {_stopwatch.ElapsedMilliseconds}ms");
    }

    public Order? GetById(int id) =>
        _db.Orders.FirstOrDefault(o => o.Id == id);

    public Order Create(Order? order)
    {
        _db.Orders.Add(order ?? throw new ArgumentNullException(nameof(order)));
        _db.SaveChanges();
        return order;
    }

    public List<Order> All() =>
        _db.Orders.ToList();

    public void Update(Order order, Order modifiedOrder)
    {
        order.Customer = modifiedOrder.Customer;
        order.OrderNumber = modifiedOrder.OrderNumber;
        order.CuttingDate = modifiedOrder.CuttingDate;
        order.PreparationDate = modifiedOrder.PreparationDate;
        order.BendingDate = modifiedOrder.BendingDate;
        order.AssemblyDate = modifiedOrder.AssemblyDate;

        _db.SaveChanges();
    }

    public Order Delete(Order order)
    {
        _db.Orders.Remove(order);
        _db.SaveChanges();
        return order;
    }
}