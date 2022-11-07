
namespace DO;

public struct Enums
{
    public enum Category { footwear, outerwear, business };
    public enum choise1 { product = 1, order, orderItem };
    public enum choise2 { a = 1, b, c, d, e, f, g };

}

//switch (choise1)
//{
//    case 1:
//}
//// switch (choice2)
////{
////    case a:
////        Console.WriteLine("enter ID:");
////        int ID1 = Convert.ToInt32(Console.ReadLine());
////        Console.WriteLine("enter name:");
////        string name = Console.ReadLine();
////        Console.WriteLine("enter price:");
////        double price = Convert.ToDouble(Console.ReadLine());
////        Console.WriteLine("enter category:");
////        int category = Convert.ToInt32(Console.ReadLine());
////        Enums.Category c = new Enums.Category();
////        switch (category)
////        {
////            case 1:
////                c = Enums.Category.footwear;
////                break;
////            case 2:
////                c = Enums.Category.outerwear;
////                break;
////            case 3:
////                c = Enums.Category.business;
////                break;
////        }
////        Console.WriteLine("enter inStock:");
////        int inStock = Convert.ToInt32(Console.ReadLine());
////        Product p1 = new Product(ID1, name, price, c, inStock);
////        _dalProduct.CreateProduct(p1);
////        break;
////    case b:
////        Console.WriteLine("enter ID:");
////        int ID2 = Convert.ToInt32(Console.ReadLine());
////        Console.WriteLine(_dalProduct.GetProduct(ID2););
////        break;
////    case c:
////        _dalProduct.GetAll();
////        break;
////    case d:
////        Console.WriteLine("enter ID of exist product:");
////        int existID4 = Convert.ToInt32(Console.ReadLine());
////        Console.WriteLine("enter ID:");
////        int ID4 = Convert.ToInt32(Console.ReadLine());
////        Console.WriteLine("enter name:");
////        string name4 = Console.ReadLine();
////        Console.WriteLine("enter price:");
////        double price4 = Convert.ToDouble(Console.ReadLine());
////        Console.WriteLine("enter category:");
////        int category4 = Convert.ToInt32(Console.ReadLine());
////        Enums.Category c4 = new Enums.Category();
////        switch (category4)
////        {
////            case 1:
////                c4 = Enums.Category.footwear;
////                break;
////            case 2:
////                c4 = Enums.Category.outerwear;
////                break;
////            case 3:
////                c4 = Enums.Category.business;
////                break;
////        }
////        Console.WriteLine("enter inStock:");
////        int inStock4 = Convert.ToInt32(Console.ReadLine());
////        Product p4 = new Product(ID4, name4, price4, c4, inStock4);
////        _dalProduct.RunOverProduct(existID4, p4);
////        break;
////    case e:
////        Console.WriteLine("enter ID:");
////        int ID5 = Convert.ToInt32(Console.ReadLine());
////        _dalProduct.DeleteProduct(ID5);
////        Console.WriteLine(ID5);

////        break;
////break;
////}

////case 2:
////    switch (choice2)
////    {
////        case 1:
////        Console.WriteLine("enter ID:");
////        int ID1 = Convert.ToInt32(Console.ReadLine());
////        Console.WriteLine("enter name:");
////        string name = Console.ReadLine();
////        Console.WriteLine("enter email:");
////        string email = Console.ReadLine();
////        Console.WriteLine("enter adress:");
////        string adress = Console.ReadLine();
////        Console.WriteLine("enter OrderDate:");
////        DateTime OrderDate = Convert.ToDateTime(Console.ReadLine());
////        Console.WriteLine("enter ShipDate:");
////        DateTime ShipDate = Convert.ToDateTime(Console.ReadLine());
////        Console.WriteLine("enter DeliveryDate:");
////        DateTime DeliveryDate = Convert.ToDateTime(Console.ReadLine());
////        Order o1 = new Order(ID1, name, email, adress, OrderDate, ShipDate, DeliveryDate);
////        _dalOrder.CreateOrder(o1);
////        break;
////    case 2:
////        Console.WriteLine("enter ID:");
////        int ID2 = Convert.ToInt32(Console.ReadLine());
////        Console.WriteLine(_dalOrder.GetOrder(ID2););
////        break;
////   case 3:
////        _dalOrder.GetAll();
////        break;
////    case 4:
////        Console.WriteLine("enter ID of exist product:");
////        int existID4 = Convert.ToInt32(Console.ReadLine());
////        Console.WriteLine("enter ID:");
////        int ID4 = Convert.ToInt32(Console.ReadLine());
////        Console.WriteLine("enter name:");
////        string name4 = Console.ReadLine();
////        Console.WriteLine("enter email:");
////        string email4 = Console.ReadLine();
////        Console.WriteLine("enter adress:");
////        string adress4 = Console.ReadLine();
////        Console.WriteLine("enter OrderDate:");
////        DateTime OrderDate4 = Convert.ToDateTime(Console.ReadLine());
////        Console.WriteLine("enter ShipDate:");
////        DateTime ShipDate4 = Convert.ToDateTime(Console.ReadLine());
////        Console.WriteLine("enter DeliveryDate:");
////        DateTime DeliveryDate4 = Convert.ToDateTime(Console.ReadLine());
////        Order o4 = new Order(ID4, name4, email4, adress4, OrderDate4, ShipDate4, DeliveryDate4);
////        _dalOrder.RunOverOrder(existID4, o4);
////        break;
////   case 5:
////        Console.WriteLine("enter ID:");
////        int ID5 = Convert.ToInt32(Console.ReadLine());
////        _dalOrder.DeleteOrder(ID5);
////        Console.WriteLine(ID5);
////        break;
//// };

////case "orderItem":
////    switch (choice2)
////    {
////        case 1:
////    Console.WriteLine("enter ProductID:");
////    int ID1 = Convert.ToInt32(Console.ReadLine());
////    Console.WriteLine("enter OrderID:");
////    int ID11 = Convert.ToInt32(Console.ReadLine());
////    Console.WriteLine("enter price:");
////    Double price = Convert.ToDouble(Console.ReadLine());
////    Console.WriteLine("enter amount:");
////    int amount = Convert.ToInt32(Console.ReadLine());
////    OrderItem oi1 = new OrderItem(ID1, ID11, price, amount);
////    _dalOrderItem.CreateOrderItem(oi1);
////    break;
////case 2:
////    Console.WriteLine("enter ID:");
////    int ID2 = Convert.ToInt32(Console.ReadLine());
////    int ID3 = Convert.ToInt32(Console.ReadLine());

////    _dalOrderItem.GetOrderItem(ID2,ID3);
////    Console.WriteLine(ID2);
////    break;
////case 3:
////    _dalOrderItem.GetAll();
////    break;
//// case 4:
////    Console.WriteLine("enter ID of exist product:");
////    int existID4 = Convert.ToInt32(Console.ReadLine());
////    Console.WriteLine("enter ProductID:");
////    int ID4 = Convert.ToInt32(Console.ReadLine());
////    Console.WriteLine("enter OrderID:");
////    int ID44 = Convert.ToInt32(Console.ReadLine());
////    Console.WriteLine("enter price:");
////    Double price4 = Convert.ToDouble(Console.ReadLine());
////    Console.WriteLine("enter amount:");
////    int amount4 = Convert.ToInt32(Console.ReadLine());
////    OrderItem oi4 = new OrderItem(ID4, ID44, price4, amount4);
////    _dalOrderItem.RunOverOrderItem(existID4, ID44, oi4);
////    break;
////case 5:
////    Console.WriteLine("enter ID:");
////    int ID5 = Convert.ToInt32(Console.ReadLine());
////    int ID6 = Convert.ToInt32(Console.ReadLine());

////    _dalOrderItem.DeleteOrderItem(ID5, ID6);
////    Console.WriteLine(ID5);
////break;
////};
