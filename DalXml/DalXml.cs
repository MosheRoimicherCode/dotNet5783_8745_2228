namespace Dal;
using DalApi;


internal class DalXml : IDal
{
    private static IDal instance = new DalXml();
    private DalXml()
    {
        DalProduct dalProduct = new();
        DalOrder dalOrder = new();
        DalOrderItem dalOrderItem = new();
    }
    public static IDal Instance => instance ?? new DalXml();

    IProduct IDal.Product => new DalProduct();
    IOrder IDal.Order => new DalOrder();
    IOrderItem IDal.OrderItem => new DalOrderItem();
}


