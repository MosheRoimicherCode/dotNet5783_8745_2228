namespace Dal;
using DalApi;
using DO;
using System.Xml.Linq;

internal class DalOrderItem : IOrderItem
{
    static readonly string path = @"..\xml\orderItem.xml";
    static readonly XElement dataBaseOrders = XElement.Load(@"..\xml\orderItem.xml"); //copy data base to code
    public int Add(OrderItem orderItem)
    {
        dataBaseOrders.Add(orderItem); //add new item
        dataBaseOrders.Save(path); //save changes
        return orderItem.ID; //return idOrder
    }

    public void Delete(int ID)
    {
        try
        {
            XElement? newDataBase = (from order2 in dataBaseOrders.Elements()
                                     where Convert.ToInt32(order2.Element("ID")?.Value) == ID
                                     select order2).FirstOrDefault(); //search element with received id
            newDataBase?.Remove();   //remove from copy
            newDataBase?.Save(path); //save changes to original
        }
        catch { }
    }

    public OrderItem? Get(Func<OrderItem?, bool> filter) => (from OrderItem in createListFromXml()
                                                        where filter(OrderItem)
                                                        select OrderItem).FirstOrDefault();

    public IEnumerable<OrderItem?> GetAll(Func<OrderItem?, bool> filter) => from OrderItem in createListFromXml()
                                                                        where filter(OrderItem)
                                                                        select OrderItem;

    public void Update(OrderItem orderItemt)
    {
        Delete(orderItemt.ID);
        Add(orderItemt);
    }

    //private methods
    IEnumerable<OrderItem?> createListFromXml() => (IEnumerable<OrderItem?>)(from OrderItem in dataBaseOrders.Elements()
                                                                     select new DO.OrderItem()
                                                                     {
                                                                         ID = Convert.ToInt32(OrderItem.Element("ID")?.Value),
                                                                         ProductID = Convert.ToInt32(OrderItem.Element("ProductID")?.Value),
                                                                         OrderID = Convert.ToInt32(OrderItem.Element("OrderID")?.Value),
                                                                         Price = Convert.ToDouble(OrderItem.Element("Price")?.Value),
                                                                         Amount = Convert.ToInt32(OrderItem.Element("Amount")?.Value),
                                                                     });
}
