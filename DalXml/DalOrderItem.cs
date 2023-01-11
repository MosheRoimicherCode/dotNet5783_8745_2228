namespace Dal;
using DalApi;
using DO;
using System.Linq;
using System.Xml.Linq;

internal class DalOrderItem : IOrderItem
{
    static readonly string path = @"..\xml\orderItem.xml";
    static readonly string pathConfig = @"..\xml\config.xml";
    static string addFunctionality = "add";
    public int Add(OrderItem orderItem)
    {
        XElement dataBase = XElement.Load(path); //copy data base to code
        if (addFunctionality != "update" ) orderItem.ID = ReturnId(); //get new automatic ID 

        XElement newOrderItem = new XElement ("OrderItem",
                                new XElement("ID", orderItem.ID),
                                new XElement("ProductID", orderItem.ProductID),
                                new XElement("OrderID", orderItem.OrderID),
                                new XElement("Price", orderItem.Price),
                                new XElement("Amount", orderItem.Amount));

        dataBase.Add(newOrderItem); //add new item
        dataBase.Save(path); //save changes
        return orderItem.ID; //return idOrder
    }

    public void Delete(int ID)
    {
        XElement dataBase = XElement.Load(path); //copy data base to code

        try
        {
            XElement? removeItem = (from order2 in dataBase.Elements()
                                     where Convert.ToInt32(order2.Element("ID")?.Value) == ID
                                     select order2).First(); //search element with received id
            removeItem.Remove();   //remove from copy
            dataBase.Save(path); //save changes to original
        }
        catch { }
    }

    public OrderItem? Get(Func<OrderItem?, bool> filter) => (from item in createListFromXml()
                                                        where filter(item)
                                                        select item).FirstOrDefault();

    public IEnumerable<OrderItem?> GetAll(Func<OrderItem?, bool>? filter) =>
        filter == null ? createListFromXml().Select(prouductInList => prouductInList)
                  : createListFromXml().Where(filter);

    public void Update(OrderItem orderItem)
    {
        addFunctionality = "update";
        Delete(orderItem.ID);
        Add(orderItem);
        addFunctionality = "add";
    }

    //private methods
    static IEnumerable<OrderItem> createIEnumerableFromXml()
    {
        XElement dataBaseOrders = XElement.Load(path); //copy data base to code

        return (from OrderItem in dataBaseOrders.Elements()
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
        foreach (var item in createIEnumerableFromXml())
        {
            DO.OrderItem? orderItem = item;
            b.Add(orderItem);
        }
        return b;
    }
    static int ReturnId()
    {
        XElement configData = XElement.Load(pathConfig); //copy data base to code
        int _idNumberItemOrder = Convert.ToInt32(configData.Element("_idNumberItemOrder")?.Value) + 1; //new id
        configData.SetElementValue("_idNumberItemOrder", _idNumberItemOrder);
        configData.Save(pathConfig);
        return _idNumberItemOrder;
    }
}
