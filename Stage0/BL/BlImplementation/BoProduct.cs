using BlApi;
using BO;
using Dal;
using DalApi;
using DO;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace BlImplementation
{
    internal class BoProduct : IBoProduct
    {
        IDal Dal = new DalList ();

        public bool CheckNewItem(BO.BoProduct item)
        {
            if (item.ID <= 0) throw new IdBOException("Negative Id!");
            if (item.Name != null) throw new ProductNameException("Name can't be null");
            if (item.Price > 0) throw new PriceException("Negative price!");
            if (item.InStock > 0) throw new InStockException(" Product out of stock");

            return true;
        } ///check previews criterios for a new item

        public BO.BoProduct ConvertProductToBoProduct( Product product)
        {
            BO.BoProduct boProduct = new BO.BoProduct();
            boProduct.ID = product.ID;
            boProduct.Name = product.Name;
            boProduct.Price = product.Price;
            boProduct.InStock = product.InStock;
            boProduct.Category = (BO.Enums.Category)product.Category;

            return boProduct;
        }

        public Product ConvertBoProductToProduct(BO.BoProduct boProduct)
        {
            Product product = new Product();
            product.ID = boProduct.ID;
            product.Name = boProduct.Name;
            product.Price = boProduct.Price;
            product.InStock = boProduct.InStock;
            product.Category = (DO.Enums.Category)boProduct.Category;

            return product;
        }

        
        public void Add(BO.BoProduct item)
        {
            if (CheckNewItem(item) == true) Dal.Product.Add(ConvertBoProductToProduct(item)); 
        }

        public BO.BoProduct Get(int Id)
        {
            if (Id < 0) throw new IdBOException("Negative Id!");
            return ConvertProductToBoProduct(Dal.Product.Get(Id));
        }

        public BO.BoProductItem Get(int Id, BO.BoCart cart)
        {

            if (Id < 0) throw new IdBOException("Negative Id!");
            try
            {
                BO.BoProductItem item = new BO.BoProductItem();
                item.ID = cart.Details.ID;
                item.AmontInCart = cart.Details.Amount;

                if ((Dal.Product.Get(cart.Details.ProductID)).InStock  > 0) item.IsInStock = true;
                else { item.IsInStock = false; }

                item.Name = cart.CustomerName;
                item.Price = cart.Details.Price;

                item.Category = (BO.Enums.Category)Dal.Product.Get(cart.Details.ProductID).Category;

                return item;
            }
            catch (IdException) { throw new IdBOException("Product with given Id didn't found"); }
        }


        public void Remove(int Id)
        {

            ///check if received id exist
            Product? product = new Product();
            product = Dal.Product.Get(Id);
            if (product == null) throw new DeleteProductException("Cant delete product. Id not found.");

            ///check if product is not inside an existing order
            Order? order = new Order();
            order = Dal.Order.Get(Id);
            if (order != null) throw new DeleteProductException("Product exist in a Order. Impossible to delete.");

            ///if id exist and its not inside order then, delete him
            Dal.Product.Delete(Id);

        }

        public void Update(BO.BoProduct item)
        {
            try { if (CheckNewItem(item) == true) Dal.Product.Update(item.ID, ConvertBoProductToProduct(item)); }

            catch (IdException) { throw new UpdateProductException("Product exist in a Order. Impossible to update."); }
        } /// if received item have right properties and exist, update it. else throw a message.


        public BO.BoProductForList GetList()
        {
            BO.BoProductForList boProduct = new BO.BoProductForList();
            return boProduct;
        }
    }
}
