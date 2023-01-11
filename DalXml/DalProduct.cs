namespace Dal;
using DalApi;
using DO;
using System.Linq;
using System.Xml.Linq;

internal class DalProduct : IProduct
{
    static readonly string path = @"..\xml\products.xml";

    public int Add(Product product)
    {
        XElement dataBaseOrders = XElement.Load(path); //copy data base to code

        XElement newProduct = new XElement("Products",
                                new XElement("ID", product.ID),
                                new XElement("Name", product.Name),
                                new XElement("Price", product.Price),
                                new XElement("Category", product.Category),
                                new XElement("InStock", product.InStock));

        dataBaseOrders.Add(newProduct); //add new item
        dataBaseOrders.Save(path); //save changes
        return product.ID; //return idOrder
    }

    public void Delete(int ID)
    {
        XElement dataBaseOrders = XElement.Load(path); //copy data base to code
        try
        {
            XElement? removrOrder = (from product in dataBaseOrders.Elements()
                                     where Convert.ToInt32(product.Element("ID")?.Value) == ID
                                     select product).First(); //search element with received id
            removrOrder.Remove();   //remove from copy
            dataBaseOrders.Save(path); //save changes to original
        }
        catch { }
    }

    public Product? Get(Func<Product?, bool> filter) => (from order in createListFromXml()
                                                         where filter(order)
                                                         select order).FirstOrDefault();
    public IEnumerable<Product?> GetAll(Func<Product?, bool>? filter) =>
       filter == null ? createListFromXml().Select(prouductInList => prouductInList)
                 : createListFromXml().Where(filter);

    public void Update(Product product)
    {
        Delete(product.ID);
        Add(product);
    }

    //private methods

    static IEnumerable<Product> createIEnumerableFromXml()
    {
        XElement dataBaseOrders = XElement.Load(path); //copy data base to code
        return (from product in dataBaseOrders.Elements()
                select new DO.Product()
                {
                    ID = Convert.ToInt32(product.Element("ID")?.Value),
                    Name = product.Element("Name")?.Value,
                    Price = Convert.ToDouble(product.Element("Price")?.Value),
                    Category = checkCategoty(product),
                    InStock = Convert.ToInt32(product.Element("InStock")?.Value),

                });
    }
    static List<Product?> createListFromXml()
    {
        List<DO.Product?> b = new();

        foreach (var item in createIEnumerableFromXml().ToList())
        {
            DO.Product? product = item;
            b.Add(product);
        }
        return b;
    }
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

