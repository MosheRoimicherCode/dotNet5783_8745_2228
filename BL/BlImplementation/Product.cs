namespace BlImplementation;

using BO;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

internal class Product : BlApi.IProduct
{
    IDal? Dal = DalApi.Factory.Get();

    private bool CheckNewItem(BO.Product item)
    {
        if (item.ID < 100000) throw new BO.IdBOException("minimum 6 digits for id!");
        if (item.Name == null) throw new BO.ProductNameException("Name can't be null");
        if (item.Price < 0) throw new BO.PriceException("Negative price!");
        if (item.InStock < 0) throw new BO.InStockException(" Product out of stock");

        return true;
    } ///check previews criterion for a new item
    private BO.Product ConvertProductToBoProduct(DO.Product product)
    {
        BO.Product boProduct = new BO.Product();
        boProduct.ID = product.ID;
        boProduct.Name = product.Name;
        boProduct.Price = product.Price;
        boProduct.InStock = product.InStock;
        boProduct.Category = (BO.Category?)product.Category;

        return boProduct;
    }
    private DO.Product ConvertBoProductToProduct(BO.Product boProduct)
    {
        DO.Product product = new DO.Product();
        product.ID = boProduct.ID;
        product.Name = boProduct.Name;
        product.Price = boProduct.Price;
        product.InStock = boProduct.InStock;
        DO.Category? category = (DO.Category?)boProduct.Category;
        product.Category = category;

        return product;
    }
    
    /// create ProductForList from Product based in delegate
    private IEnumerable<ProductForList> CreateproductForLists(Func<BO.Product?, bool>? filter = null) =>

        from product in Dal!.Product.GetAll()
        let BOProduct = ConvertProductToBoProduct((DO.Product)product!)
        where filter is null || filter!.Invoke(BOProduct)
        select new BO.ProductForList()
        {
            ID = BOProduct.ID,
            Name = BOProduct.Name,
            Price = BOProduct.Price,
            Category = BOProduct.Category
        };

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Add(BO.Product item) => Dal!.Product.Add(ConvertBoProductToProduct(item));

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Product Get(int Id) //need to turn to lambda?
    {
        if (Id <= 0) throw new BO.IdBOException("Not positive Id!");
        return ConvertProductToBoProduct((DO.Product)Dal!.Product.Get(x => x?.ID == Id)!);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.ProductItem Get(int? Id, BO.Cart cart)
    {
        if (Id <= 0) throw new BO.IdBOException("Not positive Id!");
        try
        {
            BO.OrderItem? orderItem = cart.Details.FirstOrDefault(x => x?.ProductID == Id);
            DO.Product? product = Dal!.Product.Get(x => x?.ID == Id);
            BO.ProductItem item = new()
            {
                ID = (int)product?.ID!,
                AmontInCart = orderItem?.Amount ?? 0,
                Name = product?.Name,
                Price = (int)product?.Price!,
                IsInStock = false,
                Category = (BO.Category?)(product?.Category)
            };

            if ((Dal!.Product.Get(x => x?.ID == orderItem?.ProductID)!)?.InStock > 0) { item.IsInStock = true; }
            else { item.IsInStock = false; }
            return item;
        }
        catch (IdException) { throw new BO.IdBOException("Product with given Id didn't found"); }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<BO.ProductItem> GetListOfItems(BO.Cart cart)
    {
        return from item in Dal?.Product.GetAll()
               select Get(item?.ID, cart);    
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<BO.ProductItem> GetListOfItemsInCart(BO.Cart cart)
    {
        return from item in Dal?.Product.GetAll()
               from orderItem in cart.Details
               where (item?.ID == orderItem.ProductID)
               select Get(item?.ID, cart);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Remove(int Id)
    {
        ///check if received Id exist
        DO.Product product = Dal!.Product.Get(x => x?.ID == Id) ?? throw new BO.DeleteProductException("Cant delete product. Id not found.");
        //check if product exist inside order - if yes, so throw a message
        if (Dal!.OrderItem.GetAll(x => x?.ID == Id).Count() == 0) Dal.Product.Delete(Id);
        else throw new BO.IdBOException("Product inside an exist order. cant delete."); ;
    }
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(BO.Product item)
    {
        try { if (CheckNewItem(item) == true) Dal!.Product.Update(ConvertBoProductToProduct(item)); }
        catch (IdException) { throw new BO.UpdateProductException("Id not found. Impossible to update."); }
    } /// if received item have right properties and exist, update it. else throw a message.

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<ProductForList> GetList(Func<BO.Product?, bool>? filter = null) => CreateproductForLists(filter).OrderBy(p => p.ID);
}