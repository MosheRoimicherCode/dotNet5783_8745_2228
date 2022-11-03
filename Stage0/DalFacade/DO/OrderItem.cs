using DO;
using System.Xml.Linq;
namespace DO;

public struct OrderItem
{
    public OrderItem(int PI, int OI, int P, int A) { ProductID = PI; OrderID = OI; Price = P; Amount = A; }
    public int ProductID { get; set; }
    public int OrderID { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }

    public override string ToString() => $@"
        Product ID: {ProductID}, 
        OrderID: {OrderID}
    	Price: {Price}
    	Amount in stock: {Amount}";

}
