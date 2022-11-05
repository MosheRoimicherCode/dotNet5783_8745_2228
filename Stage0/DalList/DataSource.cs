using DO;
using System.Collections;
using System.Data;
using System.Security.Cryptography;

namespace Dal;

///data source class
internal static class DataSource
{
    /// <summary>
    /// define arrays for classes;
    /// </summary>
    static internal readonly RandomNumberGenerator _randomNum = RandomNumberGenerator.Create();
    static internal Product[] _productArr = new Product[50];
    static internal Order[] _orderArr = new Order[100];
    static internal OrderItem[] _orderItemArr = new OrderItem[200];

    /// add objects to arrays functions
    static internal void AddProduct(Product p)
    {
        _productArr[Config._productArrIndex] = p;
        Config._productArrIndex++;
    }
    static internal void AddOrder(Order p)
    {
        _orderArr[Config._orderArrIndex] = p;
        Config._orderArrIndex++;
    }
    static internal void AddOrderItem(OrderItem p)
    {
        _orderItemArr[Config._orderItemArrIndex] = p;
        Config._orderItemArrIndex++;
    }


    /// <summary>
    /// 
    /// </summary>
    static void s_Initialize()
    {
        
        ///Random _randomNum = new Random();
        ///int num = _randomNum.Next();
        ///

        Product p = new Product(647238, "shirt", 80, 30);
        AddProduct(p);
        p = new Product(839422, "shoes", 200, 12);
        AddProduct(p);
        p = new Product(930494, "pens", 120, 29);
        AddProduct(p);
        p = new Product(647248, "hat", 500, 35);
        AddProduct(p);
        p = new Product(897238, "socks", 30, 80);
        AddProduct(p);
        p = new Product(197238, "suit", 1000, 90);
        AddProduct(p);
        p = new Product(372892, "tie", 150, 55);
        AddProduct(p);
        p = new Product(382984, "belt", 50, 45);
        AddProduct(p);
        p = new Product(647238, "tshirt", 35, 85);
        AddProduct(p);
        p = new Product(283944, "crocks", 200, 0);
        AddProduct(p);


        ///create a random element that create a new random number each call
        Random randomNum = new Random();
        ///
        TimeSpan t1 = TimeSpan.FromDays(randomNum.Next(0, -10));
        TimeSpan t2 = TimeSpan.FromDays(randomNum.Next(-10, -20));
        TimeSpan t3 = TimeSpan.FromDays(r.Next(-20, -30));

        for (int i = 0; i < 20; i++)
        {
            Order ord = new Order(Config.Get_idNumberItemOrder(), "mendi welner", "meniwell@gmail.com", "rambam 5 rishon lezion israel", DateTime.Now.Add(t3), DateTime.Now.Add(t2), DateTime.Now.Add(t1));
            AddOrder(ord);
            ///Config._idNumberOrder++;
        }
        for (int i = 0; i < 10; i++)
        {
            Order ord = new Order(Config.Get_idNumberItemOrder(), "yosef cohen", "yosefc@gmail.com", "ezra 31 rehovot israel", DateTime.Now.Add(t3), DateTime.Now.Add(t2), DateTime.Now.Add(t1));
            AddOrder(ord);
            ///Config._idNumberOrder++;
        }
        for (int i = 0; i < 10; i++)
        {
            Order ord = new Order(Config.Get_idNumberItemOrder(), "shimon levi", "shimonl@gmail.com", "770 eastern pky brooklyn NY", DateTime.Now.Add(t3), DateTime.Now.Add(t2), DateTime.Now.Add(t1));
            AddOrder(ord);
           /// Config._idNumberOrder++;
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