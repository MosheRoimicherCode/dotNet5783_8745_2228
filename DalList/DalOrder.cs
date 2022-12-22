namespace Dal; 

using DalApi;
using DO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using static Dal.DataSource;

///A class for connect with Order struck
internal class DalOrder : IOrder
{
    public int Add(Order order) => 
        DataSource._orderList.Exists(orderInList => orderInList?.ID == order.ID)
            ? throw new IdException("Order ID already exists")
            : DataSource.AddOrder(order); /// Add Order to Data Base

    //public Order Get(int OrderID)
    //{
    //    var order = from Order order1 in _orderList
    //                where order1.ID.Equals(OrderID)
    //                select order1;
    //    var temp = order.FirstOrDefault();
    //    if (temp.ID == 0) throw new IdException("Not found ID. (Dalorder.Get Exception)");
    //    return temp;
    //}///search for order by Id and return the specific order -- not in use

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

        try { int index = _orderItemList.FindIndex(x => x?.ID == OrderID);
            
            _orderList.RemoveAt(index);
            _orderList.Insert(index, newOrder);
        }
        catch { throw new IdException("not found id. (DalOrder.Update Exception)"); }

    }///replace order by another inside array

    public IEnumerable<Order?> GetAll(Func<Order?, bool>? filter) =>
       filter == null ? _orderList.Select(orderInList => orderInList)
                 : _orderList.Where(filter);
}