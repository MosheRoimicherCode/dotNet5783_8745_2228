using DalApi;
using DO;
using static Dal.DataSource;
using System.Linq;

namespace Dal;

///A class for connect with Product struck
internal class DalProduct : IProduct
{

    ///----------------- Constructors -------------------
 


    ///----------------------------------------------------
    ///----------------- CRUD functions -------------------

    public int Add(Product product) => DataSource.AddProduct(product);/// Add Product to Data Base

    public Product Get(int ProductID)
        {
        foreach (var product in from Product product in _productList
                                where product.ID == ProductID
                                select product)
        {
            return product;
        }

        ///in case of Id not found, throw exception
        throw new IdException(" Not found ID. (DalOrderProduct.Get Exception)"); ;
        }///search for product by Id and return the specific product

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
                }
            return;
            }
        ///if not found return a message
        throw new IdException(" Not found ID. (DalOrderProduct.Update Exception)");
    }///replace order by another inside array


    ///----------------------------------------------------
    ///----------------------------------------------------



    ///----------------------------------------------------
    ///----------------- Methods -------------------------- 
    public void GetAll()
    { 
        foreach (Product product in _productList)
        {
            if (product.ID != 0)
            {
                Console.WriteLine(product.ToString());
            }
        }
    }///ToString call for all list

    public List<Product> CopyList()
    {
        List<Product> productlist = new List<Product>();
        productlist = _productList;
        return productlist;
    }///return a new copy of product array


    ///----------------------------------------------------
}
