namespace Client.Models;

public class OrderViewModel
{
    public int Id { get; set; }
    public string Username { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderItemViewModel> OrderItems { get; set; }
}