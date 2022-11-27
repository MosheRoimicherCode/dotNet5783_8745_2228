namespace BO;
using DO;
using static BO.Enums;
public class BoCart
{

    ///data
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomeAdress { get; set; }
    public List<OrderItem> Details = new List<OrderItem>();
    public double TotalPrice { get; set; }

    /// funcs
    public override string ToString() => $@"
        CustomerName:  {CustomerName}
    	CustomerEmail: {CustomerEmail}
    	CustomeAdress: {CustomeAdress}
        details        {Details.ToString()}
        totalPrice     {TotalPrice}
    ";

}

