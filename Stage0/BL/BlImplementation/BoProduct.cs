using BlApi;
using Dal;
using DalApi;


namespace BlImplementation
{


    internal class BoProduct : IBoProduct
    {


        IDal Dal = new DalList ();

        public bool CheckNewItem(BO.BoProduct item)
        {
            if (item.ID <= 0) throw new BO.IdBOException("Negative Id!");
            if (item.Name == null) throw new BO.ProductNameException("Name can't be null");
            if (item.Price < 0) throw new BO.PriceException("Negative price!");
            if (item.InStock < 0) throw new BO.InStockException(" Product out of stock");

            return true;
        } ///check previews criterion for a new item

        public BO.BoProduct ConvertProductToBoProduct( DO.Product product)
        {
            BO.BoProduct boProduct = new BO.BoProduct();
            boProduct.ID = product.ID;
            boProduct.Name = product.Name;
            boProduct.Price = product.Price;
            boProduct.InStock = product.InStock;
            boProduct.Category = (BO.Enums.Category)product.Category;

            return boProduct;
        }

        public DO.Product ConvertBoProductToProduct(BO.BoProduct boProduct)
        {
            DO.Product product = new DO.Product();
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
            if (Id <= 0) throw new BO.IdBOException("Not positive Id!");
            return ConvertProductToBoProduct(Dal.Product.Get(Id));
        }

        public BO.BoProductItem Get(int Id, BO.BoCart cart) 
        {

            if (Id <= 0) throw new BO.IdBOException("Not positive Id!");
            try
            {
                BO.BoProductItem item = new BO.BoProductItem();
                DO.OrderItem orderItem = new DO.OrderItem();

                foreach (DO.OrderItem itemCart in cart.Details)
                {
                    if (itemCart.ProductID == Id)
                    { orderItem = itemCart; };
                }

                item.ID = orderItem.ID;
                item.AmontInCart = orderItem.Amount;

                if ((Dal.Product.Get(orderItem.ProductID)).InStock  > 0) item.IsInStock = true;
                else { item.IsInStock = false; }

                item.Name = cart.CustomerName;
                item.Price = orderItem.Price;

                item.Category = (BO.Enums.Category)Dal.Product.Get(orderItem.ProductID).Category;

                return item;
            }
            catch (IdException) { throw new BO.IdBOException("Product with given Id didn't found"); }
        }

        public void Remove(int Id)
        {

            ///check if received id exist
            DO.Product? product = new DO.Product();
            product = Dal.Product.Get(Id);
            if (product == null) throw new BO.DeleteProductException("Cant delete product. Id not found.");

            ///check if product is not inside an existing order
            DO.Order? order = new DO.Order();
            order = Dal.Order.Get(Id);
            if (order != null) throw new BO.DeleteProductException("Product exist in a Order. Impossible to delete.");

            ///if id exist and its not inside order then, delete him
            Dal.Product.Delete(Id);

        }

        public void Update(BO.BoProduct item)
        {
            try { if (CheckNewItem(item) == true) Dal.Product.Update(item.ID, ConvertBoProductToProduct(item)); }

            catch (IdException) { throw new BO.UpdateProductException("Product exist in a Order. Impossible to update."); }
        } /// if received item have right properties and exist, update it. else throw a message.

        public List<BO.BoProductForList> GetList()
        {
            List<BO.BoProductForList> listBoProduct = new List<BO.BoProductForList>();
            BO.BoProductForList boProductForList = new BO.BoProductForList();
            List<DO.Product> products = new List<DO.Product>();
            products = Dal.Product.CopyList();

            
            for (int i = 0; i < products.Count(); i++)
            {
                boProductForList.ID = products[i].ID;
                boProductForList.Name = products[i].Name;
                boProductForList.Price = products[i].Price;
                boProductForList.Category = (BO.Enums.Category)products[i].Category;

                listBoProduct.Add(boProductForList);
            }
            return listBoProduct;
        }
    }
}
