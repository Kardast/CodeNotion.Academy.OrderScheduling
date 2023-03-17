namespace CodeNotion.Academy.OrderScheduling.Models;

public class Order
{
    public int Id { get; set; }
    public string Customer { get; set; } = null!;
    public string OrderNumber { get; set; } = null!;
    public DateTime? CuttingDate { get; set; }
    public DateTime? PreparationDate { get; set; }
    public DateTime? BendingDate { get; set; }
    public DateTime? AssemblyDate { get; set; } 
}