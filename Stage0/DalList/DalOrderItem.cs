using DO;
using static Dal.DataSource;
namespace Dal;

///A class for connect with ORderItem struck
public class DalOrderItem
{
    /// Add order item item to Data Base
    public void CreateOrderItem(OrderItem orderItem)
    {
        DataSource.AddOrderItem(orderItem);            ///insert to Data Base  
    }

    ///search for order item by Id's and return the spessific order item
    public OrderItem GetOrderItem(int ProductID, int OrderID)
    {
        foreach (OrderItem orderItem in _orderItemArr)
        {

            if (orderItem.ProductID.Equals(ProductID) && orderItem.OrderID.Equals(OrderID))
            {
                return orderItem;
            }
        }
        ///in case of Id not found, throw exeption
        throw new Exception("Not found a orderItem with this Id");
    }

    ///return a new copy of orderItem array
    public OrderItem[] CopyOrderItemArray()
    {
        int tempIndex = 0;
        OrderItem[] newOrderItemArray = new OrderItem[200];
        foreach (OrderItem product in _orderItemArr)
        {
            newOrderItemArray[tempIndex++] = product;
        }
        return newOrderItemArray;
    }

    ///delete order item from data base by Id's
    public void DeleteOrderItem(int ProductID, int OrderID)
    {
        ///run over product array
        for (int i = 0; i <= Config._orderItemArrIndex; i++)
        {
            ///find specific order
            if (_orderItemArr[i].ProductID == ProductID && _orderItemArr[i].OrderID == OrderID)
            {
                ///if finded, run over the arr to delete specific order item
                for (int j = i + 1; j < Config._productArrIndex; j++)
                {
                    _orderItemArr[i] = _orderItemArr[j];
                }
                ///delete last object and resize index
                OrderItem nullOrderItem = new OrderItem();
                _orderItemArr[Config._orderItemArrIndex] = nullOrderItem;
                Config._orderItemArrIndex--;
            }
        }
        ///if not found return a message
        throw new Exception("Not found a order item with this Id's");
    }

    ///replace order item by another inside array
    public void RunOverOrderItem(int ProductID, int OrderID, OrderItem newOrderItem)
    {
        ///run over order item array
        for (int i = 0; i <= Config._orderItemArrIndex; i++)
        {
            ///find specific product
            if (_orderItemArr[i].ProductID == ProductID && _orderItemArr[i].OrderID == OrderID)
            {
                ///if finned, replace the product with a new one
                _orderItemArr[i] = newOrderItem;
            }
        }
        ///if not found return a message
        throw new Exception("Not found a order item with this Id's");
    }
}
