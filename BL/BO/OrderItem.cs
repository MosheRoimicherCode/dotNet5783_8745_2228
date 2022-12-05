namespace BO;
public class OrderItem
{
    // data
    public int ProductID { get; set; }
    public int OrderID { get; set; }
    public string? ProductName { get; set; }
    public double ProductPrice { get; set; }
    public int Amount { get; set; }
    public double TotalPrice { get; set; }

    // methods
    public override string ToString() => $@"
        Product ID: {ProductID}, 
        OrderID: {OrderID}
    	TotalPrice: {TotalPrice}
    	Amount in stock: {Amount}";
}
