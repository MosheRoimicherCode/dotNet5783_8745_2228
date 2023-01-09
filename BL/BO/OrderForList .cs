namespace BO;

public class OrderForList
{
    // data
    public int ID { get; set; }
    public string? CustomerName { get; set; }
    public Status? OrderStatus { get; set; }
    public int Amount { get; set; }  
    public double TotalPrice { get; set; }

    // methods
    public override string ToString() => $@"
        OrderID:{ID}, 
        CustomerName: {CustomerName}
    	OrderStatus: {OrderStatus}
        Amount: {Amount}
        totalPrice {TotalPrice}
    ";

}

