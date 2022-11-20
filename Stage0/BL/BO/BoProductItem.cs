namespace BO;
using static BO.Enums;
public class BoProductItem
{
    ///constructor
    public BoProductItem(int I, string N, double P, Category c, bool In, int AIC)
    {
        ID = I;
        Name = N;
        Price = P;
        Category = c;
        IsInStock = In;
        AmontInCart = AIC;
    }

    ///data
    public int ID { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public Category Category { get; set; }
    public bool IsInStock { get; set; }
    public int AmontInCart { get; set; }

    ///funcs
    public override string ToString() => $@"
        Product ID={ID}: {Name}, 
        category - {Category}
    	Price: {Price}
    	is in stock: {IsInStock}
        AmontInCart {AmontInCart}
    ";
}




