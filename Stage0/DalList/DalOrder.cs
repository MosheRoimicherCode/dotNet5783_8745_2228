using DalApi;
using DO;
using static Dal.DataSource;
namespace Dal;




///A class for connect with Order struck
public class DalOrder
{
    IdException IdError;

    ///----------------- Constructors -------------------
    public DalOrder(Order o) => DataSource.AddOrder(o);


    ///----------------------------------------------------
    ///----------------- CRUD functions -------------------


    public void Add(Order order) => DataSource.AddOrder(order);/// Add order to Data Base

    public Order Get(int OrderID)
    {
        foreach (Order order in _orderList)
        {

            if (order.ID.Equals(OrderID))
            {
                return order;
            }
        }
        ///in case of Id not found, throw exception
        throw IdError;
    }///search for order by Id and return the specific order

    public void Delete(int OrderID)  

    {
        foreach(Order order in _orderList)
        {
            if (order.ID.Equals(OrderID))
            {
                _orderList.Remove(order);
            }
        }

        ///if not found return a message
        throw IdError;
    }///delete order from data base by Id

    public void Update(int OrderID, Order newOrder)
    {
        foreach (Order order in _orderList)
        {
            if (order.ID.Equals(OrderID))
            {
                int index = _orderList.BinarySearch(order);
                _orderList.RemoveAt(index);
                _orderList.Insert(index, newOrder);
            }
        }
        ///if not found return a message
        throw IdError;
    }///replace order by another inside array


    ///----------------------------------------------------
    ///----------------------------------------------------



    ///----------------------------------------------------
    ///----------------- Methods --------------------------

    public List<Order> CopyOrderList()
    {
        List<Order> orderlist = new List<Order>();
        orderlist = _orderList;
        return orderlist;
    }///return copy of order list

    public void GetAll()
    {
        foreach (Order order in _orderList)
        {
            Console.WriteLine(order.ToString());
        }
    }///ToString call for all list


    ///----------------------------------------------------


}