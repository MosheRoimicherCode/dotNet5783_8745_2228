namespace Dal; 

using DalApi;
using DO;
using System.Linq;
using System.Runtime.CompilerServices;
using static Dal.DataSource;

///A class for connect with Order struck
internal class DalOrder : IOrder
{
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(Order order) => 
        _orderList.Exists(orderInList => orderInList?.ID == order.ID)
            ? throw new IdException("Order ID already exists")
            : AddOrder(order); /// Add Order to Data Base

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Order? Get(Func<Order?, bool> filter) => (from order in _orderList
                                                    where filter(order)
                                                    select order).FirstOrDefault();

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int OrderId)
    {
        if (_orderList.RemoveAll(x => x?.ID == OrderId) == 0)
        { throw new IdException(" Not found ID. (Dalorder.Delete Exception)"); }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Order newOrder)
    {
        Delete(newOrder.ID);
        Add(newOrder);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Order?> GetAll(Func<Order?, bool>? filter) =>
       filter == null ? _orderList.Select(orderInList => orderInList)
                 : _orderList.Where(filter);
}