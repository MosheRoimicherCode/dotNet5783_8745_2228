namespace BO;
public class OrderItem
{
    // data
    public int ID { get; set; }
    public int ProductID { get; set; }
    public int OrderID { get; set; }
    public string? ProductName { get; set; }
    public double ProductPrice { get; set; }
    public int Amount { get; set; }
    public double TotalPrice { get; set; }

    // methods
    public override string ToString() => $@"
        OrderItem ID: {ID}
        Product ID: {ProductID}, 
        OrderID: {OrderID}
        ProductName: {ProductName}
        ProductPrice: {ProductPrice}
    	Total price (of order item ID: {ID}): {TotalPrice}
    	Amount of {ProductName} in order: {Amount}";
}
