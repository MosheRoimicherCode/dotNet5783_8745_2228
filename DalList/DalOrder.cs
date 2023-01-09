namespace Dal; 

using DalApi;
using DO;
using System.Linq;
using static Dal.DataSource;

///A class for connect with Order struck
internal class DalOrder : IOrder
{
    public int Add(Order order) => 
        _orderList.Exists(orderInList => orderInList?.ID == order.ID)
            ? throw new IdException("Order ID already exists")
            : AddOrder(order); /// Add Order to Data Base
    
    public Order? Get(Func<Order?, bool> filter) => (from order in _orderList
                                                    where filter(order)
                                                    select order).FirstOrDefault();
    
    public void Delete(int OrderId)
    {
        if (_orderList.RemoveAll(x => x?.ID == OrderId) == 0)
        { throw new IdException(" Not found ID. (Dalorder.Delete Exception)"); }
    }
    
    public void Update(Order newOrder)
    {
        Delete(newOrder.ID);
        Add(newOrder);
    }

    public IEnumerable<Order?> GetAll(Func<Order?, bool>? filter) =>
       filter == null ? _orderList.Select(orderInList => orderInList)
                 : _orderList.Where(filter);
}