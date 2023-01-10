namespace Dal;
using DalApi;

internal sealed class DalXml : IDal
{
    private static IDal instance = new DalXml();
    private DalXml()
    {
        DalProduct dalProduct = new();
        DalOrder dalOrder = new();
        DalOrderItem dalOrderItem = new();
    }
    // The public Instance property to use
    public static IDal Instance => instance ?? new DalXml();

    // Implementation specific data members
    IProduct IDal.Product => new DalProduct();
    IOrder IDal.Order => new DalOrder();
    IOrderItem IDal.OrderItem => new DalOrderItem();
}


