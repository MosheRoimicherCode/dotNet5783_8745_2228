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
        try { _orderList.RemoveAll(x => x?.ID == OrderId); }
        catch (ArgumentNullException) { throw new IdException(" Not found ID. (Dalorder.Delete Exception)"); }
    }
    public void Update(int OrderID, Order newOrder)
    {
        try
        {
            int index = _orderList.Select((order, index) => (order, index)).First(x => x.order.Value.ID == OrderID).index;

            _orderList.RemoveAt(index);
            _orderList.Insert(index, newOrder);
        }
        catch { throw new IdException("not found id. (DalOrder.Update Exception)"); }
    }///replace order by another inside array
    public IEnumerable<Order?> GetAll(Func<Order?, bool>? filter) =>
       filter == null ? _orderList.Select(orderInList => orderInList)
                 : _orderList.Where(filter);
}