namespace BO;

public class ProductItem
{

    // data
    public int ID { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public Category? Category { get; set; }
    public bool IsInStock { get; set; }
    public int AmontInCart { get; set; }

    // methods
    public override string ToString() => $@"
        Product ID={ID}: {Name}, 
        category - {Category}
    	Price: {Price}
    	is in stock: {IsInStock}
        AmontInCart {AmontInCart}
    ";
}




