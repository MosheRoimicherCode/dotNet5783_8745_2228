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

    /// <summary>
    /// delete order
    /// </summary>
    /// <param name="OrderId"></param>
    /// <exception cref="IdException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int OrderId)
    {
        if (_orderList.RemoveAll(x => x?.ID == OrderId) == 0)
        { throw new IdException(" Not found ID. (Dalorder.Delete Exception)"); }
    }

    /// <summary>
    /// update parameters of order
    /// </summary>
    /// <param name="newOrder"></param>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Order newOrder)
    {
        UpdateOrderInPlace(newOrder);
    }

    /// <summary>
    /// return IEnumerable<Order?> of Orders based on delegate
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Order?> GetAll(Func<Order?, bool>? filter) =>
       filter == null ? _orderList.Select(orderInList => orderInList)
                 : _orderList.Where(filter);
}