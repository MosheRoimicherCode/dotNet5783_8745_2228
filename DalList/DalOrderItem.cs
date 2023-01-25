namespace Dal; 

using DalApi;
using DO;
using System.Runtime.CompilerServices;
using static Dal.DataSource;

///A class for connect with ORderItem struck
internal class DalOrderItem : IOrderItem
{
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(OrderItem orderItem) =>
        DataSource._orderItemList.Exists(orderItemInList => orderItemInList?.ID == orderItem.ID)
            ? throw new IdException("OrderItem ID already exists")
            : DataSource.AddOrderItem(orderItem); /// Add OrderItem to Data Base  
    [MethodImpl(MethodImplOptions.Synchronized)]
    public OrderItem? Get(Func<OrderItem?, bool> filter) => (from orderItem in _orderItemList
                                                             orderby orderItem?.ID
                                                             where filter(orderItem)
                                                             let filterValue = filter(orderItem)
                                                             select orderItem).FirstOrDefault();
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int OrderItemId)
    {
        try { _orderItemList.RemoveAll(x => x?.ID == OrderItemId); }
        catch (ArgumentNullException) { throw new IdException(" Not found ID. (Dalorder.Delete Exception)"); }
    }
    [MethodImpl(MethodImplOptions.Synchronized)]

    public void DeleteProduct(int productId)
    {
        _orderItemList.RemoveAll(x => x?.ProductID == productId);
    }
    [MethodImpl(MethodImplOptions.Synchronized)]

    public void Update(OrderItem newOrderItem)
    {
        Delete(newOrderItem.ID);
        Add(newOrderItem);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<OrderItem?> GetAll(Func<OrderItem?, bool>? filter) =>
      filter == null ? _orderItemList.Select(orderItemInList => orderItemInList)
                : _orderItemList.Where(filter);
}
