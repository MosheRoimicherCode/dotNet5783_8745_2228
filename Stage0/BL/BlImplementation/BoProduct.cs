using BlApi;
using BO;
using Dal;
using DalApi;
using DO;
using System.Security.Cryptography;

namespace BlImplementation
{
    internal class BoProduct : IBoProduct
    {
        IDal Dal = new DalList ();

        public void Add(BO.BoProduct item)
        {
            if (item.ID < 0) throw new IdError("Negative Id!");
            if (item.Name != null) throw new ProductNameError("Name can't be null");
            if (item.Price > 0) throw new PriceError("Negative price!");
            if (item.InStock > 0) throw new InStockError(" Product out of stock");
            else
            {
                Product product = new Product ();
                product.ID = item.ID;
                product.InStock = item.InStock;
                product.Name = item.Name;
                product.Price = item.Price;
                product.Category = (DO.Enums.Category)item.Category;

                Dal.Product.Add(product);
            }
            
        }

        public BO.BoProduct Get(int Id)
        {
            throw new NotImplementedException();
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
