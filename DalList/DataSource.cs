namespace Dal;

using DO;
using static DO.Enums;

///data source class
internal static class DataSource
{
    internal static List<Product?> _productList = new List<Product?>();
    internal static List<Order> _orderList = new List<Order>();
    internal static List<OrderItem> _orderItemList = new List<OrderItem>();
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
        o.ID = Config._idNumberOrder;
        return o.ID;
    }

    static internal int AddOrderItem(OrderItem p)
    {
        _orderItemList.Add(p);
        p.ID = Config._idNumberItemOrder;
        return p.ID;
    }

    static private void s_Initialize()
    {
        Product p = new()
        {
            ID = 123456,
            Name = "shirt",
            Price = 80,
            Category = Category.Business,
            InStock = 30
        };
        AddProduct(p);
        p = new Product(111111, "shoes", 200, (Category)2, 12);
        AddProduct(p);
        p = new Product(930494, "pens", 120, (Category)2, 29);
        AddProduct(p);
        p = new Product(647248, "hat", 500, (Category)2, 35);
        AddProduct(p);
        p = new Product(897238, "socks", 30, (Category)2, 80);
        AddProduct(p);
        p = new Product(197238, "suit", 1000, (Category)2, 90);
        AddProduct(p);
        p = new Product(372892, "tie", 150, (Category)2, 55);
        AddProduct(p);
        p = new Product(382984, "belt", 50, (Category)2, 45);
        AddProduct(p);
        p = new Product(647238, "t shirt", 35, (Category)2, 85);
        AddProduct(p);
        p = new Product(283944, "crocks", 200, (Category)2, 0);
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
            return generateDate;
        }

        for (int i = 0; i < 10; i++)
        {
            DateTime d = randate();
            Order ord = new Order(Config.GetIdNumberOrder(), "mendi welner", "meniwell@gmail.com", "rambam 5 rishon lezion israel", d, d.AddDays(randomNum.Next(0, 10)), d.AddDays(randomNum.Next(10, 20)));
            AddOrder(ord);
        }
        for (int i = 0; i < 5; i++)
        {
            DateTime d = randate();
            Order ord = new Order(Config.GetIdNumberOrder(), "yosef cohen", "yosefc@gmail.com", "ezra 31 rehovot israel", d, d.AddDays(randomNum.Next(0, 10)), d.AddDays(randomNum.Next(10, 20)));
            AddOrder(ord);
        }
        for (int i = 0; i < 5; i++)
        {
            DateTime d = randate();
            Order ord = new Order(Config.GetIdNumberOrder(), "shimon levi", "shimonl@gmail.com", "770 eastern pky brooklyn NY", d, d.AddDays(randomNum.Next(0, 10)), d.AddDays(randomNum.Next(10, 20)));
            AddOrder(ord);
        }

        DateTime dt1 = new DateTime(2020, 12, 31);
        DateTime dt2 = new DateTime(2029, 12, 31);
        for (int i = 0; i < 2; i++)
        {
            DateTime d = randate();
            Order ord = new Order(Config.GetIdNumberOrder(), "momo momo", "mroimicher@gmail.com", "Israel", d, dt1, dt2);
            AddOrder(ord);
        }

        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 10; i++)
            {
                OrderItem ordI = new OrderItem()
                {
                    ID = Config.GetIdNumberItemOrder(),
                    ProductID = _productList[i]?.ID ?? throw new DalApi.IdException("Internal error"),
                    OrderID = _orderList[i].ID,
                    Price = _productList[i]?.Price ?? 0,
                    Amount = randomNum.Next(0, 50),
                };
                AddOrderItem(ordI);
            }
            for (int i = 0; i < 10; i++)
            {
                OrderItem ordI = new OrderItem(Config.GetIdNumberItemOrder(), _productList[i]?.ID, _orderList[i + 10].ID, _productList[i]?.Price, _randomNum.Next(0, 50));
                AddOrderItem(ordI);
            }
        }
    }


    static internal class Config
    {
        static internal int _idNumberOrder = 1;
        static internal int _idNumberItemOrder = 1;

        /// get ID parameters funcs
        static public int GetIdNumberOrder() => _idNumberOrder++;
        static public int GetIdNumberItemOrder() => _idNumberItemOrder++;
    }

}