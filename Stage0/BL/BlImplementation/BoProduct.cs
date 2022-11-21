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

        /// check item content and if all criterion it OK, add product to list; 
        public void Add(BO.BoProduct item)
        {
            if (item.ID < 0) throw new IdBOException("Negative Id!");
            if (item.Name != null) throw new ProductNameException("Name can't be null");
            if (item.Price > 0) throw new PriceException("Negative price!");
            if (item.InStock > 0) throw new InStockException(" Product out of stock");
            
            ///create Product item equal to BoProduct to get inside Product List
            Product product = new Product ();
            product.ID = item.ID;
            product.InStock = item.InStock;
            product.Name = item.Name;
            product.Price = item.Price;
            product.Category = (DO.Enums.Category)item.Category;

            Dal.Product.Add(product);
                        
        }

        public BO.BoProduct Get(int Id)
        {
            if (Id < 0) throw new IdBOException("Negative Id!");
            try { 
                Product p = Dal.Product.Get(Id);
                BO.BoProduct item = new BO.BoProduct();
                item.ID = p.ID;
                item.InStock = p.InStock;
                item.Name = p.Name;
                item.Price = p.Price;
                item.Category = (BO.Enums.Category)p.Category;

                return item;
            
            }
            catch (IdException) { throw new IdBOException("Product with given Id didn't found"); }
            
        }
                           
        
        public BO.BoProductItem Get(int Id, BO.BoCart cart)
        {
            throw new NotImplementedException();
        }

        public void Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(BO.BoProduct item)
        {
            throw new NotImplementedException();
        }

        public BO.BoProductForList GetLists()
        {
            BO.BoProductForList product = new BO.BoProductForList();
            return product;
        }
    }
}
