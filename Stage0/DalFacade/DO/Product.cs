using static DO.Enums;
namespace DO;

///A class to restore product details
public struct Product
{
    ///constructor
    public Product(int I, string N, double P, Category c, int In)
    { 
        ID = I;
        Name = N;
        Price = P;
        Category = c;
        InStock = In; 

    }

    ///data
    public int ID { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public Category? Category { get; set; }
    public int InStock { get; set; }

    ///funcs
    public override string ToString() => $@"
        Product ID={ID}: {Name}, 
        category - {Category}
    	Price: {Price}
    	Amount in stock: {InStock}
    ";
}
