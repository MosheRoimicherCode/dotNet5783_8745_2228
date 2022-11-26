using DalApi;
using DO;
using static Dal.DataSource;
namespace Dal;

///A class for connect with ORderItem struck
internal class DalOrderItem : IOrderItem
{

    ///----------------- Constructors ------------------- 


    ///----------------------------------------------------
    ///----------------- CRUD functions -------------------

    public int Add(OrderItem orderItem) => DataSource.AddOrderItem(orderItem);


    public OrderItem Get(int ID)
    {
        foreach (OrderItem orderItem in _orderItemList)
        {

            if (orderItem.ID.Equals(ID))
            {
                return orderItem;
            }
        }
        ///in case of Id not found, throw exception
        throw new IdException(" Not found ID. (DalOrderItem.Get Exception)");
    }///search for order item by Id's and return the specific order item

    public void Delete(int ID)
    {
        foreach (OrderItem orderItem in _orderItemList)
        {
            if (orderItem.ProductID.Equals(ID) && orderItem.OrderID.Equals(ID))
            {
                _orderItemList.Remove(orderItem);
            }
        }

        ///if not found return a message
        throw new IdException(" Not found ID. (DalOrderItem.Delete Exception)");
    }///delete order item from data base by Id's

    public void Update(int ID, OrderItem newOrderItem)
    {

        for (int i = 0; i < _orderItemList.Count; i++)
        {
            var orderItem = _orderItemList[i];
            if (orderItem.ProductID.Equals(ID) && orderItem.OrderID.Equals(ID))
            {
                int index = _orderItemList.IndexOf(orderItem);
                _orderItemList.RemoveAt(index);
                _orderItemList.Insert(index, newOrderItem);
            }
            return;
        }
       
        ///if not found return a message
        throw new IdException(" Not found ID. (DalOrderItem.Update Exception)");
    }///replace order item by another inside array


    ///----------------------------------------------------
    ///----------------------------------------------------



    ///----------------------------------------------------
    ///----------------- Methods --------------------------


    public List<OrderItem> CopyList()
    {
        List<OrderItem> orderItemlist = new List<OrderItem>();
        orderItemlist = _orderItemList;
        return orderItemlist;
    }///return copy of orderItem list

    public void GetAll()
    {
        foreach (OrderItem orderItem in _orderItemList)
        {
            if (orderItem.OrderID != 0)
            {
                Console.WriteLine(orderItem.ToString());
            }
        }
    }///ToString call for all list
    ///----------------------------------------------------
}
