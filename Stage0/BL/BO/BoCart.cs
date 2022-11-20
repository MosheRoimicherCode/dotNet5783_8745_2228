namespace BO;
using DO;
using static BO.Enums;
public class BoCart
{
    ///constructor
    public BoCart(string CN, string CE, string CA, OrderItem DE, double TP)
    {
        CustomerName = CN;
        CustomerEmail = CE;
        CustomeAdress = CA;
        Details = DE;
        TotalPrice = TP;
    }

    ///data
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomeAdress { get; set; }
    public OrderItem Details { get; set; }
    public double TotalPrice { get; set; }

    /// funcs
    public override string ToString() => $@"
        CustomerName: {CustomerName}
    	CustomerEmail: {CustomerEmail}
    	CustomeAdress: {CustomeAdress}
        details {Details}
        totalPrice {TotalPrice}
    ";

}

