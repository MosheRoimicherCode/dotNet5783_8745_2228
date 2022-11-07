using DO;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;
using static DO.Enums;

namespace Dal;

///data source class
internal static class DataSource
{
    static DataSource()
    {
        s_Initialize();
    }
    
    /// define arrays for classes;
    static internal readonly RandomNumberGenerator _randomNum = RandomNumberGenerator.Create();
    static internal Product[] _productArr;
    static internal Order[] _orderArr;
    static internal OrderItem[] _orderItemArr;

    /// add objects to arrays functions
    static internal void AddProduct(Product p)
    {
        //Product[] _productArr = new Product[50];

        int index = Config._productArrIndex;
        _productArr[index] = p;
        Config._productArrIndex++;
    }
    static internal void AddOrder(Order o) 
    { 

        Order[] _orderArr = new Order[100];

        int index2 = Config._orderArrIndex;
        _orderArr[index2] = o;
        Config._orderArrIndex++;
    }
    static internal void AddOrderItem(OrderItem p)
    {
        OrderItem[] _orderItemArr = new OrderItem[200];
        _orderItemArr[Config._orderItemArrIndex].OrderID = p.OrderID;
        _orderItemArr[Config._orderItemArrIndex].ProductID = p.ProductID;
        _orderItemArr[Config._orderItemArrIndex].Amount = p.Amount;
        _orderItemArr[Config._orderItemArrIndex].Price = p.Price;
        Config._orderItemArrIndex++;
    }

    static private void s_Initialize()
    {

        //Product p = new Product(647238, "shirt", 80, (Category)2, 30);
        //AddProduct(p);
        //p = new Product(839422, "shoes", 200, (Category)2, 12);
        //AddProduct(p);
        //p = new Product(930494, "pens", 120, (Category)2, 29);
        //AddProduct(p);
        //p = new Product(647248, "hat", 500, (Category)2, 35);
        //AddProduct(p);
        //p = new Product(897238, "socks", 30, (Category)2, 80);
        //AddProduct(p);
        //p = new Product(197238, "suit", 1000, (Category)2, 90);
        //AddProduct(p);
        //p = new Product(372892, "tie", 150, (Category)2, 55);
        //AddProduct(p);
        //p = new Product(382984, "belt", 50, (Category)2, 45);
        //AddProduct(p);
        //p = new Product(647238, "tshirt", 35, (Category)2, 85);
        //AddProduct(p);
        //p = new Product(283944, "crocks", 200, (Category)2, 0);
        //AddProduct(p);


        ///create a random element that create a new random number each call
        //Random randomNum = new Random();
        /////
        //TimeSpan t1 = TimeSpan.FromDays(randomNum.Next(0, -10));
        //TimeSpan t2 = TimeSpan.FromDays(randomNum.Next(-10, -20));
        //TimeSpan t3 = TimeSpan.FromDays(randomNum.Next(-20, -30));

        int a = 0;
        Order order = new Order(Config.Get_idNumberItemOrder(),"sss","ddd","aaa", DateTime.Now,DateTime.Now,DateTime.Now);
        AddOrder(order);
        //for (int i = 0; i < 10; i++)
        //{
        //    Order ord = new Order(Config.Get_idNumberItemOrder(), "mendi welner", "meniwell@gmail.com", "rambam 5 rishon lezion israel", DateTime.Now.Add(t3), DateTime.Now.Add(t2), DateTime.Now.Add(t1));
        //    AddOrder(ord);
        //}
        //for (int i = 0; i < 5; i++)
        //{
        //    Order ord = new Order(Config.Get_idNumberItemOrder(), "yosef cohen", "yosefc@gmail.com", "ezra 31 rehovot israel", DateTime.Now.Add(t3), DateTime.Now.Add(t2), DateTime.Now.Add(t1));
        //    AddOrder(ord);
        //}
        //for (int i = 0; i < 5; i++)
        //{
        //    Order ord = new Order(Config.Get_idNumberItemOrder(), "shimon levi", "shimonl@gmail.com", "770 eastern pky brooklyn NY", DateTime.Now.Add(t3), DateTime.Now.Add(t2), DateTime.Now.Add(t1));
        //    AddOrder(ord);
        //}

        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 10; i++)
            {
                OrderItem ordI = new OrderItem(_productArr[i].ID, _orderArr[i].ID, _productArr[i].Price, randomNum.Next(0, 50));
                AddOrderItem(ordI);
            }
            for (int i = 0; i < 10; i++)
            {
                OrderItem ordI = new OrderItem(_productArr[i].ID, _orderArr[i + 10].ID, _productArr[i].Price, randomNum.Next(0, 50));
                AddOrderItem(ordI);
            }
        }
    }

    static internal class Config
    {
        /// set arrays index
        static internal int _productArrIndex = 0;
        static internal int _orderArrIndex = 0;
        static internal int _orderItemArrIndex = 0;
        static internal int _idNumberOrder = 1;
        static internal int _idNumberItemOrder = 1;


        /// get ID parameters funcs
        static public int Get_idNumberOrder()
        {
            int temp = _idNumberOrder;
            _idNumberOrder++;
            return temp;
        }
        static public int Get_idNumberItemOrder()
        {
            int temp = _idNumberItemOrder;
            _idNumberItemOrder++;
            return temp;
        }

    }

}