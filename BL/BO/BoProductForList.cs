namespace BO;
using static BO.Enums;
public struct BoProductForList
{

    // data
    public int ID { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public Category? Category { get; set; }

    // methods
    public override string ToString() => $@"
        Product ID={ID}: {Name}, 
        category - {Category}
    	Price: {Price}
    ";
}

