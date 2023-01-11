namespace Dal;
using DalApi;
using DO;
using System.Linq;
using System.Xml.Linq;

internal class DalOrderItem : IOrderItem
{
    static readonly string path = @"..\xml\orderItem.xml";
    static readonly string pathConfig = @"..\xml\config.xml";

    public int Add(OrderItem orderItem)
    {
        XElement dataBaseOrders = XElement.Load(path); //copy data base to code
        orderItem.ID = ReturnId(); //get automatic ID 

        XElement newOrderItem = new XElement("OrderItem",
                                new XElement("ID", orderItem.ID),
                                new XElement("ProductID", orderItem.ProductID),
                                new XElement("OrderID", orderItem.OrderID),
                                new XElement("Price", orderItem.Price),
                                new XElement("Amount", orderItem.Amount));

        dataBaseOrders.Add(newOrderItem); //add new item
        dataBaseOrders.Save(path); //save changes
        return orderItem.ID; //return idOrder
    }

    public void Delete(int ID)
    {
        XElement dataBaseOrders = XElement.Load(path); //copy data base to code

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

    public IEnumerable<OrderItem?> GetAll(Func<OrderItem?, bool>? filter) =>
        filter == null ? createListFromXml().Select(prouductInList => prouductInList)
                  : createListFromXml().Where(filter);

    public void Update(OrderItem orderItem)
    {
        Delete(orderItem.ID);
        Add(orderItem);
    }

    //private methods
    static IEnumerable<OrderItem?> createIEnumerableFromXml()
    {
        XElement dataBaseOrders = XElement.Load(path); //copy data base to code
        return (IEnumerable<OrderItem?>)(from OrderItem in dataBaseOrders.Elements()
                                  select new DO.OrderItem()
                                  {
                                      ID = Convert.ToInt32(OrderItem.Element("ID")?.Value),
                                      ProductID = Convert.ToInt32(OrderItem.Element("ProductID")?.Value),
                                      OrderID = Convert.ToInt32(OrderItem.Element("OrderID")?.Value),
                                      Price = Convert.ToDouble(OrderItem.Element("Price")?.Value),
                                      Amount = Convert.ToInt32(OrderItem.Element("Amount")?.Value),
                                  });
    }
    static List<OrderItem?> createListFromXml()
    {
        List<DO.OrderItem?> b = new();
        foreach (var item in createIEnumerableFromXml().ToList())
        {
            DO.OrderItem? orderItem = item;
            b.Add(orderItem);
        }
        return b;
    }
    static int ReturnId()
    {
        XElement configData = XElement.Load(pathConfig); //copy data base to code
        int _idNumberOrder = Convert.ToInt32(configData.Element("_idNumberItemOrder")?.Value) + 1; //new id
        configData.SetElementValue("_idNumberItemOrder",_idNumberOrder);
        configData.Save(pathConfig);
        return _idNumberOrder;
    }
}
