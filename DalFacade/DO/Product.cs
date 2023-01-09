namespace DO;

///A class to restore product details
public struct Product
{
    ///data
    public int ID { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public Category? Category { get; set; }
    public int InStock { get; set; }

    ///funcs
    public override string ToString() => $@"
        Product ID: {ID} 
        Product Name: {Name} 
        category: {Category}
    	Price: {Price}
    	Amount in stock: {InStock}
    ";
}
