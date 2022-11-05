using System.Text.Json.Serialization;
namespace DO;

///A class to restore customer details
public struct Order
{
    ///constractor
    public Order(int I, string CN, string CE, string CA, DateTime OD, DateTime SD, DateTime DD)
    {
        ID = I; 
        CustomerName = CN; 
        CustomerEmail = CE; 
        CustomeAdress = CA; 
        OrderDate = OD; 
        ShipDate = SD; 
        DeliveryDate = DD;
    }

    ///data
    public int ID { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomeAdress { get; set; }
    DateTime OrderDate { get; set; }
    DateTime ShipDate { get; set; }
    DateTime DeliveryDate { get; set; }

    /// funcs
    public override string ToString() => $@"
        Product ID:{ID}, 
        CustomerName: {CustomerName}
    	CustomerEmail: {CustomerEmail}
    	CustomeAdress: {CustomeAdress}
        OrderDate: {OrderDate}
        ShipDate: {ShipDate}
        DeliveryDate: {DeliveryDate}
    ";

}

