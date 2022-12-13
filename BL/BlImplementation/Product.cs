using DalApi;

namespace BlImplementation

{

    internal class Product : BlApi.IProduct
    {
        IDal? Dal = DalApi.Factory.Get();

        public bool CheckNewItem(BO.Product item)
        {
            if (item.ID < 100000) throw new BO.IdBOException("minimum 6 digits for id!");
            if (item.Name == null) throw new BO.ProductNameException("Name can't be null");
            if (item.Price < 0) throw new BO.PriceException("Negative price!");
            if (item.InStock < 0) throw new BO.InStockException(" Product out of stock");

            return true;
        } ///check previews criterion for a new item

        public BO.Product ConvertProductToBoProduct( DO.Product product)
        {
            BO.Product boProduct = new BO.Product();
            boProduct.ID = product.ID;
            boProduct.Name = product.Name;
            boProduct.Price = product.Price;
            boProduct.InStock = product.InStock;
            boProduct.Category = (BO.Enums.Category?)product.Category;

            return boProduct;
        }

        public DO.Product ConvertBoProductToProduct(BO.Product boProduct)
        {
            DO.Product product = new DO.Product();
            product.ID = boProduct.ID;
            product.Name = boProduct.Name;
            product.Price = boProduct.Price;
            product.InStock = boProduct.InStock;
            product.Category = (DO.Enums.Category?)boProduct.Category;

            return product;
        }
        
        public void Add(BO.Product item)
        {
            if (CheckNewItem(item) == true) Dal.Product.Add(ConvertBoProductToProduct(item)); 
        }

        public BO.Product Get(int Id)
        {
            if (Id <= 0) throw new BO.IdBOException("Not positive Id!");
            return ConvertProductToBoProduct(Dal.Product.Get(Id));
        }

        public BO.ProductItem Get(int Id, BO.Cart cart) 
        {

            if (Id <= 0) throw new BO.IdBOException("Not positive Id!");
            try
            {
                BO.ProductItem item = new BO.ProductItem();
                DO.OrderItem orderItem = new DO.OrderItem();

                foreach (DO.OrderItem? itemCart in cart.Details)
                {
                    //Console.WriteLine(itemCart.ToString());
                    if (itemCart?.ProductID == Id) orderItem = itemCart?? throw new BO.IdBOException("Product with given Id didn't found");
                }

                item.ID = orderItem.ID;
                item.AmontInCart = orderItem.Amount;

                if ((Dal.Product.Get(orderItem.ProductID)).InStock  > 0) item.IsInStock = true;
                else { item.IsInStock = false; }

                item.Name = cart.CustomerName;
                item.Price = orderItem.Price;

                item.Category = (BO.Enums.Category?)Dal.Product.Get(orderItem.ProductID).Category;

                return item;
            }
            catch (IdException) { throw new BO.IdBOException("Product with given Id didn't found"); }
        }

        public void Remove(int Id)
        {

            ///check if received Id exist
            DO.Product? product = new DO.Product();
            product = Dal.Product.Get(Id);
            if (product == null) throw new BO.DeleteProductException("Cant delete product. Id not found.");

            //check if product exist inside order - if yes, so throw a message
            if (SearchProductInsideExistOrders(Id).Count() == 0) Dal.Product.Delete(Id);
            else throw new BO.IdBOException("Product inside an exist order. cant delete."); ;

        }

        public void Update(BO.Product item)
        {
            try { if (CheckNewItem(item) == true) Dal.Product.Update(item.ID, ConvertBoProductToProduct(item)); }

            catch (IdException) { throw new BO.UpdateProductException("Id not found. Impossible to update."); }
        } /// if received item have right properties and exist, update it. else throw a message.

        public List<BO.ProductForList> GetList(Func<BO.Product?, bool>? filter = null)
        {//Func<Enums.Category?, bool>? f 

            List<BO.ProductForList> listBoProduct = new();

            foreach (DO.Product? product in Dal.Product.GetAll())
            {
                if (filter == null || filter(ConvertProductToBoProduct(product?? throw new BO.nullObjectBOException("null"))))
                {
                    BO.ProductForList boProductForList = new()
                    {
                        ID = product?.ID ?? throw new BO.nullObjectBOException("null ID"),
                        Name = product?.Name,
                        Price = product?.Price ?? throw new BO.nullObjectBOException("null ID"),
                        Category = (BO.Enums.Category?)product?.Category
                    };
                    listBoProduct.Add(boProductForList);
                }      
            }
            return listBoProduct;
        }
            
    

        //check if a product are inside any order
        //return a list or Id order that contain the product
        public List<int?> SearchProductInsideExistOrders(int ProductId)
        {
            List<DO.OrderItem?> dalOlist = new();
            foreach (DO.OrderItem? orderItem in Dal.OrderItem.GetAll())
                dalOlist.Add(orderItem ?? throw new BO.nullObjectBOException("null object.BoCart.Add"));

            List<int?>? orderItemsWithProduct = new List<int?>();
            foreach (DO.OrderItem? item in dalOlist)
            {
                if (item?.ProductID == ProductId) orderItemsWithProduct.Add(item?.ID);
            }
            return orderItemsWithProduct;
        }


    }
    
}
