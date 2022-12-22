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

    public OrderItem Get(int OrderItemID)
    {
        foreach (var orderItem in from OrderItem orderItem in _orderItemList
                              where orderItem.ID.Equals(OrderItemID)
                              select orderItem)
        {
            return orderItem;
        }
        throw new IdException("Not found ID. (DalorderItem.Get Exception)");
    }///search for orderItem by Id and return the specific order

    public OrderItem? Get(Func<OrderItem?, bool> filter) => (from orderItem in _orderItemList
                                                             where filter(orderItem)
                                                             select orderItem).FirstOrDefault();

    public void Delete(int OrderItemId)
    {
        try { _orderItemList.RemoveAll(x => x?.ID == OrderItemId); }
        catch (ArgumentNullException) { throw new IdException(" Not found ID. (Dalorder.Delete Exception)"); }

        //bool flag = false;
        //for (int i = 0; i < _orderItemList.Count; i++)
        //{
        //    if (_orderItemList[i]?.ID == OrderItemId)
        //    {
        //        _orderItemList.Remove(_orderItemList[i]);
        //        flag = true;
        //    }
        //}
        ////if Id not found send a MESSAGE
        //if (flag == false) Console.WriteLine(" Not found ID. (DalorderItem.Delete Exception)");
        /////delete OrsderItem from data base by Id
    }

    public void Update(int OrderItemID, OrderItem newOrderItem)
    {
        try
        {
            int index = _orderItemList.FindIndex(x => x?.ID == OrderItemID);

            _orderItemList.RemoveAt(index);
            _orderItemList.Insert(index, newOrderItem);
        }
        catch { throw new IdException("not found id. (DalOrder.Update Exception)"); }

        //for (int i = 0; i < _orderItemList.Count; i++)
        //{
           


        //    OrderItem? orderItem = new();
        //    orderItem = _orderItemList[i];
        //    if (orderItem?.ID.Equals(OrderItemID) ?? throw new IdException("null object. (DalOrderItem.Update Exception)"))
        //    {
        //        int index = _orderItemList.IndexOf(orderItem);
        //        _orderItemList.RemoveAt(index);
        //        _orderItemList.Insert(index, newOrderItem);
        //        return;
        //    }
        //}
        //throw new IdException(" Not found ID. (DalOrderItem.Update Exception)");
    }///replace orderItem by another inside array

    public IEnumerable<OrderItem?> GetAll(Func<OrderItem?, bool>? filter) =>
      filter == null ? _orderItemList.Select(orderItemInList => orderItemInList)
                : _orderItemList.Where(filter);
}
