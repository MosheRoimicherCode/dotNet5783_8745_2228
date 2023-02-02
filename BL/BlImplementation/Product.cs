namespace BlImplementation;

using BlApi;
using BO;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

internal class Product : BlApi.IProduct
{
    readonly IDal? Dal = DalApi.Factory.Get();

    /// <summary>
    /// check previews criterion for a new item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="BO.IdBOException"></exception>
    /// <exception cref="BO.ProductNameException"></exception>
    /// <exception cref="BO.PriceException"></exception>
    /// <exception cref="BO.InStockException"></exception>
    private bool CheckNewItem(BO.Product item)
    {
        // Check if the product ID is less than 100000 and throw an exception if it is
        if (item.ID < 100000) throw new BO.IdBOException("minimum 6 digits for id!");
        // Check if the product Name is null and throw an exception if it is
        if (item.Name == null) throw new BO.ProductNameException("Name can't be null");
        // Check if the product Price is less than 0 and throw an exception if it is
        if (item.Price < 0) throw new BO.PriceException("Negative price!");
        // Check if the product InStock is less than 0 and throw an exception if it is
        if (item.InStock < 0) throw new BO.InStockException(" Product out of stock");

        return true;
    }

    /// <summary>
    /// his method is named "ConvertProductToBoProduct" and it takes in a DO.Product object as a parameter. It creates a new BO.Product object and sets its properties
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    private BO.Product ConvertProductToBoProduct(DO.Product product)
    {
        // Create a new BO.Product object
        BO.Product boProduct = new BO.Product();
        // Set the BO.Product's ID to the DO.Product's ID
        boProduct.ID = product.ID;
        // Set the BO.Product's Name to the DO.Product's Name
        boProduct.Name = product.Name;
        // Set the BO.Product's Price to the DO.Product's Price
        boProduct.Price = product.Price;
        // Set the BO.Product's InStock to the DO.Product's InStock
        boProduct.InStock = product.InStock;
        // Set the BO.Product's Category to the DO.Product's Category
        boProduct.Category = (BO.Category?)product.Category;
        // Return the BO.Product object

        return boProduct;
    }

    /// <summary>
    /// This method is named "ConvertBoProductToProduct" and it takes in a BO.Product object as a parameter. It creates a new DO.Product object and sets its properties 
    /// </summary>
    /// <param name="boProduct"></param>
    /// <returns></returns>
    private DO.Product ConvertBoProductToProduct(BO.Product boProduct)
    {
        // Create a new DO.Product object
        DO.Product product = new DO.Product();
        // Set the DO.Product's ID to the BO.Product's ID
        product.ID = boProduct.ID;
        // Set the DO.Product's Name to the BO.Product's Name
        product.Name = boProduct.Name;
        // Set the DO.Product's Price to the BO.Product's Price
        product.Price = boProduct.Price;
        // Set the DO.Product's InStock to the BO.Product's InStock
        product.InStock = boProduct.InStock;
        // Set the DO.Product's Category to the BO.Product's Category
        DO.Category? category = (DO.Category?)boProduct.Category;
        product.Category = category;
        // Return the DO.Product object
        return product;
    }

    /// <summary>
    /// This method is named "CreateproductForLists" and it takes in an optional filter function as a parameter. It uses a LINQ query
    /// to select all products from the Dal.Product data source, converts each product to a BO.Product using the "ConvertProductToBoProduct" method,
    /// and applies the filter function if it is not null. 
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
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

    /// <summary>
    /// get a product and its details from the cart and the data source.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="cart"></param>
    /// <returns></returns>
    /// <exception cref="BO.IdBOException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.ProductItem Get(int? Id, BO.Cart cart)
    {
        // Check if the Id is less than or equal to 0 and throw an exception if it is
        if (Id <= 0) throw new BO.IdBOException("Not positive Id!");
        try
        {
            // Retrieve the corresponding OrderItem from the cart's details collection
            BO.OrderItem? orderItem = cart.Details.FirstOrDefault(x => x?.ProductID == Id);
            // Retrieve the corresponding Product from the Dal.Product data source
            DO.Product? product = Dal!.Product.Get(x => x?.ID == Id);
            // Create a new BO.ProductItem object
            BO.ProductItem item = new()
            {
                // Set the ID property to the ID of the Product
                ID = (int)product?.ID!,
                // Set the AmontInCart property to the Amount of the OrderItem
                AmontInCart = orderItem?.Amount ?? 0,
                // Set the Name property to the Name of the Product
                Name = product?.Name,
                // Set the Price property to the Price of the Product
                Price = (int)product?.Price!,
                // Set the IsInStock property to false
                IsInStock = false,
                // Set the Category property to the Category of the Product
                Category = (BO.Category?)(product?.Category)
            };

            // Check if the Product is in stock and set the IsInStock property accordingly
            if ((Dal!.Product.Get(x => x?.ID == orderItem?.ProductID)!)?.InStock > 0) { item.IsInStock = true; }
            else { item.IsInStock = false; }
            // Return the BO.ProductItem object
            return item;
        }
        // Catch any exception and throw a new BO.IdBOException
        catch (IdException) { throw new BO.IdBOException("Product with given Id didn't found"); }
    }

    /// <summary>
    /// This function takes in a Cart object and an optional filter function as parameters. 
    /// It uses a LINQ query to select all products from the data source, applies the filter function if it is not null.
    /// Then it calls the "Get" method passing in the product's ID and the Cart object as parameters to retrieve the 
    /// corresponding OrderItem from the cart's details collection and the corresponding Product from the data source
    /// and returns an enumerable collection of BO.ProductItem objects, that contain the details of the product and the amount of the product in the cart.
    /// </summary>
    /// <param name="cart"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<BO.ProductItem> GetListOfItems(BO.Cart cart, Func<BO.Product?, bool>? filter = null)
    {
        return from item in Dal?.Product.GetAll()
               let BOProduct = ConvertProductToBoProduct((DO.Product)item!)
               where filter is null || filter!.Invoke(BOProduct)
               select Get(item?.ID, cart);    
    }


    /// <summary>
    /// This code is a method named "GetListOfItemsInCart" that takes in a Cart object as a parameter. 
    /// It uses a LINQ query to select all products from the Dal.Product data source and all the OrderItems from the cart's details collection,
    /// then it filters the products that have the same ID as the ProductID from the OrderItems, 
    /// then it calls the "Get" method passing in the product's ID and the Cart object as parameters.
    /// "Get" method retrieves the corresponding OrderItem from the cart's details collection and the corresponding Product from the data source, and creates a new BO.ProductItem 
    /// object with the details of the product and the amount of the product in the cart, and returns an enumerable collection of BO.ProductItem objects, that contain the details of the products that present in the cart.
    /// </summary>
    /// <param name="cart"></param>
    /// <returns></returns>
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
        //check if product exist inside orders - if yes, so throw a message
        if (Dal!.OrderItem.GetAll(x => x!.Value.ProductID == Id).Count() == 0) Dal.Product.Delete(Id);

        else throw new BO.IdBOException("Product inside an exist order. cant delete."); ;
    }
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(BO.Product item)
    {
        try { if (CheckNewItem(item) == true) Dal!.Product.Update(ConvertBoProductToProduct(item)); }
        catch (IdException) { throw new BO.UpdateProductException("Id not found. Impossible to update."); }
    } /// if received item have right properties and exist, update it. else throw a message.


    ///This code is a method named "GetList" that takes in an optional filter function as a parameter. 
    ///It calls the "CreateproductForLists" method, passing in the filter function as a parameter and it returns the result of that method, sorted by ID. 
    ///The "GetList" method returns an enumerable collection of BO.ProductForList objects filtered by an optional filter function and sorted by ID.
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<ProductForList> GetList(Func<BO.Product?, bool>? filter = null) => CreateproductForLists(filter).OrderBy(p => p.ID);

    public void Delete(int id)
    {
        if (ProductExistInsideOrders(id)) throw new IdBOException("Product Exist in order. impossible to delete");
        
        Dal.Product.Delete(id);
        Dal?.OrderItem.DeleteProduct(id);
        
    }

    public bool ProductExistInsideOrders(int id)
    {
        if (Dal!.OrderItem.GetAll(x => x!.Value.ProductID == id).Count() != 0) return true;
        else return false;
    }
}