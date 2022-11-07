using DO;
using System.Xml.Linq;
namespace DO;

///A class to restore the witch products each client request
public struct OrderItem
{
    ///contructor
    public OrderItem(int PI, int OI, double P, int A) 
    { 
        ProductID = PI;
        OrderID = OI;
        Price = P;
        Amount = A; 
    }
    ///data
    public int ProductID;
    public int OrderID;
    public double Price;
    public int Amount;

    ///funcs
    public override string ToString() => $@"
        Product ID: {ProductID}, 
        OrderID: {OrderID}
    	Price: {Price}
    	Amount in stock: {Amount}";

}
