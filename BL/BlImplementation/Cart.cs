using BlApi;
using DalApi;

namespace BlImplementation
{
    internal class Cart : ICart
    {
        IDal? Dal = DalApi.Factory.Get();
        private BO.OrderItem ConvertDo2BoOrderItem(DO.OrderItem DOTemp)
        {
            BO.OrderItem BOtemp = new();

            BOtemp.ProductID = DOTemp.ProductID;
            BOtemp.OrderID = DOTemp.ID;
            BOtemp.ProductName = Dal!.Product.Get(x => x?.ID == DOTemp.ID)?.Name;
            BOtemp.ProductPrice = DOTemp.Price;
            BOtemp.Amount = DOTemp.Amount;
            BOtemp.TotalPrice = (BOtemp.Amount * BOtemp.ProductPrice);

            return BOtemp;
        }
        private DO.OrderItem ConvertBo2DoOrderItem(BO.OrderItem BOTemp)
        {
            DO.OrderItem DOtemp = new();
            DOtemp.ID = BOTemp.ID;
            DOtemp.ProductID = BOTemp.ProductID;
            DOtemp.OrderID = BOTemp.ID;
            DOtemp.Price = BOTemp.ProductPrice;
            DOtemp.Amount = BOTemp.Amount;

            return DOtemp;
        }
        private BO.Product ConvertDo2BoProduct(DO.Product DOTemp)
        {
            BO.Product BOtemp = new();

            BOtemp.ID = DOTemp.ID;
            BOtemp.Name = DOTemp.Name;
            BOtemp.Price = DOTemp.Price;
            BOtemp.Category = (BO.Enums.Category)DOTemp.Category!;
            BOtemp.InStock = DOTemp.InStock;

            return BOtemp;
        }
        private BO.Order ConvertDo2BoOrder(DO.Order DOTemp)
        {
            BO.Order BOtemp = new();
            BOtemp.ID = DOTemp.ID;
            BOtemp.CustomerName = DOTemp.CustomerName;
            BOtemp.CustomerEmail = DOTemp.CustomerEmail;
            BOtemp.CustomerAdress = DOTemp.CustomeAdress;
            BOtemp.OrderDate = DOTemp.OrderDate;
            BOtemp.ShipDate = DOTemp.ShipDate;
            BOtemp.DeliveryDate = DOTemp.DeliveryDate;
            if (DOTemp.ShipDate < DOTemp.DeliveryDate) BOtemp.OrderStatus = BO.Enums.Status.shiped;
            else if (DOTemp.ShipDate < DateTime.Today) BOtemp.OrderStatus = BO.Enums.Status.shiped;
            else BOtemp.OrderStatus = BO.Enums.Status.approved;
            BOtemp.Details = new();

            return BOtemp;
        }
        ///add product to Cart, returns updated cart
        public BO.Cart Add(BO.Cart boCart, int productId)
        {

            //create a copy of current cart to return it
            List<BO.Product> productList = new();
            foreach (DO.Product? item in Dal!.Product.GetAll() ?? throw new BO.nullObjectBOException("null object.BoCart.Add") ) productList.Add(ConvertDo2BoProduct(item ?? throw new BO.nullObjectBOException("null object.BoCart.Add")));

            List<BO.OrderItem> orderItemList = new();
            foreach (DO.OrderItem? item in Dal!.OrderItem.GetAll() ?? throw new BO.nullObjectBOException("null object.BoCart.Add")) orderItemList.Add(ConvertDo2BoOrderItem(item ?? throw new BO.nullObjectBOException("null object.BoCart.Add")));

            List<BO.Order> orderList = new();
            foreach (DO.Order? item in Dal!.Order.GetAll() ?? throw new BO.nullObjectBOException("null object.BoCart.Add")) orderList.Add(ConvertDo2BoOrder(item ?? throw new BO.nullObjectBOException("null object.BoCart.Add")));

            BO.Cart newBoCart = new();

            newBoCart.CustomerName = boCart.CustomerName;
            newBoCart.CustomerEmail = boCart.CustomerEmail;
            newBoCart.CustomeAdress = boCart.CustomeAdress;
            newBoCart.TotalPrice = boCart.TotalPrice;
            newBoCart.Details = boCart.Details.ToList();
            //end of copy cart - end stage 1

            //search for product if exist inside cart
            if (newBoCart.Details.Any(x => x?.ProductID == productId))
            {
                IEnumerable<BO.OrderItem> updateDetails =  from product in productList
                                                           from orderItem in boCart.Details
                                                           where (product.ID == productId && product.InStock > 0 && orderItem.ProductID == productId)
                                                           select new BO.OrderItem()
                                                                   {
                                                                       ID = orderItem.ID,
                                                                       ProductID = orderItem.ProductID,
                                                                       OrderID = orderItem.OrderID,
                                                                       ProductName = orderItem.ProductName,
                                                                       ProductPrice = orderItem.ProductPrice,
                                                                       Amount = orderItem.Amount + 1,
                                                                       TotalPrice = orderItem.TotalPrice + orderItem.ProductPrice
                                                                   };

                newBoCart.Details.RemoveAll(x => x?.ProductID == productId);
                newBoCart.Details.AddRange(updateDetails);

            }
            
            else //if product is not yet inside cart
            {
                IEnumerable<BO.OrderItem> updateDetails = from product in productList
                                                          where (product.ID == productId && product.InStock > 0)
                                                          select new BO.OrderItem()
                                                          {
                                                              ID = 887799,
                                                              ProductID = product.ID,
                                                              //OrderID,
                                                              ProductName = product.Name,
                                                              ProductPrice = product.Price,
                                                              Amount = 1,
                                                              TotalPrice = product.Price
                                                          };

                newBoCart.Details.Add(updateDetails.First());
            }

            return newBoCart;
        }
        ///updated the amount in the cart
        public BO.Cart UpdateAmount(BO.Cart boCart, int Id, int NewAmount)
        {
            BO.Cart newBoCart = new BO.Cart();
            newBoCart.CustomerName = boCart.CustomerName;
            newBoCart.CustomerEmail = boCart.CustomerEmail;
            newBoCart.CustomeAdress = boCart.CustomeAdress;
            newBoCart.Details = boCart.Details;
            newBoCart.TotalPrice = boCart.TotalPrice;

            foreach (BO.OrderItem? item in boCart.Details)
            {
                if (item?.ProductID == Id)
                {
                    if (NewAmount == item.Amount) throw new BO.IdBOException("Amount has not changed");
                    else if (NewAmount == 0)
                    {
                        newBoCart.TotalPrice -= item.ProductPrice * item.Amount;
                        newBoCart.Details.Remove(item);
                        return newBoCart;
                    }
                    else if (NewAmount > item.Amount)
                    {
                        BO.OrderItem newOrderItem = item;
                        newBoCart.TotalPrice += (NewAmount - newOrderItem.Amount) * newOrderItem.ProductPrice;
                        newOrderItem.Amount = NewAmount;
                        newBoCart.Details.Remove(item);
                        newBoCart.Details.Add(newOrderItem);
                        return newBoCart;
                    }
                    else if (NewAmount < item.Amount)
                    {
                        BO.OrderItem newOrderItem = item;
                        newBoCart.TotalPrice -= (newOrderItem.Amount - NewAmount) * newOrderItem.ProductPrice;
                        newOrderItem.Amount = NewAmount;
                        newBoCart.Details.Remove(item);
                        newBoCart.Details.Add(newOrderItem);
                        return newBoCart;
                    }
                }
            }
            throw new BO.IdBOException("Item not found");
        }
        
        ///Confirm the Cart and build objects of order
        public void ConfirmCart(BO.Cart boCart, string Name, string Email, string Addres)
        {
            foreach (BO.OrderItem? item in boCart.Details)
            {
                if (Dal!.Product.GetAll(x => x!.Value.ID == item?.ID).Count() < 0)  throw new BO.IdBOException("not all the products in cart exist");
                if (item?.Amount <= 0)                                             throw new BO.IdBOException("negative Amount");
                if (item?.Amount > Dal?.Product?.Get(x => x?.ID == item!.ProductID)!.Value.InStock) throw new BO.IdBOException("not enough in stock");
                if (boCart.CustomerName == "" || boCart.CustomerName == null)      throw new BO.IdBOException("Customer Name is not empty");
                if (boCart.CustomeAdress == "" || boCart.CustomeAdress == null)    throw new BO.IdBOException("Customer address is empty");
                if (boCart.CustomerEmail == "" || boCart.CustomerEmail == null)    throw new BO.IdBOException("Customer email is not valid");
            }

            DO.Order newOrder = new DO.Order();
            newOrder.CustomerName = Name;
            newOrder.CustomeAdress = Addres;
            newOrder.CustomerEmail = Email;
            newOrder.OrderDate = DateTime.Now;
            newOrder.ID = Dal!.Order.Add(newOrder);

            foreach (BO.OrderItem? item in boCart.Details)
            {
                DO.OrderItem? newOrderItem = ConvertBo2DoOrderItem(item ?? throw new BO.nullObjectBOException("null object.BoCart.Add"));
                Dal.OrderItem.Add(newOrderItem ?? throw new BO.nullObjectBOException("null"));
            }

        }
    }
}
/// interface of product items for manager and client
