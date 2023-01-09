namespace Dal; 

using DalApi;
using DO;
using static Dal.DataSource;

///A class for connect with ORderItem struck
internal class DalOrderItem : IOrderItem
{
    public int Add(OrderItem orderItem) =>
        DataSource._orderItemList.Exists(orderItemInList => orderItemInList?.ID == orderItem.ID)
            ? throw new IdException("OrderItem ID already exists")
            : DataSource.AddOrderItem(orderItem); /// Add OrderItem to Data Base  
    public OrderItem? Get(Func<OrderItem?, bool> filter) => (from orderItem in _orderItemList
                                                             orderby orderItem?.ID
                                                             where filter(orderItem)
                                                             let filterValue = filter(orderItem)
                                                             select orderItem).FirstOrDefault();
    public void Delete(int OrderItemId)
    {
        try { _orderItemList.RemoveAll(x => x?.ID == OrderItemId); }
        catch (ArgumentNullException) { throw new IdException(" Not found ID. (Dalorder.Delete Exception)"); }
    }
    public void Update(OrderItem newOrderItem)
    {
        Delete(newOrderItem.ID);
        Add(newOrderItem);
    }
    
    public IEnumerable<OrderItem?> GetAll(Func<OrderItem?, bool>? filter) =>
      filter == null ? _orderItemList.Select(orderItemInList => orderItemInList)
                : _orderItemList.Where(filter);
}
