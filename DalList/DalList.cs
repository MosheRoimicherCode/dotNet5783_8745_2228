namespace Dal;
using DalApi;
using DO;

internal sealed class DalList : IDal
{

    public static IDal? Instance
    {
        get { return s_instance; }
    }//singleton door (Name: Instance)


    private static DalList s_instance = new DalList();
    private DalList()
    {
        Product product = new Product();
        Order order = new Order();
        OrderItem orderItem = new OrderItem();
    }

    // Implementation specific data members
    IProduct IDal.Product { get; }
    IOrder IDal.Order {get;}
    IOrderItem IDal.OrderItem { get; }
}