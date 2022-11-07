using static DO.Enums;
namespace DO;

///A class to restore product details
public struct Product
{
    ///constractor

    public Product(int I, string N, double P, Category c, int In)
    { 
        ID = I;
        Name = N;
        Price = P;
        Category = c;
        InStock = In; 
    }

    ///data
    public int ID;
    public string Name;
    public double Price;
    public Category Category;
    public int InStock;

    ///funcs
    public override string ToString() => $@"
        Product ID={ID}: {Name}, 
        category - {Category}
    	Price: {Price}
    	Amount in stock: {InStock}
    ";
}
