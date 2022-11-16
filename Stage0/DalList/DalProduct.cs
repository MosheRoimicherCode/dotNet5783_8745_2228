using DalApi;
using DO;
using static Dal.DataSource;
namespace Dal;

///A class for connect with Product struck
public class DalProduct 
{

    ///----------------- Constructors -------------------
    public DalProduct(Product p) => DataSource.AddProduct(p);


    ///----------------------------------------------------
    ///----------------- CRUD functions -------------------

    public void Add(Product product) => DataSource.AddProduct(product);/// Add Product to Data Base

    public Product Get(int ProductID)
        {
            foreach (Product product in _productList)
            {

                if (product.ID.Equals(ProductID))
                {
                    return product;
                }
            }
        ///in case of Id not found, throw exception
        throw new IdException(); ;
        }///search for product by Id and return the specific product

    public void Delete(int ProductID)
        {
            foreach (Product product in _productList)
            {
                if (product.ID.Equals(ProductID))
                {
                _productList.Remove(product);
                }
            }

        ///if not found return a message
        throw new IdException();
    }///delete product from data base by Id

     ///replace product by another inside array
    public void Update(int ProductID, Product newProduct)
        {
            foreach (Product product in _productList)
            {
                if (product.ID.Equals(ProductID))
                {
                    int index = _productList.IndexOf(product);
                    _productList.RemoveAt(index);
                    _productList.Insert(index, newProduct);
                }
            }
        ///if not found return a message
        throw new IdException();
    }///replace order by another inside array


    ///----------------------------------------------------
    ///----------------------------------------------------



    ///----------------------------------------------------
    ///----------------- Methods -------------------------- 
    public void GetAll()
    { 
        foreach (Product product in _productList)
        {
            Console.WriteLine(product.ToString());
        }
    }///ToString call for all list

    public List<Product> CopyProductList()
    {
        List<Product> productlist = new List<Product>();
        productlist = _productList;
        return productlist;
    }///return a new copy of product array


    ///----------------------------------------------------
}
