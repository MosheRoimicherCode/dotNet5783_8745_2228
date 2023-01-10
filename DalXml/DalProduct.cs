namespace Dal;
using DalApi;
using DO;
using System.Xml.Linq;

internal class DalProduct : IProduct
{
    static readonly string path = @"..\xml\orders.xml";
    static readonly XElement dataBaseOrders = XElement.Load(@"..\xml\orders.xml"); //copy data base to code

    public int Add(Product product)
    {
        dataBaseOrders.Add(product); //add new item
        dataBaseOrders.Save(path); //save changes
        return product.ID; //return idOrder
    }

    public void Delete(int ID)
    {
        try
        {
            XElement? newDataBase = (from product in dataBaseOrders.Elements()
                                     where Convert.ToInt32(product.Element("ID")?.Value) == ID
                                     select product).FirstOrDefault(); //search element with received id
            newDataBase?.Remove();   //remove from copy
            newDataBase?.Save(path); //save changes to original
        }
        catch { }
    }

    public Product? Get(Func<Product?, bool> filter) => (from order in createListFromXml()
                                                         where filter(order)
                                                         select order).FirstOrDefault();

    public IEnumerable<Product?> GetAll(Func<Product?, bool>? filter) => from order in createListFromXml()
                                                                         where filter(order)
                                                                         select order;

    public void Update(Product product)
    {
        Delete(product.ID);
        Add(product);
    }

    IEnumerable<Product?> createListFromXml() => (IEnumerable<Product?>)(from product in dataBaseOrders.Elements()
                                                                     select new DO.Product()
                                                                     {
                                                                         ID = Convert.ToInt32(product.Element("ID")?.Value),
                                                                         Name = product.Element("Name")?.Value,
                                                                         Price = Convert.ToDouble(product.Element("Price")?.Value),
                                                                         Category = checkCategoty(product),
                                                                         InStock = Convert.ToInt32(product.Element("InStock")?.Value),
                                                                         
                                                                     });
    static Category checkCategoty(XElement cat)
    {
        switch (cat.Element("Category")?.Value.ToString())
        {
            case "footwear":
                return Category.footwear;
            case "Business":
                return Category.Business;
            case "outerwear":
                return Category.outerwear;

            default: return Category.All;
        }
    }


}

