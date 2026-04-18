namespace OrderApp.Models;

public class Order
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public string City { get; set; }
    public string State { get; set; }
}
