using BlApi;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Xml.Linq;

internal class Program
{
    static public void Main()
    {
        IBl bl;
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
                ProductAdd();
                break;
            case "g1":
                ProductGet1();
                break;
            case "g2":
                ProductGet2();
                break;
            case "r":
                ProductRemove();
                break;
            case "u":
                ProductUpdate();
                break;
            case "l":
                ProductGetList();
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