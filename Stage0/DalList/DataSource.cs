using DO;
using System.Collections;
using System.Security.Cryptography;

namespace Dal;

internal static class DataSource
{
    /// <summary>
    /// define arrays for classes;
    /// </summary>
    static internal readonly RandomNumberGenerator _randomNum = RandomNumberGenerator.Create();
    static internal Product[] _productArr = new Product[50];
    static internal Order[] _orderArr = new Order[100];
    static internal OrderItem[] _orderItemArr = new OrderItem[200];

    /// <summary>
    /// set arrays index
    /// </summary>
    static internal int _productArrIndex = 0;
    static internal int _orderArrIndex = 0;
    static internal int _orderItemArrIndex = 0;

    /// <summary>
    /// add objects to arrays functions
    /// </summary>
    /// <param name="p"></param>
    static void AddProduct(Product p)
    {
        _productArr[_productArrIndex] = p;
        _productArrIndex++;
    }
    static void AddOrder(Order p)
    {
        _orderArr[_orderArrIndex] = p;
        _orderArrIndex++;
    }
    static void AddOrderItem(OrderItem p)
    {
        _orderItemArr[_orderItemArrIndex] = p;
        _orderItemArrIndex++;
    }

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

        for (int i = 0; i < 40; i++)
        {
            
            OrderItem o = new OrderItem();
            AddOrderItem(o);
        }
        for (int i = 0; i < 40; i++)
        {
            Order ord = new Order();
            AddOrder(ord);
        }

    }
}