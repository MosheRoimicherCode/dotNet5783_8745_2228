namespace BlApi;
using BO;

public interface IProduct
{
    ///add product to data base - for manager user
    public void Add(Product item);

    ///search for a product with specific Id for manager user 
    /// <returns> IBoProduct item </returns>
    public Product Get(int Id);

    ///search for a product with specific Id inside client cart (for client user)
    /// <returns> BoProductItem </returns>
    public ProductItem Get(int? Id, Cart cart);

    ///remove product from data base - for manager user
    public void Remove(int Id);

    ///update data for product in data base
    public void Update(Product item);

    /// return a list with all product in client order
    /// <returns> Product list </returns>
    public IEnumerable<ProductForList> GetList(Func<Product?, bool>? filter = null);

    public IEnumerable<BO.ProductItem> GetListOfItems(BO.Cart cart, Func<Product?, bool>? filter = null);

    public IEnumerable<BO.ProductItem> GetListOfItemsInCart(BO.Cart cart);

    public void Delete(int id);
}
/// interface of product items for manager and client
