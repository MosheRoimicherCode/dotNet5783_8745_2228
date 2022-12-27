namespace BlImplementation;

using BO;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;

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
        boProduct.Category = (BO.Enums.Category?)product.Category;

        return boProduct;
    }
    private DO.Product ConvertBoProductToProduct(BO.Product boProduct)
    {
        DO.Product product = new DO.Product();
        product.ID = boProduct.ID;
        product.Name = boProduct.Name;
        product.Price = boProduct.Price;
        product.InStock = boProduct.InStock;
        DO.Enums.Category? category = (DO.Enums.Category?)boProduct.Category;
        product.Category = category;

        return product;
    }
    public void Add(BO.Product item) => Dal!.Product.Add(ConvertBoProductToProduct(item));
    public BO.Product Get(int Id) //need to turn to lambda?
    {
        if (Id <= 0) throw new BO.IdBOException("Not positive Id!");
        return ConvertProductToBoProduct((DO.Product)Dal!.Product.Get(x => x?.ID == Id)!);
    }
    public BO.ProductItem Get(int Id, BO.Cart cart)
    {

        if (Id <= 0) throw new BO.IdBOException("Not positive Id!");
        try
        {
            BO.ProductItem? item = new();
            BO.OrderItem? orderItem = new();

            foreach (BO.OrderItem? itemCart in cart.Details) if (itemCart?.ProductID == Id) orderItem = itemCart ?? throw new BO.IdBOException("Product with given Id didn't found");

            item.ID = orderItem.ProductID;
            item.AmontInCart = orderItem.Amount;

            if ((Dal!.Product.Get(x => x?.ID == orderItem.ProductID)!).Value.InStock > 0) item.IsInStock = true;
            else { item.IsInStock = false; }

            item.Name = cart.CustomerName;
            item.Price = orderItem.ProductPrice;

            item.Category = (BO.Enums.Category?)Dal.Product.Get(x => x?.ID == orderItem.ProductID)!.Value.Category;
            return item;
        }
        catch (IdException) { throw new BO.IdBOException("Product with given Id didn't found"); }
    }
    public void Remove(int Id)
    {
        ///check if received Id exist
        DO.Product product = Dal!.Product.Get(x => x?.ID == Id) ?? throw new BO.DeleteProductException("Cant delete product. Id not found.");
        //check if product exist inside order - if yes, so throw a message
        if (Dal!.OrderItem.GetAll(x => x!.Value.ID == Id).Count() == 0) Dal.Product.Delete(Id);
        else throw new BO.IdBOException("Product inside an exist order. cant delete."); ;
    }
    public void Update(BO.Product item)
    {
        try { if (CheckNewItem(item) == true) Dal!.Product.Update(item.ID, ConvertBoProductToProduct(item)); }
        catch (IdException) { throw new BO.UpdateProductException("Id not found. Impossible to update."); }
    } /// if received item have right properties and exist, update it. else throw a message.

    /// <summary>
    /// create ProductForList from Product based in delegate
    /// </summary>
    /// <param name="filter" - delegate ></param>
    /// <returns> ProductForList item </returns>
    private List<ProductForList?> CreateproductForLists(Func<DO.Product?, bool>? filter = null)
    {
        IEnumerable<DO.Product?>? list = null;
        if (filter != null) { list = Dal!.Product.GetAll(filter); }
        else { list = Dal!.Product.GetAll(); }

        List<BO.ProductForList?> L_listBoProduct = new();
        foreach (DO.Product? product in list)  //create list of product for list
        {
            BO.Product BOProduct = ConvertProductToBoProduct((DO.Product)product!);
            BO.ProductForList boProductForList = new()
            {
                ID = BOProduct.ID,
                Name = BOProduct.Name,
                Price = BOProduct.Price,
                Category = BOProduct?.Category
            };
            L_listBoProduct.Add(boProductForList);
        }
        return L_listBoProduct;
    }
    public List<ProductForList?> GetList(Func<DO.Product?, bool>? filter = null) =>
        filter == null ? CreateproductForLists() : CreateproductForLists(filter);
}