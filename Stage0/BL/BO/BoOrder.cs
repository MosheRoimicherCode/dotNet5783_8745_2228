namespace BO;
using DO;
using static BO.Enums;
public class BoOrder
{
    ///constructor
    public BoOrder(int I, string CN, string CE, string CA, Status OA, DateTime OD, DateTime SD, DateTime DD,OrderItem DE, double TP)
    {
        ID = I;
        CustomerName = CN;
        CustomerEmail = CE;
        CustomeAdress = CA;
        OrderStatus = OA;
        OrderDate = OD;
        ShipDate = SD;
        DeliveryDate = DD;
        details = DE;
        totalPrice = TP;
    }

    ///data
    public int ID { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomeAdress { get; set; }
    public Status OrderStatus { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime ShipDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public OrderItem details { get; set; }
    public double totalPrice { get; set; }

    /// funcs
    public override string ToString() => $@"
        Product ID:{ID}, 
        CustomerName: {CustomerName}
    	CustomerEmail: {CustomerEmail}
    	CustomeAdress: {CustomeAdress}
        OrderDate: {OrderDate}
        ShipDate: {ShipDate}
        DeliveryDate: {DeliveryDate}
        details: {details}
        totalPrice {totalPrice}
    ";

}

