using DO;
namespace Dal;

internal class Program
{
    static DalOrder _dalOrder = new DalOrder(); 
    static Product _productArr = new Product();
    static OrderItem _orderItemArr = new OrderItem();

    public static void Main()
    {
        Console.WriteLine("enter name of Item:");
        string choice1 = Console.ReadLine();
        Console.WriteLine("enter function number:");
        string choice2 = Console.ReadLine();

        switch (choice1)
        {
            case "product":
                switch (choice2)
                {
                    case "1":
                        Console.WriteLine("enter ID:");
                        int ID = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("enter name:");
                        string name = Console.ReadLine();
                        Console.WriteLine("enter price:");
                        double price = Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine("enter category:");
                        int category = Convert.ToInt32(Console.ReadLine());
                        Enums.Category c = new Enums.Category();
                        switch (category)
                        {
                            case 1:
                                c = Enums.Category.footwear;
                                break;
                            case 2:
                                c = Enums.Category.outerwear;
                                break;
                            case 3:
                                c = Enums.Category.business;
                                break;
                        }
                        Console.WriteLine("enter inStock:");
                        int inStock = Convert.ToInt32(Console.ReadLine());

                        Product p = new Product(ID, name, price, c, inStock);
                        _productArr.cre
                        break;
                }
                break;
            case "product":
                break;
            case "orderitem":
                break;
        }

    }
}