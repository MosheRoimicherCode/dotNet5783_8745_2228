using DO;
using static Dal.DataSource;
namespace Dal;

///A class for connect with Product struck
public class DalProduct { 

        ///const
        public DalProduct()
        {
        Product product = new Product();
        DataSource.AddProduct(product);
        }

        /// Add Product to Data Base
        public void CreateProduct(Product product)
        {
            DataSource.AddProduct(product);            ///insert to Data Base  
        }

        ///search for product by Id and return the spessific product
        public Product GetProduct(int ProductID)
        {
            foreach (Product product in _productArr)
            {

                if (product.ID.Equals(ProductID))
                {
                    return product;
                }
            }
            ///in case of Id not found, throw exeption
            throw new Exception("Not found a product with this Id");
        }

        public Product GetAll()
        {
            foreach (Product product in _productArr)
            {
                GetProduct(product.ID);
            }
            ///in case of Id not found, throw exeption
            throw new Exception("Not found a product with this Id");
        }

        ///return a new copy of product array
        public Product[] CopyProductArray()
        {
            int tempIndex = 0;
            Product[] newProductArray = new Product[50];
            foreach (Product product in _productArr)
            {
                newProductArray[tempIndex++] = product;
            }
            return newProductArray;
        }

        ///delete product from data base by Id
        public void DeleteProduct(int ProductID)
        {
            ///run over product array
            for (int i = 0; i <= Config._productArrIndex; i++)
            {
                ///find specific order
                if (_productArr[i].ID == ProductID)
                {
                    ///if finned, run over the arr to delete specific order
                    for (int j = i + 1; j < Config._productArrIndex; j++)
                    {
                        _productArr[i] = _productArr[j];
                    }
                    ///delete last object and resize index
                    Product nullProduct = new Product();
                    _productArr[Config._productArrIndex] = nullProduct;
                    Config._productArrIndex--;
                }
            }
            ///if not found return a message
            throw new Exception("Not found a product with this Id");
        }

        ///replace product by another inside array
        public void RunOverProduct(int ProductID, Product newProduct)
        {
            ///run over products array
            for (int i = 0; i <= Config._productArrIndex; i++)
            {
                ///find specific product
                if (_productArr[i].ID == ProductID)
                {
                    ///if finned, replace the product with a new one
                    _productArr[i] = newProduct;
                }
            }
            ///if not found return a message
            throw new Exception("Not found a product with this Id");
        }

}
