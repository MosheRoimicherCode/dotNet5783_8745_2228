﻿using BlApi;
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
        //result.strInput = Console.ReadLine();

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
        Bl? p = new Bl();
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

        BoCart cart = new BoCart();
        cart.CustomeAdress = "BneiBrak";
        cart.CustomerEmail = "mroimicher@gmail.com";
        cart.CustomerName = "Moshe";
        cart.Details = list;
        cart.TotalPrice = 2800;
    ////for checks use---------------------------------e---------------------------------e---------------------------------

    MainMenu:
        try
        {
            Console.WriteLine("enter name of Item:\n  " +
                "p for product\n  " +
                "o for order\n  " +
                "c for cart" +
                "e anytime for end program");
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
               "a  - for adding a product  \n  " +
               "g1 - for getting a product  \n  " +
               "g2 - for getting a product-Item  \n  " +
               "r  - for remove a product  \n  " +
               "u  - for update data for product \n  " +
               "l  - for getting a list with all the products \n"
            );
            string? ProductOperationsChoice = Console.ReadLine();


            switch (ProductOperationsChoice)
            {
                case "a":
                    Console.WriteLine("-----------------------------------------------ADDpRODUCT--------------------------------------------------------");
                    p.BoProduct.Add(createBoProduct());
                    goto MainMenu;

                case "g1":
                    Console.WriteLine("-----------------------------------------------G1--------------------------------------------------------");

                    //Check ID input
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    BO.BoProduct boProduct = new BoProduct();
                    if (verification.boolInput == true) boProduct = p.BoProduct.Get(verification.intInput);
                    Console.WriteLine(boProduct);
                    goto MainMenu;

                case "g2":
                    Console.WriteLine("-----------------------------------------------g2--------------------------------------------------------");

                    //Check ID input
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    BO.ProductItem boProduct1 = new ProductItem();
                    boProduct1 = p.BoProduct.Get(verification.intInput, cart);
                    Console.WriteLine(boProduct1);
                    goto MainMenu;

                case "r":
                    Console.WriteLine("-----------------------------------------------REMOVE--------------------------------------------------------");

                    //Check ID input
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    int Id = verification.intInput;
                    p.BoProduct.Remove(Id);
                    goto MainMenu;

                case "u":
                    Console.WriteLine("-----------------------------------------------UPDATE--------------------------------------------------------");

                    p.BoProduct.Update(createBoProduct());
                    goto MainMenu;

                case "l":
                    Console.WriteLine("-----------------------------------------------LIST--------------------------------------------------------");

                    List<BO.BoProductForList> listBoProduct = p.BoProduct.GetList();
                    foreach (BO.BoProductForList item in listBoProduct)
                    {
                        Console.WriteLine(item);
                    }
                    goto MainMenu;

                case "e":
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
                "g for getting an order  \n  " +
                "l for getting a lists of orders  \n  " +
                "u1 for Update Shipping \n  " +
                "u2 for update Providing \n  " +
                "t for Order Tracking \n"
            );
            string? OrderOperationsChoice = Console.ReadLine();
            switch (OrderOperationsChoice)
            {
                case "g":
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    BO.Order boOrder = new Order();
                    if (verification.boolInput == true) boOrder = p.BoOrder.Get(verification.intInput);
                    Console.WriteLine(boOrder);
                    goto MainMenu;

                case "l":
                    List<BO.BoOrderForList> boOrderForList = p.BoOrder.GetList();
                    foreach (BO.BoOrderForList item in boOrderForList)
                    {
                        Console.WriteLine(item);
                    }
                    goto MainMenu;

                case "u1":
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    BO.Order b = p.BoOrder.UpdateShipping(verification.intInput);
                    Console.WriteLine(b);
                    goto MainMenu;

                case "u2":
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    BO.Order b1 = p.BoOrder.UpdateProviding(verification.intInput);
                    Console.WriteLine(b1);
                    goto MainMenu;

                case "t":
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    BO.BoOrderTracking boOrderTracking = new BoOrderTracking();
                    if (verification.boolInput == true) boOrderTracking = p.BoOrder.OrderTracking(verification.intInput);
                    Console.WriteLine(boOrderTracking);
                    goto MainMenu;

                case "e":
                    goto end;
                default:
                    Console.WriteLine("ERROR choice!");
                    goto OrderOperations;
            }

        CartOperations:
            Console.WriteLine
            (
                 "enter name of operation:\n  " +
                 "a for adding a product to cart  \n  " +
                 "u for update amount in cart  \n  " +
                 "c for Confirm cart \n "
            );
            string? CartOperationsChoice = Console.ReadLine();
            switch (CartOperationsChoice)
            {
                case "a":
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    BO.BoCart boCart = p.BoCart.Add(cart, verification.intInput);
                    Console.WriteLine(boCart.ToString());
                    goto MainMenu;

                case "u":
                    Console.WriteLine("Please enter the ID of product.");
                    verification = checkInput("int");
                    int temp = verification.intInput;
                    Console.WriteLine("Please enter a new amount.");
                    verification = checkInput("int");
                    BO.BoCart boCart1 = p.BoCart.UpdateAmount(cart, temp, verification.intInput);
                    Console.WriteLine(boCart1.ToString());
                    goto MainMenu;

                case "c":

                    //Console.WriteLine("Please enter the name of client.");
                    //string? nameClient = Console.ReadLine();
                    //Console.WriteLine("Please enter the address of client.");
                    //string? nameAdress = Console.ReadLine();
                    //verification = checkInput("email");

                    p.BoCart.ConfirmCart(cart, cart.CustomerName, cart.CustomerEmail, cart.CustomeAdress);
                    goto MainMenu;

                case "e":
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