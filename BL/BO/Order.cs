namespace BO;
using static BO.Enums;
public class Order
{
    // data
    public int ID { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomeAdress { get; set; }
    public Status? OrderStatus { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? ShipDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public List<BO.OrderItem>? Details { get; set; }
    public double TotalPrice { get; set; }

    // methods
    public override string ToString() => $@"
        Product ID:{ID}, 
        CustomerName: {CustomerName}
    	CustomerEmail: {CustomerEmail}
    	CustomeAdress: {CustomeAdress}
        OrderDate: {OrderDate}
        ShipDate: {ShipDate}
        DeliveryDate: {DeliveryDate}
        details: {string.Join("\n",Details!)}
        totalPrice {TotalPrice}
    ";

}

