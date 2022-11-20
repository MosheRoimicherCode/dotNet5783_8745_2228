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
        details = DE;
        totalPrice = TP;
    }

    ///data
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomeAdress { get; set; }
    public OrderItem details { get; set; }
    public double totalPrice { get; set; }

    /// funcs
    public override string ToString() => $@"
        CustomerName: {CustomerName}
    	CustomerEmail: {CustomerEmail}
    	CustomeAdress: {CustomeAdress}
        details {details}
        totalPrice {totalPrice}
    ";

}

