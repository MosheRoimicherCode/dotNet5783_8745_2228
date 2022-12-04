using DO;
using System.Xml.Linq;
namespace DO;

///A class to restore the witch products each client request
public struct OrderItem
{
    ///constructor
    public OrderItem(int I, int PI, int OI, double P, int A) 
    {
        ID = I;
        ProductID = PI;
        OrderID = OI;
        Price = P;
        Amount = A; 
    }
    ///data
    public int ID { get; set; }
    public int ProductID { get; set; }
    public int OrderID { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }

    ///funcs
    public override string ToString() => $@"
        ID: {ID}, 
        Product ID: {ProductID}, 
        OrderID: {OrderID}
    	Price: {Price}
    	Amount in stock: {Amount}";

}
