using static DO.Enums;
namespace DO;

///A class to restore product details
public struct Product
{
    ///constractor
    public Product()
    {
        int ID = 9999;
        string Name = "test";
        double Price = 9.99;
        Category Category = Category.business;
        int InStock = 999;
    }

    public Product(int I, string N, double P, Category c, int In)
    { 
        int ID = I;
        string Name = N;
        double Price = P;
        Category Category = c;
        int InStock = In; 
    }

    ///data
    public int ID { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public Category Category { get; set; }
    public int InStock { get; set; }

    ///funcs
    public override string ToString() => $@"
        Product ID={ID}: {Name}, 
        category - {Category}
    	Price: {Price}
    	Amount in stock: {InStock}
    ";
}
