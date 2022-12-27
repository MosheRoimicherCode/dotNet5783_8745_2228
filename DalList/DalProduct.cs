namespace Dal;

using DalApi;
using DO;
using static Dal.DataSource;

///A class for connect with Product struck
internal class DalProduct : IProduct
{
    public int Add(Product product) =>
        _productList.Exists(productInList => productInList?.ID == product.ID)
            ? throw new IdException("Product ID already exists (DalProduct.Add)")
            : AddProduct(product); /// Add Product to Data Base
    public Product? Get(Func<Product?, bool> filter) => (from product in _productList
                                                         where filter(product)
                                                         orderby (product.Value.ID)
                                                         select product.Value).FirstOrDefault();
    public void Delete(int ProductID)
    {
        try { _productList.RemoveAll(x => x?.ID == ProductID); }
        catch (ArgumentNullException) { throw new IdException(" Not found ID. (Dalproduct.Delete Exception)"); }
    }
    public void Update(int ProductID, Product newProduct)
    {
        try
        {
            int index = _productList.FindIndex(x => x?.ID == ProductID);

            _productList.RemoveAt(index);
            _productList.Insert(index, newProduct);
        }
        catch { throw new IdException("not found id. (DalProduct.Update Exception)"); }
    } ///replace product by another inside array
    public IEnumerable<Product?> GetAll(Func<Product?, bool>? filter) =>
        filter == null ? _productList.Select(prouductInList => prouductInList)
                  : _productList.Where(filter);
}    