using BlApi;
using BO;
using System.ComponentModel.DataAnnotations;

internal class Program
{

    //checks user input methods---------------------------------------------------------------------------------------------
    public struct CheckInput
    {
        public string? strInput;
        public int intInput;
        public double doubleInput;
        public bool? boolInput;
        public Enums.Category? c;
    }  //struck for support input checks 
    static public CheckInput checkInput(string check)
    {
    Reenter:
        CheckInput result = new CheckInput();
        
        if (check == "int")
        {
            result.boolInput = Int32.TryParse(Console.ReadLine(), out result.intInput);
            if (result.boolInput == false)
            {
                Console.WriteLine("Can't convert input to int. Enter an integer format...");
                goto Reenter;
            }
        }

        if (check == "double")
        {
            result.boolInput = Double.TryParse(Console.ReadLine(), out result.doubleInput);
            if (result.boolInput == false)
            {
                Console.WriteLine("Can't convert input to double. Enter an double format...");
                goto Reenter;
            }
        }

        if (check == "category")
        {
            result.boolInput = Int32.TryParse(Console.ReadLine(), out result.intInput);
            if (result.boolInput == true)
            {

                switch (result.intInput)
                {
                    case 1:
                        result.c = BO.Enums.Category.footwear;
                        break;
                    case 2:
                        result.c = BO.Enums.Category.outerwear;
                        break;
                    case 3:
                        result.c = BO.Enums.Category.business;
                        break;
                }
            }
            else
            {
                Console.WriteLine("Can't convert input to Category type. Enter an int (1-3) format...");
                goto Reenter;
            }
        }

        if (check == "email")
        {
            result.strInput = Console.ReadLine();
            result.boolInput = new EmailAddressAttribute().IsValid(result.strInput);
            if (result.boolInput == false)
            {
                Console.WriteLine("invalid Email! Please choose a valid Email");
                goto Reenter;
            }
        }

        return result;
    }//a function to check user inputs
    static public Product createBoProduct()
    {
        Product boProduct = new();
        CheckInput verification1 = new CheckInput(); //for inputs checks

        //ID input
        Console.WriteLine("Please enter the ID of product.");
        verification1 = checkInput("int");
        if (verification1.boolInput == true) boProduct.ID = verification1.intInput;

        //Name input
        Console.WriteLine("Please enter the name of product.");
        boProduct.Name = Console.ReadLine();

        //Price input
        Console.WriteLine("Please enter the price of product.");
        verification1 = checkInput("double");
        if (verification1.boolInput == true) boProduct.Price = verification1.doubleInput;

        //Category input
        Console.WriteLine("Please enter a category for this product (1 - footwear /2 - outwear /3 - business).");
        verification1 = checkInput("int");
        if (verification1.boolInput == true) boProduct.Category = verification1.c;

        //In stock input
        Console.WriteLine("Please inform how many items of this product have in stock");
        verification1 = checkInput("int");
        if (verification1.boolInput == true) boProduct.InStock = verification1.intInput;

        return boProduct;

    } /// a create function of BoProduct item for checks

    //-----------------------------------------------------------------------------------------------checks user input methods

    static public void Main()
    {
        IBl? p = BlApi.Factory.Get();

        CheckInput verification = new CheckInput(); //for inputs checks


        ////for checks use---------------------------------e---------------------------------e---------------------------------
        DO.OrderItem order = new DO.OrderItem();
        order.ID = 789;
        order.OrderID = 1;
        order.Price = 80;
        order.ProductID = 123456;
        order.Amount = 5;

        DO.OrderItem order2 = new DO.OrderItem();
        order2.ID = 987;
        order2.OrderID = 2;
        order2.Price = 200;
        order2.ProductID = 111111;
        order2.Amount = 5;


        List<DO.OrderItem?> list = new List<DO.OrderItem?>();
        list.Add(order);
        list.Add(order2);

        Cart cart = new();
        cart.CustomeAdress = "BneiBrak";
        cart.CustomerEmail = "mroimicher@gmail.com";
        cart.CustomerName = "Moshe";
        //cart.Details = list;
        cart.TotalPrice = 2800;
    ////for checks use---------------------------------e---------------------------------e---------------------------------

    MainMenu:
        try
        {
            Console.WriteLine("enter name of Item:\n  " +
                "p - product\n  " +
                "o - order\n  " +
                "c - cart" +
                "0 - anytime for end program");
            string? MainMenuChoice = Console.ReadLine();

            switch (MainMenuChoice)
            {
                case "p":
                    goto ProductOperations;
                case "o":
                    goto OrderOperations;
                case "c":
                    goto CartOperations;
                case "e":
                    goto end;
                default:
                    Console.WriteLine("ERROR choice!");
                    goto MainMenu;
            }

        ProductOperations:
            Console.WriteLine
            (
               "chose operation:\n  " +
               "1 - adding a product  \n  " +
               "2 - getting a product  \n  " +
               "3 - getting a product-Item  \n  " +
               "4 - remove a product  \n  " +
               "5 - update data for product \n  " +
               "6 - getting a list with all the products \n"
            );
            string? ProductOperationsChoice = Console.ReadLine();


            switch (ProductOperationsChoice)
            {
                case "1":
                    Console.WriteLine("-----------------------------------------------ADDpRODUCT--------------------------------------------------------");
                    p.Product.Add(createBoProduct());
                    goto MainMenu;

                case "2":
                    Console.WriteLine("-----------------------------------------------G1--------------------------------------------------------");

                    //Check ID input
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    BO.Product boProduct = new();
                    if (verification.boolInput == true) boProduct = p.Product.Get(verification.intInput);
                    Console.WriteLine(boProduct);
                    goto MainMenu;

                case "3":
                    Console.WriteLine("-----------------------------------------------g2--------------------------------------------------------");

                    //Check ID input
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    BO.ProductItem boProduct1 = new ProductItem();
                    boProduct1 = p.Product.Get(verification.intInput, cart);
                    Console.WriteLine(boProduct1);
                    goto MainMenu;

                case "4":
                    Console.WriteLine("-----------------------------------------------REMOVE--------------------------------------------------------");

                    //Check ID input
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    int Id = verification.intInput;
                    p.Product.Remove(Id);
                    goto MainMenu;

                case "5":
                    Console.WriteLine("-----------------------------------------------UPDATE--------------------------------------------------------");

                    p.Product.Update(createBoProduct());
                    goto MainMenu;

                case "6":
                    Console.WriteLine("-----------------------------------------------LIST--------------------------------------------------------");

                    List<BO.ProductForList?> listBoProduct = p.Product.GetList();
                    foreach (BO.ProductForList item in listBoProduct)
                    {
                        Console.WriteLine(item);
                    }
                    goto MainMenu;

                case "0":
                    Console.WriteLine("-----------------------------------------------END--------------------------------------------------------");

                    goto end;
                default:
                    Console.WriteLine("ERROR choice!");
                    goto MainMenu;
            }

        OrderOperations:
            Console.WriteLine
            (
                "enter name of operation:\n  " +
                "1 - getting an order  \n  " +
                "2 - getting a lists of orders  \n  " +
                "3 - Update Shipping \n  " +
                "4 - update Providing \n  " +
                "5 - Order Tracking \n"
            );
            string? OrderOperationsChoice = Console.ReadLine();
            switch (OrderOperationsChoice)
            {
                case "1":
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    BO.Order boOrder = new Order();
                    if (verification.boolInput == true) boOrder = p.Order.Get(verification.intInput);
                    Console.WriteLine(boOrder);
                    goto MainMenu;

                case "2":
                    List<BO.OrderForList> boOrderForList = p.Order.GetList();
                    foreach (BO.OrderForList item in boOrderForList)
                    {
                        Console.WriteLine(item);
                    }
                    goto MainMenu;

                case "3":
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    BO.Order b = p.Order.UpdateShipping(verification.intInput);
                    Console.WriteLine(b);
                    goto MainMenu;

                case "4":
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    BO.Order b1 = p.Order.UpdateProviding(verification.intInput);
                    Console.WriteLine(b1);
                    goto MainMenu;

                case "5":
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    BO.OrderTracking boOrderTracking = new OrderTracking();
                    if (verification.boolInput == true) boOrderTracking = p.Order.OrderTracking(verification.intInput);
                    Console.WriteLine(boOrderTracking);
                    goto MainMenu;

                case "0":
                    goto end;
                default:
                    Console.WriteLine("ERROR choice!");
                    goto OrderOperations;
            }

        CartOperations:
            Console.WriteLine
            (
                 "enter name of operation:\n  " +
                 "1 - adding a product to cart  \n  " +
                 "2 - update amount in cart  \n  " +
                 "3 - Confirm cart \n "
            );
            string? CartOperationsChoice = Console.ReadLine();
            switch (CartOperationsChoice)
            {
                case "1":
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    BO.Cart boCart = p.Cart.Add(cart, verification.intInput);
                    Console.WriteLine(boCart.ToString());
                    goto MainMenu;

                case "2":
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    int temp = verification.intInput;
                    Console.WriteLine("Please enter a new amount.");
                    verification = checkInput("int");
                    BO.Cart boCart1 = p.Cart.UpdateAmount(cart, temp, verification.intInput);
                    Console.WriteLine(boCart1.ToString());
                    goto MainMenu;

                case "3":                
                    p.Cart.ConfirmCart(cart, cart.CustomerName, cart.CustomerEmail, cart.CustomeAdress);
                    goto MainMenu;

                case "0":
                    goto end;

                default:
                    Console.WriteLine("ERROR choice!");
                    goto CartOperations;
            }

        end:
            Console.WriteLine("end");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}