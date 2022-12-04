namespace BO;
using DO;
using static BO.Enums;
public class BoOrderForList
{
    // data
    public int ID { get; set; }
    public string? CustomerName { get; set; }
    public Status? OrderStatus { get; set; }
    public int Amount { get; set; }  
    public double TotalPrice { get; set; }

    // methods
    public override string ToString() => $@"
        Product ID:{ID}, 
        CustomerName: {CustomerName}
    	OrderStatus: {OrderStatus}
        Amount: {Amount}
        totalPrice {TotalPrice}
    ";

}

