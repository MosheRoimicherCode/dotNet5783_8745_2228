namespace Dal;
using DalApi;
using DO;
using System.Xml.Linq;

internal class DalOrder : IOrder
{
    string path = @"..\xml\orders.xml";
    public int Add(Order t)
    {
        XElement newOrder = XElement.Load(path);
        return 0;    
           
    }

    public void Delete(int n)
    {
        throw new NotImplementedException();
    }

    public Order? Get(Func<Order?, bool> f)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Order?> GetAll(Func<Order?, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Order t)
    {
        throw new NotImplementedException();
    }
}

