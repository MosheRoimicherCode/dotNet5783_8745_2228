namespace BO;
using DO;
using static BO.Enums;
public class BoOrderForList
{
    ///constructor
    public BoOrderForList(int I, string CN, string CE, int A, string CA, Status OA, DateTime OD, DateTime SD, DateTime DD, OrderItem DE, double TP)
    {
        ID = I;
        CustomerName = CN;
        OrderStatus = OA;
        Amount = A;
        totalPrice = TP;
    }

    ///data
    public int ID { get; set; }
    public string CustomerName { get; set; }
    public Status OrderStatus { get; set; }
    public int Amount { get; set; }  
    public double totalPrice { get; set; }

    /// funcs
    public override string ToString() => $@"
        Product ID:{ID}, 
        CustomerName: {CustomerName}
    	OrderStatus: {OrderStatus}
        Amount: {Amount}
        totalPrice {totalPrice}
    ";

}

