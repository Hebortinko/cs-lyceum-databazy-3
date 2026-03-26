namespace EshopApp2.Models;

public class OrderDetail
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; }
    public string ProductName { get; set; }
    public decimal Total { get; set; }
}