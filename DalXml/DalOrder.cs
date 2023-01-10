namespace Dal;
using DalApi;
using DO;
using System.Xml.Linq;

internal class DalOrder : IOrder
{
    static readonly string path = @"..\xml\orders.xml";
    static readonly string pathConfig = @"..\xml\config.xml";


    public int Add(Order order)
    {
        XElement dataBaseOrders = XElement.Load(path); //copy data base to code
        order.ID = ReturnId(); //get automatic ID 
        dataBaseOrders.Add(order); //add new item
        dataBaseOrders.Save(path); //save changes
        return order.ID; //return idOrder
    }
    
    public void Delete(int ID)
    {
        XElement dataBaseOrders = XElement.Load(path); //copy data base to code

        try
        {
            var newDataBase = (from order2 in dataBaseOrders.Elements()
                               where Convert.ToInt32(order2.Element("ID")?.Value) == ID
                               select order2).FirstOrDefault(); //search element with received id
            newDataBase?.Remove();   //remove from copy
            newDataBase?.Save(path); //save changes to original
        }
        catch { }
    }

    public Order? Get(Func<Order?, bool> filter) => (from order in createListFromXml()
                                                     where filter(order)
                                                     select order).FirstOrDefault();

    public IEnumerable<Order?> GetAll(Func<Order?, bool> filter) => from order in createListFromXml()
                                                                    where filter(order)
                                                                    select order;

    public void Update(Order order)
    {
        Delete(order.ID);
        Add(order);
    }


    //private methods
    IEnumerable<Order?> createListFromXml()
    {
        XElement dataBaseOrders = XElement.Load(path); //copy data base to code

        return (IEnumerable<Order?>)(from order in dataBaseOrders.Elements()
                              select new DO.Order()
                              {
                                  ID = Convert.ToInt32(order.Element("ID")?.Value),
                                  CustomerName = order.Element("CustomerName")?.Value,
                                  CustomeAdress = order.Element("CustomeAdress")?.Value,
                                  CustomerEmail = order.Element("CustomerEmail")?.Value,
                                  DeliveryDate = Convert.ToDateTime(order.Element("OrderDate")?.Value),
                                  ShipDate = Convert.ToDateTime(order.Element("ShipDate")?.Value),
                                  OrderDate = Convert.ToDateTime(order.Element("OrderDate")?.Value)
                              });
    }

    int ReturnId()
    {
        XElement configData = XElement.Load(pathConfig); //copy data base to code
        int _idNumberOrder = Convert.ToInt32(configData.Element("_idNumberOrder")?.Value) + 1; //new id
        configData.SetAttributeValue("_idNumberOrder", _idNumberOrder);
        return _idNumberOrder;
    }
}

