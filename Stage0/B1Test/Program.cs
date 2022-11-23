using BlApi;
using BlImplementation;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Xml.Linq;
using BO;
using System;
using System.Security.Cryptography.X509Certificates;
using DO;
using DalApi;

internal class Program
{
    public static object Success { get; private set; }

    public struct CheckInput
    {
        public string strInput;
        public int intInput;
        public double doubleInput;
        public bool boolInput;
        public BO.Enums.Category c;
    }
    static public CheckInput checkInput(string check)
    {
        Reenter:
        CheckInput result = new CheckInput();
        result.strInput = Console.ReadLine();

        if (check == "int") {
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
        return result;
    }
    static public BoProduct createBoProduct()
    {
        BoProduct boProduct = new BoProduct();
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
        Console.WriteLine("Please enter a category for this product (1 - footwewar /2 - outwear /3 - business).");
        verification1 = checkInput("int");
        if (verification1.boolInput == true) boProduct.Category = verification1.c;

        //Instock input
        Console.WriteLine("Please inform how many items of this product have in stock");
        verification1 = checkInput("int");
        if (verification1.boolInput == true) boProduct.InStock = verification1.intInput;

        return boProduct;

    }



    static public void Main()
    {
        Bl? p = null;

        CheckInput verification = new CheckInput(); //for inputs checks

MainMenu:
        
        Console.WriteLine("enter name of Item:\n  " +
            "p for product\n  " +
            "o for order\n  " +
            "c for cart" +
            "e anytime for end program");
        string? MainMenuChoice = Console.ReadLine();
        switch (MainMenuChoice)
        {
            case "p":
                ProductOperations();
                break;
            case "o":
                OrderOperations();
                break;
            case "c":
                CartOperations();
                break;
            case "e":
                goto end;
            default:
                Console.WriteLine("ERROR choice!");
                goto MainMenu;
        }

    ProductOperations:
        void ProductOperations()
        {
            Console.WriteLine("enter name of operation:\n  " +
                "a for adding a product  \n  " +
                "g1 for getting a product  \n  " +
                "g2 for getting a product-Item  \n  " +
                "r for remove a product  \n  " +
                "u for update data for product \n  " +
                "l for getting a list with all the products");
        }
        string? ProductOperationsChoice = Console.ReadLine();
        switch (ProductOperationsChoice)
        {
            case "a":
                p.BoProduct.Add(createBoProduct());
                break;
        
            case "g1":

                //Check ID input
                Console.WriteLine("Please enter the ID of product.");
                verification = checkInput("int");
                if (verification.boolInput == true) p.BoProduct.Get(verification.intInput);
                break;

            case "g2":

                //Check ID input
                Console.WriteLine("Please enter the ID of product.");
                verification = checkInput("int");

                BoCart cart = new BoCart();  ///empty cart
                p.BoProduct.Get(verification.intInput, cart);
                break;

            case "r":

                //Check ID input
                Console.WriteLine("Please enter the ID of product.");
                verification = checkInput("int");
                p.BoProduct.Remove(verification.intInput);
                break;

            case "u":
                p.BoProduct.Update(createBoProduct());
                break;

            case "l":
                p.BoProduct.GetList();
                break;

            case "e":
                goto end;
            default:
                Console.WriteLine("ERROR choice!");
                goto ProductOperations;
        }

    OrderOperations:
        void OrderOperations()
        {
            Console.WriteLine("enter name of operation:\n  " +
               "g for getting an order  \n  " +
               "l for getting a lists of orders  \n  " +
               "u1 for Update Shipping \n  " +
               "u2 for update Providing \n  " +
               "t for Order Tracking");
        }
        string? OrderOperationsChoice = Console.ReadLine();
        switch (ProductOperationsChoice)
        {
            case "g":
                OrderGet();
                break;
            case "l":
                OrderGetList();
                break;
            case "u1":
                OrderUpdateShipping();
                break;
            case "u2":
                OrderUpdateProviding();
                break;
            case "t":
                OrderTracking();
                break;
            case "e":
                goto end;
            default:
                Console.WriteLine("ERROR choice!");
                goto OrderOperations;
        }

    CartOperations:
        void CartOperations()
        {
            Console.WriteLine("enter name of operation:\n  " +
              "a for adding a product to cart  \n  " +
              "u for update amount in cart  \n  " +
              "c for Confirm cart");
        }
        string? CartOperationsChoice = Console.ReadLine();
        switch (ProductOperationsChoice)
        {
            case "a":
                CartAdding();
                break;
            case "u":
                CartUpdate();
                break;
            case "c":
                CartConfirm();
                break;
            case "e":
                goto end;
            default:
                Console.WriteLine("ERROR choice!");
                goto CartOperations;
        }


    end:
    Console.WriteLine("end");
    }
}