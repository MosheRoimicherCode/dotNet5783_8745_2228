namespace Dal;

using DalApi;
using DO;
using System.Runtime.CompilerServices;
using static Dal.DataSource;

///A class for connect with Product struck
internal class DalProduct : IProduct
{
    /// <summary>
    /// add product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    /// <exception cref="IdException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(Product product) =>
        _productList.Exists(productInList => productInList?.ID == product.ID)
            ? throw new IdException("Product ID already exists (DalProduct.Add)")
            : AddProduct(product); 

    /// <summary>
    /// get product based on delegate filter. using LINQ methds
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Product? Get(Func<Product?, bool> filter) => (from product in _productList
                                                         where filter(product)
                                                         orderby (product?.ID)
                                                         select product.Value).FirstOrDefault();
    
    /// <summary>
    /// delete product
    /// </summary>
    /// <param name="ProductID"></param>
    /// <exception cref="IdException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int ProductID)
    {
        try { _productList.RemoveAll(x => x?.ID == ProductID); }
        catch (ArgumentNullException) { throw new IdException(" Not found ID. (Dalproduct.Delete Exception)"); }
    }
    
    /// <summary>
    /// update product details
    /// </summary>
    /// <param name="newProduct"></param>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Product newProduct)
    {
        Delete(newProduct.ID);
        Add(newProduct);
    } ///replace product by another inside array


    /// <summary>
    /// get IEnumerable<Product?> object based on delegate filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Product?> GetAll(Func<Product?, bool>? filter) =>
        filter == null ? _productList.Select(prouductInList => prouductInList)
                  : _productList.Where(filter);
}