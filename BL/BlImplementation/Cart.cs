using BlApi;
using DalApi;
using System.Diagnostics;

namespace BlImplementation
{
    internal class Cart : ICart
    {
        private static readonly IDal? Dal = DalApi.Factory.Get();
        private static BO.OrderItem ConvertDo2BoOrderItem(DO.OrderItem DOTemp)
        {
            BO.OrderItem BOtemp = new()
            {
                ProductID = DOTemp.ProductID,
                OrderID = DOTemp.ID,
                ProductName = Dal!.Product.Get(x => x?.ID == DOTemp.ID)?.Name,
                ProductPrice = DOTemp.Price,
                Amount = DOTemp.Amount
            };
            BOtemp.TotalPrice = (BOtemp.Amount * BOtemp.ProductPrice);

            return BOtemp;
        }
        private static DO.OrderItem ConvertBo2DoOrderItem(BO.OrderItem BOTemp)
        {
            DO.OrderItem DOtemp = new()
            {
                ID = BOTemp.ID,
                ProductID = BOTemp.ProductID,
                OrderID = BOTemp.ID,
                Price = BOTemp.ProductPrice,
                Amount = BOTemp.Amount
            };

            return DOtemp;
        }
        private static  BO.Product ConvertDo2BoProduct(DO.Product DOTemp)
        {
            BO.Product BOtemp = new()
            {
                ID = DOTemp.ID,
                Name = DOTemp.Name,
                Price = DOTemp.Price,
                Category = (BO.Enums.Category)DOTemp.Category!,
                InStock = DOTemp.InStock
            };

            return BOtemp;
        }
        private static BO.Order ConvertDo2BoOrder(DO.Order DOTemp)
        {
            BO.Order BOtemp = new()
            {
                ID = DOTemp.ID,
                CustomerName = DOTemp.CustomerName,
                CustomerEmail = DOTemp.CustomerEmail,
                CustomerAdress = DOTemp.CustomeAdress,
                OrderDate = DOTemp.OrderDate,
                ShipDate = DOTemp.ShipDate,
                DeliveryDate = DOTemp.DeliveryDate
            };
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

            BO.Cart newBoCart = new()
            {
                CustomerName = boCart.CustomerName,
                CustomerEmail = boCart.CustomerEmail,
                CustomeAdress = boCart.CustomeAdress,
                TotalPrice = boCart.TotalPrice,
                Details = boCart.Details.ToList()
            };
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
            newBoCart.TotalPrice = 0;
            foreach (var item in newBoCart.Details)
            {
                newBoCart.TotalPrice += item!.TotalPrice;
            }
            
            return newBoCart;
        }
        ///updated the amount in the cart
        public BO.Cart UpdateAmount(BO.Cart boCart, int Id, int NewAmount)
        {
            BO.Cart newBoCart = new()
            {
                CustomerName = boCart.CustomerName,
                CustomerEmail = boCart.CustomerEmail,
                CustomeAdress = boCart.CustomeAdress,
                Details = boCart.Details,
                TotalPrice = boCart.TotalPrice
            };

            foreach (BO.OrderItem? item in boCart.Details)
            {
                if (item?.ProductID == Id)
                {
                    if (NewAmount == item.Amount) return newBoCart;
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
                if (Dal!.Product.GetAll(x => x!.Value.ID == item?.ID).Count() < 0) throw new BO.IdBOException("not all the products in cart exist");
                if (item?.Amount <= 0)                                             throw new BO.IdBOException("negative Amount");
                if (item?.Amount > Dal?.Product?.Get(x => x?.ID == item!.ProductID)!.Value.InStock) throw new BO.IdBOException("not enough in stock");
                if (boCart.CustomerName == "" || boCart.CustomerName == null)      throw new BO.IdBOException("Customer Name is not empty");
                if (boCart.CustomeAdress == "" || boCart.CustomeAdress == null)    throw new BO.IdBOException("Customer address is empty");
                if (boCart.CustomerEmail == "" || boCart.CustomerEmail == null)    throw new BO.IdBOException("Customer email is not valid");
            }

            DO.Order newOrder = new()
            {
                CustomerName = Name,
                CustomeAdress = Addres,
                CustomerEmail = Email,
                OrderDate = DateTime.Now
            };
            newOrder.ID = Dal!.Order.Add(newOrder);

            foreach (BO.OrderItem? item in boCart.Details)
            {
                DO.OrderItem? newOrderItem = ConvertBo2DoOrderItem(item ?? throw new BO.nullObjectBOException("null object.BoCart.Add"));
                int id = newOrderItem.Value.ProductID;
                var productFromSource = Dal.Product.Get(x => x!.Value.ID == id);

                DO.Product dp = new()
                {
                    ID = id,
                    Name = productFromSource!.Value.Name,
                    Price = productFromSource.Value.Price,
                    Category = productFromSource.Value.Category,
                    InStock = productFromSource.Value.InStock - newOrderItem.Value.Amount,
                };
                Dal.Product.Update(id, dp);
                Dal.OrderItem.Add(newOrderItem ?? throw new BO.nullObjectBOException("null"));
            }

        }
    }
}
/// interface of product items for manager and client
