namespace Dal; 

using DalApi;
using DO;

internal class Program
{
    static public void Main()
    {
        try
        {
            IDal dal = Factory.Get() ?? throw new Exception("dal Factory return null value (DalTest.Idal)");

            while (true)
            {
                Console.WriteLine("select an Item: \n 0 - exit \n 1 - product \n 2 - order \n 3 - orderItem \n");
                string? choice1 = Console.ReadLine();
                if (choice1 == "0") throw new ProgramExit("Exit Program");


                Console.WriteLine("enter function number:" +
                    "\n 0 - exit " +
                    "\n 1 - add " +
                    "\n 2 - get" +
                    "\n 3 - getAll" +
                    "\n 4 - update " +
                    "\n 5 - delete");
                string? choice2 = Console.ReadLine();

                if (choice1 == "1")
                {
                    switch (choice2)
                    {
                        case "0":
                            throw new ProgramExit("Exit Program");
                        case "1":
                            Console.WriteLine("enter ID:");
                            int ID1 = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("enter name:");
                            string? name = Console.ReadLine();
                            Console.WriteLine("enter price:");
                            double price = Convert.ToDouble(Console.ReadLine());
                            Console.WriteLine("enter category:");
                            var category = Convert.ToInt32(Console.ReadLine());
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
                                    c = Enums.Category.Business;
                                    break;
                            }
                            Console.WriteLine("enter inStock:");
                            int inStock = Convert.ToInt32(Console.ReadLine());
                            Product p1 = new()
                            {
                                ID = ID1,
                                Name = name,
                                Price = price,
                                Category = c,
                            };
                            dal.Product.Add(p1);
                            break;
                        case "2":
                            Console.WriteLine("enter ID:");
                            int ID2 = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine(dal.Product.Get(ID2));
                            break;
                        case "3":
                            var item = dal.Product.GetAll();
                            foreach (var item2 in item) Console.WriteLine(item2);
                            break;
                        case "4":
                            Console.WriteLine("enter ID of exist product:");
                            int existID4 = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("enter ID:");
                            int ID4 = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("enter name:");
                            string? name4 = Console.ReadLine();
                            Console.WriteLine("enter price:");
                            double price4 = Convert.ToDouble(Console.ReadLine());
                            Console.WriteLine("enter category:");
                            int category4 = Convert.ToInt32(Console.ReadLine());
                            Enums.Category c4 = new Enums.Category();
                            switch (category4)
                            {
                                case 1:
                                    c4 = Enums.Category.footwear;
                                    break;
                                case 2:
                                    c4 = Enums.Category.outerwear;
                                    break;
                                case 3:
                                    c4 = Enums.Category.Business;
                                    break;
                            }
                            Console.WriteLine("enter inStock:");
                            int inStock4 = Convert.ToInt32(Console.ReadLine());
                            Product p4 = new()
                            {
                                ID = ID4,
                                Name = name4,
                                Price = price4,
                                Category = c4,
                                InStock = inStock4
                            };
                            dal.Product.Update(existID4, p4);
                            break;
                        case "5":
                            Console.WriteLine("enter ID:");
                            int ID5 = Convert.ToInt32(Console.ReadLine());
                            dal.Product.Delete(ID5);
                            Console.WriteLine(ID5);
                            break;
                    }
                }
                else if (choice1 == "2")
                {
                    switch (choice2)
                    {
                        case "0":
                            throw new ProgramExit("Exit Program");
                        case "1":
                            Console.WriteLine("enter ID:");
                            int ID1 = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("enter name:");
                            string? name = Console.ReadLine();
                            Console.WriteLine("enter email:");
                            string? email = Console.ReadLine();
                            Console.WriteLine("enter adress:");
                            string? adress = Console.ReadLine();
                            Console.WriteLine("enter OrderDate:");
                            DateTime OrderDate = Convert.ToDateTime(Console.ReadLine());
                            Console.WriteLine("enter ShipDate:");
                            DateTime ShipDate = Convert.ToDateTime(Console.ReadLine());
                            Console.WriteLine("enter DeliveryDate:");
                            DateTime DeliveryDate = Convert.ToDateTime(Console.ReadLine());
                            Order o1 = new()
                            {
                                ID = ID1,
                                CustomerName = name,
                                CustomerEmail = email,
                                CustomeAdress = adress,
                                OrderDate = OrderDate,
                                ShipDate = ShipDate,
                                DeliveryDate = DeliveryDate
                            };
                           dal.Order.Add(o1);
                            break;
                        case "2":
                            Console.WriteLine("enter ID:");
                            int ID2 = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine(dal.Order.Get(ID2));
                            break;
                        case "3":
                            var item = dal.Order.GetAll();
                            foreach (var item2 in item) Console.WriteLine(item2);
                            break;
                        case "4":
                            Console.WriteLine("enter ID of exist product:");
                            int existID4 = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("enter ID:");
                            int ID4 = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("enter name:");
                            string? name4 = Console.ReadLine();
                            Console.WriteLine("enter email:");
                            string? email4 = Console.ReadLine();
                            Console.WriteLine("enter address:");
                            string? adress4 = Console.ReadLine();
                            Console.WriteLine("enter OrderDate:");
                            DateTime OrderDate4 = Convert.ToDateTime(Console.ReadLine());
                            Console.WriteLine("enter ShipDate:");
                            DateTime ShipDate4 = Convert.ToDateTime(Console.ReadLine());
                            Console.WriteLine("enter DeliveryDate:");
                            DateTime DeliveryDate4 = Convert.ToDateTime(Console.ReadLine());
                            Order o4 = new()
                            {
                                ID = ID4,
                                CustomerName = name4,
                                CustomerEmail = email4,
                                CustomeAdress = adress4,
                                OrderDate = OrderDate4,
                                ShipDate = ShipDate4,
                                DeliveryDate = DeliveryDate4
                            };
                            dal.Order.Update(existID4, o4);
                            break;
                        case "5":
                            Console.WriteLine("enter ID:");
                            int ID5 = Convert.ToInt32(Console.ReadLine());
                            dal.Order.Delete(ID5);
                            Console.WriteLine(ID5);
                            break;
                    }
                }
                else if (choice1 == "3")
                {
                    switch (choice2)
                    {
                        case "0":
                            throw new ProgramExit("Exit Program");
                        case "1":
                            Console.WriteLine("enter OrderItem ID:");
                            int ID0 = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("enter ProductID:");
                            int ID1 = Convert.ToInt32(Console.ReadLine());
                            dal.Product.Get(ID1);
                            Console.WriteLine("enter OrderID:");
                            int ID11 = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("enter price:");
                            Double price = Convert.ToDouble(Console.ReadLine());
                            Console.WriteLine("enter amount:");
                            int amount = Convert.ToInt32(Console.ReadLine());
                            OrderItem oi1 = new()
                            {
                                ID = ID0,
                                ProductID = ID1,
                                OrderID = ID11,
                                Price = price,
                                Amount = amount
                            };
                            dal.OrderItem.Add(oi1);
                            break;
                        case "2":
                            Console.WriteLine("enter ID:");
                            int ID2 = Convert.ToInt32(Console.ReadLine());
                            DO.OrderItem item3 = dal.OrderItem.Get(ID2);
                            Console.WriteLine(item3.ToString());
                            break;
                        case "3":
                            var item = dal.OrderItem.GetAll();
                            foreach (var item2 in item) Console.WriteLine(item2);
                            break;
                        case "4":
                            Console.WriteLine("enter ID of exist OrderItem:");
                            int existID = Convert.ToInt32(Console.ReadLine());
                            dal.OrderItem.Get(existID);                         
                            Console.WriteLine("enter ProductID:");
                            int ID4 = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("enter OrderID:");
                            int ID44 = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("enter price:");
                            Double price4 = Convert.ToDouble(Console.ReadLine());
                            Console.WriteLine("enter amount:");
                            int amount4 = Convert.ToInt32(Console.ReadLine());
                            OrderItem oi4 = new()
                            {
                                ID = existID,
                                ProductID = ID4,
                                OrderID = ID44,
                                Price = price4,
                                Amount = amount4
                            };
                            dal.OrderItem.Update(existID, oi4);
                            break;
                        case "5":
                            Console.WriteLine("enter ID:");
                            int ID5 = Convert.ToInt32(Console.ReadLine());
                            dal.OrderItem.Delete(ID5);
                            Console.WriteLine(ID5);
                            break;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

}
