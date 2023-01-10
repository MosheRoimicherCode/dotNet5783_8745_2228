namespace Dal;
using DalApi;

internal sealed class DalList : IDal
{
    private static IDal instance = new DalList();
    private DalList()
    {
        DalProduct dalProduct = new();
        DalOrder dalOrder = new ();
        DalOrderItem dalOrderItem = new ();
    }
    // The public Instance property to use
    public static IDal Instance => instance ?? new DalList() ;

    // Implementation specific data members
    IProduct IDal.Product => new DalProduct();
    IOrder IDal.Order => new DalOrder();
    IOrderItem IDal.OrderItem => new DalOrderItem();
}