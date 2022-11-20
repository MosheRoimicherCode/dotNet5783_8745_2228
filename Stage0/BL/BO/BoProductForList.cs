namespace BO;
using static BO.Enums;
public struct BoProductForList
{
    ///constructor
    public BoProductForList(int I, string N, double P, Category c)
    {
        ID = I;
        Name = N;
        Price = P;
        Category = c;
    }

    ///data
    public int ID { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public Category Category { get; set; }

    ///funcs
    public override string ToString() => $@"
        Product ID={ID}: {Name}, 
        category - {Category}
    	Price: {Price}
    ";
}

