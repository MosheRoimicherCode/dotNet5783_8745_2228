namespace Dal;

using DalApi;
using DO;
using static Dal.DataSource;

///A class for connect with Product struck
internal class DalProduct :  
{
    public int Add(Product product) =>
        DataSource._productList.Exists(productInList => productInList?.ID == product.ID)
            ? throw new IdException("Product ID already exists")
            : DataSource.AddProduct(product); /// Add Product to Data Base

    public Product Get(int ProductID)
    {
        foreach (var product in from Product product in _productList
                                where product.ID == ProductID
                                select product) return product;

        throw new IdException(" Not found ID. (DalProduct.Get Exception)");
    }///search for product by Id and return the specific product

    public Product? Get(Func<Product?, bool> filter) =>
                                                    (from product in _productList
                                                    where filter(product)
                                                    select product).FirstOrDefault();

    public void Delete(int ProductID)
    {
        bool flag = false;
        for (int i = 0; i < _productList.Count; i++)
        {
            if (_productList[i]?.ID == ProductID)
            {
                _productList.Remove(_productList[i]);
                flag = true;
            }
        }
        //if Id not found send a MESSAGE
        if (flag == false) Console.WriteLine(" Not found ID. (DalProduct.Delete Exception)");
        ///delete product from data base by Id
    }
    ///replace product by another inside array
    public void Update(int ProductID, Product newProduct)
    {
        for (int i = 0; i < _productList.Count; i++)
        {
            Product? product = new();
            product = _productList[i];
            if (product?.ID.Equals(ProductID) ?? throw new IdException(" Not found ID. (DalOrderProduct.Update Exception)"))
            {
                int index = _productList.IndexOf(product);
                _productList.RemoveAt(index);
                _productList.Insert(index, newProduct);
                return;
            }
        }
    }///replace order by another inside array

    public IEnumerable<Product?> GetAll(Func<Product?, bool>? filter) =>
        filter == null ? _productList.Select(prouductInList => prouductInList)
                  : _productList.Where(filter);

}
