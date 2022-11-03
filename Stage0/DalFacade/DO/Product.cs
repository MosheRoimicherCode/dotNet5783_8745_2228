using System.ComponentModel;

namespace DO;

public struct Product
{
    public Product(int I, string N, double P, int In) { ID = I; Name = N; Price = P; InStock = In; }
    public int ID { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public Enums.Category Category { get; set; }
    public int InStock { get; set; }

    public override string ToString() => $@"
        Product ID={ID}: {Name}, 
        category - {Category}
    	Price: {Price}
    	Amount in stock: {InStock}";
}
