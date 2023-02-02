namespace Dal;
using DalApi;
using DO;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

internal class DalOrder : IOrder
{
    static readonly string path = @"..\xml\orders.xml";
    static readonly string pathConfig = @"..\xml\config.xml";
    static string addFunctionality = "add";

    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(Order order)
    {
        XElement dataBase = XElement.Load(path); //copy data base to code
        if (addFunctionality != "update")  order.ID = ReturnId(); //get new automatic ID 
        
        XElement newOrder = new XElement("Orders",
                            new XElement("ID", order.ID),
                            new XElement("CustomerName", order.CustomerName),
                            new XElement("CustomerEmail", order.CustomerEmail),
                            new XElement("CustomeAdress", order.CustomeAdress),
                            new XElement("OrderDate", order.OrderDate),
                            new XElement("ShipDate", order.ShipDate),
                            new XElement("DeliveryDate", order.DeliveryDate));

        dataBase.Add(newOrder); //add new item
        dataBase.Save(path); //save changes
        return order.ID; //return idOrder
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int ID)
    {
        XElement dataBase = XElement.Load(path); //copy data base to code

        try
        {
            var removeItem = (from order2 in dataBase.Elements()
                               where Convert.ToInt32(order2.Element("ID")?.Value) == ID
                               select order2).FirstOrDefault(); //search element with received id
            removeItem?.Remove();   //remove from copy
            dataBase?.Save(path); //save changes to original
        }
        catch { }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Order? Get(Func<Order?, bool> filter) => (from item in createListFromXml()
                                                     where filter(item)
                                                     select item).FirstOrDefault();

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Order?> GetAll(Func<Order?, bool>? filter) =>
        filter == null ? createListFromXml().Select(prouductInList => prouductInList)
                  : createListFromXml().Where(filter);

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Order order)
    {
        addFunctionality = "update";
        Delete(order.ID);
        Add(order);
        addFunctionality = "add";
    }

    //private methods
    static IEnumerable<Order> createIEnumerableFromXml()
    {
        XElement dataBase = XElement.Load(path); //copy data base to code

        List<DO.Order> orderList = new();
        foreach ( var item in dataBase.Elements())
        {
            XmlReader reader = XmlReader.Create(path);

            DO.Order order = new()
            {
                ID = Convert.ToInt32(item.Element("ID")?.Value),
                CustomerName = item.Element("CustomerName")?.Value,
                CustomeAdress = item.Element("CustomeAdress")?.Value,
                CustomerEmail = item.Element("CustomerEmail")?.Value,
                OrderDate = DateTime.ParseExact(item.Element("OrderDate")?.Value.Substring(0, 10), "yyyy-MM-dd", null)
            };

            try {order.ShipDate = DateTime.ParseExact(item.Element("ShipDate")?.Value.Substring(0, 10), "yyyy-MM-dd", null);}
            catch { order.ShipDate = null; }
            try { order.DeliveryDate = DateTime.ParseExact(item.Element("DeliveryDate")?.Value.Substring(0, 10), "yyyy-MM-dd", null); }
            catch { order.DeliveryDate = null; }

            orderList.Add(order);
        }
        var a = from item in orderList select item;
        return a;
    }

    static List<Order?> createListFromXml()
    {
        List<DO.Order?> b = new();

        foreach (var item in createIEnumerableFromXml().ToList())
        {
            DO.Order? order = item;
            b.Add(order);
        }
        return b;
    }
    int ReturnId()
    {
        XElement configData = XElement.Load(pathConfig); //copy data base to code
        int _idNumberOrder = Convert.ToInt32(configData.Element("_idNumberOrder")?.Value) + 1; //new id
        configData.SetElementValue("_idNumberOrder", _idNumberOrder);
        configData.Save(pathConfig);
        return _idNumberOrder;
    }
}

