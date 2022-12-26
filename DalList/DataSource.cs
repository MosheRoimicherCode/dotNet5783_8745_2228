﻿namespace Dal;

using DO;
using System;
using static DO.Enums;

///data source class
internal static class DataSource
{
    internal static List<Product?> _productList = new List<Product?>();
    internal static List<Order?> _orderList = new List<Order?>();
    internal static List<OrderItem?> _orderItemList = new List<OrderItem?>();
    static DataSource() => s_Initialize();

    /// define arrays for classes;
    static internal readonly Random _randomNum = new Random();

    /// add objects to arrays functions
    static internal int AddProduct(Product p)
    {
        _productList.Add(p);
        return p.ID;
    }

    static internal int AddOrder(Order o)
    {
        _orderList.Add(o);
        return Config._idNumberOrder;
    }

    static internal int AddOrderItem(OrderItem p)
    {
        _orderItemList.Add(p);
        return Config._idNumberItemOrder;
    }

    static private void s_Initialize()
    {
        Product p = new()
        {
            ID = 123456,
            Name = "shirt",
            Price = 80,
            Category = Category.outerwear,
            InStock = 30
        };
        AddProduct(p);
        p = new()
        {
            ID = 111111,
            Name = "shoes",
            Price = 200,
            Category = Category.footwear,
            InStock = 12
        };
        AddProduct(p);
        p = new()
        {
            ID = 930494,
            Name = "pens",
            Price = 120,
            Category = Category.outerwear,
            InStock = 29
        };
        AddProduct(p);
        p = new()
        {
            ID = 647248,
            Name = "hat",
            Price = 500,
            Category = Category.Business,
            InStock = 35
        };
        AddProduct(p);
        p = new()
        {
            ID = 897238,
            Name = "socks",
            Price = 30,
            Category = Category.footwear,
            InStock = 80
        };
        AddProduct(p);
        p = new()
        {
            ID = 197238,
            Name = "suit",
            Price = 1000,
            Category = Category.Business,
            InStock = 90
        };
        AddProduct(p);
        p = new()
        {
            ID = 372892,
            Name = "tie",
            Price = 150,
            Category = Category.Business,
            InStock = 55
        };
        AddProduct(p);
        p = new()
        {
            ID = 382984,
            Name = "belt",
            Price = 50,
            Category = Category.Business,
            InStock = 45
        };
        AddProduct(p);
        p = new()
        {
            ID = 647238,
            Name = "t shirt",
            Price = 35,
            Category = Category.outerwear,
            InStock = 85
        };
        AddProduct(p);
        p = new()
        {
            ID = 283944,
            Name = "crocks",
            Price = 200,
            Category = Category.Business,
            InStock = 0
        };
        AddProduct(p);

        //create a random element that create a new random number each call
        TimeSpan t1 = TimeSpan.FromDays(_randomNum.Next(20, 30));
        TimeSpan t2 = TimeSpan.FromDays(_randomNum.Next(10, 20));
        TimeSpan t3 = TimeSpan.FromDays(_randomNum.Next(0, 10));

        static DateTime randate()
        {
            DateTime datetoday = DateTime.Now;

            int rndYear = _randomNum.Next(1995, datetoday.Year);
            int rndMonth = _randomNum.Next(1, 12);
            int rndDay = _randomNum.Next(1, 31);

            DateTime generateDate = new DateTime(rndYear, rndMonth, rndDay);
            return generateDate  ;
        }
        

        ///creation of orders
        Order order = new();
        for (int i = 0; i < 10; i++)
        {
            DateTime dateTimeRandom = randate();
            order = new()
            {
                ID = Config.GetIdNumberOrder(),
                CustomerName = "mendi welner",
                CustomerEmail = "meniwell@gmail.com",
                CustomeAdress = "rambam 5 rishon lezion israel",
                OrderDate = dateTimeRandom,
                ShipDate = dateTimeRandom.AddDays(_randomNum.Next(0, 10)),
                DeliveryDate = dateTimeRandom.AddDays(_randomNum.Next(10, 20))
            };
            AddOrder(order);
        }
        for (int i = 0; i < 5; i++)
        {
            DateTime dateTimeRandom = randate();
            order = new()
            {
                ID = Config.GetIdNumberOrder(),
                CustomerName = "yosef cohen",
                CustomerEmail = "yosefc@gmail.com",
                CustomeAdress = "ezra 31 rehovot israel",
                OrderDate = dateTimeRandom,
                ShipDate = dateTimeRandom.AddDays(_randomNum.Next(0, 10)),
                DeliveryDate = dateTimeRandom.AddDays(_randomNum.Next(10, 20))
            };
            AddOrder(order);
        }
        for (int i = 0; i < 5; i++)
        {
            DateTime dateTimeRandom = randate();
            order = new()
            {
                ID = Config.GetIdNumberOrder(),
                CustomerName = "shimon levi",
                CustomerEmail = "shimonl@gmail.com",
                CustomeAdress = "770 eastern pky brooklyn NY",
                OrderDate = dateTimeRandom,
                ShipDate = dateTimeRandom.AddDays(_randomNum.Next(0, 10)),
                DeliveryDate = dateTimeRandom.AddDays(_randomNum.Next(10, 20))
            };
            AddOrder(order);
        }

        DateTime dateTime1 = new DateTime(2020, 12, 31);
        DateTime dateTime2 = new DateTime(2029, 12, 31);

        for (int i = 0; i < 2; i++)
        {
            DateTime dateTimeRandom = randate();
            order = new()
            {
                ID = Config.GetIdNumberOrder(),
                CustomerName = "momo momo",
                CustomerEmail = "mroimicher@gmail.com",
                CustomeAdress = "Israel",
                OrderDate = dateTimeRandom,
                ShipDate = dateTime1,
                DeliveryDate = dateTime2
            };
            AddOrder(order);
        }
        ///end of order creation

        OrderItem ordItem = new OrderItem();
        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 10; i++)
            {
                ordItem = new()
                {
                    ID = Config.GetIdNumberItemOrder(),
                    ProductID = _productList[i]?.ID ?? throw new DO.IdException("Internal error.DataSource.CreateOrderItem"),
                    OrderID = _orderList[i]?.ID ?? throw new DO.IdException("Internal error.DataSource.CreateOrderItem"),
                    Price = _productList[i]?.Price ?? 0,
                    Amount = _randomNum.Next(0, 50),
                };
                AddOrderItem(ordItem);
            }
            for (int i = 0; i < 10; i++)
            {
                ordItem = new()
                {
                    ID = Config.GetIdNumberItemOrder(),
                    ProductID = _productList[i]?.ID ?? throw new DO.IdException("Internal error.DataSource.CreateOrderItem"),
                    OrderID = _orderList[i + 10]?.ID ?? throw new DO.IdException("Internal error.DataSource.CreateOrderItem"),
                    Price = _productList[i]?.Price ?? 0,
                    Amount = _randomNum.Next(0, 50),
                };
                AddOrderItem(ordItem);
            }
        }

        p = new()
        {
            ID = 222222,
            Name = "glasses",
            Price = 200,
            Category = Category.Business,
            InStock = 5
        };
        AddProduct(p);
    }

    static internal class Config
    {
        static internal int _idNumberOrder = 1;
        static internal int _idNumberItemOrder = 1;

        /// get ID parameters functions
        static public int GetIdNumberOrder() => _idNumberOrder++;
        static public int GetIdNumberItemOrder() => _idNumberItemOrder++;
    }

}