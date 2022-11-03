using DO;
using static Dal.DataSource;

namespace Dal;

public class DalOrder
{
    
    /// Add order to Data Base
    public int CreateOrder(Order order)
    {
        order.ID = Config.Get_idNumberOrder();     ///insert automatic ID (delete input ID)
        DataSource.AddOrder(order);                ///insert to Data Base 
        return order.ID;                           /// return the final Id order.
    }

    ///search for order by Id and return the spefic order
    public Order GetOrder(int OrderID)
    {
        foreach (Order order in _orderArr)
        {

            if (order.ID.Equals(OrderID))
            {
                return order;
            }
        }
        ///in case of Id not found, throw exeption
        throw new Exception("Not found a order with this Id");
    }

    ///return order array
    public Order[] CopyOrderArray()
    {
        int tempIndex = 0;
        Order[] newArray = new Order[100];
        foreach (Order order in _orderArr)
        {
            newArray[tempIndex] = order;
        }
        return newArray;
    }

    ///delete order from data base by Id
    public void DeleteOrder(int OrderID)
    {
        ///run over orders array
        for (int i = 0; i <= Config._orderArrIndex; i++)
        {
            ///find specific order
            if (_orderArr[i].ID == OrderID)
            {
                ///if finned, run over the arr to delete specific order
                for (int j = i+1; j < Config._orderArrIndex; j++)
                {
                    _orderArr[i] = _orderArr[j];
                }
            }
        }
        ///if not found return a message
        throw new Exception("Not found a order with this Id");
    }

    ///replace order by another inside array
    public void RunOverOrder(int OrderID, Order newOrder)
    {
        ///run over orders array
        for (int i = 0; i <= Config._orderArrIndex; i++)
        {
            ///find specific order
            if (_orderArr[i].ID == OrderID)
            {
                ///if finned, replace the order with a new one
                _orderArr[i] = newOrder;
            }
        }
        ///if not found return a message
        throw new Exception("Not found a order with this Id");
    }

}