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
using DalApi;
using System.ComponentModel.DataAnnotations;


internal class Program
{
    public static object Success { get; private set; }


    //checks user input methods---------------------------------------------------------------------------------------------
    public struct CheckInput
    {
        public string strInput;
        public int intInput;
        public double doubleInput;
        public bool boolInput;
        public BO.Enums.Category c;
    }  //element for support input checks 
    static public CheckInput checkInput(string check)
    {
        Reenter:
        CheckInput result = new CheckInput();
        result.strInput = Console.ReadLine();

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
                Console.WriteLine("invalid Email! Please chouse a valid Email");
                goto Reenter;
            }
        }

        return result;
    }//a function to check user inputs
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

    } /// a create function of BoProduct item for checks
    //-----------------------------------------------------------------------------------------------checks user input methods


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
            Console.WriteLine
               (
                   "chose operation:\n  " +
                   "a  - for adding a product  \n  " +
                   "g1 - for getting a product  \n  " +
                   "g2 - for getting a product-Item  \n  " +
                   "r  - for remove a product  \n  " +
                   "u  - for update data for product \n  " +
                   "l  - for getting a list with all the products \n"
               );
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
            Console.WriteLine
                (
                    "enter name of operation:\n  " +
                    "g for getting an order  \n  " +
                    "l for getting a lists of orders  \n  " +
                    "u1 for Update Shipping \n  " +
                    "u2 for update Providing \n  " +
                    "t for Order Tracking \n"
                );
        }
        string? OrderOperationsChoice = Console.ReadLine();
        switch (OrderOperationsChoice)
            {
                case "g":
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    p.BoOrder.Get(verification.intInput);
                    break;

                case "l":
                    p.BoOrder.GetLists();
                    break;

                case "u1":
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    p.BoOrder.UpdateShipping(verification.intInput);
                    break;

                case "u2":
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    p.BoOrder.UpdateProviding(verification.intInput);
                    break;

                case "t":
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    p.BoOrder.OrderTracking(verification.intInput);
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
            Console.WriteLine
                 (
                     "enter name of operation:\n  " +
                     "a for adding a product to cart  \n  " +
                     "u for update amount in cart  \n  " +
                     "c for Confirm cart \n "
                  );
        }
        string? CartOperationsChoice = Console.ReadLine();
        switch (CartOperationsChoice)
            {
                case "a":
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    BoCart cart = new BoCart();
                    p.BoCart.Add(cart, verification.intInput);
                    break;

                case "u":
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    BoCart cart2 = new BoCart();
                    int temp = verification.intInput;
                    Console.WriteLine("Please enter a new amount.");
                    verification = checkInput("int");
                    p.BoCart.UpdateAmount(cart2, temp, verification.intInput);
                    break;

                case "c":
                    BoCart cart3 = new BoCart();

                    Console.WriteLine("Please enter the name of client.");
                    string? nameClient = Console.ReadLine();
                    Console.WriteLine("Please enter the address of client.");
                    string? nameAdress = Console.ReadLine();
                    verification = checkInput("email");

                    p.BoCart.ConfirmCart(cart3, nameClient, verification.strInput, nameAdress);
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