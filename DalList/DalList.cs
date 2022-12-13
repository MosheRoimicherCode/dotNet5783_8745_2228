namespace Dal;
using DalApi;
internal sealed class DalList : IDal
{
    private static readonly IDal instance = new DalList();

    // Implementation specific data members
    IProduct IDal.Product => new DalProduct();
    IOrder IDal.Order => new DalOrder();
    IOrderItem IDal.OrderItem => new DalOrderItem();

    //My singleton
    private DalList() { }

    // The public Instance property to use
    public static IDal Instance => instance;
}