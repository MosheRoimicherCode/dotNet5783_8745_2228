using System.Text.Json.Serialization;
namespace DO;

public struct Order
{
    public int ID { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomeAdress { get; set; }
    DateTime OrderDate { get; set; }
    DateTime ShipDate { get; set; }
    DateTime DeliveryDate { get; set; }

    public override string ToString() => $@"
        Product ID={ID}: {Name}, 
        category - {Category}
    	Price: {Price}
    	Amount in stock: {InStock}
    ";

    public 
}

