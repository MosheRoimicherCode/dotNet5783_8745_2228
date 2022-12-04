namespace Dal;

using DalApi;
using DO;
using static Dal.DataSource;

///A class for connect with Product struck
internal class DalProduct : IProduct
{
    ///----------------------------------------------------
    ///----------------- CRUD functions -------------------

    public int Add(Product product) =>
        DataSource._productList.Exists(p => p?.ID == product.ID)
            ? throw new IdException("Product ID already exists")
            : DataSource.AddProduct(product); /// Add Product to Data Base

    public Product Get(int ProductID)
    {
        foreach (var product in from Product product in _productList
                                where product.ID == ProductID
                                select product)
        {
            return product;
        }

        ///in case of Id not found, throw exception
        throw new IdException(" Not found ID. (DalOrderProduct.Get Exception)");
    }///search for product by Id and return the specific product

    public Product? Get(Func<Product?, bool> f) =>
        (from product in _productList
         where f(product)
         select product).FirstOrDefault();

    public void Delete(int ProductID)
    {
        bool flag = false;
        for (int i = 0; i < _productList.Count; i++)
        {
            if (_productList[i].ID == ProductID)
            {
                _productList.Remove(_productList[i]);
                flag = true;
            }

            ///if not found return a message

        }
        if (flag == false) Console.WriteLine(" Not found ID. (DalProduct.Delete Exception)");
        ///delete product from data base by Id
    }
    ///replace product by another inside array
    public void Update(int ProductID, Product newProduct)
    {
        for (int i = 0; i < _productList.Count; i++)
        {
            var product = _productList[i];
            if (product.ID.Equals(ProductID))
            {
                int index = _productList.IndexOf(product);
                _productList.RemoveAt(index);
                _productList.Insert(index, newProduct);
                return;
            }

        }
        ///if not found return a message
        throw new IdException(" Not found ID. (DalOrderProduct.Update Exception)");
    }///replace order by another inside array


    ///----------------------------------------------------
    ///----------------------------------------------------



    ///----------------------------------------------------
    ///----------------- Methods -------------------------- 

    public IEnumerable<Product?> GetAll(Func<Product?, bool>? f) =>
        f == null ? _productList.Select(p => p)
                  : _productList.Where(f);

    ///----------------------------------------------------
}
