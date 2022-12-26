namespace Dal; 

using DalApi;
using DO;
using static Dal.DataSource;

///A class for connect with Order struck
internal class DalOrder : IOrder
{
    public int Add(Order order) => 
        DataSource._orderList.Exists(orderInList => orderInList?.ID == order.ID)
            ? throw new IdException("Order ID already exists")
            : DataSource.AddOrder(order); /// Add Order to Data Base


    public Order Get(int OrderID)
    {
        foreach (var order in from Order order in _orderList
                               where order.ID.Equals(OrderID)
                               select order)
        {
            return order;
        }
        throw new IdException("Not found ID. (Dalorder.Get Exception)");
    }///search for order by Id and return the specific order

    public Order? Get(Func<Order?, bool> filter) =>
                                                    (from order in _orderList
                                                     where filter(order)
                                                     select order).FirstOrDefault();

    public void Delete(int OrderId)
    {
        if (_orderList.RemoveAll(order => order?.ID == OrderId) == 0)
            throw new IdException("Can't delete non-existing order");
    }

    public void Update(int OrderID, Order newOrder)
    {
        for (int i = 0; i < _orderList.Count; i++)
        {
            Order? order = new();
            order = _orderList[i];
            if (order?.ID.Equals(OrderID) ?? throw new IdException(" null object. (DalOrder.Update Exception)"))
            {
                int index = _orderList.IndexOf(order);
                _orderList.RemoveAt(index);
                _orderList.Insert(index, newOrder);
                return;
            }
        }
        throw new IdException("not found id. (DalOrder.Update Exception)");
    }///replace order by another inside array

    public IEnumerable<Order?> GetAll(Func<Order?, bool>? filter) =>
       filter == null ? _orderList.Select(orderInList => orderInList)
                 : _orderList.Where(filter);
}