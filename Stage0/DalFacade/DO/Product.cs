using System.ComponentModel;

namespace DO;

///A class to restore product details
public struct Product
{
    ///constractor
    public Product(int I, string N, double P, int In)
    { 
        ID = I;
        Name = N; 
        Price = P; 
        InStock = In; 
    }

    ///data
    public int ID { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public Enums.Category Category { get; set; }
    public int InStock { get; set; }

    ///funcs
    public override string ToString() => $@"
        Product ID={ID}: {Name}, 
        category - {Category}
    	Price: {Price}
    	Amount in stock: {InStock}";
}
